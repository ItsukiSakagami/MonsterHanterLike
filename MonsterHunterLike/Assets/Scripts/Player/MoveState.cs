using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : IState
{
    Player _player;

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        Move();
        if (InputSystem.actions["Attack"].ReadValue<float>() >= 0.0f)
        {
            _player.ChangeState(new AttackState());
        }
        else if (InputSystem.actions["Jump"].ReadValue<float>() >= 0.0f)
        {
            _player.ChangeState(new JumpState());
        }
    }

    public void OnExit()
    {

    }

    void Move()
    {
        Vector2 moveInput = InputSystem.actions["Move"].ReadValue<Vector2>();

        Quaternion cameraRotation = Quaternion.Euler(0, _player.cameraHolder.eulerAngles.y, 0);
        Vector3 forward = cameraRotation * Vector3.forward;
        Vector3 right = cameraRotation * Vector3.right;
        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        // ì¸óÕÇ™Ç†ÇÈÇ∆Ç´ÇæÇØâÒì]Ç≥ÇπÇÈ
        if (moveInput.sqrMagnitude > 0.01f) // Å‡ moveInput != Vector2.zero
        {
            _player.transform.rotation =
                Quaternion.Slerp(
                    _player.transform.rotation,
                    Quaternion.LookRotation(moveDirection),
                    0.2f
                    );
        }

        Vector3 velocity = moveDirection * _player.moveSpeed;
        velocity.y = _player.rb.linearVelocity.y;

        _player.rb.linearVelocity = velocity;
    }
}
