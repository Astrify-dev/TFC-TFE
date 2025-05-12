using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class S_movement : MonoBehaviour{
    [Header("Paramètres de déplacement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private bool canJump = true;

    [Header("Paramètres physiques")]
    [SerializeField] private Rigidbody rb;

    [Header("Paramètres des commandes")]
    [SerializeField] private S_Commandes inputSettings;

    private bool isGrounded = true;
    private bool isDashing = false;

    private void Start(){
        if (rb is null){
            rb = GetComponent<Rigidbody>();
        }
    }

    private void Update(){
        if (isDashing) return;

        float moveInput = 0f;
        if (Input.GetKey(inputSettings.moveForward)){
            moveInput = 1f;
        }else if (Input.GetKey(inputSettings.moveBackward)){
            moveInput = -1f;
        }

        Vector3 move = new Vector3(0, 0, moveInput * moveSpeed);
        rb.velocity = new Vector3(0, rb.velocity.y, move.z);

        if (canJump && isGrounded && Input.GetKeyDown(inputSettings.jump)){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKeyDown(inputSettings.dash)){
            StartCoroutine(Dash(moveInput));
        }
    }

    private IEnumerator Dash(float direction){
        isDashing = true;
        float originalSpeed = moveSpeed;
        moveSpeed = dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        moveSpeed = originalSpeed;
        isDashing = false;
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Ground")){
            isGrounded = true;
        }
    }
}