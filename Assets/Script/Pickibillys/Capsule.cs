using UnityEngine;

public class Capsule : MonoBehaviour
{
    [SerializeField]
    private SFXPlayer[] playersSFX;

    private void OnTriggerEnter(Collider other)
    {
        var companionController = other.GetComponent<CompanionController>();

        if (companionController != null)
        {
            if (companionController.CurrentSpawnedCompanion == null) return;

            companionController.OnCapsuleItemCollected();
            PlaySFXByTriggerKey("PICK");
            DestroyGameObject();
        }
    }

    private void DestroyGameObject()
    {
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
