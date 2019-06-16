using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRockController : MonoBehaviour
{
    public GameObject chest;

    public ColorRockPrincipalScript cristal1;
    public ColorRockPrincipalScript cristal2;
    public ColorRockPrincipalScript cristal3;

    public float feedbackCooldown;
    private bool justSuccces;

    [FMODUnity.EventRef]
    public string successPuzzleEvent;

    private bool soundDone = false;
    private void Start()
    {
        justSuccces = true;
    }
    void Update()
    {
        if (cristal1.activated && cristal2.activated && cristal3.activated)
        {
            chest.GetComponent<Animator>().SetBool("Open", true);
            if (!soundDone)
            {
                FMODUnity.RuntimeManager.PlayOneShot(successPuzzleEvent, transform.position);
                soundDone = true;
            }


            if (justSuccces)CameraController.instance.StartScriptedMovement(chest, feedbackCooldown);
            justSuccces = false;
        }
    }
}
