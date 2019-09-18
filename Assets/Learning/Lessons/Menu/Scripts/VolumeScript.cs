using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VolumeScript : MonoBehaviour
{

    public TypeAudio typeAudio;

    private AudioSource _audio;

    // Use this for initialization
    void Start()
    {
        _audio = gameObject.GetComponent<AudioSource>();
        if (typeAudio == TypeAudio.Music)
        {
            _audio.volume = LoadLevel.GlobalVolume * LoadLevel.MusicVolume;
        }
        if (typeAudio == TypeAudio.Sound)
        {
            _audio.volume = LoadLevel.GlobalVolume * LoadLevel.SoundVolume;
        }
        LoadLevel.VolumeChanged += Changed;
    }

    private void Changed(TypeAudio typeAudio, float volume)
    {
        if (this.typeAudio == TypeAudio.Music && (this.typeAudio == typeAudio | typeAudio == TypeAudio.Global))
        {
            _audio.volume = LoadLevel.GlobalVolume * LoadLevel.MusicVolume;
        }
        if (this.typeAudio == TypeAudio.Sound && (this.typeAudio == typeAudio | typeAudio == TypeAudio.Global))
        {
            _audio.volume = LoadLevel.GlobalVolume * LoadLevel.SoundVolume;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        LoadLevel.VolumeChanged -= Changed;
    }
}
