using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField]
    private SFXPlayer[] playersSFX;

    private DialogWindow window;

    [SerializeField]
    private string[] textFields;

    private int textFieldCounter = -1;

    private Player player;

    private void Awake()
    {
        window = Game.Instance.uiHudRef.GetDialogBoxRef();
        window.WriteTextToDialogBox("");
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<Player>();
        if (player == null) return;

        player.GetComponent<PlayerMovement>().SetMovementEnabled(false);
        player.GetComponent<PlayerMovement>().SetPlayerAnimationIdle();
        player.GetComponent<PlayerMovement>().StopPlayerMovement();
        Game.Instance.gameObject.GetComponent<PauseMenu>().canBePaused = false;

        player.SetDialogTrigger(this);

        window.FadeDialogBox(Fade.IN);

        AdvanceDialog();
    }

    public void AdvanceAction()
    {
        if (window == null) return;

        PlaySFXByTriggerKey("DIALOGUE");
        AdvanceDialog();
        return;
    }

    private void AdvanceDialog()
    {
        textFieldCounter++;

        if (textFieldCounter >= textFields.Length)
        {
            window.FadeDialogBox(Fade.OUT);
            player.GetComponent<PlayerMovement>().SetMovementEnabled(true);
            player.GetComponent<PlayerMovement>().SetPlayerAnimationIdle();
            player.SetDialogTrigger(null);
            Game.Instance.gameObject.GetComponent<PauseMenu>().canBePaused = true;
            Destroy(gameObject);
        }
        else
        {
            window.WriteTextToDialogBox(textFields[textFieldCounter]);
        }
    }

    private void PlaySFXByTriggerKey(string key)
    {
        foreach (SFXPlayer audioplayer in playersSFX)
        {
            audioplayer.PlayOnTriggerKey(key);
        }
    }
}
