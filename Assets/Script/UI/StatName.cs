using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatName : MonoBehaviour
{
    private TMP_Text textUI;

    private void Awake()
    {
       textUI = GetComponent<TMP_Text>(); 
    }

    public void WriteText(string info)
    {
        textUI.text = info;
    }
}
