using UnityEngine;
using UnityEngine.UI;

public class PlayerJoinUI : MonoBehaviour
{
    private Image image;

    private float currentTime;

    private float duration = 0.5f;

    private void Start()
    {
        image = GetComponent<Image>();
        currentTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= currentTime + duration) 
        { 
            ChangeIconVisibility();
            currentTime = Time.time;
        }
    }

    private void ChangeIconVisibility()
    {
        if (image.color == Color.white) 
        {
            image.color = Color.clear;
        }
        else
        {
            image.color = Color.white;
        }
    }
}
