using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CompanionPortrait : MonoBehaviour
{
    private Image imageComponent;

    private PlayableDirector playableDirector;

    [SerializeField]
    private Sprite[] meleeCompanion;

    [SerializeField]
    private Sprite[] rangedCompanion;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        playableDirector = GetComponent<PlayableDirector>();
        imageComponent.enabled = false;
    }

    public void SetCompanionPortrait(EnemyType type)
    {
        switch (type) 
        {
            case EnemyType.NONE: 
                imageComponent.sprite = null;
                imageComponent.enabled = false;
                break;
            case EnemyType.MELEE:
                imageComponent.enabled = true;
                imageComponent.sprite = meleeCompanion[0];
                break;
            case EnemyType.RANGED:
                imageComponent.enabled = true;
                imageComponent.sprite = rangedCompanion[0];
                break;
        }
    }

    public void ShowDamagePortrait()
    {
        if (imageComponent.sprite == meleeCompanion[0]) 
        {
            imageComponent.sprite = meleeCompanion[1];
        }
        else if (imageComponent.sprite == rangedCompanion[0])
        {
            imageComponent.sprite = rangedCompanion[1];
        }

        playableDirector.Play();
    }

    public void ShowNormalPortrait()
    {
        if (imageComponent.sprite == meleeCompanion[1]) 
        {
            imageComponent.sprite = meleeCompanion[0];
        }
        else if (imageComponent.sprite == rangedCompanion[1])
        {
            imageComponent.sprite = rangedCompanion[0];
        }
    }
}
