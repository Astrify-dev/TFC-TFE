using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PickUpSystem : MonoBehaviour{
    public SO_Pickup pickupScriptable;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if (pickupScriptable is not null){
                S_playerManagerStates playerManagerStates = other.GetComponent<S_playerManagerStates>();
                pickupScriptable.ExecutePickup(playerManagerStates);
            }
            Destroy(gameObject);
        }
    }
}
