using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    #endregion

    public Item branchItem;
    public Item swordItem;
    public Item gantletItem;
    public Item dashItem;
    public bool pause;
    public RectTransform provisionalGUIMenu;
    public Item[] liveUpPool;
    private Vector2 provisionalGUIMenuOnScreenPos;
    [HideInInspector]
    public Vector2 hidePos;

    public Enemy[] enemiesPool;
    public Enemy[] enemiesEventPool;
    public List<Projectile> projectilePool;
    public float respawnCoolDown;
    [SerializeField]
    private float actualRespawnCoolDown;
    public GameObject levelCheckPoint;

    private Vector3 startCheckPointPosition;
    private Vector3 branchItem_InitLocation;
    private Vector3 swordItem_InitLocation;

    public GameObject gameOver;

    [SerializeField]
    private bool deathByFall;
    [SerializeField]
    private bool playerDead;
    private bool respawnDone;

    void Start()
    {
        actualRespawnCoolDown = respawnCoolDown;
        provisionalGUIMenuOnScreenPos = provisionalGUIMenu.anchoredPosition;
        hidePos = Vector2.down * 1000;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerController.instance.transform.position = levelCheckPoint.transform.position;
        startCheckPointPosition = levelCheckPoint.transform.position;
        branchItem_InitLocation = branchItem.transform.position;
        swordItem_InitLocation = swordItem.transform.position;
        respawnDone = true;
    }
    void Update()
    {

        if (Input.GetButtonDown("Start") || Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                pause = false;
            }
            else
            {
                pause = true;
            }
        }
        if (pause)
        {
            //Debug.Log("Entering Pause");
            PlayerController.instance.noInput = true;
            if (provisionalGUIMenu.anchoredPosition != provisionalGUIMenuOnScreenPos) provisionalGUIMenu.anchoredPosition = provisionalGUIMenuOnScreenPos;
            if (Input.GetButtonDown("Back") || Input.GetKeyDown(KeyCode.Q))
            {
                Application.Quit();
            }
        }
        else
        {
            //Debug.Log("Entering Quiting Pause");
            PlayerController.instance.noInput = false;
            if (provisionalGUIMenu.anchoredPosition != hidePos) provisionalGUIMenu.anchoredPosition = hidePos;
        }
        /////// PLAYER DEATH CONTROLER ////////
        ///By FALLING
        if (deathByFall)
        {
            if (actualRespawnCoolDown == respawnCoolDown)
            {
                //Debug.Log("Actual respawn == resapwn Cooldown");
                PlayerHitFeedbackController.instance.FallHit();
                SetRespawnDone(false);
                //PlayerController.instance.SetCanMove(false);
                PlayerController.instance.noInput = true;
                PlayerController.instance.ChangeState(PlayerController.instance.deathState);
                CameraController.instance.SetActualBehavior(CameraController.Behavior.PLAYER_DEATH);
            }

            actualRespawnCoolDown -= Time.deltaTime;

            if (actualRespawnCoolDown <= 0)
            {
                if (PlayerController.instance.transform.position != levelCheckPoint.transform.position)
                {
                    PlayerController.instance.transform.position = levelCheckPoint.transform.position;
                }
                else
                //if (PlayerController.instance.transform.position == levelCheckPoint.transform.position)
                {
                    //Debug.Log("After death, player on CheckPointPosition: " + levelCheckPoint.transform.position);
                    actualRespawnCoolDown = respawnCoolDown;
                    PlayerController.instance.actualPlayerLive--;
                    PlayerController.instance.deathByFall = false;
                    //PlayerController.instance.SetCanMove(true);
                    PlayerController.instance.noInput = false;
                    PlayerController.instance.ChangeState(PlayerController.instance.movementState);
                    deathByFall = false;
                    if (PlayerController.instance.actualPlayerLive <= 0)
                    {
                        PlayerDead();
                    }
                }
            }

            if (actualRespawnCoolDown < 0.1f)
            {
                CameraController.instance.SetActualBehavior(CameraController.Behavior.FOLLOW_PLAYER);
            }
        }

        ///By DIYNG
        if (playerDead)
        {
            //Debug.Log("Entering Death by Dying");
            PlayerController.instance.noInput = true;
            gameOver.SetActive(true);
            
            if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.Q))
            {
                FMOD.Studio.Bus playerBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
                playerBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
                SceneManager.LoadScene("Main Menu");
            }
            if (Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //ResetGame();
            }
        }
    }
    public Item GetRandomLiveUpItem()
    {
        int random = Random.Range(0, liveUpPool.Length);
        if (liveUpPool[random].collected)
        {
            //liveUpPool[random].collected = false;
            return liveUpPool[random];
        }
        else
        {
            return null;
        }
    }

    public Projectile GetProjectileNotActive()
    {
        for (int i = 0; i < projectilePool.Count; i++)
        {

            if(projectilePool[i].activated == false)
            {
                projectilePool[i].activated = true;
                return projectilePool[i];
            }    
        }

        return null;
    }
    public float GetActualRespawnCooldown()
    {
        return actualRespawnCoolDown;
    }
    private void ResetGame()
    {
        levelCheckPoint.transform.position = startCheckPointPosition;
        PlayerController.instance.transform.position = levelCheckPoint.transform.position;
        PlayerController.instance.SetCanMove(true);
        PlayerController.instance.deathByFall = false;
        PlayerController.instance.actualPlayerLive = PlayerController.instance.playerLive;

        if (!PlayerController.instance.startWithAllSkills)
        {
            //PlayerAnimationController.instance.SetWeaponAnim(true);

            PlayerManager.instance.branchWeaponForAnimations.SetActive(false);
            PlayerManager.instance.leafWeaponForAnimations.SetActive(false);

            PlayerManager.instance.branchWeaponSlot = null;
            PlayerManager.instance.leafSwordSlot = null;
            PlayerManager.instance.powerGauntletSlot = null;

            PlayerAnimationController.instance.SetWeaponAnim(false);
        }
        CameraController.instance.p_Camera.transform.position = PlayerController.instance.transform.position + CameraController.instance.cameraOffSet;

        if (branchItem.collected) branchItem.SetItem(branchItem_InitLocation, Vector3.up * 45);
        if (swordItem.collected) swordItem.SetItem(swordItem_InitLocation, Vector3.up * 45);
        playerDead = false;
    }
    public void DeathByFall()
    {
        deathByFall = true;
    }
    public void PlayerDead()
    {
        playerDead = true;
        if (PlayerController.instance.currentState != PlayerController.instance.deathState) PlayerController.instance.ChangeState(PlayerController.instance.deathState);
    }
    public void SetRespawnDone(bool b)
    {
        respawnDone = b;
    }
    public bool GetRespawnDone()
    {
        return respawnDone;
    }
}