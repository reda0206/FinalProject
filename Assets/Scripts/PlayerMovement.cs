using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public float health = 100f;
    public bool isGrounded;
    public Transform playerCamera;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    public GameObject Lazer;
    public float shootCooldown = 1f;
    private float lastShootingTime = -Mathf.Infinity;
    public float damageCooldown = 2f;
    private float lastDamageTime = -Mathf.Infinity;
    public float jumpCooldownn = 1f;
    private float lastJumpTime = -Mathf.Infinity;
    public float rotationSpeed = 10f;

    private Vector3 movement;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (Time.time - lastJumpTime >= jumpCooldownn)
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                lastJumpTime = Time.time;
            }
        }
    }

    private void FixedUpdate()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }

        if (playerCamera != null && rb != null)
        {
            float camYaw = playerCamera.eulerAngles.y;
            Quaternion target = Quaternion.Euler(0, camYaw, 0f);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, target, rotationSpeed * Time.fixedDeltaTime));
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (playerCamera != null)
        {
            Vector3 camFoward = Vector3.ProjectOnPlane(playerCamera.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(playerCamera.right, Vector3.up).normalized;

            Vector3 desiredDirection = camRight * horizontal + camFoward * vertical;

            if (desiredDirection.magnitude > 1f)
            {
                desiredDirection.Normalize();
            }

            movement = desiredDirection * speed;

            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }

        else
        {
            movement = new Vector3(horizontal * speed, 0f, vertical * speed);
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }

        if (Input.GetMouseButton(0) && Time.time - lastShootingTime >= shootCooldown)
        {
            Vector3 spawnPosition = playerCamera.position + playerCamera.forward;
            Quaternion spawnRotation = Quaternion.LookRotation(playerCamera.forward);
            Instantiate(Lazer, spawnPosition, spawnRotation);
            lastShootingTime = Time.time;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(12f);
        }
        else if (collision.gameObject.CompareTag("EnemyLazer"))
        {
            TakeDamage(6f);
        }
    }

    public void TakeDamage(float amount)
    {
        if (Time.time - lastDamageTime < damageCooldown)
            return;

        lastDamageTime = Time.time;

        health -= amount;
        if (health <= 0f)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            int deadSceneIndex = SceneManager.GetActiveScene().buildIndex;
            GameOverButtons.lastDeadSceneIndex = deadSceneIndex;

            SceneManager.LoadScene("GameOverScene");
        }
    }
}

