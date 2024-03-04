using UnityEngine;
using UnityEngine.InputSystem;

public class CaptureManager : MonoBehaviour, ILevelLoadEvent
{
    private CompanionController companionControllerRef;

    private CompanionGamepadBindings companionGamepadBindings;

    private Gamepad pairedGamepad;
    public Gamepad PairedGamepad
    {
        get { return pairedGamepad; }
        set 
        { 
            pairedGamepad = value;
            SetGamepadInCompanion();
        }
    }

    private CapturedEnemy capturedSlot;

    private bool wasSpawnedInLastLevel = false;
    public bool WasSpawnedInLastLevel 
    { 
        get { return wasSpawnedInLastLevel; } 
        set { wasSpawnedInLastLevel = value; } 
    }

    [SerializeField]
    public GameObject[] enemyPrefabs;

    public void SetEnemyInCaptureSlot(CapturedEnemy capturedEnemy)
    {
        capturedSlot = capturedEnemy;

        GetCompanionControllerReference();
        companionControllerRef.OnEnemyCaptured();
    }

    public void WithDrawToCaptureSlot(CapturedEnemy capturedEnemy)
    {
        capturedSlot = capturedEnemy;
    }

    public void LoadCompanionFromSaveFile()
    {
        GameSaveFile file = SaveFileManager.Instance.LoadedGameSaveFile;

        if (file.hasPlayerCompanion)
        {
            CapturedEnemy companion = new CapturedEnemy(file.companionType,
            file.companionBehavior, file.companionStats);

            capturedSlot = companion;
            GetCompanionControllerReference();
            if (companion.EnemyType == EnemyType.RANGED || companion.EnemyType == EnemyType.RANGED_CRATE)
            {
                Game.Instance.uiHudRef.SetCompanionPortrait(EnemyType.RANGED);
            }
            else
            {
                Game.Instance.uiHudRef.SetCompanionPortrait(EnemyType.MELEE);
            }
        }
    }

    public CapturedEnemy GetEnemyInCaptureSlot()
    {
        return capturedSlot;
    }

    public void ClearCaptureSlot()
    {
        GetCompanionControllerReference();
        capturedSlot = null;
        companionControllerRef.CurrentSpawnedCompanion = null;
    }

    public void ChangeStatsInCaptureSlot(Stats stats)
    {
        capturedSlot.OverwriteStats(stats);
    }

    public GameObject GetEnemyPrefab(EnemyType type)
    {
        foreach (GameObject obj in enemyPrefabs)
        {
            if (obj.GetComponent<Enemy>().EnemyAttackingType == type)
            {
                return obj;
            }
        }
        return null;
    }

    private void GetCompanionControllerReference()
    {
        companionControllerRef = FindObjectOfType<CompanionController>();
    }

    public void SetGamepadInCompanion()
    {
        if (pairedGamepad == null) return;
        GetCompanionControllerReference();
        
        if (companionControllerRef.CurrentSpawnedCompanion == null) return;
        companionGamepadBindings = companionControllerRef.CurrentSpawnedCompanion.GetComponent<CompanionGamepadBindings>();
        if (companionGamepadBindings.gameObject.tag != "Player") return;
        if (companionGamepadBindings == null) return;

        companionGamepadBindings.PairedGamepad = pairedGamepad;
    }

    public void OnLevelLoadEvent()
    {
        
    }

    public void OnLevelUnloadEvent()
    {
        
    }

    public void OnPlayerWasInstanced()
    {
        
    }
}
