using UnityEngine;

public class SmoothCameraWithWallsZY : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Camera mainCamera;

    [Header("Offset Settings")]
    public Vector2 startOffset = new Vector2(-4f, -3f); // Z (horizontal), Y (vertical)
    public Vector2 followOffset = new Vector2(0f, 0f);
    public float transitionDuration = 2f;

    [Header("Smoothing")]
    public float smoothTime = 0.3f;

    [Header("Collision Bounds")]
    public Vector2 cameraSize = new Vector2(10f, 6f); // Width (Z), Height (Y)
    public LayerMask cameraWallLayer;

    private float transitionTimer;
    private Vector3 velocity = Vector3.zero;

    private Vector2 currentMin = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
    private Vector2 currentMax = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

    void LateUpdate()
    {
        if (target == null) return;

        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / transitionDuration);
        Vector2 offset = Vector2.Lerp(startOffset, followOffset, t);

        float desiredZ = target.position.z + offset.x;
        float desiredY = target.position.y + offset.y;

        DetectCameraConstraints();

        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;

        // ✅ Inversion de logique : bord gauche colle au bord gauche du mur, etc.
        float minZ = currentMin.x - halfWidth;
        float maxZ = currentMax.x + halfWidth;
        float minY = currentMin.y - halfHeight;
        float maxY = currentMax.y + halfHeight;

        float clampedZ = Mathf.Clamp(desiredZ, minZ, maxZ);
        float clampedY = Mathf.Clamp(desiredY, minY, maxY);


        Vector3 targetPosition = new Vector3(
            transform.position.x, // Caméra latérale (X figé)
            clampedY,
            clampedZ
        );

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void DetectCameraConstraints()
    {
        // ❗ Zone de détection toujours à X = 0 (là où sont les colliders)
        Vector3 boxCenter = new Vector3(
            0f,
            target.position.y,
            target.position.z
        );

        Vector3 halfExtents = new Vector3(0.5f, cameraSize.y / 2f, cameraSize.x / 2f);

        Collider[] hits = Physics.OverlapBox(
            boxCenter,
            halfExtents,
            Quaternion.identity,
            cameraWallLayer
        );

        // Reset des bornes
        currentMin = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
        currentMax = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

        foreach (var col in hits)
        {
            Bounds b = col.bounds;

            float minZ = b.min.z;
            float maxZ = b.max.z;
            float minY = b.min.y;
            float maxY = b.max.y;

            // Le mur le plus à gauche (pour le bord droit de la caméra)
            if (minZ > currentMin.x) currentMin.x = minZ;
            // Le mur le plus à droite (pour le bord gauche de la caméra)
            if (maxZ < currentMax.x) currentMax.x = maxZ;

            if (minY > currentMin.y) currentMin.y = minY;
            if (maxY < currentMax.y) currentMax.y = maxY;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Gizmos.color = Color.green;

        // 🎯 On affiche la zone de détection (à X = 0)
        Vector3 boxCenter = new Vector3(
            0f,
            target.position.y,
            target.position.z
        );

        Vector3 size = new Vector3(1f, cameraSize.y, cameraSize.x);
        Gizmos.DrawWireCube(boxCenter, size);
    }
#endif
}
