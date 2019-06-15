using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death_PlayerState : State
{
    private bool touchingGround;
    public override void Enter()
    {
        touchingGround = false;
    }
    public override void Execute()
    {
        if (!touchingGround)
        {
            PlayerController.instance.imGrounded = PlayerController.instance.p_controller.isGrounded;
            PlayerController.instance.p_controller.Move(PlayerController.instance.movement * Time.deltaTime);
        }
        if (PlayerController.instance.imGrounded)
        {
            touchingGround = true;
            PlayerController.instance.imGrounded = true;
        }
    }
    public override void Exit()
    {
        touchingGround = false;
    }
}
