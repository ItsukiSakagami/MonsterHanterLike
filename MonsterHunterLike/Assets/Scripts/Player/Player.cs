using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _cameraHolder;
    public Transform cameraHolder { get => _cameraHolder; }

    [SerializeField] private float _moveSpeed = 10.0f;

    public float moveSpeed { get => _moveSpeed; }

    [SerializeField] private float _jumpPower = 200.0f;
    public float jumpPower { get => _jumpPower; }

    private Rigidbody _rb = null;
    public Rigidbody rb { get => _rb; }

    private Animator _anim;
    public Animator anim { get => _anim; }

    public float _groundedJumpDelay = 0.5f; // ← 地面に着いてからのジャンプ禁止時間

    private float _initialGroundedJumpDelay;
    public float firstgroundedJumpDelay { get => _initialGroundedJumpDelay; }

    private bool _isGrounded = false;
    public bool isGrounded { get => _isGrounded; }

    [SerializeField]
    private float _flyVel = 0.7f;


    [System.NonSerialized]
    public bool _pressedJump = false; // 修正: "pressd" を "pressed" に変更  

    [System.NonSerialized]
    public bool _pressdJump = false;

    private bool _isMoveing = false;
    private bool _isAttack = false; //どうしてもバグる　アニメーションイベント使用
    private bool _attackRequested = false;
    private bool _isFlying = false;
    [SerializeField]
    private float _animTime = 0.5f;
    private float _initialAnimTime;


    private IState _currentState;

    [SerializeField]
    private GameObject _weapon;

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

        _initialGroundedJumpDelay = _groundedJumpDelay;

        _initialAnimTime = _animTime;
    }

    void FixedUpdate()
    {
        if (_isAttack)
        {
            _animTime -= Time.deltaTime;
        }
        if (_animTime <= 0.0f)
        {
            _isAttack = false;
            _weapon.tag = "Untagged";
            _animTime = _initialAnimTime;
        }

        Movement();

        TryJump();

        if (_isMoveing == false && !_isAttack && _isGrounded)
        {
            _anim.Play("Idle1");
        }

        Fly();
    }
    void Movement()
    {
        if (_isAttack)
        {
            _rb.linearVelocity = Vector3.zero;
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
            if (!_isFlying)
            {
                _anim.Play("Run1");
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.2f);
        }

        Vector3 velocity = moveDirection * _moveSpeed;
        velocity.y = _rb.linearVelocity.y;

        _rb.linearVelocity = velocity;
        if (moveInput == Vector2.zero)
        {
            _isMoveing = false;
            Vector3 newVel = _rb.linearVelocity;
            newVel.x = 0.0f;
            newVel.z = 0.0f;
            _rb.linearVelocity = newVel;
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
            _groundedJumpDelay = _initialGroundedJumpDelay;
            _pressdJump = false;
        }
    }

    void Attack()
    {
        if (_pressdJump)
        {
            return;
        }
        _isAttack = true;
        _weapon.tag = "Attack";
        _anim.Play("Attack1");
    }
    void Attack2()
    {
        Debug.Log("連続攻撃");
    }

    void Fly()
    {
        if (_isGrounded)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            _isFlying = true;
            _anim.Play("Fly");
            Vector3 vel = _rb.linearVelocity;
            vel.y = _flyVel;

            _rb.linearVelocity = vel;
        }
        else
        {
            _isFlying = false;
        }
    }

    private void OnEnable()
    {
        InputSystem.actions["Attack"].performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        InputSystem.actions["Attack"].performed -= OnAttackPerformed;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (_isAttack)
        {
            _attackRequested = true;  // 攻撃中なら次の攻撃リクエストを予約
            return;
        }

        Attack();
    }

    public void AttackFlagFalse()
    {
        _isAttack = false;
        _weapon.tag = "Untagged";

        if (_attackRequested)
        {
            _attackRequested = false;
            Attack2();  // 攻撃終了後に予約してた攻撃を実行
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && _weapon.CompareTag("Attack"))
        {
            UntaggedWeaponTag();
        }
    }

    //アニメーションイベント用
    public void UntaggedWeaponTag()
    {
        _weapon.tag = "Untagged";
    }
}


