using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
   [SerializeField] private CharacterController _controller;
   [SerializeField] private float _speed;

    float turnSmoothVelocity;
    public float turnMoveTime = 0.1f;
    [SerializeField] private Transform _camTransform;

    [SerializeField] private Rigidbody _rb;
    [SerializeField]  private bool _isOnTheFloor = true;

    private void Start()
    {
    }

    private void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space) && _isOnTheFloor)
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        //if (direction.magnitude >= 0.1f)
        //{
        //    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camTransform.eulerAngles.y;
        //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnMoveTime);
        //    transform.rotation = Quaternion.Euler(0, angle, 0);

        //    Vector3 moveDir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        //    _controller.Move(direction * _speed * Time.deltaTime);

        //}

    }

    private void Jump()
    {
        _rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
        _isOnTheFloor = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Floor")
        {
            _isOnTheFloor = true;
        }
    }
}
