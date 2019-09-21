using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class LoadLevel
{
    public const int countChallenge = 3;

    public static event VolumeHelper VolumeChanged;
    public static event ParticalHelper ParticalChanged;
    public static event CameraHelper FieldOfViewChanged;

    public static string namePlayer = "Anonim";
    public static string name;

    public static float GlobalVolume
    {
        get
        {
            return s_globalVolume;
        }
        set
        {
            s_globalVolume = value;
            VolumeChanged?.Invoke(TypeAudio.Global, value);
        }
    }
    public static float MusicVolume
    {
        get
        {
            return s_musicVolume;
        }
        set
        {
            s_musicVolume = value;
            VolumeChanged?.Invoke(TypeAudio.Music, value);
        }
    }
    public static float SoundVolume
    {
        get
        {
            return s_soundVolume;
        }
        set
        {
            s_soundVolume = value;
            VolumeChanged?.Invoke(TypeAudio.Sound, value);
        }
    }
    public static bool Partical
    {
        get
        {
            return s_partical;
        }
        set
        {
            s_partical = value;
            ParticalChanged?.Invoke(value, LevelDetal);
        }
    }
    public static LevelDetal LevelDetal
    {
        get
        {
            return s_levelDetal;
        }
        set
        {
            s_levelDetal = value;
            ParticalChanged?.Invoke(Partical, value);
        }
    }
    public static float FieldOfView
    {
        get
        {
            return s_fieldOfView;
        }
        set
        {
            s_fieldOfView = value;
            FieldOfViewChanged?.Invoke(value);
        }
    }
    private static float s_globalVolume;
    private static float s_musicVolume;
    private static float s_soundVolume;
    private static bool s_partical;
    private static LevelDetal s_levelDetal;
    private static float s_fieldOfView;
}
//sdsd
public class SelectLevel
{
    public Animator gate;
    public Animator[] nombers;
    public GameObject lookAt;
    public string name;

    public SelectLevel(Animator gate, Animator[] nombers, GameObject lookAt, string name)
    {
        this.gate = gate;
        this.nombers = nombers;
        this.lookAt = lookAt;
        this.name = name;
    }

    public void GateOpen(bool open = true)
    {
        gate.SetBool("Open", open);
    }

    public void HideNombers(bool hide = true)
    {
        foreach(var c in nombers)
        {
            if (hide)
            {
                c.SetTrigger("Hide");
            }
            else
            {
                c.SetTrigger("Show");
            }
        }
    }

}

public enum TypeAudio
{
    Music = 0,
    Sound = 1,
    Global = 2
}

public enum LevelDetal
{
    Low = 0,
    Medium = 1,
    High = 2
}

public delegate void SelectHelper(int index);
public delegate void ParticalHelper(bool partical,LevelDetal levelDetal);
public delegate void VolumeHelper(TypeAudio typeAudio, float level);
public delegate void CameraHelper(float fieldOfView);

public class MenuScript : MonoBehaviour {

    public event SelectHelper SelectIndexChanged;
    public InputField nameField;
    public float speedMoveCam;
    public float speedAngleCam;

    public int SelectedIndex
    {
        get
        {
            return _selectedIndex;
        }
        set
        {
            _selectedIndex = value;
            LoadLevel.name = _selectLevels[value].name;
            SelectIndexChanged?.Invoke(value);
            if (value > 0)
            {
                preViewButton.SetActive(true);
            }
            else
            {
                preViewButton.SetActive(false);
            }
            if (value < 2)
            {
                nextViewButton.SetActive(true);
            }
            else
            {
                nextViewButton.SetActive(false);
            }
        }
    }

    public GameObject camMain;
    public GameObject preViewButton;
    public GameObject nextViewButton;

    public GameObject minionLookAt;
    public GameObject simmulationLookAt;
    public GameObject templeLookAt;

    public GameObject mainPanel;
    public GameObject gamePanel;
    public GameObject settingPanel;
    public GameObject controlPanel;
    public GameObject scorePanel;

    public GameObject minionGate;
    public GameObject simulationGate;
    public GameObject templeGate;

    public GameObject minionNombers;
    public GameObject simulationNombers;
    public GameObject templeNombers;

    
    private int _selectedIndex;

    private Animator _minionGate;
    private Animator _simulationGate;
    private Animator _templeGate;

    private Animator[] _minionNombers;
    private Animator[] _simulationNombers;
    private Animator[] _templeNombers;

    private SelectLevel[] _selectLevels;
    private bool _go = false;
    private int _right;
    private Transform _target;
    private Vector3 _vectorToTarget;
    private AsyncOperation _asyncOperation;

    public void PlayButtonClick()
    {
        _asyncOperation.allowSceneActivation = true;
        Time.timeScale = 1;
    }

