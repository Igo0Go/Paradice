using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel
{
    public Animator gate;
    public Animator[] nombers;
    public GameObject lookAt;

    public SelectLevel(Animator gate, Animator[] nombers, GameObject lookAt)
    {
        this.gate = gate;
        this.nombers = nombers;
        this.lookAt = lookAt;
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

public delegate void SelectHelper(int index);

public class MenuScript : MonoBehaviour {

    public event SelectHelper SelectIndexChanged;
    public float speedMoveCam;
    public float speedAngleCam;

    [SerializeField]
    public int SelectedIndex
    {
        get
        {
            return _selectedIndex;
        }
        set
        {
            _selectedIndex = value;
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
    public GameObject preViewButton;
    public GameObject nextViewButton;

    public GameObject minionLookAt;
    public GameObject simmulationLookAt;
    public GameObject templeLookAt;

    public GameObject gamePanel;
    public GameObject settingPanel;
    public GameObject mainPanel;

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
    private bool _go;
    private int _right;
    private Transform _target;
    private Vector3 _vectorToTarget;
    private AsyncOperation _asyncOperation;

    public void PlayButtonClick()
    {
        _asyncOperation.allowSceneActivation = true;
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

    public void ExitButtonClick()
    {
        Application.Quit();
    }

    public void BackSettingButtonClick()
    {
        mainPanel.SetActive(true);
        settingPanel.SetActive(false);
    }

    public void BackGameButtonClick()
    {
        gamePanel.SetActive(false);
        mainPanel.SetActive(true);
        _selectLevels[SelectedIndex].GateOpen(false);
        _selectLevels[SelectedIndex].HideNombers(false);
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
    //sad
    private void Start()
    {
        gamePanel.SetActive(false);
        mainPanel.SetActive(true);
        settingPanel.SetActive(false);

        _minionGate = minionGate.GetComponent<Animator>();
        _simulationGate = simulationGate.GetComponent<Animator>();
        _templeGate = templeGate.GetComponent<Animator>();

        _minionNombers = minionNombers.GetComponentsInChildren<Animator>();
        _simulationNombers = simulationNombers.GetComponentsInChildren<Animator>();
        _templeNombers = templeNombers.GetComponentsInChildren<Animator>();

        _selectLevels = new SelectLevel[]
        {
            new SelectLevel(_minionGate, _minionNombers,minionLookAt),
            new SelectLevel(_simulationGate, _simulationNombers,simmulationLookAt),
            new SelectLevel(_templeGate, _templeNombers,templeLookAt)
        };

        SelectedIndex = 0;
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
    }

    private void SelectIndexChange(int index)
    {
        _go = true;
        _target = _selectLevels[SelectedIndex].lookAt.transform;
        _vectorToTarget = _target.position - gameObject.transform.position;

    }

    private void Go()
    {
        if ((gameObject.transform.position != _target.position) && (gameObject.transform.rotation != _target.rotation)) 
        {
            if (gameObject.transform.position != _target.position)
            {
                if ((gameObject.transform.position - _target.position).magnitude > speedMoveCam * Time.deltaTime)
                {
                    gameObject.transform.position += _vectorToTarget.normalized * speedMoveCam * Time.deltaTime;
                }
                else
                {
                    gameObject.transform.position = _target.position;
                }
            }
            if (gameObject.transform.rotation != _target.rotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _target.rotation, speedAngleCam);
                //if (Quaternion.Angle(transform.rotation, _target.rotation) >= speedAngleCam*10)
                //{
                //    transform.rotation = Quaternion.Slerp(transform.rotation, _target.rotation, speedAngleCam);
                //}
                //else
                //{
                //    transform.rotation = _target.rotation;
                //    Debug.Log("if (gameObject.transform.rotation != _target.rotation)");
                //}

            }
        }
        else
        {
            _selectLevels[SelectedIndex].GateOpen();
            _selectLevels[SelectedIndex].HideNombers();
            _go = false;
            Debug.Log("if ((gameObject.transform.position != _target.position) && (gameObject.transform.rotation != _target.rotation))");
        }
    }
}
