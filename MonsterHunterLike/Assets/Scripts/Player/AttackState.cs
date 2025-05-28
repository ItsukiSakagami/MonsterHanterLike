using UnityEngine;
using UnityEngine.InputSystem;

public class AttackState : IState
{
    Player _player;

    public void OnEnter()
    {

    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {

    }

    public void Attack()
    {
        float jumpInput = InputSystem.actions["Attack"].ReadValue<float>();

        if (jumpInput > 0)
        {
            _player.anim.Play("Attack1");
            Debug.Log("a");
        }
    }
}
