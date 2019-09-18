using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScript : MonoBehaviour {

    public float defaultFieldOfView;
    public float defaultState;
    public Slider globalSlider;
    public Slider musicSlider;
    public Slider soundSlider;
    public Toggle particalToggle;
    public Dropdown dropdownTypeLevel;
    public Slider fieldOfViewSlider;

  
    void Start () {
		if (LoadLevel.GlobalVolume == 0)
        {
            LoadLevel.FieldOfView = defaultFieldOfView;
            LoadLevel.GlobalVolume = LoadLevel.MusicVolume = LoadLevel.SoundVolume = defaultState;
            LoadLevel.Partical = true; 
            LoadLevel.LevelDetal = (LevelDetal)0;
        }
        globalSlider.value = LoadLevel.GlobalVolume;
        musicSlider.value = LoadLevel.MusicVolume;
        soundSlider.value = LoadLevel.SoundVolume;
        fieldOfViewSlider.value = LoadLevel.FieldOfView * 0.01f;
        particalToggle.isOn = LoadLevel.Partical;
        dropdownTypeLevel.value = (int)LoadLevel.LevelDetal;
        globalSlider.onValueChanged.AddListener(delegate { GlobalSliderValueChanged(); });
        musicSlider.onValueChanged.AddListener(delegate { MusicSliderValueChanged(); });
        soundSlider.onValueChanged.AddListener(delegate { SoundSliderValueChanged(); });
        particalToggle.onValueChanged.AddListener(delegate { ParticalToggleChanged(); });
        ParticalToggleChanged();
        dropdownTypeLevel.onValueChanged.AddListener(delegate { TypeLevelValueChanged(); });
        fieldOfViewSlider.onValueChanged.AddListener(delegate { FieldOfViewSliderValueChanged(); });
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
    public void FieldOfViewSliderValueChanged()
    {
        LoadLevel.FieldOfView = fieldOfViewSlider.value*100;
    }
}
