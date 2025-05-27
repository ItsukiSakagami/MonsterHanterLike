using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform cameraHolder;

    [SerializeField] private float _moveSpeed = 0.01f;

    [SerializeField] private float _jumpPower = 1000.0f;

    private Rigidbody _rb = null;

    [SerializeField] private float _groundedJumpDelay = 0.5f; // ← 地面に着いてからのジャンプ禁止時間
    private float _firstgroundedJumpDelay;

    private bool _isGrounded = false;

    private bool _pressdJump = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _firstgroundedJumpDelay = _groundedJumpDelay;
    }

    void FixedUpdate()
    {
        if (!_pressdJump)
        {
            Movement();
        }
        TryJump();

        Attack();
    }

    void Movement()
    {
        /*
        Vector2 moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();

        Quaternion cameraRotation = Quaternion.Euler(0, cameraHolder.eulerAngles.y, 0);
        Vector3 forward = cameraRotation * Vector3.forward;
        Vector3 right = cameraRotation * Vector3.right;
        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.2f);

        Vector3 velocity = moveDirection * _moveSpeed;
        velocity.y = _rb.linearVelocity.y;

        _rb.linearVelocity = velocity;
        */

        Vector2 moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();

        Quaternion cameraRotation = Quaternion.Euler(0, cameraHolder.eulerAngles.y, 0);
        Vector3 forward = cameraRotation * Vector3.forward;
        Vector3 right = cameraRotation * Vector3.right;
        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        // 入力があるときだけ回転させる
        if (moveInput.sqrMagnitude > 0.01f) // ≒ moveInput != Vector2.zero
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.2f);
        }

        Vector3 velocity = moveDirection * _moveSpeed;
        velocity.y = _rb.linearVelocity.y;

        _rb.linearVelocity = velocity;
    }

    void TryJump()
    {
        float jumpInput = InputSystem.actions["Jump"].ReadValue<float>();

        if (jumpInput != 0.0f && !_pressdJump)
        {
            _rb.AddForce(Vector3.up * _jumpPower);
            _pressdJump = true;
        }

        if (_pressdJump && _isGrounded)
        {
            _groundedJumpDelay -= Time.deltaTime;
        }

        if (_groundedJumpDelay <= 0.0f)
        {
            _groundedJumpDelay = _firstgroundedJumpDelay;
            _pressdJump = false;
        }
    }

    void Attack()
    {
        float jumpInput = InputSystem.actions["Attack"].ReadValue<float>();

        if (jumpInput > 0)
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}


