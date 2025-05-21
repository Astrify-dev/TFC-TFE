using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_shootingStar : MonoBehaviour
{
    private Vector3 direction = new Vector3(-1, -1, 0).normalized;
    private float speed;
    private float delay = 3f;
    private Camera mainCamera;
    private bool hasPassedBottom = false;

    public void Initialize(float moveSpeed)
    {
        speed = moveSpeed;
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (!hasPassedBottom)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

            if (screenPos.y < 0)
            {
                hasPassedBottom = true;
                StartCoroutine(DestroyAfterDelay());
            }
        }
    }
    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
