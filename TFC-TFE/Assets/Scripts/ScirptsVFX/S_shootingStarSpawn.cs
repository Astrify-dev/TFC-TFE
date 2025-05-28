using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_shootingStarSpawn : MonoBehaviour
{
    [SerializeField] private GameObject shootingStarPrefab;
    [SerializeField] private float repeatRate = 1.5f;
    [SerializeField] private float speedMin = 3f;
    [SerializeField] private float speedMax = 6f;

    [SerializeField] Vector2 squareSize = new Vector2(5f, 5f);
    [SerializeField] Vector3 centerPosition = Vector3.zero;

    void Start()
    {
        InvokeRepeating(nameof(SpawnShootingStar), 1f, repeatRate);
    }

    void SpawnShootingStar()
    {

        float randomY = Random.Range(centerPosition.y - squareSize.x / 2f, centerPosition.y + squareSize.x / 2f);
        float randomZ = Random.Range(centerPosition.z - squareSize.y / 2f, centerPosition.z + squareSize.y / 2f);

        Vector3 spawnPos = new Vector3(-15, randomY, randomZ);

        GameObject star = Instantiate(shootingStarPrefab, spawnPos, Quaternion.identity);

        float speed = Random.Range(speedMin, speedMax);

        star.GetComponent<S_shootingStar>().Initialize(speed);
    } 
}
