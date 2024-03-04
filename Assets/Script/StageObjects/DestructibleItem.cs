using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.VFX;

public class DestructibleItem : MonoBehaviour
{
    [SerializeField]
    private float emptyDropChance;

    [SerializeField]
    private float dropChanceHealthSmall;

    [SerializeField]
    private GameObject healthSmall;

    [SerializeField]
    private float dropChanceHealthBig;

    [SerializeField]
    private GameObject healthBig;

    [SerializeField]
    private float dropChanceCapsule;

    [SerializeField]
    private GameObject capsule;

    [SerializeField]
    private Material[] materials;

    [SerializeField]
    private Mesh[] meshes;

    private MeshRenderer meshRenderer;

    private MeshFilter meshFilter;

    private VisualEffect effect;

    private int randomdropIndex;

    private List<SFXPlayer> playerSFX = new List<SFXPlayer>();


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        RandomModel();
        GetSFXPlayers();
    }

    private void RandomModel()
    {
        meshFilter.mesh = meshes[Random.Range(0, meshes.Length)];
        meshRenderer.material = materials[Random.Range(0, materials.Length)];
        effect = GetComponentInChildren<VisualEffect>();
    }

    private void GetSFXPlayers()
    {
        foreach (SFXPlayer audioPlayer in GetComponentsInChildren<SFXPlayer>())
        {
            playerSFX.Add(audioPlayer);
        }
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in playerSFX)
        {
            audioplayer.PlayOnTriggerKey(key);
        }
    }

    public void OnDestructibleItemHit()
    {
        DropItem(RandomizeDrop());
        DestroyGameObject();
    }

    private void DropItem(GameObject item)
    {
        if (item == null) return;

        Instantiate(item,transform.position, Quaternion.identity, GetParentTransform());
        DestroyGameObject();
    }

    private GameObject RandomizeDrop()
    {
        float randomPercent = Random.Range(0.0f, 100.0f);

        if (randomPercent < dropChanceCapsule)
        {
            if (Game.Instance.captureManagerRef.GetEnemyInCaptureSlot() == null) return null;

            return capsule;
        }
        
        if (randomPercent < dropChanceHealthSmall + dropChanceCapsule)
        {
            return healthSmall;
        }


        if (randomPercent < dropChanceHealthBig + dropChanceHealthSmall + dropChanceCapsule)
        {
            return healthBig;
        }

        return null;
    }

    private void DestroyGameObject()
    {
        PlaySFXByTriggerKey("DESTROYED");
        effect.Play();
        foreach (Component component in GetComponents<Component>())
        {
            if (component == component.GetComponent<Transform>()) continue;

            Destroy(component);
        }
        
    }

    private Transform GetParentTransform()
    {
        return GetComponentsInParent<Transform>()[1];
    }

}
