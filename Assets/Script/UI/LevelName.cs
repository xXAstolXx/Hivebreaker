using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelName : MonoBehaviour
{
    public void SetNameInTMP(string name)
    {
        GetComponent<TextMeshProUGUI>().text = name;
    }
}
