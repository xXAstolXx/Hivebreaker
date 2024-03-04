using UnityEngine;
using UnityEngine.VFX;

public class SaveArea : MonoBehaviour
{
    private Teleporter teleporter;

    [SerializeField]
    private float interactRadius;

    [SerializeField]
    private Color gizmosColor;

    private Gizmos gizmos;

    public SphereCollider sphereCollider;

    [SerializeField]
    private VisualEffect effect;

    [SerializeField]
    private float dissolveSpeed = 1.0f;

    private void Awake()
    {
        teleporter = GetComponentInParent<Teleporter>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireSphere(transform.position, interactRadius * transform.localScale.x);
    }

    private void OnValidate()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = interactRadius;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!teleporter.IsPlayerInSaveArea) return;

        if(other.gameObject.GetComponent<Player>() != null)
        {
            teleporter.IsPlayerInSaveArea = false;
            effect.SetFloat("DissolveOffset", 0.0f);
            TriggerRoomStartEvents();
        }
    }

    private void Update()
    {
        if (!teleporter.IsPlayerInSaveArea)
        {
            float dissolveValue = effect.GetFloat("DissolveOffset") + (dissolveSpeed * Time.deltaTime);
            effect.SetFloat("DissolveOffset", dissolveValue);
        }
    }

    private void TriggerRoomStartEvents()
    {
        var room = FindObjectOfType<Room>(false);

        room.SetRoomCombatStateInEnemies();
        room.SetRoomCombatStateToMusic();
    }
}
