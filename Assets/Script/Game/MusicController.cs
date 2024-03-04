using UnityEngine;

public enum GameState
{
    NONE,
    COMBAT,
    TUTORIAL,
    SPAWNED_FREE,
    FREE_SIDEROOM,
    ROOM_CLEARED,
    BONUS_FLOOR
    
}

public class MusicController : MonoBehaviour
{
    private MusicPlayer musicPlayer;

    private GameState lastState = GameState.NONE;

    private string lastTrack;

    private void Start()
    {
        musicPlayer = Game.Instance.musicPlayerRef;
        SelectMusicByGameState(lastState);
    }

    public void SelectMusicByGameState(GameState gameState)
    {

        if (gameState == lastState) return;

        lastState = gameState;

        switch (gameState)
        {
            case GameState.NONE:
                musicPlayer.SetupAudioByKey("OFF");
                break;
            case GameState.COMBAT:
                lastTrack = RandomizeCombatSong();
                musicPlayer.SetupAudioByKey(lastTrack);
                break;
            case GameState.TUTORIAL:
                musicPlayer.SetupAudioByKey("TUTORIAL");
                break;
            case GameState.SPAWNED_FREE:
                lastTrack = "MAIN_LONG";
                musicPlayer.SetupAudioByKey("MAIN_LONG");
                break;
            case GameState.FREE_SIDEROOM:
                int randomIndex = Random.Range(0, 2);
                switch (randomIndex)
                {
                    case 0:
                        musicPlayer.SetupAudioByKey("MAIN_LONG");
                        break;
                    case 1:
                        musicPlayer.SetupAudioByKey("RELAX");
                        break;
                    case 2:
                        musicPlayer.SetupAudioByKey("OFF");
                        break;
                }
                break;
            case GameState.BONUS_FLOOR:
                musicPlayer.SetupAudioByKey("RELAX");
                break;
            case GameState.ROOM_CLEARED:
                switch (lastTrack)
                {
                    case "LOAD_COMBAT1":
                        musicPlayer.SetupAudioByKey("COMBAT1_END");
                        break;
                    case "LOAD_COMBAT12":
                        musicPlayer.SetupAudioByKey("COMBAT1_END");
                        break;
                    case "LOAD_COMBAT2":
                        musicPlayer.SetupAudioByKey("COMBAT2_END");
                        break;
                    case "MAIN_LONG":
                        musicPlayer.SetupAudioByKey("OFF");
                        break;
                }
                break;
        }
    }


    public string RandomizeCombatSong()
    {
        int randomIndex = Random.Range(0, 3);

        switch (randomIndex) 
        {
            case 0:
                return "LOAD_COMBAT1";
            case 1:
                return "LOAD_COMBAT12";
            case 2:
                return "LOAD_COMBAT2";
        }
        return "";
    }

}
