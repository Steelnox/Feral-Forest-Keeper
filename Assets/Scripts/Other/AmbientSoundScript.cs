using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundScript : MonoBehaviour
{
    public FMOD.Studio.EventInstance BSOEvent;
    [FMODUnity.EventRef]
    public string BSODir;
    public FMOD.Studio.EventInstance windEvent;
    [FMODUnity.EventRef]
    public string windDir;
    public FMOD.Studio.EventInstance waterEvent;
    [FMODUnity.EventRef]
    public string waterDir;
    public FMOD.Studio.EventInstance birdsEvent;
    [FMODUnity.EventRef]
    public string birdsDir;


    public bool BSODoing;
    public bool soundWindDoing;
    public bool soundWaterDoing;
    public bool soundBirdsDoing;

    #region Singleton

    public static AmbientSoundScript instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    void Start()
    {
        FMOD.Studio.Bus playerBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        playerBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);

        BSOEvent = FMODUnity.RuntimeManager.CreateInstance(BSODir);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(BSOEvent, transform, GetComponent<Rigidbody>());

        windEvent = FMODUnity.RuntimeManager.CreateInstance(windDir);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(windEvent, transform, GetComponent<Rigidbody>());

        waterEvent = FMODUnity.RuntimeManager.CreateInstance(waterDir);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(waterEvent, transform, GetComponent<Rigidbody>());

        birdsEvent = FMODUnity.RuntimeManager.CreateInstance(birdsDir);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(birdsEvent, transform, GetComponent<Rigidbody>());

        BSOEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        windEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        waterEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        birdsEvent.start();

        soundWindDoing = false;
        soundWaterDoing = false;
        soundBirdsDoing = true;
        BSODoing = false;


    }

    void Update()
    {
        //pushEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

    }
}
