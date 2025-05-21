using System;
using UnityEngine;

public class S_ennemyAttack : MonoBehaviour{
    [Header("Zone d'attaque")]
    [Tooltip("Le BoxCollider utilisé pour détecter le joueur.")]
    public BoxCollider areAttack;

    public event Action<Transform> OnPlayerEnter; 
    public event Action OnPlayerExit; 

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            Debug.Log("Le joueur est entré dans la zone d'attaque !");
            OnPlayerEnter?.Invoke(other.transform);
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.CompareTag("Player")){
            Debug.Log("Le joueur est sorti de la zone d'attaque !");
            OnPlayerExit?.Invoke();
        }
    }

    private void OnDrawGizmos(){
        if (areAttack is not null){
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(areAttack.transform.position + areAttack.center, areAttack.size);
        }
    }
}
