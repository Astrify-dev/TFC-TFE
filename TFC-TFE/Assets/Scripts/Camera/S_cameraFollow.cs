using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_cameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _position;
    [SerializeField] private float _distanceSpeed = 1;

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(_player.transform.position, transform.position);
        distance *= _distanceSpeed;

        Vector3 targetPosition = _player.transform.position + _position;

        Vector3 cameraPosition = Vector3.MoveTowards(transform.position, targetPosition, distance * _speed * Time.deltaTime);
        //Vector3 cameraPosition = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, cameraPosition.y, cameraPosition.z);
    }


}
