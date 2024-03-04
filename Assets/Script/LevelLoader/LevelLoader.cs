using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorPack
{
    FLOOR1_10 = 0,
    FLOOR11_20 = 1,
    FLOOR21_30 = 2,
    FLOOR31_Plus = 3
}

public class LevelLoader : MonoBehaviour
{
    private int currentFloorNumber = 0;
    public int CurrentFloorNumber { get { return currentFloorNumber; } }

    private int floorNumberInCurrentPack = 0;

    private int everyTenFloor = 0;
    public int EveryTenFloor
    { get; private set; }

    private FloorPack floorPack = FloorPack.FLOOR1_10;

    private string currentLevelName;

    private string deletedLevels;

    [HideInInspector]
    public Teleporter teleporter;

    private Stats m_playerStats;

    private bool hasPlayerCaptureBullet = false;
    public bool HasPlayerCaptureBullet 
    { get {  return hasPlayerCaptureBullet; } }

    private float chanceToEndRun = 0;
    public float ChanceToEndRun { get; set; }

    [SerializeField]
    GameObject testLevel;

    [SerializeField]
    List<GameObject> floor1_10 = new List<GameObject>();

    [SerializeField]
    List<GameObject> floor11_20 = new List<GameObject>();

    [SerializeField]
    List<GameObject> floor21_30 = new List<GameObject>();

    [SerializeField]
    List<GameObject> floor31_PLUS = new List<GameObject>();

    [SerializeField]
    private GameObject tutorialLevel;

    public void RememberPlayerStatsInLeveloader(Stats playerStats, bool value)
    {
        if (playerStats != null)
        {
            m_playerStats = playerStats;
        }

        hasPlayerCaptureBullet = value;
    }

    public Stats GetPlayerStats()
    {
        if (m_playerStats == null)
        {
            return null;
        }
        
        return m_playerStats;
    }

    public void OnGameStart()
    {
        if (SaveFileManager.Instance.playingTutorial)
        {
            StartCoroutine(LoadLevel(tutorialLevel));
            SaveFileManager.Instance.playingTutorial = false;
            return;
        }

        UnloadLevel();

        if (SaveFileManager.Instance.SaveDataExists && SaveFileManager.Instance.isContinue)
        {
            LoadLevelFromSaveFile();
            SetPlayerStatsFromSaveFile();
            Game.Instance.captureManagerRef.LoadCompanionFromSaveFile();
            Game.Instance.highscoreSystemRef.SetHighscoreDataFromSaveFile();
        }
        else
        {
            GameObject level = PickRandomLevel();
            currentLevelName = level.name;
            StartCoroutine(LoadLevel(level));
        }

    }

    private void SetPlayerStatsFromSaveFile()
    {
        GameSaveFile file = SaveFileManager.Instance.LoadedGameSaveFile;

        m_playerStats = file.playerStats;
        hasPlayerCaptureBullet = file.hasPlayerCaptureItem;
    }

    private void LoadLevelFromSaveFile()
    {
        GameSaveFile file = SaveFileManager.Instance.LoadedGameSaveFile;

        currentFloorNumber = file.currentFloorNumber;
        floorNumberInCurrentPack = file.floorNumberInCurrentPack;
        floorPack = file.floorPack;
        currentLevelName = file.loadedLevelName;

        deletedLevels = file.deletedLevelsInPackName;

        List<string> allDeletedLevels = new List<string>();
        List<GameObject> currentFloorSet = PickListByFloorPack(floorPack);
        List<GameObject> toBeDeleted = new List<GameObject>();

        foreach (string deletedLevel in deletedLevels.Split(','))
        {
            allDeletedLevels.Add(deletedLevel);

            foreach (GameObject floor in currentFloorSet)
            {
                if (floor.name == deletedLevel)
                {
                    toBeDeleted.Add(floor);
                }
            }
        }

        foreach (GameObject floor in toBeDeleted)
        {
            if (floorPack != FloorPack.FLOOR31_Plus)
            {
                if (currentFloorSet.Contains(floor))
                {
                    currentFloorSet.Remove(floor);
                }
            }
        }

        foreach (GameObject floor in currentFloorSet)
        {
            if (floor.name == currentLevelName)
            {
                StartCoroutine(LoadLevel(floor));
                currentLevelName = floor.name;
                if (floorPack != FloorPack.FLOOR31_Plus)
                {
                    currentFloorSet.Remove(floor);
                }
                break;
            }
        }

    }

