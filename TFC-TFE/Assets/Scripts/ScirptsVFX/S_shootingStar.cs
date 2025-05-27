using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_shootingStar : MonoBehaviour
{
    private Vector3 direction = new Vector3(0, -1, -1).normalized;
    private float speed;
    private float delay = 15f;
    private bool hasPassedBottom = false;
    [SerializeField] GameObject endVFX;

    public void Initialize(float moveSpeed)
    {
        speed = moveSpeed;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        StartCoroutine(DestroyAfterDelay());
    }
    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Instantiate(endVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
