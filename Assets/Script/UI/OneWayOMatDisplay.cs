using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class OneWayOMatDisplay : MonoBehaviour
{
    [SerializeField]
    private StatName statName;
    [SerializeField]
    private StatNumber statNumber;
    [SerializeField]
    private StatIcon statIcon;


    private void Awake()
    {
        //statName = GetComponentInChildren<StatName>();
        //statNumber = GetComponentInChildren<StatNumber>();
        //statIcon = GetComponentInChildren<SetStatIcon>();
    }

    public void SetStatInfos(StatPack pack)
    {
        statName.WriteText(pack.statName);
        statIcon.SetIcon(pack.statName);

        if (pack.statName == "MOVESPEED" | pack.statName == "BULLET_SPD")
        {
            statNumber.WriteStatValue(pack.statValue.ToShortString());
        }
        else
        {
            statNumber.WriteStatValue(pack.statValue.ToString());
        }

        //todo icon
    }


}
