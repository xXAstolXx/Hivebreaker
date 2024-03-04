using UnityEngine;

public class VoiceLineTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject speechBubble;

    private float showTime = 3.0f;

    private float currentTime = 0.0f;

    private void Update()
    {
        if (currentTime > 0.0f) 
        {
            if (Time.time >= currentTime + showTime)
            {
                currentTime = 0.0f;
                ShowBubble(false, "");
            }
        }
    }

    public void ShowBubble(bool value, string text) 
    {
        currentTime = Time.time;

        speechBubble.SetActive(value);
        speechBubble.GetComponent<DialogBox>().WriteTextToDialogBox(text, 25);
    }

}
