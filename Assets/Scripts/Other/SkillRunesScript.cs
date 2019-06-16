using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRunesScript : MonoBehaviour
{

    public PlayerManager playerManager;
    public Item gauntlet;
    public Item dash;

    public bool dashRune;
    public bool forceRune;

    [FMODUnity.EventRef]
    public string successEvent;

    private void Start()
    {
        playerManager = PlayerManager.instance;
    }

    public void TakeSkill()
    {
        if (dashRune)
        {
            FMODUnity.RuntimeManager.PlayOneShot(successEvent, transform.position);

            playerManager.dashSkillSlot = dash;
        }

        if (forceRune)
        {
            FMODUnity.RuntimeManager.PlayOneShot(successEvent, transform.position);

            playerManager.powerGauntletSlot = gauntlet;
        }
    }
}
