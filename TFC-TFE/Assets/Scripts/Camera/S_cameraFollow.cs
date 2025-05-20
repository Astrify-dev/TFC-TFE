using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_cameraFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _player;

    [Header("Camera Settings")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Vector3 _position = new Vector3(50, 15, 0);
    [SerializeField] private float _distanceSpeed = 1f;

    [Header("Ground Detection")]
    [Tooltip("Layer utilisé pour détecter le sol.")]
    [SerializeField] private LayerMask _groundLayer;

    [Tooltip("Distance max du raycast (affecte aussi le gizmo).")]
    [SerializeField] private float _rayLength = 100f;

    [Tooltip("Distance max pour considérer que le joueur est 'au sol'.")]
    [SerializeField] private float _groundThreshold = 0.5f;

    [Tooltip("Distance à partir de laquelle l'offset Y atteint 0.")]
    [SerializeField] private float _airMaxDistance = 5f;

    // Debug info
    private Vector3 _lastTargetPosition;
    private float _lastRayDistance;
    private float _currentOffsetY;

    private void FixedUpdate()
    {
        if (_player == null) return;

        Vector3 playerPos = _player.transform.position;
        Vector3 dynamicOffset = _position;

        RaycastHit hit;
        if (Physics.Raycast(playerPos, Vector3.down, out hit, _rayLength, _groundLayer))
        {
            _lastRayDistance = hit.distance;

            if (hit.distance <= _groundThreshold)
            {
                dynamicOffset.y = _position.y;
            }
            else
            {
                float t = Mathf.Clamp01((hit.distance - _groundThreshold) / (_airMaxDistance - _groundThreshold));
                dynamicOffset.y = Mathf.Lerp(_position.y, 0f, t);
                Debug.Log($"<color=cyan>Dist sol: {hit.distance:F2} | t = {t:F2} | OffsetY = {dynamicOffset.y:F2}</color>");
            }
        }

        _currentOffsetY = dynamicOffset.y;

        Vector3 targetPos = playerPos + dynamicOffset;
        _lastTargetPosition = targetPos;

        float moveStep = Vector3.Distance(transform.position, targetPos) * _distanceSpeed * _speed * Time.deltaTime;
        Vector3 moveTo = Vector3.MoveTowards(transform.position, targetPos, moveStep);

        transform.position = new Vector3(transform.position.x, moveTo.y, moveTo.z);
    }

    private void OnDrawGizmos()
    {
        if (_player != null)
        {
            Vector3 playerPos = _player.transform.position;

            // Raycast vers le sol (rouge)
            Gizmos.color = Color.red;
            Gizmos.DrawLine(playerPos, playerPos + Vector3.down * _rayLength);

            // Position cible (vert)
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_lastTargetPosition, 0.3f);

            // Ligne de l’offset actuel (bleu)
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(playerPos, playerPos + Vector3.up * _currentOffsetY);

#if UNITY_EDITOR
            // Affiche la distance au sol (jaune)
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.Label(playerPos + Vector3.up * 2f, $"Dist sol: {_lastRayDistance:F2}");

            // Affiche l’offset dynamique (cyan)
            UnityEditor.Handles.color = Color.cyan;
            UnityEditor.Handles.Label(playerPos + Vector3.up * (_currentOffsetY + 1f), $"OffsetY: {_currentOffsetY:F2}");
#endif
        }
    }
}
