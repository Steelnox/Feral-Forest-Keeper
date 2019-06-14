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

    private void Start()
    {
        playerManager = PlayerManager.instance;
    }

    public void TakeSkill()
    {
        if (dashRune)
        {
            playerManager.dashSkillSlot = dash;
        }

        if (forceRune)
        {
            playerManager.powerGauntletSlot = gauntlet;
        }
    }
}