    public void GameButtonClick()
    {
        gamePanel.SetActive(true);
        mainPanel.SetActive(false);
        _selectLevels[SelectedIndex].GateOpen();
        _selectLevels[SelectedIndex].HideNombers();
    }

    public void SettingButtonClick()
    {
        mainPanel.SetActive(false);
        settingPanel.SetActive(true);
    }

    public void ControlButtonClick()
    {
        mainPanel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void ScoreButtonClick()
    {
        mainPanel.SetActive(false);
        scorePanel.SetActive(true);
    }

    public void ExitButtonClick()
    {
        Application.Quit();
    }

    public void BackSettingButtonClick()
    {
        mainPanel.SetActive(true);
        settingPanel.SetActive(false);
    }

    public void BackScoreButtonClick()
    {
        mainPanel.SetActive(true);
        scorePanel.SetActive(false);
    }

    public void BackGameButtonClick()
    {
        gamePanel.SetActive(false);
        mainPanel.SetActive(true);
        _selectLevels[SelectedIndex].GateOpen(false);
        _selectLevels[SelectedIndex].HideNombers(false);
    }

    public void BackControlButtonClick()
    {
        mainPanel.SetActive(true);
        controlPanel.SetActive(false);
    }

    public void PreViewButtonClick()
    {
        if (SelectedIndex > 0)
        {
            _selectLevels[SelectedIndex].GateOpen(false);
            _selectLevels[SelectedIndex].HideNombers(false);
            SelectedIndex--;
            _right = -1;
        }
    }

    public void NextViewButtonClick()
    {
        if (SelectedIndex <2)
        {
            _selectLevels[SelectedIndex].GateOpen(false);
            _selectLevels[SelectedIndex].HideNombers(false);
            SelectedIndex++;
            _right = 1;
        }
    }
    public void NameFieldValueChanged()
    {
        LoadLevel.namePlayer = nameField.text;
    }
    //sad
    private void Start()
    {
        gamePanel.SetActive(false);
        mainPanel.SetActive(true);
        settingPanel.SetActive(false);
        controlPanel.SetActive(false);
        scorePanel.SetActive(false);

        _minionGate = minionGate.GetComponent<Animator>();
        _simulationGate = simulationGate.GetComponent<Animator>();
        _templeGate = templeGate.GetComponent<Animator>();

        _minionNombers = minionNombers.GetComponentsInChildren<Animator>();
        _simulationNombers = simulationNombers.GetComponentsInChildren<Animator>();
        _templeNombers = templeNombers.GetComponentsInChildren<Animator>();

        _selectLevels = new SelectLevel[]
        {
            new SelectLevel(_minionGate, _minionNombers,minionLookAt,"RaycastAnim"),
            new SelectLevel(_simulationGate, _simulationNombers,simmulationLookAt,"UICam"),
            new SelectLevel(_templeGate, _templeNombers,templeLookAt,"RaycastParticle")
        };
        nameField.text = LoadLevel.namePlayer + ".";
        SelectedIndex = 0;
        nameField.onValueChanged.AddListener(delegate { NameFieldValueChanged(); });
        LoadLevel.name = _selectLevels[SelectedIndex].name;
        _go = false;
        _right = 0;
        SelectIndexChanged += SelectIndexChange;
        _asyncOperation = SceneManager.LoadSceneAsync("LOADING!!");
        _asyncOperation.allowSceneActivation = false;
    }

    private void Update()
    {
        if (_go)
        {
            Go();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePanel.activeSelf)
            {
                BackGameButtonClick();
            }
            if (settingPanel.activeSelf)
            {
                BackSettingButtonClick();
            }
        }
    }

    private void SelectIndexChange(int index)
    {
        _go = true;
        _target = _selectLevels[SelectedIndex].lookAt.transform;
        _vectorToTarget = _target.position - camMain.gameObject.transform.position;

    }

    private void Go()
    {
        if ((camMain.gameObject.transform.position != _target.position) || (camMain.gameObject.transform.rotation != _target.rotation)) 
        {
            if (camMain.gameObject.transform.position != _target.position)
            {
                if ((camMain.gameObject.transform.position - _target.position).magnitude > speedMoveCam * Time.deltaTime)
                {
                    camMain.gameObject.transform.position += _vectorToTarget.normalized * speedMoveCam * Time.deltaTime;
                }
                else
                {
                    camMain.gameObject.transform.position = _target.position;
                }
            }
            if (camMain.gameObject.transform.rotation != _target.rotation)
            {
                camMain.transform.rotation = Quaternion.Slerp(camMain.transform.rotation, _target.rotation, speedAngleCam);
            }
        }
        else
        {
            _selectLevels[SelectedIndex].GateOpen();
            _selectLevels[SelectedIndex].HideNombers();
            _go = false;
        }
    }
}
