using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSign : MonoBehaviour
{
    public TypeLetterByLetter letterScript;
    public GameObject canvas;

    [FMODUnity.EventRef]
    public string appearEvent;

    public void ActivateLetterScript()
    {
        FMODUnity.RuntimeManager.PlayOneShot(appearEvent, transform.position);

        canvas.SetActive(true);
        StartCoroutine(letterScript.ShowText());
    }
}
