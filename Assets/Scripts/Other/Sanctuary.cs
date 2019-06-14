using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanctuary : MonoBehaviour
{
    public GameObject checkpoint;
    public PlayerController player;
    public PlayerManager playerManager;

    public Vector3 positionCheckpoint;
    public Material materialActivated;

    private bool activatedSanctuary;
    private bool actionDone;
    private Renderer renderer;

    void Start()
    {
        player = PlayerController.instance;
        playerManager = PlayerManager.instance;
        activatedSanctuary = false;
        actionDone = false;
        positionCheckpoint = new Vector3(transform.position.x, transform.position.y , transform.position.z - 1 );
        renderer = GetComponent<Renderer>();
    }

    
    void Update()
    {
        if(activatedSanctuary && !actionDone)
        {
            renderer.material = materialActivated;
            checkpoint.transform.position = positionCheckpoint;
            


            actionDone = true;
        }
    }

    public void ActivateSanctuary()
    {
        activatedSanctuary = true;
    }
}
