using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteDuplicator : MonoBehaviour
{
    [Header("Configuration du Pooling")]
    [SerializeField, Tooltip("Nombre total de duplicatas à créer pour le sprite. Plus ce nombre est élevé, plus le scrolling sera fluide, mais cela consomme plus de mémoire.")]
    private int poolSize = 5;

    [SerializeField, Tooltip("Index à partir duquel un sprite sera repositionné lorsqu'il sort de l'écran. Par exemple, si la valeur est 2, le sprite sera repositionné après avoir dépassé 2 fois sa taille.")]
    private int spriteRepositionIndex = 2;

    [SerializeField, Tooltip("Correction pour éviter des chevauchements ou des espaces entre les duplicatas. Ajustez cette valeur si vous remarquez des artefacts visuels.")]
    private float spriteRepositionCorrection = 0.03f;

    private Transform[] duplicatesPool;
    private float spriteSize;

    private void Start()
    {
        duplicatesPool = new Transform[poolSize];

        // Calcul de la taille du sprite uniquement sur l'axe Z
        var spriteBounds = GetComponent<SpriteRenderer>().bounds.size;
        spriteSize = spriteBounds.z; // Taille sur l'axe Z

        duplicatesPool[0] = transform;

        var startingPos = transform.position;

        for (var i = 1; i < poolSize; i++)
        {
            // Positionnement des duplicatas uniquement sur l'axe Z
            var position = startingPos;
            position.z += spriteSize - spriteRepositionCorrection;
            startingPos = position;

            // Création du duplicata avec une rotation Y forcée à 0
            var duplicate = Instantiate(gameObject, position, transform.rotation, transform.parent).transform;

            duplicatesPool[i] = duplicate;

            // Suppression du script sur les duplicatas pour éviter des comportements indésirables
            Destroy(duplicatesPool[i].GetComponent<SpriteDuplicator>());
        }
    }

    private void Update()
    {
        foreach (var duplicate in duplicatesPool)
        {
            // Vérifie si le duplicata est hors de l'écran sur l'axe Z
            if (duplicate.transform.position.z < -spriteSize * spriteRepositionIndex)
            {
                var rightmostSprite = GetRightMostSprite();
                var startingPos = rightmostSprite.position;
                var position = startingPos;
                position.z += spriteSize - spriteRepositionCorrection;

                duplicate.transform.position = position;
            }
        }
    }

    private Transform GetRightMostSprite()
    {
        // Trouve le duplicata le plus avancé sur l'axe Z
        var rightmostValue = Mathf.NegativeInfinity;
        Transform rightmostSprite = null;

        foreach (var duplicate in duplicatesPool)
        {
            if (duplicate.position.z > rightmostValue)
            {
                rightmostSprite = duplicate;
                rightmostValue = duplicate.position.z;
            }
        }

        return rightmostSprite;
    }
}