    private GameObject PickRandomLevel()
    {
        if (testLevel != null)
        {
            return testLevel;
        }

        GameObject levelPrefab = floor1_10[0];

        int randomIndex;

        List<GameObject> currentFloorSet = PickListByFloorPack(floorPack);

        randomIndex = UnityEngine.Random.Range(0, currentFloorSet.Count);
        levelPrefab = currentFloorSet[randomIndex];

        if (floorPack != FloorPack.FLOOR31_Plus)
        {
            currentFloorSet.Remove(levelPrefab);
        }

        return levelPrefab;
    }

    private List<GameObject> PickListByFloorPack(FloorPack floorPack)
    {
        switch (floorPack)
        {
            case FloorPack.FLOOR1_10:
                return floor1_10;
            case FloorPack.FLOOR11_20:
                return floor11_20;
            case FloorPack.FLOOR21_30:
                return floor21_30;
            case FloorPack.FLOOR31_Plus:
                return floor31_PLUS;
        }
        return null;
    }

    public void OnLevelCompleted()
    {
        currentFloorNumber++;
        floorNumberInCurrentPack++;
        if (floorNumberInCurrentPack == 9) 
        {
            deletedLevels = "";
            floorNumberInCurrentPack = 0;
            floorPack++;
            EveryTenFloor += 1;
        }
        Game.Instance.highscoreSystemRef.ClearedFloors++;

        if (floorPack != FloorPack.FLOOR31_Plus)
        {
            deletedLevels += currentLevelName + ",";
        }



        GameObject level = PickRandomLevel();
        currentLevelName = level.name;

        SaveProgressInGame();
        ChangeLevel(level);
        IncreaseChanceToEndRun();
    }

    private void IncreaseChanceToEndRun()
    {
        if (floorPack == FloorPack.FLOOR31_Plus)
        {
            float chanceIncrease = 1.0f + ((float)currentFloorNumber / 50.0f);
            ChanceToEndRun += chanceIncrease;
            ChanceToEndRun = Mathf.Clamp(ChanceToEndRun, 0.0f, 100.0f);
        }
    }

    public FloorPack GetCurrentFloorPackEntry()
    {
        return floorPack;
    }

    private IEnumerator LoadLevel(GameObject levelPrefab)
    {
        Instantiate(levelPrefab, transform);
        Game.Instance.uiHudRef.SetLevelNameInUI(currentLevelName);
        yield return new WaitForEndOfFrame();
        Game.Instance.OnLevelPrefabLoaded();
        yield break;
    }

    private void UnloadLevel()
    {
        Game.Instance.OnLevelUnloaded();
        DestroyChildren();
    }

    private void ChangeLevel(GameObject levelPrefab)
    {
        UnloadLevel();
        StartCoroutine(LoadLevel(levelPrefab));
    }

    private void DestroyChildren()
    {
        foreach (Transform instancedObject in GetComponentsInChildren<Transform>())
        {
            if (instancedObject.gameObject == gameObject) continue;
            Destroy(instancedObject.gameObject);
        }
    }

    private void SaveProgressInGame()
    {
        if (SaveFileManager.Instance == null) return;

        bool hasPlayerCompanion;
        CapturedEnemy companion;
        if (Game.Instance.captureManagerRef.GetEnemyInCaptureSlot() != null)
        {
            companion = Game.Instance.captureManagerRef.GetEnemyInCaptureSlot();
            hasPlayerCompanion = true;
        }
        else
        {
            Stats dummyStats = new Stats();
            companion = new CapturedEnemy(EnemyType.NONE, AIBehavior.NONE, dummyStats);
            hasPlayerCompanion = false;
        }

        int killCounter = Game.Instance.highscoreSystemRef.KillCounter;
        TimeSpan duration = Game.Instance.highscoreSystemRef.CalculateRunTime();

        GameSaveFile saveFile = new GameSaveFile(currentFloorNumber, floorNumberInCurrentPack, floorPack,
            currentLevelName, deletedLevels, 0, m_playerStats, companion,
            hasPlayerCaptureBullet, hasPlayerCompanion, killCounter, 0, duration);

        SaveFileManager.Instance.SaveClassToJson<GameSaveFile>(saveFile, "saveData");
        SaveFileManager.Instance.SaveDataExists = true;
    }
}
