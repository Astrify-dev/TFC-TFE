using UnityEngine;

public class S_cameraFollow : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject _player;

    [Header("Movement")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _distanceSpeed = 1f;

    [Header("Camera Offset")]
    [SerializeField] private Vector3 _baseOffset;

    [Header("Level Boundaries (Y axis)")]
    [SerializeField] private float _minHeight = -10f;
    [SerializeField] private float _maxHeight = 50f;
    [SerializeField] private float _triggerDistance = 5f;

    private float _fixedX = 50f;

    private Vector3 _lastTargetPosition;

    private void FixedUpdate()
    {
        if (!_player) return;

        float distance = Vector3.Distance(_player.transform.position, transform.position);
        distance *= _distanceSpeed;

        float playerY = _player.transform.position.y;
        float offsetY = CalculateDynamicOffsetY(playerY);

        Vector3 dynamicOffset = new Vector3(0f, offsetY, _baseOffset.z);
        Vector3 targetPosition = _player.transform.position + dynamicOffset;
        _lastTargetPosition = targetPosition;

        Vector3 cameraPosition = Vector3.MoveTowards(transform.position, targetPosition, distance * _speed * Time.deltaTime);
        transform.position = new Vector3(_fixedX, cameraPosition.y, cameraPosition.z);
    }

    private float CalculateDynamicOffsetY(float playerY)
    {
        // Vers le bas
        if (playerY < _minHeight + _triggerDistance)
        {
            float t = Mathf.InverseLerp(_minHeight, _minHeight + _triggerDistance, playerY);
            return Mathf.Lerp(_baseOffset.y, 0f, t);
        }
        // Vers le haut
        else if (playerY > _maxHeight - _triggerDistance)
        {
            float t = Mathf.InverseLerp(_maxHeight, _maxHeight - _triggerDistance, playerY);
            return Mathf.Lerp(-_baseOffset.y, 0f, t);
        }

        // Entre les deux, pas de décalage
        return 0f;
    }

    private void OnDrawGizmos()
    {
        if (!_player) return;

        Color levelColor = new Color(0f, 1f, 0f, 0.3f);
        Color triggerColor = new Color(1f, 1f, 0f, 0.3f);
        Color targetColor = new Color(0f, 0.5f, 1f, 1f);

        // --- BORNES DU NIVEAU SUR Z ---
        Gizmos.color = levelColor;
        Gizmos.DrawLine(new Vector3(_fixedX, _minHeight, -1000), new Vector3(_fixedX, _minHeight, 1000)); // Min Y
        Gizmos.DrawLine(new Vector3(_fixedX, _maxHeight, -1000), new Vector3(_fixedX, _maxHeight, 1000)); // Max Y

        // --- ZONES DE TRANSITION SUR Z ---
        Gizmos.color = triggerColor;
        Gizmos.DrawLine(new Vector3(_fixedX, _minHeight + _triggerDistance, -1000), new Vector3(_fixedX, _minHeight + _triggerDistance, 1000));
        Gizmos.DrawLine(new Vector3(_fixedX, _maxHeight - _triggerDistance, -1000), new Vector3(_fixedX, _maxHeight - _triggerDistance, 1000));

        // --- OFFSET ACTUEL ---
        float dynamicOffsetY = CalculateDynamicOffsetY(_player.transform.position.y);
        Vector3 offsetPosition = _player.transform.position + new Vector3(0f, dynamicOffsetY, _baseOffset.z);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(offsetPosition, 0.3f);

        // --- POSITION CIBLE CAMERA ---
        Gizmos.color = targetColor;
        Gizmos.DrawLine(_player.transform.position, offsetPosition);
        Gizmos.DrawWireSphere(_lastTargetPosition, 0.5f);
    }
}