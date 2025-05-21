using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType{
    Dash
}

[CreateAssetMenu(fileName = "NewPickup", menuName = "ScriptableObjects/Pickup")]
public class SO_Pickup : ScriptableObject{
    public PickupType pickupType;
    public int NbRecup=1;
    public void Awake(){
        
    }
    public void ExecutePickup(S_playerManagerStates playerManagerStates){
        switch (pickupType){
            case PickupType.Dash:
                Debug.Log("Tu as regagné un 'Dash' !");
                playerManagerStates.AddAirDash(NbRecup);
                break;
            default:
                Debug.Log("Pickup inconnu !");
                break;
        }
    }
}
