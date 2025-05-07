using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = 9.81f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D ou Q/D
        float vertical = Input.GetAxis("Vertical"); // W/S ou Z/S

        Vector3 moveDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        moveDirection.y = 0; // Empêche le déplacement vertical
        moveDirection.Normalize();

        controller.Move(moveDirection * speed * Time.deltaTime);

        if (!controller.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = -1f;
        }

        controller.Move(velocity * Time.deltaTime);
    }
}