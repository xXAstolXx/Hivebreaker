using UnityEngine;

enum Channel
{
    NONE,
    CHANNEL1,
    CHANNEL2,
    CHANNEL3
}

public class GameMusicEvent : MonoBehaviour
{
    private MusicPlayer musicPlayer;

    [Header("Set a trigger key")]
    [Header("Use CAPS LOCK and underscore!")]
    [SerializeField]
    private string triggerKey;

    [Header("---Select Tracks---")]
    [SerializeField]
    private bool isChangingTracks = false;

    [Header("This will just load the file, not actually play it")]
    [Header("Can be none")]

    [Header("Select a track to put in Channel 1")]
    [SerializeField]
    private AudioTracks track1;

    [Header("Select a track to put in Channel 2")]
    [SerializeField]
    private AudioTracks track2;

    [Header("Select a track to put in Channel 3")]
    [SerializeField] 
    private AudioTracks track3;

    [Header("---Crossfade---")]

    [SerializeField]
    private bool isCrossfading = false;

    [Header("Select fade duration")]
    [SerializeField, Range(0.1f, 30.0f)]
    private float fadeTime = 1.0f;

    [Header("Select channels to crossfade")]

    [Header("Select a channel that is supposed to Fade in")]
    [Header("This will fade from whatever is playing right now")]
    [Header("to whatever is selected here")]
    [Header("Both to NONE will fade ALL out")]

    [SerializeField]
    private Channel channelToFade1;
    
    [SerializeField]
    private Channel channelToFade2;

    [Header("---Audio Effects---")]
    [Header("CAN'T BE USED WHEN FADING!")]

    [SerializeField]
    private bool isChangingEffects = false;

    [Header("Set effects on Mixers")]
    [Header("The value determines the passed dB")]
    [Header("-80.0 will turn the effect off")]

    [SerializeField]
    private MixerGroup mixerGroup1;

    [SerializeField]
    private AudioEffect effect1 = AudioEffect.VOLUME;

    [SerializeField, Range(-80.0f, 0.0f)]
    private float value1 = 0.0f;

    [SerializeField]
    private MixerGroup mixerGroup2;

    [SerializeField]
    private AudioEffect effect2 = AudioEffect.VOLUME;

    [SerializeField, Range(-80.0f, 0.0f)]
    private float value2 = 0.0f;

    [SerializeField]
    private MixerGroup mixerGroup3;

    [SerializeField]
    private AudioEffect effect3 = AudioEffect.VOLUME;

    [SerializeField, Range(-80.0f, 0.0f)]
    private float value3 = 0.0f;

    [SerializeField]
    private MixerGroup mixerGroup4;

    [SerializeField]
    private AudioEffect effect4 = AudioEffect.VOLUME;

    [SerializeField, Range(-80.0f, 0.0f)]
    private float value4 = 0.0f;

    [SerializeField]
    private MixerGroup mixerGroup5;

    [SerializeField]
    private AudioEffect effect5 = AudioEffect.VOLUME;

    [SerializeField, Range(-80.0f, 0.0f)]
    private float value5 = 0.0f;

    [Header("---Play Tracks---")]

    [SerializeField]
    private bool startPlayer1 = false;

    [SerializeField]
    private bool isLoopPlayer1 = false;

    [SerializeField]
    private bool startPlayer2 = false;

    [SerializeField]
    private bool isLoopPlayer2 = false;

    [SerializeField]
    private bool startPlayer3 = false;

    [SerializeField]
    private bool isLoopPlayer3 = false;

    private void Awake()
    {
        musicPlayer = GetComponentInParent<MusicPlayer>();
    }

    public void OnTriggerKey(string key)
    {
        if (key != triggerKey) return;

        if (isChangingTracks)
        {
            SetAudioClipsToPlayer();
        }

        if (isCrossfading)
        {
            CrossFade();
        }

        if (isChangingEffects && !isCrossfading)
        {
            SetAudioEffects();
        }

        if (startPlayer1)
        {
            musicPlayer.PlayTrackInChannel(MixerGroup.CHANNEL1, isLoopPlayer1);
        }

        if (startPlayer2)
        {
            musicPlayer.PlayTrackInChannel(MixerGroup.CHANNEL2, isLoopPlayer2);
        }

        if (startPlayer3)
        {
            musicPlayer.PlayTrackInChannel(MixerGroup.CHANNEL3, isLoopPlayer3);
        }
    }

    private void SetAudioClipsToPlayer()
    {
        musicPlayer.SetAudioClipToPlayer(track1, MixerGroup.CHANNEL1);
        musicPlayer.SetAudioClipToPlayer(track2, MixerGroup.CHANNEL2);
        musicPlayer.SetAudioClipToPlayer(track3, MixerGroup.CHANNEL3);
    }

    private void CrossFade()
    {
        int channelsSelected1 = 0;
        int channelsSelected2 = 0;

        if (channelToFade1 != Channel.NONE && channelToFade2 != Channel.NONE)
        {
            if (channelToFade1 == channelToFade2)
            {
                Debug.Log("Can't fade to the same channel!");
                return;
            }
        }

        switch (channelToFade1)
        {
            case Channel.CHANNEL1:
                channelsSelected1 += 1;
                break;
            case Channel.CHANNEL2:
                channelsSelected1 += 2;
                break;
            case Channel.CHANNEL3:
                channelsSelected1 += 3;
                break;
        }

        switch (channelToFade2)
        {
            case Channel.CHANNEL1:
                channelsSelected2 += 10;
                break;
            case Channel.CHANNEL2:
                channelsSelected2 += 20;
                break;
            case Channel.CHANNEL3:
                channelsSelected2 += 30;
                break;
        }

        int selectedResult = channelsSelected1 + channelsSelected2;

        switch (selectedResult) 
        { 
            case 0:
                musicPlayer.CrossfadeTo(MixerGroup.NONE, fadeTime);
                break;
            case 1:
                musicPlayer.CrossfadeTo(MixerGroup.CHANNEL1, fadeTime);
                break;
            case 2:
                musicPlayer.CrossfadeTo(MixerGroup.CHANNEL2, fadeTime);
                break;
            case 3:
                musicPlayer.CrossfadeTo(MixerGroup.CHANNEL3, fadeTime);
                break;
            case 21:
                musicPlayer.CrossfadeTo(MixerGroup.CHANNEL1, MixerGroup.CHANNEL2, fadeTime);
                break;
            case 31:
                musicPlayer.CrossfadeTo(MixerGroup.CHANNEL1, MixerGroup.CHANNEL3, fadeTime);
                break;
            case 32:
                musicPlayer.CrossfadeTo(MixerGroup.CHANNEL2, MixerGroup.CHANNEL3, fadeTime);
                break;
        }
    }

    private void SetAudioEffects()
    {
        if (mixerGroup1 != MixerGroup.NONE)
        {
            musicPlayer.SetValueInExposed(mixerGroup1, effect1, value1);
        }
        if (mixerGroup2 != MixerGroup.NONE)
        {
            musicPlayer.SetValueInExposed(mixerGroup2, effect2, value2);
        }
        if (mixerGroup3 != MixerGroup.NONE)
        {
            musicPlayer.SetValueInExposed(mixerGroup3, effect3, value3);
        }
        if (mixerGroup4 != MixerGroup.NONE)
        {
            musicPlayer.SetValueInExposed(mixerGroup4, effect4, value4);
        }
        if (mixerGroup2 != MixerGroup.NONE)
        { 
        musicPlayer.SetValueInExposed(mixerGroup5, effect5, value5);
        }
    }
}