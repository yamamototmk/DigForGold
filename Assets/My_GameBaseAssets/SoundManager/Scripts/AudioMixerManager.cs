using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

#pragma warning disable 649
public class AudioMixerManager : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] AudioMixerGroup master, bgm, environment, gameSe, menuSe, voice, jungle;

    public float MasterVolumeByLinear
    {
        get
        {
            return master.GetVolumeByLinear();
        }

        set
        {
            master.SetVolumeByLinear(value);
        }
    }
    public float BgmVolumeByLinear
    {
        get
        {
            return bgm.GetVolumeByLinear();
        }

        set
        {
            bgm.SetVolumeByLinear(value);
        }
    }
    public float GameSeVolumeByLinear
    {
        get
        {
            return gameSe.GetVolumeByLinear();
        }

        set
        {
            gameSe.SetVolumeByLinear(value);
        }
    }

    public AudioMixerGroup GetGameSeAudioMicerGroup()
    {
        return gameSe;
    }
}
