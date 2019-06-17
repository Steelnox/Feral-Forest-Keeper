using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundMainMenu : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string BSOEvent;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        FMODUnity.RuntimeManager.PlayOneShot(BSOEvent, transform.position);
    }

}
