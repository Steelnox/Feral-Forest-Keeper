using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundMainMenu : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string BSOEvent;

    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot(BSOEvent, transform.position);

    }

    void Update()
    {
        
    }
}
