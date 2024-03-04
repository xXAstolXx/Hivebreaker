using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class FloorCounter : MonoBehaviour
{
    private TMP_Text counterTxt;

    private int currentFloorCount;
    private void Awake()
    {
        counterTxt = GetComponentInChildren<TMP_Text>();
    }

    public void IncreaseFloorNumer()
    {
        int shownNumber = Game.Instance.highscoreSystemRef.ClearedFloors + 1;

        counterTxt.text = shownNumber.ToString();
    }

    public void FloorCounterDisplay(bool value)
    {
        gameObject.SetActive(value);

    }
}
