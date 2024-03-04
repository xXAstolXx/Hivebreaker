using UnityEngine;
using UnityEngine.Audio;

public enum AudioEffect
{
    VOLUME,
    REVERB,
    ECHO
}

public enum MixerGroup
{
    NONE,
    CHANNEL1,
    CHANNEL2,
    CHANNEL3,
    SFX,
    ENVIRONMENT
}

public enum AudioTracks
{
    NONE,
    MAIN_LONG,
    MAIN_SHORT,
    COMBAT_V1,
    COMBAT_V1_2,
    COMBAT_V2,
    COMBAT_V1_TRANSITION,
    COMBAT_V2_TRANSITION,
    RELAX,
    RELAX_SHORT,
    BONUS
}

public class MusicPlayer : MonoBehaviour, ILevelLoadEvent
{
    [SerializeField]
    private AudioClip[] audioClips;

    [SerializeField]
    private AudioSource[] audioSource;

    [SerializeField]
    private AudioMixer audioMixer;

    private AudioTracks trackLoaded1 = AudioTracks.NONE;
    public AudioTracks TrackLoaded1 { get; }

    private AudioTracks trackLoaded2 = AudioTracks.NONE;
    public AudioTracks TrackLoaded2 { get; }

    private AudioTracks trackLoaded3 = AudioTracks.NONE;
    public AudioTracks TrackLoaded3 { get; }

    private void Start()
    {
        InitialSettings();
    }

    private void InitialSettings()
    {
        SetEffectActive(MixerGroup.CHANNEL1, AudioEffect.REVERB, false);
        SetEffectActive(MixerGroup.CHANNEL1, AudioEffect.ECHO, false);
        SetEffectActive(MixerGroup.CHANNEL2, AudioEffect.REVERB, false);
        SetEffectActive(MixerGroup.CHANNEL2, AudioEffect.ECHO, false);
        SetEffectActive(MixerGroup.CHANNEL3, AudioEffect.REVERB, false);
        SetEffectActive(MixerGroup.CHANNEL3, AudioEffect.ECHO, false);
        SetEffectActive(MixerGroup.SFX, AudioEffect.ECHO, false);
        SetEffectActive(MixerGroup.SFX, AudioEffect.REVERB, false);
        SetEffectActive(MixerGroup.ENVIRONMENT, AudioEffect.REVERB, false);
        SetEffectActive(MixerGroup.ENVIRONMENT, AudioEffect.ECHO, false);
    }

    public void SetupAudioByKey(string key)
    {
        foreach (GameMusicEvent musicEvent in GetComponentsInChildren<GameMusicEvent>())
        {
            musicEvent.OnTriggerKey(key);
        }
    }

    public void CrossfadeTo(MixerGroup toGroup, float time)
    {
        GetMixerGroupSnapshot(toGroup).TransitionTo(time);
    }

    public void CrossfadeTo(MixerGroup toGroup1, MixerGroup toGroup2, float time)
    {
        GetMixerGroupSnapshot(toGroup1, toGroup2).TransitionTo(time);
    }

    private AudioMixerSnapshot GetMixerGroupSnapshot(MixerGroup toGroup)
    {
        if (toGroup == MixerGroup.SFX || toGroup == MixerGroup.ENVIRONMENT)
        {
            Debug.Log("Can't transition SFX! " + toGroup.ToString());
            return null;
        }

        if (toGroup == MixerGroup.NONE)
        {
            return audioMixer.FindSnapshot("None_Active");
        }

        string snapshotName = "Default";

        switch (toGroup)
        {
            case MixerGroup.CHANNEL1:
                snapshotName = "CH1";
                break;
            case MixerGroup.CHANNEL2:
                snapshotName = "CH2";
                break;
            case MixerGroup.CHANNEL3:
                snapshotName = "CH3";
                break;
        }
        
        if (snapshotName != "Default")
        {
            snapshotName += "_Active";
        }

        return audioMixer.FindSnapshot(snapshotName);
    }

    private AudioMixerSnapshot GetMixerGroupSnapshot(MixerGroup toGroup1, MixerGroup toGroup2)
    {
        if (toGroup1 == MixerGroup.SFX || toGroup1 == MixerGroup.ENVIRONMENT)
        {
            Debug.Log("Can't transition SFX! " + toGroup1.ToString());
            return null;

        }

        if (toGroup2 == MixerGroup.SFX || toGroup2 == MixerGroup.ENVIRONMENT)
        {
            Debug.Log("Can't transition SFX! " + toGroup2.ToString());
            return null;
        }

        if (toGroup1 == toGroup2)
        {
            Debug.Log("Can't transition to the same group!");
            return null;
        }

        string name = "Default";
        int channel = 0;

        switch (toGroup1)
        {
            case MixerGroup.CHANNEL1:
                channel += 1;
                break;
            case MixerGroup.CHANNEL2:
                channel += 2;
                break;
            case MixerGroup.CHANNEL3:
                channel += 3;
                break;
        }

        switch (toGroup2)
        {
            case MixerGroup.CHANNEL1:
                channel += 1;
                break;
            case MixerGroup.CHANNEL2:
                channel += 2;
                break;
            case MixerGroup.CHANNEL3:
                channel += 3;
                break;
        }

        switch (channel)
        {
            case 3:
                name = "CH12_Active";
                break;
            case 4:
                name = "CH13_Active";
                break;
            case 5:
                name = "CH23_Active";
                break;
        }

        if (name != "Default")
        {
            name += "_Active";
        }
        return audioMixer.FindSnapshot(name);
    }

