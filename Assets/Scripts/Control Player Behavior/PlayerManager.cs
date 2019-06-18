using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public List<Item> items;

    public Item branchWeaponSlot;
    public Item leafSwordSlot;
    public Item powerGauntletSlot;
    public Item dashSkillSlot;
    public int actualLeafQuantity;
    public int actualKeyQuantity;

    public GameObject branchWeaponForAnimations;
    public GameObject leafWeaponForAnimations;

    public float woodSignInteractionDistance;
    public float simonMushroomInteractionDistance;
    public float colorsPuzzlePilarInteractionDistance;
    public float skillPillarsInteractionDistance;
    public float sanctuaryInteractionDistance;

    private bool haveDashSkill;
    private bool havePowerSkill;

    public GameObject canvas;

    void Start()
    {
        haveDashSkill = false;
        havePowerSkill = false;

        if (PlayerController.instance.startWithAllSkills)
        {
            leafSwordSlot = GameManager.instance.swordItem;
            CheckIfHaveLeafWeaponItem();

            powerGauntletSlot = GameManager.instance.gantletItem;
            dashSkillSlot = GameManager.instance.dashItem;
        }
        else
        {
            branchWeaponForAnimations.SetActive(false);
            leafWeaponForAnimations.SetActive(false);
            branchWeaponSlot = null;
            leafSwordSlot = null;
            powerGauntletSlot = null;
            dashSkillSlot = null;
        }
    }

    void Update()
    {
        if(dashSkillSlot == null)
        {
            canvas.SetActive(false);
        }
        else
        {
            if (PlayerController.instance.dashCooldown >= PlayerController.instance.dashCooldownTime)
            {
                canvas.SetActive(false);
            }
            else
            {
                canvas.SetActive(true);
            }
        }
        if (!haveDashSkill && dashSkillSlot != null)
        {
            PlayerParticlesSystemController.instance.SetPickUpSkillPaticlesOnScene(PlayerController.instance.playerRoot.transform.position);
            haveDashSkill = true;
        }
        if (!havePowerSkill && powerGauntletSlot != null)
        {
            PlayerParticlesSystemController.instance.SetPickUpSkillPaticlesOnScene(PlayerController.instance.playerRoot.transform.position);
            havePowerSkill = true;
        }
    }
    public void AddItemToInventary(Item i)
    {
        items.Add(i);
    }
    public void QuitItemElementFromItemsList(Item _i)
    {
        foreach(Item item in items)
        {
            if (item == _i)
            {
                items.Remove(item);
            }
        }
    }
    public void CheckIfHaveBranchWeaponItem()
    {
        foreach (Item item in items)
        {
            if (item.itemType == Item.ItemType.BRANCH_WEAPON)
            {
                branchWeaponSlot = item;
            }
        }
        if (branchWeaponSlot != null)
        {
            branchWeaponForAnimations.SetActive(true);
            PlayerAnimationController.instance.SetWeaponAnim(true);
        }
        else
        {
            PlayerAnimationController.instance.SetWeaponAnim(false);
        }
    }
    public void CheckIfHaveLeafWeaponItem()
    {
        foreach(Item item in items)
        {
            if (item.itemType == Item.ItemType.LEAF_WEAPON)
            {
                leafSwordSlot = item;
                branchWeaponSlot = null;
            }
        }
        if (leafSwordSlot != null)
        {
            branchWeaponForAnimations.SetActive(false);
            leafWeaponForAnimations.SetActive(true);
            PlayerAnimationController.instance.SetWeaponAnim(true);
        }
        else
        {
            PlayerAnimationController.instance.SetWeaponAnim(false);
        }
    }
    public void CountKeys()
    {
        int count = 0;
        foreach(Item item in items)
        {
            if (item.itemType == Item.ItemType.KEY) count++;
        }
        actualKeyQuantity = count;
    }
    public void CountLeafs()
    {
        int count = 0;
        foreach (Item item in items)
        {
            if (item.itemType == Item.ItemType.BRANCH_WEAPON) count++;
        }
        actualLeafQuantity = count;
    }
    public bool FindKeysInInventory(Item[] keysList)
    {
        bool result = false;
        int numKeys = 0;

        for (int i = 0; i < keysList.Length; i++)
        {
            foreach (Item item in items)
            {
                if (item == keysList[i])
                {
                    numKeys++;
                }
            }
        }
        if (numKeys == keysList.Length) result = true;
        return result;
    }
}
