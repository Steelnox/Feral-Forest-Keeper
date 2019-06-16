using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSoundScript : MonoBehaviour
{
    public bool soundWindBool;
    public bool soundWaterBool;
    public bool soundBirdsBool;

    private AmbientSoundScript ambientScript;

    private void Start()
    {
        ambientScript = AmbientSoundScript.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (soundWaterBool)
            {
                if (ambientScript.soundWaterDoing)
                {
                    ambientScript.waterEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    ambientScript.soundWaterDoing = false;
                }
                else
                {
                    ambientScript.waterEvent.start();
                    ambientScript.soundWaterDoing = true;

                }
            }

            if (soundWindBool)
            {
                if (ambientScript.soundWindDoing)
                {
                    ambientScript.windEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    ambientScript.soundWindDoing = false;
                }
                else
                {
                    ambientScript.windEvent.start();
                    ambientScript.soundWindDoing = true;

                }
            }

            if (soundBirdsBool)
            {
                if (ambientScript.soundBirdsDoing)
                {
                    ambientScript.birdsEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    ambientScript.soundBirdsDoing = false;
                }
                else
                {
                    ambientScript.birdsEvent.start();
                    ambientScript.soundBirdsDoing = true;

                }
            }


            if (ambientScript.BSODoing)
            {
                ambientScript.BSOEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                ambientScript.BSODoing = false;
            }
            else
            {
                ambientScript.BSOEvent.start();
                ambientScript.BSODoing = true;

            }

        }
    }

}
