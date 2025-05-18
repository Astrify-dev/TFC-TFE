using UnityEngine;

public class SpriteMovement : MonoBehaviour{
    public float speed = 1f; 

    private void Update(){
        var newPosition = transform.position;
        newPosition.z -= speed * Time.deltaTime;
        transform.position = newPosition;
    }
}
