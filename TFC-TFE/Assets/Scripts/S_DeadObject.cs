using UnityEngine;

public class S_DeadObject : MonoBehaviour{
    [Header("Paramètres de mort")]
    [SerializeField] private bool isGameOverOnDeath = false;
    [SerializeField] private S_CheckpointManager checkpointManager;
    private void Start(){
        checkpointManager.RespawnPlayer();
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.collider.CompareTag("Player")){
            Debug.Log("Game Over !");
            S_playerManagerStates playerManagerStates = collision.collider.GetComponent<S_playerManagerStates>();
            if (playerManagerStates is not null){
                if (isGameOverOnDeath){
                    playerManagerStates.SwitchState(playerManagerStates.DeadState);
                }else{
                    checkpointManager.RespawnPlayer();
                }

                S_deathAlongueSpline.RestDeadZone?.Invoke();
                S_TimerSpeedrun.OnPlayerDeath?.Invoke();
                S_CanvasEnd.OnPlayerDeath?.Invoke();
            }
        }
    }
}
