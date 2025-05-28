using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_colliderDeathUpdate : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private float _pourcentageEnable;

    private void Start()
    {
        _collider.enabled = false;
    }

    private void OnEnable()
    {
        S_deathAlongueSpline.UpdateCollider += DeathAlongueSpline_UpdateCollider;
        S_deathAlongueSpline.RestDeadZone += ResetCollider;
    }

    private void OnDisable()
    {
        S_deathAlongueSpline.UpdateCollider -= DeathAlongueSpline_UpdateCollider;
        S_deathAlongueSpline.RestDeadZone -= ResetCollider;
    }

    private void DeathAlongueSpline_UpdateCollider(float Value)
    {
        if(_pourcentageEnable < Value)
        {
            Debug.Log("EnableCollider: "+ Value);
            S_deathAlongueSpline.UpdateCollider -= DeathAlongueSpline_UpdateCollider;
            _collider.enabled = true;
        }
    }

    private void ResetCollider()
    {
        S_deathAlongueSpline.UpdateCollider -= DeathAlongueSpline_UpdateCollider;
        S_deathAlongueSpline.UpdateCollider += DeathAlongueSpline_UpdateCollider;

        _collider.enabled = false;
    }
}
