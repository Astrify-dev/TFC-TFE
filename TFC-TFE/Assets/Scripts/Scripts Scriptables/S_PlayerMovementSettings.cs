using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "ScriptableObjects/PlayerMovementSettings", order = 1)]
public class PlayerMovementSettings : ScriptableObject{
    [BoxGroup("Rigidbody Settings")]
    [SerializeField, Tooltip("La masse du Rigidbody.")]
    public float rigidbodyMass = 1f;

    [BoxGroup("Rigidbody Settings"), SerializeField, Tooltip("Le drag (résistance) du Rigidbody.")]
    public float rigidbodyDrag = 0f;

    [BoxGroup("Rigidbody Settings"), SerializeField, Tooltip("Le drag angulaire du Rigidbody.")]
    public float rigidbodyAngularDrag = 0.05f;

    [BoxGroup("Rigidbody Settings"), SerializeField, Tooltip("Activer ou désactiver la gravité pour le Rigidbody.")]
    public bool useGravity = true;

    [BoxGroup("Rigidbody Settings"), SerializeField, Tooltip("Valeur de la gravité globale (affecte tous les objets dans la scène).")]
    public Vector3 globalGravity = new Vector3(0, -9.81f, 0);

    [BoxGroup("Movement Settings")]
    [SerializeField, Tooltip("La vitesse de déplacement du joueur.")]
    public float moveSpeed = 5f;

    [BoxGroup("Movement Settings")]
    [SerializeField, Tooltip("La force de plonger du joueur.")]
    public float dive = 5f;

    [BoxGroup("Jump Settings"), SerializeField, Tooltip("La force de saut appliquée au joueur.")]
    public float jumpForce = 5f;

    [BoxGroup("Slide Settings"), SerializeField, Tooltip("La vitesse de quand on reste sur un mur et qu'on glisse.")]
    public float wallSlideSpeed = 2f;

    [BoxGroup("Wall Jump Settings"), SerializeField, Tooltip("Le delay pour que le joueur puisse a nouveau sauter.")]
    public float wallJumpCoyoteTime = 0.2f;

    [BoxGroup("Wall Jump Settings"), SerializeField, Tooltip("La force du wallJump")]
    public float wallJumpForce = 1.2f;

    [BoxGroup("Dash sol Settings"), SerializeField, Tooltip("La force appliquée lors d'un dash au sol.")]
    public float groundDashForce = 10f;

    [BoxGroup("Dash sol Settings"), SerializeField, Tooltip("cooldown entre 2 dash au sol ?")]
    public bool groundDashCooldown = false ;    
    
    [BoxGroup("Dash sol Settings"), SerializeField, Tooltip("Le cooldown entre 2 dash au sol.")]
    public float groundDashCooldownTime = 0.2f;

    [BoxGroup("Dash air Settings"), SerializeField, Tooltip("La force appliquée lors d'un dash en l'air.")]
    public float airDashForce = 10f;

    [BoxGroup("Slow Motion Settings")]
    [SerializeField, Tooltip("L'intensité du slow motion.")]
    public float slowMotionIntensity = 0.5f;

    [BoxGroup("Slow Motion Settings"), SerializeField, Tooltip("La durée du slow motion.")]
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
