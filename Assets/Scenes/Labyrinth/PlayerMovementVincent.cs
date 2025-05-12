// PlayerMovement.cs
using UnityEngine;

public class PlayerMovementVincent : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Camera Reference")]
    [SerializeField] private Transform cameraTransform;
    
    [Header("Physics")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundMask;
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("PlayerMovement requires a CharacterController component!");
            enabled = false;
        }
        
        // If camera reference is not set, try to find the main camera
        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
            }
            else
            {
                Debug.LogWarning("Main camera not found. Please assign a camera reference.");
            }
        }
    }
    
    private void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        ApplyGravity();
        FollowCameraRotation();
    }
    
    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }
    }
    
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // Calculate movement direction based on player's orientation
        Vector3 moveDirection = (transform.forward * verticalInput) + (transform.right * horizontalInput);
        
        // Normalize to prevent diagonal movement being faster
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }
        
        // Apply movement
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
    
    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    
    private void FollowCameraRotation()
    {
        // Update player rotation to match camera's horizontal rotation
        if (cameraTransform != null)
        {
            // Only take the Y-axis rotation from the camera
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;
            
            if (cameraForward.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(cameraForward);
            }
        }
    }
}