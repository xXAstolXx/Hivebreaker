
using UnityEngine;

public class CaptureItem : Interactable
{
    [SerializeField]
    private SFXPlayer[] playersSFX;

    [SerializeField]
    private CaptureBullet captureBullet;

    protected override void Start()
    {
        base.Start();

        interactName = "CAPTURE_ITEM";
    }

    public override void InteractWith()
    {
        base.InteractWith();

        var player = otherObject.GetComponent<Player>();
        if (player.HasCaptureBullet) return;

        player.HasCaptureBullet = true;
        player.IsCaptureBulletReady = true;

        interactKeyImage.SetActive(false);

        DestroyItem();
    }

    protected override void ShowInteractKey(bool value)
    {
        if (otherObject == null)
        {
            base.ShowInteractKey(false);
            return;
        }
        var player = otherObject.GetComponent<Player>();
        if (player == null) return;

        if (player.HasCaptureBullet)
        {
            base.ShowInteractKey(false);
            return;
        }

        base.ShowInteractKey(value);
    }

    private void DestroyItem()
    {
        PlaySFXByTriggerKey("PICK");
        Destroy(gameObject);
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in playersSFX)
        {
            audioplayer.PlayOnTriggerKey(key);
        }
    }
}
