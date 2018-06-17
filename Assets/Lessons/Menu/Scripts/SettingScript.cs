﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScript : MonoBehaviour {

    public float defaultState;
    public Slider globalSlider;
    public Slider musicSlider;
    public Slider soundSlider;
    public Toggle particalToggle;
    public Dropdown dropdownTypeLevel;

	// Use this for initialization
	void Start () {
		if (LoadLevel.GlobalVolume == 0)
        {
            globalSlider.value = musicSlider.value = soundSlider.value = defaultState;
            LoadLevel.GlobalVolume = LoadLevel.MusicVolume = LoadLevel.SoundVolume = defaultState;
            particalToggle.isOn = LoadLevel.Partical = true;
            dropdownTypeLevel.value = 0;
            LoadLevel.LevelDetal = (LevelDetal)0;
        }
        globalSlider.onValueChanged.AddListener(delegate { GlobalSliderValueChanged(); });
        musicSlider.onValueChanged.AddListener(delegate { MusicSliderValueChanged(); });
        soundSlider.onValueChanged.AddListener(delegate { SoundSliderValueChanged(); });
        particalToggle.onValueChanged.AddListener(delegate { ParticalToggleChanged(); });
        dropdownTypeLevel.onValueChanged.AddListener(delegate { TypeLevelValueChanged(); });
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void GlobalSliderValueChanged()
    {
        LoadLevel.GlobalVolume = globalSlider.value;
    }
    public void MusicSliderValueChanged()
    {
        LoadLevel.MusicVolume = musicSlider.value;
    }
    public void SoundSliderValueChanged()
    {
        LoadLevel.SoundVolume = soundSlider.value;
    }
    public void ParticalToggleChanged()
    {
        LoadLevel.Partical = particalToggle.isOn;
        dropdownTypeLevel.interactable = particalToggle.isOn;
    }
    public void TypeLevelValueChanged()
    {
        LoadLevel.LevelDetal = (LevelDetal)dropdownTypeLevel.value;
    }
}
