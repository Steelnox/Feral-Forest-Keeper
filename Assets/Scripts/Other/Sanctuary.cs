using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanctuary : MonoBehaviour
{
    public GameObject checkpoint;
    public PlayerController player;
    public PlayerManager playerManager;

    public Vector3 positionCheckpoint;

    private bool activatedSanctuary;
    private bool actionDone;

    [FMODUnity.EventRef]
    public string successEvent;

    //public GameObject particles1;
    //public GameObject particles2;
    public GameObject particlesSpawn;
    public ParticlesCompositeSystem activeSanctiaryParticles;
    public Particles_Behavior firstWave;


    void Start()
    {
        player = PlayerController.instance;
        playerManager = PlayerManager.instance;
        activatedSanctuary = false;
        actionDone = false;
        positionCheckpoint = new Vector3(transform.position.x, transform.position.y , transform.position.z - 1 );
    }

    
    void Update()
    {
        if(activatedSanctuary && !actionDone)
        {
            checkpoint.transform.position = positionCheckpoint;

            FMODUnity.RuntimeManager.PlayOneShot(successEvent, transform.position);

            activeSanctiaryParticles.PlayComposition(particlesSpawn.transform.position);
            firstWave.SetParticlesOnScene(transform.position);
            //particles1.SetActive(true);
            //particles2.SetActive(true);


            actionDone = true;
        }
    }

    public void ActivateSanctuary()
    {
        activatedSanctuary = true;
    }
}
