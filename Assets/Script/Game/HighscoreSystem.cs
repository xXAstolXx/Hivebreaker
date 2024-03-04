using System;
using UnityEngine;

public class HighscoreSystem : MonoBehaviour
{
    private float upTimeSinceStart;

    private int killCounter = 0;
    public int KillCounter 
    { 
        get { return killCounter; } 
        set 
        { 
            killCounter = value; 
            Game.Instance.uiHudRef.UpdateKillCounter(killCounter); 
        } 
    } 

    private int clearedFloors = 0;
    public int ClearedFloors 
    {
        get { return clearedFloors; } 
        set { clearedFloors = value; } 
    }

    private int bonusScore = 0;

    private TimeSpan spanFromSave;

    private void Awake()
    {
        SetGameUpTime();
    }

    public void SetHighscoreDataFromSaveFile()
    {
        GameSaveFile file = SaveFileManager.Instance.LoadedGameSaveFile;

        clearedFloors = file.currentFloorNumber;
        killCounter = file.killCounter;
        bonusScore = file.bonusScore;

        spanFromSave = new TimeSpan(0, 0, file.minutes, file.seconds, file.milliseconds);
    }

    public void SetGameUpTime()
    {
        upTimeSinceStart = Time.time;
    }


    public TimeSpan CalculateRunTime()
    {
        float timeInSeconds = Time.time - upTimeSinceStart;

        int seconds = Mathf.FloorToInt(timeInSeconds);
        int minutes = Mathf.FloorToInt(seconds / 60);
        seconds -= (minutes * 60);

        float milliseconds = timeInSeconds - Mathf.FloorToInt(timeInSeconds);

        string millisecondsString = "000";
        if (milliseconds > 0.0f)
        {
            millisecondsString = milliseconds.ToString("F3");
            millisecondsString = millisecondsString.Split(',')[1];
        }

        TimeSpan duration = new TimeSpan(0, 0, minutes, seconds, int.Parse(millisecondsString));

        if (spanFromSave != null)
        {
            duration += spanFromSave;
        }

        return duration;
    }

    public int CalculateHighScore()
    {
        int tempkillCounter = killCounter * 100;

        int minutes = CalculateRunTime().Minutes;
        int tempFloorCounter;
        if (minutes > 0 )
        {
             tempFloorCounter = clearedFloors / minutes * 100;

        }
        else
        {
            tempFloorCounter = clearedFloors * 100;
        }

        return (tempkillCounter + tempFloorCounter + bonusScore);
    }
}
