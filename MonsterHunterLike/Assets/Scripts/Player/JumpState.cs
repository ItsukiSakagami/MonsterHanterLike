using UnityEngine;
using UnityEngine.InputSystem;

public class JumpState : IState
{
    Player _player;

    public void OnEnter()
    {

    }
    public void OnUpdate()
    {
        Jump();
    }
    public void OnExit()
    {

    }
    void Jump()
    {
        float jumpInput = InputSystem.actions["Jump"].ReadValue<float>();

        if (jumpInput != 0.0f && !_player._pressdJump)
        {
            _player.rb.AddForce(Vector3.up * _player.jumpPower);
            _player._pressdJump = true;
        }

        if (_player._pressdJump && _player.isGrounded)
        {
            _player._groundedJumpDelay -= Time.deltaTime;
        }

        if (_player._groundedJumpDelay <= 0.0f)
        {
            _player._groundedJumpDelay = _player.initialgroundedJumpDelay;
            _player._pressdJump = false;
        }
    }
}
