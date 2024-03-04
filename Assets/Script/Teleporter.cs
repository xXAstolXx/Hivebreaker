using System.Collections.Generic;
using UnityEngine;


public class Teleporter : Interactable, ILevelLoadEvent
{
    [SerializeField]
    private SFXPlayer[] playersSFX;

    private bool isExitReady = false;

    private bool isJustInstanced = true;

    private bool isPlayerInSaveArea = true;
    public bool IsPlayerInSaveArea 
    { 
        get { return isPlayerInSaveArea; } 
        set 
        { 
            isPlayerInSaveArea = value; 
            if(!value)
            {
                DisableTeleporterEvent();
            }
        } 
    }

    private bool hasChanceToEndRun = false;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private List<Material> onMaterials;

    [SerializeField]
    private List<Material> offMaterials;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private GameObject lockVFX;

    [SerializeField]
    private GameObject activeVFX;

    [SerializeField]
    private GameObject chanceToEndRunVFX;

    private LevelLoader levelLoader;

    private SaveArea saveArea;

    private void Awake()
    {
        saveArea = GetComponentInChildren<SaveArea>();
        isInteractable = false;
    }

    protected override void Start()
    {
        base.Start();

        interactName = "TELEPORTER";
    }

    public override void InteractWith()
    {
        base.InteractWith();

        if (isInteractable)
        {
            if (isExitReady == false) return;

            if (hasChanceToEndRun)
            {
                Game.Instance.uiHudRef.ShowEndRun();
            }


            ExitLevel(otherObject);
        }
    }

    private bool HasChanceToEndRun()
    {
        float chance = Game.Instance.levelLoaderRef.ChanceToEndRun;

        if (Random.Range(0.0f, 100.0f) <= chance)
        {
            Game.Instance.levelLoaderRef.ChanceToEndRun = -0.5f;
            return true;
        }
        return false;
    }

    public void SpawnPlayer()
    {
        Instantiate(playerPrefab, transform.position, Quaternion.identity,
            Game.Instance.levelLoaderRef.gameObject.transform);
        PlaySFXByTriggerKey("TELEPORT_IN");
    }

    public void SetExitReady(bool value)
    {
        isExitReady = value;
        isInteractable = value;

        if (value)
        {
            EnableTeleporterEvent();
        }
    }

    public void ExitLevel()
    {
        PlaySFXByTriggerKey("TELEPORT_OUT");
        ExitLevel(otherObject);
    }

    private void ExitLevel(GameObject player)
    {
        levelLoader = Game.Instance.levelLoaderRef;
        levelLoader.RememberPlayerStatsInLeveloader(player.GetComponent<Player>().Stats, player.GetComponent<Player>().HasCaptureBullet);
        player.GetComponent<CompanionController>().OnLevelExit();
        player.GetComponent<PlayerMovement>().SetMovementEnabled(false);
        player.GetComponent<PlayerMovement>().SetPlayerAnimationIdle();
        player.GetComponent<PlayerMovement>().StopPlayerMovement();

        Game.Instance.uiHudRef.ScreenFade(Fade.OUT, "TELEPORTER");
    }

    private void EnableTeleporterEvent()
    {
        meshRenderer.SetMaterials(onMaterials);
        lockVFX.SetActive(false);

        hasChanceToEndRun = HasChanceToEndRun();
        if (hasChanceToEndRun)
        {
            chanceToEndRunVFX.SetActive(true);
        }
        else
        {
            activeVFX.SetActive(true);
        }
    }

    public void DisableTeleporterEvent()
    {
        meshRenderer.SetMaterials(offMaterials);

        activeVFX.SetActive(false);
        
    }

    protected override void ShowInteractKey(bool value)
    {
        if (isExitReady)
        {
            base.ShowInteractKey(true);
            lockVFX.SetActive(false);
        }
        else
        {
            interactKeyImage.SetActive(false);
            lockVFX.SetActive(!value);
        }
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in playersSFX)
        {
            audioplayer.PlayOnTriggerKey(key);
        }
    }

    #region ILevelLoadEvent

    void ILevelLoadEvent.OnLevelLoadEvent()
    {
        if (isJustInstanced == false) return;

        levelLoader = Game.Instance.levelLoaderRef;
        SpawnPlayer();
    }
    

    void ILevelLoadEvent.OnLevelUnloadEvent()
    {
        isJustInstanced = false;
    }

    void ILevelLoadEvent.OnPlayerWasInstanced()
    {
        
    }
    
    #endregion ILevelLoadEvent

}