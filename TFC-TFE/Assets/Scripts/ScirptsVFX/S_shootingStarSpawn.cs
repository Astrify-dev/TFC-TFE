using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_shootingStarSpawn : MonoBehaviour
{
    [SerializeField] private GameObject shootingStarPrefab;
    [SerializeField] private float repeatRate = 1.5f;
    [SerializeField] private float time = 1f;
    [SerializeField] private float rangeXNegative = -5f;
    [SerializeField] private float rangeXPositive = 5f;
    [SerializeField] private float speedMin = 3f;
    [SerializeField] private float speedMax = 6f;
    private float addY = 5f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        InvokeRepeating(nameof(SpawnShootingStar), 1f, repeatRate);
    }

    void SpawnShootingStar()
    {
        Vector3 topOfScreen = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height + addY, 10f));

        float randomX = Random.Range(rangeXNegative, rangeXPositive);
        Vector3 spawnPos = new Vector3(topOfScreen.x + randomX, topOfScreen.y + 1f, 0);

        GameObject star = Instantiate(shootingStarPrefab, spawnPos, Quaternion.identity);
        float speed = Random.Range(speedMin, speedMax);
        star.AddComponent<S_shootingStar>().Initialize(speed);
    }
}
