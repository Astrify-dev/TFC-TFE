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

    [BoxGroup("Movement Settings")]
    [SerializeField, Tooltip("La vitesse de déplacement du joueur.")]
    public float moveSpeed = 5f;

    [BoxGroup("Movement Settings")]
    [SerializeField, Tooltip("La force de plonger du joueur.")]
    public float dive = 5f;

    [BoxGroup("Movement Settings"), SerializeField, Tooltip("Vitesse minimale du joueur (vitesse de départ).")]
    public float minMoveSpeed = 5f;

    [BoxGroup("Movement Settings"), SerializeField, Tooltip("Vitesse maximale du joueur.")]
    public float maxMoveSpeed = 10f;

    [BoxGroup("Movement Settings"), SerializeField, Tooltip("Vitesse à laquelle on atteint la vitesse max.")]
    public float accelerationRate = 20f;

    [BoxGroup("Air Movement Settings")]
    [SerializeField, Tooltip("Vitesse max dans les airs.")]
    public float airMaxMoveSpeed = 6f;

    [BoxGroup("Air Movement Settings")]
    [SerializeField, Tooltip("Taux d'accélération dans les airs.")]

    public float airAccelerationRate = 10f;
    [BoxGroup("Air Movement Settings")]
    [SerializeField, Tooltip("Vitesse de chute normale quand on ne plonge pas.")]
    public float fallSpeed = 3f;

    [BoxGroup("Jump Settings")]
    [SerializeField, Tooltip("Force minimale de saut.")]
    public float minJumpForce = 4f;

    [BoxGroup("Jump Settings")]
    [SerializeField, Tooltip("Force maximale de saut.")]
    public float maxJumpForce = 9f;

    [BoxGroup("Jump Settings")]
    [SerializeField, Tooltip("Temps max de charge du saut.")]
    public float jumpChargeTime = 0.25f;

    [BoxGroup("Slide Settings"), SerializeField, Tooltip("La vitesse de quand on reste sur un mur et qu'on glisse.")]
    public float wallSlideSpeed = 2f;

    [BoxGroup("Wall Jump Settings"), SerializeField, Tooltip("La force du wallJump")]
    public float wallJumpForce = 1.2f;

    [BoxGroup("Wall Jump Settings"), SerializeField, Tooltip("La force du wallJump")]
    public Vector2 directionImpulsion;

    [BoxGroup("Dash sol Settings"), SerializeField, Tooltip("La force appliquée lors d'un dash au sol.")]
    public float groundDashForce = 10f;

    [BoxGroup("Dash sol Settings"), SerializeField, Tooltip("cooldown entre 2 dash au sol ?")]
    public bool groundDashCooldown = false ;    
    
    [BoxGroup("Dash sol Settings"), SerializeField, Tooltip("Le cooldown entre 2 dash au sol.")]
    public float groundDashCooldownTime = 0.2f;

    [BoxGroup("Dash air Settings"), SerializeField, Tooltip("La force appliquée lors d'un dash en l'air.")]
    public float airDashForce = 10f;

    [BoxGroup("Dash air Settings"), SerializeField, Tooltip("Un petit multiplicateur qui donne un boost a chaque rebond contre qqch.")]
    public float rebounceBoostMultiplier = 1.1f;

    [BoxGroup("Dash air Settings"), SerializeField, Tooltip("Le temps avant que le 'rebond' se fini et on rechute'")]
    public float reboundTimer = 1f;

    [BoxGroup("Dash air Settings"), SerializeField, Tooltip("Le temps qu'on peux appuier pour rebondir contre un mur etc")]
    public float reboundInputWindow = 1f;

    [BoxGroup("Dash Jump Settings"), SerializeField, Tooltip("Direction de l'impulsion pour un saut pendant un dash")]
    public Vector2 dashJumpDirection = new Vector2(1f, 1f); // X = horizontal (Z), Y = vertical

    [BoxGroup("Dash Jump Settings"), SerializeField, Tooltip("Force appliquée pour un saut pendant un dash")]
    public float dashJumpForce = 1.2f;

    [BoxGroup("Slow Motion Settings")]
    [SerializeField, Tooltip("L'intensité du slow motion.")]
    public float slowMotionIntensity = 0.5f;

    [BoxGroup("Slow Motion Settings"), SerializeField, Tooltip("La durée du slow motion.")]
    public float slowMotionTimer = 10f;

    [BoxGroup("Collision Settings"), SerializeField, Tooltip("Couches permettant de resauter.")]
    public LayerMask jumpResetLayers;

    [BoxGroup("Collision Settings"), SerializeField, Tooltip("Couches permettant de restaurer le dash.")]
    public LayerMask dashResetLayers;

    [BoxGroup("Collision Settings"), SerializeField, Tooltip("Couches qui permettent le rebond pendant un dash aérien.")]
    public LayerMask bounceLayers;

    [BoxGroup("Collision Settings"), SerializeField, Tooltip("Couches autorisant wall slide et wall jump.")]
    public LayerMask wallJumpLayers;

    public void ApplySettingsToRigidbody(Rigidbody rb){
        if (rb is null) return;

        rb.mass = rigidbodyMass;
        rb.drag = rigidbodyDrag;
        rb.angularDrag = rigidbodyAngularDrag;
        rb.useGravity = useGravity;
    }
}
