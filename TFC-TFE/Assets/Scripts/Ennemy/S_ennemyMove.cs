using UnityEngine;

public class RandomMovement : MonoBehaviour{
    [Header("Paramètres")]
    [Tooltip("L'enfant visuel à déplacer.")]
    public Transform visualChild;

    [Tooltip("L'objet contenant le BoxCollider.")]
    public BoxCollider movementArea;

    [Tooltip("Vitesse de déplacement.")]
    public float moveSpeed = 1f;

    private Vector3 targetPosition;

    void Start(){
        if (movementArea is null || visualChild is null){
           enabled = false;
            return;
        }
        SetRandomTargetPosition();
    }

    void Update(){
        if (visualChild is null || movementArea is null) return;

        visualChild.localPosition = Vector3.MoveTowards(visualChild.localPosition, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(visualChild.localPosition, targetPosition) < 0.1f){
            SetRandomTargetPosition();
        }
    }

    private void SetRandomTargetPosition(){
        Vector3 areaSize = movementArea.size;
        Vector3 areaCenter = movementArea.center;

        float randomY = Random.Range(-areaSize.y / 2, areaSize.y / 2);
        float randomZ = Random.Range(-areaSize.z / 2, areaSize.z / 2);

        targetPosition = new Vector3(visualChild.localPosition.x, areaCenter.y + randomY, areaCenter.z + randomZ);
    }
}
