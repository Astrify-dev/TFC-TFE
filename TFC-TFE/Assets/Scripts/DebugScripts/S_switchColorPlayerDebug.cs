using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_switchColorPlayerDebug : MonoBehaviour
{
    [SerializeField] S_playerManagerStates _playerState;
    [SerializeField] Color _playerColorNormal;
    [SerializeField] Color _playerColorDash;
    [SerializeField] Color _playerColorRebounds;
    [SerializeField] Material _playerMaterial;

    // Update is called once per frame
    void Update()
    {
        if(_playerState.CurrentState == _playerState.AirDashState)
        {
            _playerMaterial.SetColor("_BaseColor", _playerColorDash);
            return;
        }

        if ( _playerState.CurrentState == _playerState.WallReboundState)
        {
            _playerMaterial.SetColor("_BaseColor", _playerColorRebounds);
            return;
        }

        _playerMaterial.SetColor("_BaseColor", _playerColorNormal);
        
       
    }
}
