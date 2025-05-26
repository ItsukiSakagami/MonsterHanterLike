using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 0.01f;
    [SerializeField] private float _jumpPower = 1000;
    [SerializeField] private float _cameraSpeed = 0.01f;

    private Rigidbody _rb = null;

    private bool _isHit = false;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        var _move = InputSystem.actions["Move"].ReadValue<Vector2>();

        if (_isHit)
        {
            transform.Translate(_move.x * _moveSpeed, 0.0f, _move.y * _moveSpeed);
        }

        var _jump = InputSystem.actions["Jump"].ReadValue<float>();

        if (_isHit)
        {
            _rb.AddForce(0.0f, _jump * _jumpPower, 0.0f);
        }

        var _camera = InputSystem.actions["Look"].ReadValue<Vector2>();

        transform.Rotate(_camera.y * _cameraSpeed, 0.0f,_camera.x * _cameraSpeed);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isHit = true;
            Debug.Log("地面に接地");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isHit = false;
            Debug.Log("地面に接地");
        }
    }
}


