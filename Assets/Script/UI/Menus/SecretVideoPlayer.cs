using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SecretVideoPlayer : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Image secretImage;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            backgroundImage.gameObject.SetActive(false);
            videoPlayer.gameObject.SetActive(true);
            secretImage.gameObject.SetActive(true);
            
        }
    }
}
