using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [Header("Param�tres")]
    [Tooltip("L'enfant visuel � d�placer.")]
    public Transform visualChild;

    [Tooltip("L'objet contenant le BoxCollider.")]
    public BoxCollider movementArea;

    [Tooltip("Vitesse de d�placement al�atoire.")]
    public float moveSpeed = 1f;

    [Tooltip("Vitesse de d�placement lorsqu'il suit le joueur.")]
    public float followSpeed = 2f;

    [Tooltip("R�f�rence au script d'attaque.")]
    public S_ennemyAttack ennemyAttack;

    private Vector3 targetPosition;
    private bool isPlayerInAttackZone = false;
    private Transform playerTransform;

    void Start()
    {
        if (movementArea is null || visualChild is null || ennemyAttack is null)
        {
            Debug.LogError("Veuillez assigner toutes les r�f�rences n�cessaires dans l'inspecteur.");
            enabled = false;
            return;
        }

        ennemyAttack.OnPlayerEnter += StartFollowingPlayer;
        ennemyAttack.OnPlayerExit += StopFollowingPlayer;

        SetRandomTargetPosition();
    }

    void Update()
    {
        if (visualChild is null || movementArea is null) return;

        if (isPlayerInAttackZone && playerTransform is not null)
        {
            // Suivre le joueur avec la vitesse de suivi
            visualChild.position = Vector3.MoveTowards(visualChild.position, playerTransform.position, followSpeed * Time.deltaTime);
        }
        else
        {
            // Mouvement al�atoire avec la vitesse normale
            visualChild.localPosition = Vector3.MoveTowards(visualChild.localPosition, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(visualChild.localPosition, targetPosition) < 0.1f)
            {
                SetRandomTargetPosition();
            }
        }
    }

    private void SetRandomTargetPosition()
    {
        Vector3 areaSize = movementArea.size;
        Vector3 areaCenter = movementArea.center;

        float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);
        float randomZ = Random.Range(-areaSize.z / 2, areaSize.z / 2);

        targetPosition = new Vector3(visualChild.localPosition.x, areaCenter.y + randomY, areaCenter.z + randomZ);
    }

    private void StartFollowingPlayer(Transform player)
    {
        Debug.Log("L'ennemi commence � suivre le joueur !");
        isPlayerInAttackZone = true;
        playerTransform = player;
    }

    private void StopFollowingPlayer()
    {
        Debug.Log("L'ennemi arr�te de suivre le joueur !");
        isPlayerInAttackZone = false;
        playerTransform = null;
    }

    private void OnDestroy()
    {
        if (ennemyAttack is not null)
        {
            ennemyAttack.OnPlayerEnter -= StartFollowingPlayer;
            ennemyAttack.OnPlayerExit -= StopFollowingPlayer;
        }
    }
}
