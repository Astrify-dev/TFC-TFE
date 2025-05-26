using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class S_DeadObject : MonoBehaviour{
    [Header("Paramètres de mort")]
    [SerializeField] private bool isGameOverOnDeath = false;
    [SerializeField] private S_CheckpointManager checkpointManager;
    [SerializeField] private SoundSystem _SFX_Corruption;
    [SerializeField] private bool _corruption;
    private void Start(){
        checkpointManager.RespawnPlayer();
        if (_corruption){
             if (_SFX_Corruption is not null){
                Vector3 _positionSound = new Vector3(3.5f, transform.position.y, transform.position.z);
                _SFX_Corruption.Play(_positionSound);
            }
        }

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
                StartCoroutine(InvokeCanvasEndEventWithDelay(1f));
            }
        }
    }

    private IEnumerator InvokeCanvasEndEventWithDelay(float delay){
        yield return new WaitForSeconds(delay);
        S_CanvasEnd.OnPlayerDeath?.Invoke();
    }
}
