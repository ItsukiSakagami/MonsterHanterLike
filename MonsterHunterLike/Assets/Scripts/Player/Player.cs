using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _cameraHolder;
    public Transform cameraHolder { get => _cameraHolder; }

    [SerializeField] private float _moveSpeed = 0.01f;

    public float moveSpeed { get => _moveSpeed; }

    [SerializeField] private float _jumpPower = 1000.0f;
    public float jumpPower { get => _jumpPower; }

    private Rigidbody _rb = null;
    public Rigidbody rb { get => _rb; }

    private Animator _anim;
    public Animator anim { get => _anim; }

    public float _groundedJumpDelay = 0.5f; // ← 地面に着いてからのジャンプ禁止時間

    private float _firstgroundedJumpDelay;
    public float firstgroundedJumpDelay { get => _firstgroundedJumpDelay; }

    private bool _isGrounded = false;
    public bool isGrounded { get => _isGrounded; }


    [System.NonSerialized]
    public bool _pressdJump = false;

    private bool _isMoveing = false;
    private bool _isAttack = false; //どうしてもバグる　アニメーションイベント使用



    private IState _currentState;

    public void ChangeState(IState newState)
    {
        _currentState?.OnExit();

        _currentState = newState;

        _currentState.OnEnter();

    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _firstgroundedJumpDelay = _groundedJumpDelay;
    }

    void FixedUpdate()
    {
        if (!_pressdJump)
        {
            Movement();
        }
        TryJump();

        if (_isMoveing == false && !_isAttack && _isGrounded)
        {
            _anim.Play("Idle1");
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))  // Fire1はデフォルトでCtrlやコントローラーのボタンに割り当てられていることが多い
        {
            Debug.Log("a");
        }
    }
    void Movement()
    {
        if (_isAttack)
        {
            return;
        }

        Vector2 moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();

        Quaternion cameraRotation = Quaternion.Euler(0, _cameraHolder.eulerAngles.y, 0);
        Vector3 forward = cameraRotation * Vector3.forward;
        Vector3 right = cameraRotation * Vector3.right;
        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        // 入力があるときだけ回転させる
        if (moveInput.sqrMagnitude > 0.01f) // ≒ moveInput != Vector2.zero
        {
            _isMoveing = true;
            _anim.Play("Run1");
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.2f);
        }

        Vector3 velocity = moveDirection * _moveSpeed;
        velocity.y = _rb.linearVelocity.y;

        _rb.linearVelocity = velocity;
        if (moveInput == Vector2.zero)
        {
            _isMoveing = false;
        }
    }

    void TryJump()
    {
        if (_isAttack)
        {
            return;
        }

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
        _isAttack = true;
        _anim.Play("Attack1");
    }

    private void OnEnable()
    {
        InputSystem.actions["Attack"].performed += OnAttackPerformed;
        _isAttack = true;
    }

    private void OnDisable()
    {
        InputSystem.actions["Attack"].performed -= OnAttackPerformed;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        Attack();
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

    public void ChangeAttackFlag()
    {
        _isAttack = false;
    }
}


