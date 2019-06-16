using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonRock : MonoBehaviour
{
    private SimonController simonMaster;
    public Color colorRock;
    public Material materialRock;

    [FMODUnity.EventRef]
    public string colorChangeEvent;

    void Start()
    {
        simonMaster = SimonController.instance;
        materialRock = GetComponent<Renderer>().material;

    }

    public void InteractWithSimonRock()
    {
        if (simonMaster.sequenceDone && !simonMaster.fail)
        {
            FMODUnity.RuntimeManager.PlayOneShot(colorChangeEvent, transform.position);

            simonMaster.CheckSimon(this.gameObject);
        }
    }
}
