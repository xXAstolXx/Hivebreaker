using System;

public class GameSaveFile
{
    public int currentFloorNumber;
    public int floorNumberInCurrentPack;
    public FloorPack floorPack;

    public string loadedLevelName;
    public string deletedLevelsInPackName;
    public float chanceToEndRun;

    public Stats playerStats;
    public Stats companionStats;
    public EnemyType companionType;
    public AIBehavior companionBehavior;

    public bool hasPlayerCaptureItem;
    public bool hasPlayerCompanion;
    public int killCounter;

    public int bonusScore;

    public int minutes;
    public int seconds;
    public int milliseconds;

    public bool hasTutorialBeenPlayed;

    public GameSaveFile(int currentFloorNumber, int floorNumberInCurrentPack, FloorPack floorPack,
        string loadedLevelName, string deletedLevelsInPackName, float chanceToEndRun, Stats playerStats,
        CapturedEnemy companion, bool hasPlayerCaptureItem,
        bool hasPlayerCompanion, int killCounter, int bonusScore, TimeSpan duration)
    {
        this.currentFloorNumber = currentFloorNumber;
        this.floorNumberInCurrentPack = floorNumberInCurrentPack;
        this.floorPack = floorPack;
        this.loadedLevelName = loadedLevelName;
        this.deletedLevelsInPackName = deletedLevelsInPackName;
        this.chanceToEndRun = chanceToEndRun;
        this.playerStats = playerStats;
        this.companionStats = companion.Stats;
        this.companionType = companion.EnemyType;
        this.companionBehavior = companion.AIBehavior;
        this.companionStats = companion.Stats;
        this.hasPlayerCaptureItem = hasPlayerCaptureItem;
        this.hasPlayerCompanion = hasPlayerCompanion;
        this.killCounter = killCounter;
        this.bonusScore = bonusScore;
        this.minutes = duration.Minutes;
        this.seconds = duration.Seconds;
        this.milliseconds = duration.Milliseconds;
        
    }
}
