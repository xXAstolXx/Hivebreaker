using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TutorialTeleporter : Interactable, ILevelLoadEvent
{
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

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private List<Material> onMaterials;

    [SerializeField]
    private List<Material> offMaterials;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private GameObject[] turnOff;

    [SerializeField]
    private GameObject finalDialog;

    private LevelLoader levelLoader;

    private SaveArea saveArea;

    [SerializeField]
    private VisualEffect effect;

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

            if (HasChanceToEndRun())
            {
                Game.Instance.uiHudRef.ShowEndRun();
                return;
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
        ExitLevel(otherObject);
    }

    private void ExitLevel(GameObject player)
    {
        levelLoader = Game.Instance.levelLoaderRef;
        player.GetComponent<PlayerMovement>().SetMovementEnabled(false);
        player.GetComponent<PlayerMovement>().SetPlayerAnimationIdle();
        player.GetComponent<PlayerMovement>().StopPlayerMovement();
        player.GetComponent<CompanionController>().KillCompanion();

        Camera.main.GetComponentInChildren<CreatureCaptureItemDisplay>().ShowItem(false);
        Game.Instance.highscoreSystemRef.KillCounter = 0;
        Game.Instance.highscoreSystemRef.SetGameUpTime();
        Game.Instance.levelLoaderRef.OnGameStart();
    }

    private void EnableTeleporterEvent()
    {
        meshRenderer.SetMaterials(onMaterials);
        foreach (GameObject thing in turnOff)
        {
            thing.SetActive(true);
        }

        if (finalDialog == null) return;
        finalDialog.SetActive(true);
    }

    protected override void ShowInteractKey(bool value)
    {
        IsPlayerInSaveArea = value;
    }

    public void DisableTeleporterEvent()
    {
        meshRenderer.SetMaterials(offMaterials);
        foreach (GameObject thing in turnOff)
        {
            thing.SetActive(false);
        }
    }

    private void Update()
    {
        if (!IsPlayerInSaveArea)
        {
            float dissolveValue = effect.GetFloat("DissolveOffset") + (1.0f * Time.deltaTime);
            effect.SetFloat("DissolveOffset", dissolveValue);
        }

        if (GetComponentsInChildren<DialogTrigger>(false).Length <= 0)
        {
            if (finalDialog == null) return;
            finalDialog.SetActive(true) ;
            SetExitReady(true); 
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