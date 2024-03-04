using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class TwoWayOMatDisplay : MonoBehaviour
{
    [SerializeField]
    private StatName increaseStatName;
    [SerializeField]
    private StatNumber increaseStatNumber;
    [SerializeField]
    private StatIcon increaseStatIcon;

    [SerializeField]
    private StatName decreaseStatName;
    [SerializeField]
    private StatNumber decreaseStatNumber;
    [SerializeField]
    private StatIcon decreaseStatIcon;

    private void Awake()
    {
        //statName = GetComponentInChildren<StatName>();
        //statNumber = GetComponentInChildren<StatNumber>();
        //statIcon = GetComponentInChildren<SetStatIcon>();
    }

    public void SetStatInfos(StatPack increasePack, StatPack decreasePack)
    {
        increaseStatName.WriteText(increasePack.statName);
        increaseStatIcon.SetIcon(increasePack.statName);

        if (increasePack.statName == "MOVESPEED" | increasePack.statName == "BULLET_SPD")
        {
            increaseStatNumber.WriteStatValue(increasePack.statValue.ToShortString());
        }
        else
        {
            increaseStatNumber.WriteStatValue(increasePack.statValue.ToString());
        }

        decreaseStatName.WriteText(decreasePack.statName);
        decreaseStatIcon.SetIcon(decreasePack.statName);

        if (decreasePack.statName == "MOVESPEED" | decreasePack.statName == "BULLET_SPD")
        {
            decreaseStatNumber.WriteStatValue(decreasePack.statValue.ToShortString());
        }
        else
        {
            decreaseStatNumber.WriteStatValue(decreasePack.statValue.ToString());
        }
    }


}
