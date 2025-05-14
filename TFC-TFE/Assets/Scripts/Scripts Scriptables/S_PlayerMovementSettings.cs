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

    [BoxGroup("Movement Settings")]
    [SerializeField, Tooltip("La vitesse de d�placement du joueur.")]
    public float moveSpeed = 5f;

    [BoxGroup("Movement Settings")]
    [SerializeField, Tooltip("La force de plonger du joueur.")]
    public float dive = 5f;

    [BoxGroup("Movement Settings"), SerializeField, Tooltip("Vitesse minimale du joueur (vitesse de d�part).")]
    public float minMoveSpeed = 5f;

    [BoxGroup("Movement Settings"), SerializeField, Tooltip("Vitesse maximale du joueur.")]
    public float maxMoveSpeed = 10f;

    [BoxGroup("Movement Settings"), SerializeField, Tooltip("Vitesse � laquelle on atteint la vitesse max.")]
    public float accelerationRate = 20f;

    [BoxGroup("Jump Settings"), SerializeField, Tooltip("La force de saut appliqu�e au joueur.")]
    public float jumpForce = 5f;

    [BoxGroup("Jump Settings")]
    [SerializeField, Tooltip("Force minimale de saut.")]
    public float minJumpForce = 4f;

    [BoxGroup("Jump Settings")]
    [SerializeField, Tooltip("Force maximale de saut.")]
    public float maxJumpForce = 9f;

    [BoxGroup("Slide Settings"), SerializeField, Tooltip("La vitesse de quand on reste sur un mur et qu'on glisse.")]
    public float wallSlideSpeed = 2f;

    [BoxGroup("Wall Jump Settings"), SerializeField, Tooltip("Le delay pour que le joueur puisse a nouveau sauter.")]
    public float wallJumpCoyoteTime = 0.2f;

    [BoxGroup("Wall Jump Settings"), SerializeField, Tooltip("La force du wallJump")]
    public float wallJumpForce = 1.2f;

    [BoxGroup("Dash sol Settings"), SerializeField, Tooltip("La force appliqu�e lors d'un dash au sol.")]
    public float groundDashForce = 10f;

    [BoxGroup("Dash sol Settings"), SerializeField, Tooltip("cooldown entre 2 dash au sol ?")]
    public bool groundDashCooldown = false ;    
    
    [BoxGroup("Dash sol Settings"), SerializeField, Tooltip("Le cooldown entre 2 dash au sol.")]
    public float groundDashCooldownTime = 0.2f;

    [BoxGroup("Dash air Settings"), SerializeField, Tooltip("La force appliqu�e lors d'un dash en l'air.")]
    public float airDashForce = 10f;

    [BoxGroup("Dash air Settings"), SerializeField, Tooltip("Un petit multiplicateur qui donne un boost a chaque rebond contre qqch.")]
    public float rebounceBoostMultiplier = 1.1f;

    [BoxGroup("Dash air Settings"), SerializeField, Tooltip("Le temps avant que le 'rebond' se fini et on rechute'")]
    public float reboundTimer = 1f;

    [BoxGroup("Dash air Settings"), SerializeField, Tooltip("Le temps qu'on peux appuier pour rebondir contre un mur etc")]
    public float reboundInputWindow = 1f;

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
    }
}
