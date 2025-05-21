using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonnageController : MonoBehaviour
{
    [Header("Mouvement du Personnage")]
    public float speed = 5f;
    public float sprintMultiplier = 2f;
    public float rotationSpeed = 10f;

    [Header("Paramètres de la Caméra")]
    public Transform cameraTransform;
    public float distanceFromPlayer = 5f;
    public float heightOffset = 2f;
    public float cameraRotationSpeed = 2f;
    public float verticalRotationSpeed = 1.5f;
    public float cameraAngleOffset = -45f;

    [Header("Limites de Rotation Verticale")]
    public float minPitch = -20f;
    public float maxPitch = 45f;

    [Header("Saut")]
    public float jumpForce = 5f;
    public float fallMultiplier = 2.5f;
    public LayerMask groundLayer;
    private bool isGrounded;

    private Rigidbody rb;
    private Vector3 movementDirection;
    private float currentYaw;
    private float currentPitch;

    [SerializeField] private Animator animator;


    // Référence au script PauseMenu
    public PauseMenu pauseMenu;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentYaw = cameraAngleOffset;
        currentPitch = 10f;
        UpdateCameraPosition();

      
    }

    private void Update()
    {
        if (pauseMenu.gameIsPaused) return;

        HandleMovement();
        HandleCameraRotation();
        HandleJump();
    }


    private void LateUpdate()
    {
        if ( pauseMenu.gameIsPaused) return;

        UpdateCameraPosition();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        movementDirection = forward * vertical + right * horizontal;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float finalSpeed = isSprinting ? speed * sprintMultiplier : speed;

        if (movementDirection.magnitude >= 0.1f)
        {
            rb.MovePosition(transform.position + movementDirection * finalSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("isRunning", true);
            animator.SetBool("isFastRun", isSprinting);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isFastRun", false);
        }
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        currentYaw += mouseX * cameraRotationSpeed;
        currentPitch -= mouseY * verticalRotationSpeed;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
    }

    private void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            isGrounded = false;
            animator.SetTrigger("Jump");
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distanceFromPlayer);
        cameraTransform.position = transform.position + offset + Vector3.up * heightOffset;
        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f);
    }
}
