using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "ScriptableObjects/PlayerMovementSettings", order = 1)]
public class PlayerMovementSettings : ScriptableObject {
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

    [field: BoxGroup("Movement Settings")]
    [field: SerializeField, Tooltip("La force acceleratrice de plonger du joueur.")]
    public float FallSpeed { get; private set; } = 5f;

    [BoxGroup("Movement Settings")]
    [SerializeField, Tooltip("La force acceleratrice plonger max du joueur.")]
    public float MaxFallSpeed = 5f;

    [BoxGroup("Movement Settings")]
    [SerializeField, Tooltip("La velocite max de chut du joueur.")]
    public float MaxBottomVelocity = 50f;

    [BoxGroup("Movement Settings"), SerializeField, Tooltip("Vitesse minimale du joueur (vitesse de départ).")]
    public float minMoveSpeed = 5f;

    [BoxGroup("Movement Settings"), SerializeField, Tooltip("Vitesse maximale du joueur.")]
    public float maxMoveSpeed = 10f;

    [BoxGroup("Movement Settings"), SerializeField, Tooltip("Vitesse à laquelle on atteint la vitesse max.")]
    public float accelerationRate = 20f;

    [BoxGroup("Air Movement Settings")]
    [SerializeField, Tooltip("Activer l'air control.")]
    public bool AirControlEnable = true;

    [BoxGroup("Air Movement Settings")]
    [SerializeField, Tooltip("Vitesse max dans les airs.")]
    public float airMaxMoveSpeed  = 6f;

    [BoxGroup("Air Movement Settings")]
    [SerializeField, Tooltip("Taux d'accélération dans les airs.")]

    public float airAccelerationRate = 10f;
    [field: BoxGroup("Air Movement Settings")]
    [field: SerializeField, Tooltip("Vitesse de chute normale quand on ne plonge pas.")]
    public float Dive { get; private set; } = 3f;

    [field: BoxGroup("Jump Settings")]
    [field: SerializeField, Tooltip("Force minimale de saut.")]
    public float minJumpForce { get; private set; } = 4f;

    [field:BoxGroup("Jump Settings")]
    [field:SerializeField, Tooltip("Force maximale de saut.")]
    public float maxJumpForce { get; private set; } = 9f;

    [field:BoxGroup("Jump Settings")]
    [field:SerializeField, Tooltip("Temps max de charge du saut.")]
    public float jumpChargeTime { get; private set; } = 0.25f;

    [field:BoxGroup("Jump Settings")]
    [field: SerializeField, Tooltip("Repartition de la force durant le saut.")]
    public AnimationCurve jumpCurve { get; private set; }

    [field: BoxGroup("Slide Settings"), SerializeField, Tooltip("La vitesse de quand on reste sur un mur et qu'on glisse.")]
    public float wallSlideSpeed { get; private set; } = 2f;

    [field: BoxGroup("Wall Jump Settings"), SerializeField, Tooltip("La force du wallJump")]
    public float wallJumpForce { get; private set; } = 1.2f;

    [field: BoxGroup("Wall Jump Settings"), SerializeField, Tooltip("La force du wallJump")]
    public Vector2 directionImpulsion { get; private set; }

    [field: BoxGroup("Wall Jump Settings"), SerializeField, Tooltip("Temps avant qu'on puisse se racrocher sur un mur apres un wallJump")]
    public float SlideWallCooldownTimeAfterWallJump { get; private set; }

    [field: BoxGroup("Dash sol Settings"), SerializeField, Tooltip("La force appliquée lors d'un dash au sol.")]
    public float groundDashForce { get; private set; } = 10f;

    [field: BoxGroup("Dash sol Settings"), SerializeField, Tooltip("cooldown entre 2 dash au sol ?")]
    public bool groundDashCooldown { get; private set; } = false;

    [field: BoxGroup("Dash sol Settings"), SerializeField, Tooltip("Le cooldown entre 2 dash au sol.")]
    public float groundDashCooldownTime { get; private set; } = 0.2f;

    [field: BoxGroup("Dash air Settings"), SerializeField, Tooltip("La force appliquée lors d'un dash en l'air.")]
    public float airDashForce { get; private set; } = 10f ;

