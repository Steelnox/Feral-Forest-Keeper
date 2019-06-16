using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishAnimationController : MonoBehaviour
{
    private bool dashArriveIsDone;
    private bool attackFinish;

    [FMODUnity.EventRef]
    public string walkEvent;

    [FMODUnity.EventRef]
    public string attackEvent;

    [FMODUnity.EventRef]
    public string fallingEvent;

    
    void Start()
    {
        dashArriveIsDone = true;
    }

    public bool GetDashArriveIsDone()
    {
        return dashArriveIsDone;
    }
    public void DashArriveFinish()
    {
        dashArriveIsDone = true;
    }
    public void StartDashing()
    {
        dashArriveIsDone = false;
    }
    public void FinishAttack()
    {
        attackFinish = true;
    }
    public void StartAttack()
    {
        attackFinish = false;
    }
    public bool GetAttackFinish()
    {
        return attackFinish;
    }

    public void StepSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(walkEvent, transform.position);
    }

    public void AttackSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(attackEvent, transform.position);
    }

    public void FallingSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(fallingEvent, transform.position);
    }

}
