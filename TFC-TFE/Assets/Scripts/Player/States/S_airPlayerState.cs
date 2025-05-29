using Cinemachine;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class S_airPlayerState : S_basePlayerStates
{
    //private bool _justEntered = false;

    //public override void EnterState(S_playerStates Player)
    //{
    //    Player.IsGrounded = false;
    //    Player.CanJump = false;
    //    _justEntered = true;
    //}

    //public override void OnEnable(S_playerStates Player) { }

    //public override void OnDisable(S_playerStates Player) { }

    //public override void UpdateState(S_playerStates Player)
    //{
    //    if (Player.MovementLockTimer > 0f)
    //    {
    //        Player.MovementLockTimer -= Time.deltaTime;
    //        return; // Ne rien faire tant que le saut est verrouillé
    //    }

    //    if (Player.IsDashing){
    //        return;
    //    }

    //    if (_justEntered)
    //    {
    //        _justEntered = false;
    //        return;
    //    }

    //    if (Player.CheckGrounded())
    //    {
    //        Player.SwitchState(Player.GroundState);
    //        return;
    //    }

    //    if (Player.CheckWall())
    //    {
    //        Player.SwitchState(Player.SlideWallState);
    //        return;
    //    }

    //    // ❗ Appliquer air control uniquement si non verrouillé
    //    if (Player.AirControlLockTimer <= 0f)
    //    {
    //        float targetSpeed = Player.MoveInput.x * Player.Settings.airMaxMoveSpeed;
    //        Vector3 velocity = Player.Rigidbody.velocity;
    //        velocity.z = Mathf.MoveTowards(velocity.z, targetSpeed, Player.Settings.airAccelerationRate * Time.deltaTime);
    //        Player.Rigidbody.velocity = velocity;
    //    }


    //    Player.HandleFlip(Player.MoveInput.x);

    //    // Dive ou chute
    //    if (Player.MoveInput.y < 0 && !Player.IsWallSliding){
    //        Vector3 diveForce = new Vector3(0, Player.MoveInput.y, 0) * Player.Settings.dive;
    //        Player.Rigidbody.AddForce(diveForce, ForceMode.Acceleration);
    //    }else{
    //        Vector3 vel = Player.Rigidbody.velocity;

    //        vel.y -= Player.Settings.fallSpeed * Time.deltaTime;

    //        vel.y = Mathf.Max(vel.y, -Player.Settings.fallSpeed);

    //        Player.Rigidbody.velocity = vel;
    //    }

    private S_playerManagerStates _player;
    private float _fallSpeed;

    public override void EnterState(S_playerManagerStates Player)
    {
        _player = Player;
        _fallSpeed = 0;

        

    }
    public override void OnEnable(S_playerManagerStates Player)
    {
        Player.Inputs.OnDashEvent += Inputs_OnDashEvent;
    }
    public override void OnDisable(S_playerManagerStates Player)
    {
        Player.Inputs.OnDashEvent -= Inputs_OnDashEvent;
    }
    public override void UpdateState(S_playerManagerStates Player)
    {
        if (Player.CheckGrounded())
        {
            if(!GroundRebound(Player.Rigidbody.velocity.y))
                Player.SwitchState(Player.GroundState);
            return;
        }

        if (Player.EnableWallSlide && Player.CheckWall(Player.WallCheckDistance))
        {
            Player.SwitchState(Player.SlideWallState);
            return;
        }

        Player.HandleFlip(Player.DirectionInput.x);

        Vector2 AirControlDir = Player.DirectionInput.normalized;
        AirControlDir.y = Mathf.Min(AirControlDir.y, 0) * Player.MovementSettings.Dive;
        AirControlDir.x *= Player.MovementSettings.airMaxMoveSpeed;

        Vector3 VelocityDir = new Vector3(0, AirControlDir.y, AirControlDir.x);

        if (Player.MovementSettings.AirControlEnable)
            Player.Rigidbody.AddForce(VelocityDir * Time.deltaTime, ForceMode.Acceleration);

        _fallSpeed += Player.MovementSettings.FallSpeed * Time.deltaTime;
        _fallSpeed = Mathf.Min(_fallSpeed, Player.MovementSettings.MaxFallSpeed);
        Player.Rigidbody.AddForce(Vector3.down * _fallSpeed * Time.deltaTime, ForceMode.Acceleration);
        Player.Rigidbody.velocity = new Vector3(0, Mathf.Max(Player.Rigidbody.velocity.y, -Player.MovementSettings.MaxBottomVelocity), Player.Rigidbody.velocity.z);

    }



    private void Inputs_OnDashEvent()
    {
        if(_player.AirDashCount > 0)
            _player.SwitchState(_player.SlowMotionDashState);
    }

    private bool GroundRebound(float VelocityFall)
    {

        float MinRebounds = _player.MovementSettings.JuiceFallVelocityMinRebounds;
        float MultiplyRebounds = _player.MovementSettings.JuiceMultiplyerFallVelocityRebounds;

        if (VelocityFall < -MinRebounds)
        {
            _player.Rigidbody.AddForce(Vector3.up * -VelocityFall * MultiplyRebounds, ForceMode.Impulse);
            _player.SwitchState(_player.AirState);
            return true;
        }
        return false;
    }
}

