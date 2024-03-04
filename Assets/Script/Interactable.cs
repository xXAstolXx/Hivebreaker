using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected UI_HUD ui;

    [SerializeField]
    private float interactRadius;

    [SerializeField]
    private Color gizmosColor;

    [SerializeField]
    protected GameObject interactKeyImage;

    private Gizmos gizmos;

    public SphereCollider sphereCollider;

    protected GameObject otherObject;

    protected bool isInteractable = true;

    protected string interactName;


    protected virtual void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        ui = Game.Instance.uiHudRef;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!isInteractable) return;

        if (other.gameObject.GetComponent<Player>() != null)
        {
            otherObject = other.gameObject;

            other.GetComponent<Player>().SetInteractItem(gameObject, interactName);

            ShowInteractKey(true);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            otherObject = null;

            other.GetComponent<Player>().ClearInteractItem();

            ShowInteractKey(false);
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireSphere(transform.position, interactRadius * transform.localScale.x);
    }

    protected void OnValidate()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = interactRadius;
    }

    protected virtual void DisplayWindow(bool ShowWindow)
    {

    }

    public virtual void InteractWith()
    {
        if (otherObject == null) return;
    }

    protected virtual void ShowInteractKey(bool value)
    {
        if (interactKeyImage == null) return;
        interactKeyImage.SetActive(value);
    }

}