    [field: BoxGroup("Dash air Settings"), SerializeField, Tooltip("Le temps avant que le 'rebond' se fini et on rechute'")]
    public float reboundTimer { get; private set; } = 1f;

    [field: BoxGroup("Dash air Settings"), SerializeField, Tooltip("Le temps qu'on peux appuier pour rebondir contre un mur etc")]
    public float reboundInputWindow { get; private set; } = 1f;

    [field: BoxGroup("Dash air Settings"), SerializeField, Tooltip("Duree du AirDash.")]
    public float AirDashDuration { get; private set; } = 2;

    [field: BoxGroup("Dash air Settings"), SerializeField, Tooltip("Frenage du dash apres celui-ci.")]
    public int BrakeAirDashPower { get; private set; } = 4;

    [field: BoxGroup("Dash air Settings"), SerializeField, Tooltip("Correction de la capsule pour le top et le bottom.")]
    public float ReboundCorrectionValueCapsule { get; private set; } = 2;

    [field: BoxGroup("Dash air Settings"), SerializeField, Tooltip("Maximum du nombre de Dash en generale.")]
    public int MaxAirDashCount { get; private set; } = 1;

    [field: BoxGroup("Dash air Settings"), SerializeField, Tooltip("Le nombre de Dash quand on touche le sol.")]
    public int GroundAirDashCount { get; private set; } = 1;

    [field: BoxGroup("Dash air Settings"), SerializeField, Tooltip("Le nombre de Dash Maximum qui sont donné par le WallDash.")]
    public int WallDashMaxAirDashCount { get; private set; } = 1;

    [BoxGroup("Dash Jump Settings"), SerializeField, Tooltip("Direction de l'impulsion pour un saut pendant un dash")]
    public Vector2 dashJumpDirection = new Vector2(1f, 1f); // X = horizontal (Z), Y = vertical

    [BoxGroup("Dash Jump Settings"), SerializeField, Tooltip("Force appliquée pour un saut pendant un dash")]
    public float dashJumpForce = 1.2f;

    [BoxGroup("Slow Motion Settings")]
    [SerializeField, Tooltip("L'intensité du slow motion.")]
    public float slowMotionIntensity = 0.5f;

    [BoxGroup("Slow Motion Settings"), SerializeField, Tooltip("La durée du slow motion.")]
    public float slowMotionTimer = 10f;

    [field: BoxGroup("Slow Motion Settings")]
    [field: SerializeField, Tooltip("Repartition du TimeScale durant l'activation du SlowMotion")]
    public AnimationCurve SlowMotionAnimEnableCurve {  get; private set; }

    [field: BoxGroup("Slow Motion Settings")]
    [field: SerializeField, Tooltip("Duree de l'animation du SlowMotion")]
    public float SlowMotionAnimSpeed { get; private set; } = 10;

    [BoxGroup("Collision Settings"), SerializeField, Tooltip("Couches permettant de resauter.")]
    public LayerMask jumpResetLayers;

    [BoxGroup("Collision Settings"), SerializeField, Tooltip("Couches permettant de restaurer le dash.")]
    public LayerMask dashResetLayers;

    [BoxGroup("Collision Settings"), SerializeField, Tooltip("Couches qui permettent le rebond pendant un dash aérien.")]
    public LayerMask bounceLayers;

    [BoxGroup("Collision Settings"), SerializeField, Tooltip("Couches autorisant wall slide et wall jump.")]
    public LayerMask wallJumpLayers;

    [field: BoxGroup("Juice"), SerializeField, Tooltip("Valeur min de down velocity pour declencher un rebond")]
    public float JuiceFallVelocityMinRebounds { get; private set; } = 30f;

    [field: BoxGroup("Juice"), SerializeField, Tooltip("Multiplicateur de la down velocity pour le rebond")]
    public float JuiceMultiplyerFallVelocityRebounds { get; private set; } = 1.1f;
    public void ApplySettingsToRigidbody(Rigidbody rb){
        if (rb is null) return;

        rb.mass = rigidbodyMass;
        rb.drag = rigidbodyDrag;
        rb.angularDrag = rigidbodyAngularDrag;
        rb.useGravity = useGravity;
    }
}
