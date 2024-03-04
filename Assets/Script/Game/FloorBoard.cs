using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloorBoard : MonoBehaviour
{
    private TMP_Text counter;

    private void Awake()
    {
        counter = GetComponentInChildren<TMP_Text>();
    }

    void Start()
    {
        int shownNumber = Game.Instance.highscoreSystemRef.ClearedFloors + 1;

        counter.text = shownNumber.ToString();
    }
}
