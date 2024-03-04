
using UnityEngine;

public class DialogInteractable : Interactable
{
    [SerializeField]
    private GameObject speechBubble;

    [SerializeField]
    private string[] textFields;

    [Header("Will break line after given value. 0 disables this.")]
    [SerializeField]
    private int maxCharPerLine = 30;

    [SerializeField]
    private float fontSize = 0.5f;

    private int textFieldCounter = -1;

    protected override void DisplayWindow(bool ShowWindow)
    {
        base.DisplayWindow(ShowWindow);
    }

    protected override void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        interactName = "DIALOG_INTERACT";
        speechBubble.SetActive(false);
    }

    public override void InteractWith()
    {
        base.InteractWith();

        if (speechBubble.activeSelf == false)
        {
            speechBubble.SetActive(true);
            otherObject.GetComponent<PlayerMovement>().SetMovementEnabled(false);
            Game.Instance.gameObject.GetComponent<PauseMenu>().canBePaused = false;

            AdvanceDialog();
        }
        else
        {
            AdvanceDialog();
        }
    }

    private void AdvanceDialog()
    {
        textFieldCounter++;

        if (textFieldCounter >= textFields.Length)
        {
            speechBubble.SetActive(false);
            otherObject.GetComponent<PlayerMovement>().SetMovementEnabled(true);
            Game.Instance.gameObject.GetComponent<PauseMenu>().canBePaused = true;
            textFieldCounter = -1;
        }
        else
        {
            speechBubble.GetComponent<DialogBox>().SetTextSize(fontSize);
            speechBubble.GetComponent<DialogBox>().WriteTextToDialogBox(textFields[textFieldCounter], maxCharPerLine);
        }
    }
}
