using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class S_movement : MonoBehaviour{
    [Header("Param�tres de d�placement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private bool canJump = true;

    [Header("Param�tres physiques")]
    [SerializeField] private Rigidbody rb;

    private bool isGrounded = true;

    private void Start(){
        if (rb is null){
            rb = GetComponent<Rigidbody>();
        }
    }

    private void Update(){
        float moveInput = Input.GetAxis("Vertical"); 
        Vector3 move = new Vector3(0, 0, moveInput * moveSpeed);
        rb.velocity = new Vector3(0, rb.velocity.y, move.z);

        if (canJump && isGrounded && Input.GetButtonDown("Jump")){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; 
        }
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Ground")){
            isGrounded = true;
        }
    }

}
