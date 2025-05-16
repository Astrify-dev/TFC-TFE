using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MovingPlatform : MonoBehaviour
{
    [Header("Structure")]
    [SerializeField] private Transform movingPart;  // Ex: "Plateforme"
    [SerializeField] private Transform pointsRoot;  // Ex: "POINT"

    [Header("Mouvement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTimeAtPoints = 0.5f;
    [SerializeField] private bool loop = true;

    private List<Transform> waypoints = new();
    private int currentIndex = 0;
    private bool movingForward = true;

    private Rigidbody movingRigidbody;
    private Vector3 lastPosition;
    private Vector3 platformVelocity;

    private void Awake()
    {
        if (movingPart == null || pointsRoot == null)
        {
            Debug.LogError($"[{name}] movingPart ou pointsRoot n'est pas assigné !");
            enabled = false;
            return;
        }

        movingRigidbody = movingPart.GetComponent<Rigidbody>();
        if (movingRigidbody == null)
        {
            Debug.LogError($"[{name}] Le movingPart doit avoir un Rigidbody !");
            enabled = false;
            return;
        }

        // Récupérer tous les enfants de POINT
        waypoints.Clear();
        foreach (Transform child in pointsRoot)
        {
            waypoints.Add(child);
        }

        if (waypoints.Count < 2)
        {
            Debug.LogWarning($"[{name}] Il faut au moins 2 waypoints pour que la plateforme bouge.");
            enabled = false;
        }
    }

    private void Start()
    {
        movingRigidbody.position = waypoints[0].position;
        lastPosition = movingRigidbody.position;
        StartCoroutine(MoveAlongPath());
    }

    private void FixedUpdate()
    {
        platformVelocity = (movingRigidbody.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = movingRigidbody.position;
    }

    private IEnumerator MoveAlongPath()
    {
        while (true)
        {
            Vector3 target = waypoints[currentIndex].position;

            while (Vector3.Distance(movingRigidbody.position, target) > 0.05f)
            {
                Vector3 dir = (target - movingRigidbody.position).normalized;
                Vector3 newPos = movingRigidbody.position + dir * moveSpeed * Time.fixedDeltaTime;
                movingRigidbody.MovePosition(newPos);
                yield return new WaitForFixedUpdate();
            }

            movingRigidbody.position = target;
            yield return new WaitForSeconds(waitTimeAtPoints);

            if (loop)
            {
                currentIndex = (currentIndex + 1) % waypoints.Count;
            }
            else
            {
                if (movingForward)
                {
                    currentIndex++;
                    if (currentIndex >= waypoints.Count)
                    {
                        currentIndex = waypoints.Count - 2;
                        movingForward = false;
                    }
                }
                else
                {
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

    public Vector3 GetPlatformVelocity()
    {
        return platformVelocity;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (pointsRoot == null) return;

        Gizmos.color = Color.cyan;
        foreach (Transform point in pointsRoot)
        {
            Gizmos.DrawSphere(point.position, 0.1f);
        }

        Gizmos.color = Color.yellow;
        Transform prev = null;
        foreach (Transform point in pointsRoot)
        {
            if (prev != null)
                Gizmos.DrawLine(prev.position, point.position);
            prev = point;
        }

        if (loop && pointsRoot.childCount > 1)
        {
            Gizmos.DrawLine(pointsRoot.GetChild(pointsRoot.childCount - 1).position, pointsRoot.GetChild(0).position);
        }
    }
#endif
}
