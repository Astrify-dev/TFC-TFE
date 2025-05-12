using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "ScriptableObjects/PlayerMovementSettings", order = 1)]
public class PlayerMovementSettings : ScriptableObject{
    [BoxGroup("Rigidbody Settings")]
    [SerializeField, Tooltip("La masse du Rigidbody.")]
    public float rigidbodyMass = 1f;

    [BoxGroup("Rigidbody Settings"), SerializeField, Tooltip("Le drag (r�sistance) du Rigidbody.")]
    public float rigidbodyDrag = 0f;

    [BoxGroup("Rigidbody Settings"), SerializeField, Tooltip("Le drag angulaire du Rigidbody.")]
    public float rigidbodyAngularDrag = 0.05f;

    [BoxGroup("Rigidbody Settings"), SerializeField, Tooltip("Activer ou d�sactiver la gravit� pour le Rigidbody.")]
    public bool useGravity = true;

    [BoxGroup("Rigidbody Settings"), SerializeField, Tooltip("Valeur de la gravit� globale (affecte tous les objets dans la sc�ne).")]
    public Vector3 globalGravity = new Vector3(0, -9.81f, 0);

    [BoxGroup("Movement Settings")]
    [SerializeField, Tooltip("La vitesse de d�placement du joueur.")]
    public float moveSpeed = 5f;

    [BoxGroup("Movement Settings")]
    [SerializeField, Tooltip("La force de plonger du joueur.")]
    public float dive = 5f;

    [BoxGroup("Jump Settings"), SerializeField, Tooltip("La force de saut appliqu�e au joueur.")]
    public float jumpForce = 5f;

    [BoxGroup("Dash Settings"), SerializeField, Tooltip("La force appliqu�e lors d'un dash.")]
    public float dashForce = 10f;

    [BoxGroup("Dash Settings"), SerializeField, Tooltip("cooldown entre 2 dash ?")]
    public bool dashCooldown = false ;    
    
    [BoxGroup("Dash Settings"), SerializeField, Tooltip("Le cooldown entre 2 dash.")]
    public float dashCooldownTime = 0.2f;

    [BoxGroup("Slow Motion Settings")]
    [SerializeField, Tooltip("L'intensit� du slow motion.")]
    public float slowMotionIntensity = 0.5f;

    [BoxGroup("Slow Motion Settings"), SerializeField, Tooltip("La dur�e du slow motion.")]
    public float slowMotionTimer = 10f;

    [BoxGroup("Collision Settings"), SerializeField, Tooltip("Couches permettant de resauter.")]
    public LayerMask jumpResetLayers;

    [BoxGroup("Collision Settings"), SerializeField, Tooltip("Couches permettant de restaurer le dash.")]
    public LayerMask dashResetLayers;

    public void ApplySettingsToRigidbody(Rigidbody rb){
        if (rb is null) return;

        rb.mass = rigidbodyMass;
        rb.drag = rigidbodyDrag;
        rb.angularDrag = rigidbodyAngularDrag;
        rb.useGravity = useGravity;

        Physics.gravity = globalGravity;
    }
}