    public void SetValueInExposed(MixerGroup group, AudioEffect effect, float value)
    {
        audioMixer.SetFloat(BuildEffectName(group, effect), value);
    }

    public float GetValueInExposed(MixerGroup group, AudioEffect effect)
    {
        audioMixer.GetFloat(BuildEffectName(group, effect), out float returnValue);
        return returnValue;
    }

    private string BuildEffectName(MixerGroup group, AudioEffect effect)
    {
        string effectName = "";

        switch (group)
        {
            case MixerGroup.CHANNEL1:
                effectName += "CH1";
                break;
            case MixerGroup.CHANNEL2:
                effectName += "CH2";
                break;
            case MixerGroup.CHANNEL3:
                effectName += "CH3";
                break;
            case MixerGroup.SFX:
                effectName += "SFX";
                break;
            case MixerGroup.ENVIRONMENT:
                effectName += "ENV";
                break;
        }

        effectName += "_";

        switch (effect)
        {
            case AudioEffect.VOLUME:
                effectName += "Volume";
                break;
            case AudioEffect.REVERB:
                effectName += "Reverb";
                break;
            case AudioEffect.ECHO:
                effectName += "Echo";
                break;
        }

        return effectName;
    }

    public void SetEffectActive(MixerGroup group, AudioEffect effect, bool value)
    {
        float effectValue = 0.0f;

        if (!value) 
        { 
            effectValue = -80.0f; 
        }

        SetValueInExposed(group, effect, effectValue);
    }

    private AudioClip GetAudioClipByEnum(AudioTracks track)
    {
        AudioClip audioClip = null;

        switch (track)
        {
            case AudioTracks.NONE:
                break;
            case AudioTracks.MAIN_LONG:
                audioClip = audioClips[0];
                break;
            case AudioTracks.MAIN_SHORT:
                audioClip = audioClips[1];
                break;
            case AudioTracks.COMBAT_V1:
                audioClip = audioClips[2];
                break;
            case AudioTracks.COMBAT_V1_2:
                audioClip = audioClips[3];
                break;
            case AudioTracks.COMBAT_V2:
                audioClip = audioClips[4];  
                break;
            case AudioTracks.COMBAT_V1_TRANSITION:
                audioClip = audioClips[5];
                break;
            case AudioTracks.COMBAT_V2_TRANSITION:
                audioClip = audioClips[6];
                break;
            case AudioTracks.RELAX:
                audioClip = audioClips[7];
                break;
            case AudioTracks.RELAX_SHORT:
                audioClip = audioClips[8];
                break;
            case AudioTracks.BONUS:
                audioClip = audioClips[9];
                break;
        }

        return audioClip;
    }

    public void SetAudioClipToPlayer(AudioTracks track, MixerGroup group)
    {
        if (group == MixerGroup.SFX || group == MixerGroup.ENVIRONMENT)
        {
            Debug.Log("Can't use SFX here!" + group.ToString());
            return;
        }

        switch (group)
        {
            case MixerGroup.CHANNEL1:
                audioSource[0].clip = GetAudioClipByEnum(track);
                trackLoaded1 = track;
                break;
            case MixerGroup.CHANNEL2:
                audioSource[1].clip = GetAudioClipByEnum(track);
                trackLoaded2 = track;
                break;
            case MixerGroup.CHANNEL3:
                audioSource[2].clip = GetAudioClipByEnum(track);
                trackLoaded3 = track;
                break;
        }
    }

    public void PlayTrackInChannel(MixerGroup group, bool isLooping)
    {
        if (group == MixerGroup.SFX || group == MixerGroup.ENVIRONMENT)
        {
            Debug.Log("Can't use SFX here!" + group.ToString());
            return;
        }

        switch (group)
        {
            case MixerGroup.CHANNEL1:
                audioSource[0].Play();
                audioSource[0].loop = isLooping;
                break;
            case MixerGroup.CHANNEL2:
                audioSource[1].Play();
                audioSource[1].loop = isLooping;
                break;
            case MixerGroup.CHANNEL3:
                Debug.Log("Triggered");
                audioSource[2].Play();
                audioSource[2].loop = isLooping;
                break;
        }
    }

    #region ILevelLoadEvent

    public void OnLevelLoadEvent()
    {

    }

    public void OnLevelUnloadEvent()
    {
        
    }

    public void OnPlayerWasInstanced()
    {
        
    }

    #endregion ILevelLoadEvent

}
