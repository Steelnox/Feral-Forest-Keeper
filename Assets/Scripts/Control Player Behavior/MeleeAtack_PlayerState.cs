using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAtack_PlayerState : State
{

    public override void Enter()
    {
        //PlayerController.instance.SetCanMove(false);
        PlayerController.instance.attacking = true;
        PlayerAnimationController.instance.AttackAnim();
        PlayerAnimationController.instance.finishAnimationController.StartAttack();
    }
    public override void Execute()
    {
        if (PlayerAnimationController.instance.finishAnimationController.GetAttackFinish()) PlayerController.instance.ChangeState(PlayerController.instance.movementState);
        PlayerController.instance.p_controller.Move(Vector3.zero);
        PlayerController.instance.imGrounded = PlayerController.instance.p_controller.isGrounded;
        

        ///Movement While Attaking Added
        if (PlayerController.instance.movingWhileAttacking)
        {
            if (PlayerController.instance.playerRoot.transform.forward != PlayerController.instance.modelForwardDirection)
            {
                PlayerController.instance.playerRoot.transform.forward = Vector3.Slerp(PlayerController.instance.playerRoot.transform.forward, PlayerController.instance.modelForwardDirection, Time.deltaTime * PlayerController.instance.smooth);
            }
            PlayerController.instance.p_controller.Move(PlayerController.instance.movement * Time.deltaTime);
        }
        else
        if (PlayerController.instance.orientWhileAttaking)
        {
            if (PlayerController.instance.playerRoot.transform.forward != PlayerController.instance.modelForwardDirection)
            {
                PlayerController.instance.playerRoot.transform.forward = Vector3.Slerp(PlayerController.instance.playerRoot.transform.forward, PlayerController.instance.modelForwardDirection, Time.deltaTime * PlayerController.instance.smooth);
            }
        }
    }
    public override void Exit()
    {
        //PlayerController.instance.SetCanMove(true);
        PlayerController.instance.attacking = false;
    }
}
