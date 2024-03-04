

using UnityEngine;

public class SecretSFXPlayer : MonoBehaviour
{
    [SerializeField]
    private SFXPlayer player;

    [SerializeField]
    private SFXPlayer playerPitched;

    private AudioSource audioSource;
    private string cheatCode = "";

    private int maxCheatCodeLength = 4;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            cheatCode += "F";
            Debug.Log(cheatCode);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            cheatCode += "A";
            Debug.Log(cheatCode);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            cheatCode += "B";
            Debug.Log(cheatCode);

        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            cheatCode += "I";
            Debug.Log(cheatCode);

        }

        if (cheatCode == "FABI")
        {
            audioSource.pitch = 1;
            player.PlayOnTriggerKey("FABI");
            cheatCode = "";
        }

        if (cheatCode == "IBAF")
        {
            playerPitched.PlayOnTriggerKey("IBAF");
            cheatCode = "";
        }

        if (cheatCode.Length > maxCheatCodeLength - 1)
        {
            cheatCode = "";
        }
    }
}
