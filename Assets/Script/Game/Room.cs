using System.Collections.Generic;
using UnityEngine;

public enum RoomLocation 
{
    CENTER,
    LEFT,
    RIGHT
}

public class Room : MonoBehaviour, ILevelLoadEvent
{
    private Teleporter teleporter;

    private RoomManager roomManager;

    [SerializeField]
    private GameObject lightPack;

    [SerializeField]
    private GameObject postProccesingPack;

    [SerializeField]
    public RoomLocation roomLocation;

    [SerializeField]
    public GameState gameState = GameState.NONE;

    private List<GameObject> enemyList = new List<GameObject>();

    private bool isRoomCleared = false;
    public bool IsRoomCleared { get { return isRoomCleared; } }

    private void Awake()
    {
        if (lightPack != null)
        {
            InstantiatePrefab(lightPack);
        }

        if (postProccesingPack != null)
        {
            InstantiatePrefab(postProccesingPack);
        }
    }

    private void GetAllEnemiesToList()
    {
        enemyList.Clear();
        foreach (Enemy enemy in GetComponentsInChildren<Enemy>())
        {
            if (enemy.gameObject.tag == "Enemy")
            {
                enemyList.Add(enemy.gameObject);
            }
        }

        isRoomCleared = !HasRoomEnemies();
        SetGameState(false);
        SetRoomCombatStateInEnemies();

        SetExits();
    }

    private void SetGameState(bool isEndOfCombat)
    {
        if (gameState == GameState.TUTORIAL) return;
        if (gameState == GameState.BONUS_FLOOR) return;

        if (roomLocation == RoomLocation.CENTER)
        {
            if (!HasRoomEnemies())
            {
                if (isEndOfCombat)
                {
                    gameState = GameState.ROOM_CLEARED;
                }
                else
                {
                    gameState = GameState.SPAWNED_FREE;
                }
            }
            else
            {
                gameState = GameState.COMBAT;
            }
        }
        else
        {
            if (!HasRoomEnemies())
            {
                if (isEndOfCombat)
                { 
                    gameState = GameState.ROOM_CLEARED;
                }
                else
                {
                    gameState = GameState.FREE_SIDEROOM;
                }
            }
            else
            {
                gameState = GameState.COMBAT;
            }
        }
    }

    public void SetRoomCombatStateInEnemies()
    {
        if (teleporter != null)
        {
            if (teleporter.IsPlayerInSaveArea == true) return;
        }

        foreach (GameObject enemy in enemyList)
        {
            enemy.GetComponent<EnemyAI>().SetupEnemyAIByRoomCombatState(enemyList, isRoomCleared);
        }

        foreach (GameObject withTagPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (withTagPlayer.GetComponent<EnemyAI>() != null)
            {
                withTagPlayer.GetComponent<EnemyAI>().SetupEnemyAIByRoomCombatState(enemyList, isRoomCleared);
            }
        }
    }

    public void SetRoomCombatStateToMusic()
    {
        Game.Instance.musicPlayerRef.GetComponent<MusicController>().SelectMusicByGameState(gameState);
    }

    public bool HasRoomEnemies()
    {
        if (enemyList.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemy == null) return;

        enemyList.Remove(enemy);

        isRoomCleared = !HasRoomEnemies();
        SetGameState(true);

        SetRoomCombatStateInEnemies();
        SetRoomCombatStateToMusic();
        TriggerPlayerVoiceLine();

        SetExits();
    }

    private void TriggerPlayerVoiceLine()
    {
        if (isRoomCleared) 
        { 
            FindObjectOfType<Player>().RandomVoiceLine();
        }
    }

    private void SetExits()
    {
        if (!isRoomCleared) return;

        if (teleporter != null)
        {
            teleporter.SetExitReady(true);
        }

        foreach (Door door in GetComponentsInChildren<Door>())
        {
            door.OpenDoor();
        }
        
    }

    private void InstantiatePrefab(GameObject roomObject)
    {
        GameObject instanced = Instantiate(roomObject, transform);
    }

    public void OnRoomEnabled()
    {
        roomManager = Game.Instance.roomManagerRef;
        GetAllEnemiesToList();
        SetRoomCombatStateInEnemies();
        SetRoomCombatStateToMusic();
    }

    #region ILevelLoadEvent

    public void OnLevelLoadEvent()
    {
        
    }

    public void OnLevelUnloadEvent()
    {
        
    }

    public void OnPlayerWasInstanced()
    {
        roomManager = Game.Instance.roomManagerRef;

        if (roomLocation == RoomLocation.CENTER)
        {
            teleporter = GetComponentInChildren<Teleporter>();
            OnRoomEnabled();
        }
    }

    #endregion ILevelLoadEvent

}
