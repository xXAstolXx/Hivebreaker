using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour
{
    private Image imageComponent;

    private PlayableDirector playableDirector;

    [SerializeField]
    private Sprite[] playerPortraits;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        playableDirector = GetComponent<PlayableDirector>();
    }

    public void ShowPlayerDamagePortrait()
    {
        imageComponent.sprite = playerPortraits[1];
        playableDirector.Play();
    }

    public void ShowPlayerPortrait() 
    { 
        imageComponent.sprite = playerPortraits[0];
    }
}
