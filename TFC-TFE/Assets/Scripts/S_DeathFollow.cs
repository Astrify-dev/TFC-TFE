using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class S_DeathFollow : MonoBehaviour{
    [Header("Structure")]
    [SerializeField] private Transform movingPart;
    [SerializeField] private Transform pointsRoot;

    [Header("Mouvement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTimeAtPoints = 0.5f;
    [SerializeField] private bool loop = true;

    public static event Action OnPlayerDead;

    private List<Transform> waypoints = new();
    private int currentIndex = 0;
    private bool movingForward = true;

    private Rigidbody movingRigidbody;

    private void Awake(){
        if (movingPart is null || pointsRoot is null){
            enabled = false;
            return;
        }

        movingRigidbody = movingPart.GetComponent<Rigidbody>();
        if (movingRigidbody is null){
            enabled = false;
            return;
        }

        waypoints.Clear();
        foreach (Transform child in pointsRoot){
            waypoints.Add(child);
        }

        if (waypoints.Count < 2){
            enabled = false;
        }
    }

    private void Start(){
        movingRigidbody.position = waypoints[0].position;
        StartCoroutine(MoveAlongPath());
    }

    private IEnumerator MoveAlongPath(){
        while (true){
            Vector3 target = waypoints[currentIndex].position;

            while (Vector3.Distance(movingRigidbody.position, target) > 0.05f){
                Vector3 dir = (target - movingRigidbody.position).normalized;
                Vector3 newPos = movingRigidbody.position + dir * moveSpeed * Time.fixedDeltaTime;

                // Calcul de la rotation en fonction de la direction
                float angleX = Mathf.Atan2(dir.y, dir.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(angleX, 0, 0);
                movingRigidbody.MoveRotation(targetRotation);

                movingRigidbody.MovePosition(newPos);
                yield return new WaitForFixedUpdate();
            }

            movingRigidbody.position = target;
            yield return new WaitForSeconds(waitTimeAtPoints);

            if (loop){
                currentIndex = (currentIndex + 1) % waypoints.Count;
            }else{
                if (movingForward){
                    currentIndex++;
                    if (currentIndex >= waypoints.Count)
                    {
                        currentIndex = waypoints.Count - 2;
                        movingForward = false;
                    }
                }else{
                    currentIndex--;
                    if (currentIndex < 0)
                    {
                        currentIndex = 1;
                        movingForward = true;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player")){
            Debug.Log("GameOver");
            OnPlayerDead?.Invoke();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos(){
        if (pointsRoot is null) return;

        Gizmos.color = Color.red;
        foreach (Transform point in pointsRoot){
            Gizmos.DrawSphere(point.position, 0.1f);
        }

        Gizmos.color = Color.yellow;
        Transform prev = null;
        foreach (Transform point in pointsRoot){
            if (prev != null)
                Gizmos.DrawLine(prev.position, point.position);
            prev = point;
        }

        if (loop && pointsRoot.childCount > 1){
            Gizmos.DrawLine(pointsRoot.GetChild(pointsRoot.childCount - 1).position, pointsRoot.GetChild(0).position);
        }
    }
#endif
}
