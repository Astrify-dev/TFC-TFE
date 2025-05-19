using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType{
    Dash,
    Jump
}

[CreateAssetMenu(fileName = "NewPickup", menuName = "ScriptableObjects/Pickup")]
public class SO_Pickup : ScriptableObject{
    public PickupType pickupType;

    public void ExecutePickup(){
        switch (pickupType){
            case PickupType.Dash:
                Debug.Log("Tu as regagné un 'Dash' !");
                break;
            case PickupType.Jump:
                Debug.Log("Tu as regagné un 'Jump' !");
                break;
            default:
                Debug.Log("Pickup inconnu !");
                break;
        }
    }
}
