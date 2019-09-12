using System.Collections;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ShootHandler(Vector3 pos);

public class NewRelictusController : MonoBehaviour
{
    public PauseScript pauseScript;
    public event ShootHandler ShootSound;
    public List<ObjectForMission> Keys;
    public List<GameObject> KeysImages;
    public Animator Connect;
    public Animator EnergyFill;
    public Camera Cam;
    public Text InterfaceText;
    public Text MissionText;
    public LineRenderer LineRenderer;
    public Transform GunEnd;
    public AudioSource Music;
    public AudioSource ShootAudio;
    public GameObject VisorPanel;
    public Slider Energy;
    public Slider Health;
    public float Speed;
    public float RotateSpeed;
    public float MaxVert;
    public float MinVert;
    public float EnergyTime;
    public int EnergySpeedCurrent;
    public int EnergySpeedDefault = 2;
    public int EnergySpeedCharge = 10;
    public int EnergySpeedDown = -5;
    public int EnergyShootDown = 4;
    public int ShootDelayMiliseconds = 500;
    public float WaitShootTime = 0;
    public bool Fast;
    public GameObject ShootParticle;
    public CamScript camScript;

    private Animator _camAnim;
    private Animator _anim;
    private CharacterController _controller;
    private WaitForSeconds _lineRendVisTime;
    private Vector3 _moveVector;
    private Vector3 _savePosition;
    private ScoreScript _score;
    private float _speed;
    private float _rotationX;
    private float _rotationY;
    private bool _reforce = false;
    private int _defaultLayerMask;

    public float EnergyValue
    {
        get => Energy.value;
        set => Energy.value = value;
    }
    public bool Reload = false;


    //гравитация
    private float _grav = -9.8f;
    private float _jumpSpeed = 5;
    private float _vertSpeed;

    void Start()
    {
        _score = GetComponent<ScoreScript>();
        _defaultLayerMask = Cam.cullingMask;
        _camAnim = Cam.GetComponent<Animator>();
        VisorPanel.SetActive(false);
        _lineRendVisTime = new WaitForSeconds(WaitShootTime);
        LineRenderer.positionCount = 2;
        Health.value = 100;
        _savePosition = transform.position;
        _speed = Speed;
        EnergySpeedCurrent = EnergySpeedDefault;
        _anim = GetComponent<Animator>();
        InterfaceText.text = "Новая задача";
        Invoke("EmptyInterfaceText", 2);
        EnergyValue = 100;
        _controller = GetComponent<CharacterController>();
        _rotationX = _rotationY = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach (var c in KeysImages)
            c.SetActive(false);
    }

    void Update()
    {
        RelictusSet();
    }
    void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            if (Health.value > 0)
            {
                Shoot();
                RelictusMove();
                MaxSpeed();
                Visor();
            }
            else
                FatlError();
            EnergyChanger();
        }
    }

    void LateUpdate()
    {
        if (Time.timeScale > 0)
            if (Health.value > 0)
                Rotate();
    }

    float x, z;
    bool y;
    private void RelictusSet()
    {
        h = Input.GetAxis("Mouse X");
        vert = Input.GetAxis("Mouse Y");
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        y = Input.GetKeyDown(KeyCode.Space);
        lS = Input.GetKey(KeyCode.LeftShift);
        v = Input.GetKeyDown(KeyCode.V);
        shoot = Input.GetMouseButtonDown(0);
    }
    private void RelictusMove()
    {
        _moveVector = transform.right * x + transform.forward * z;
        if (x != 0 || z != 0)
            _anim.SetFloat("RunWalk", Mathf.Clamp(_moveVector.magnitude * _speed / (Speed * 2), 0, 1));
        else
            _anim.SetFloat("RunWalk", 0);

        if (_controller.isGrounded)
        {
            _vertSpeed = 0;
            if (y)
            {
                _vertSpeed = _jumpSpeed;
                EnergyTime = 0;
            }
        }

        _vertSpeed += _grav * Time.deltaTime;

        _moveVector = new Vector3(_moveVector.x * _speed * Time.fixedDeltaTime, _vertSpeed * Time.deltaTime,
            _moveVector.z * _speed * Time.fixedDeltaTime);

        if (_moveVector != Vector3.zero)
            _controller.Move(_moveVector);

    }
    float h, vert;
    private void Rotate()
    {
        if (h != 0 || vert != 0)
        {
            _rotationX = transform.localEulerAngles.y + h * RotateSpeed;
            _rotationY += vert * RotateSpeed;
            _rotationY = Mathf.Clamp(_rotationY, MinVert, MaxVert);
            transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0);
        }
    }
    bool lS;
    private void MaxSpeed()
    {
        if (lS && _anim.GetFloat("RunWalk") > 0 && !_reforce)
        {
            Fast = true;
            _speed = Speed * 2;
            EnergySpeedCurrent = EnergySpeedDown;
            EnergyTime = 0;
        }
        else
        {
            Fast = false;
            _speed = Speed;
            EnergySpeedCurrent = EnergySpeedDefault;
        }
    }

    private void FatlError()
    {
        _anim.SetTrigger("Fatal");
        Cursor.lockState = CursorLockMode.None;
        if (Input.anyKeyDown)
            pauseScript.RetryButtonClick();
    }

    public void GetSpecKey(ObjectForMission key)
    {
        InterfaceText.text = string.Empty;
        switch (key)
        {
            case ObjectForMission.GreenKey:
                {
                    KeysImages[0].SetActive(true);
                    Keys.Add(key);
                    break;
                }
            case ObjectForMission.YellowKey:
                {
                    KeysImages[1].SetActive(true);
                    Keys.Add(key);
                    break;
                }
            case ObjectForMission.RedKey:
                {
                    KeysImages[2].SetActive(true);
                    Keys.Add(key);
                    break;
                }
        }
    }

    public void RemoveSpecKey(ObjectForMission key)
    {
        switch (key)
        {
            case ObjectForMission.GreenKey:
                {
                    KeysImages[0].SetActive(false);
                    Keys.Remove(key);
                    break;
                }
            case ObjectForMission.YellowKey:
                {
                    KeysImages[1].SetActive(false);
                    Keys.Remove(key);
                    break;
                }
            case ObjectForMission.RedKey:
                {
                    KeysImages[2].SetActive(false);
                    Keys.Remove(key);
                    break;
                }
        }
    }
    bool v;
    public void Visor()
    {
        if (VisorPanel.activeSelf)
        {
            EnergyValue -= 0.1f;
            EnergyTime = 0;
            if (EnergyValue <= 0)
            {
                VisorPanel.SetActive(false);
            }
        }
        if (VisorPanel.activeSelf)
        {
            Cam.cullingMask = -1;
        }
        else
        {
            Cam.cullingMask = _defaultLayerMask;
        }
        if (v)
        {
            VisorPanel.SetActive(!VisorPanel.activeSelf);
        }
    }

    public void MakeSound()
    {
        ShootSound?.Invoke(transform.position);
    }

    public void EmptyInterfaceText()
    {
        InterfaceText.text = string.Empty;
    }

    //выстрелы НАЧАЛО
    bool shoot;
    private async void Shoot()
    {
        if (shoot && !Reload && EnergyValue > EnergyShootDown)
        {
            Reload = true;
            MakeSound();
            EnergyValue -= EnergyShootDown;
            _anim.SetFloat("RunWalk", 0);
            _anim.SetTrigger("Shoot");
            EnergyTime = 0;
            DrowShoot();
            await Delay(ShootDelayMiliseconds, delegate { Reload = false; });
        }
    }

    private async Task Delay(int miliseconddelay,Action action)
    {
        await Task.Delay(miliseconddelay);
        action.Invoke();
        return;
    }

    private void DrowShoot()
    {
        Ray ray = new Ray(GunEnd.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, ~(1 << 2)))
            MakeShoot(hit.point, -hit.normal, hit.collider.GetComponent<Rigidbody>(), hit);
    }

    private void MakeShoot(Vector3 shootPoint, Vector3 shootForce, Rigidbody targetRb, RaycastHit hit)
    {
        LineRenderer.enabled = true;
        LineRenderer.SetPosition(0, GunEnd.position);
        LineRenderer.SetPosition(1, shootPoint);
        TargetDamage(hit.collider.gameObject);
        if (targetRb)
        {
            targetRb.AddForceAtPosition(shootForce * 1000, shootPoint);
        }

        StartCoroutine(HandleLineRenderer());
        StartCoroutine(ShootPart(shootPoint));
        ShootAudio.Play();
    }

    private void TargetDamage(GameObject hit)
    {

        if (hit.tag.Equals("Bullet"))
        {
            EnemyDamage ED = hit.GetComponent<EnemyDamage>();
            Animator anim = hit.GetComponent<Animator>();
            DemonController DC = hit.GetComponent<DemonController>();
            DC.Alarm = true;
            DC.StartStun(0.5f);
            if (ED.Health - 40 > 0)
            {
                _score.Score += 20;
                ED.GetDamage(40);
                anim.SetInteger("Damage", 1);
                anim.SetTrigger("GetDamage");
            }
            else
            {
                _score.Score += 50;
                ED.GetDamage(ED.Health);
            }
        }
        if (hit.tag.Equals(""))
        {
            hit.GetComponent<EnemyDamage>().GetDamage(100);
        }
        if (hit.tag.Equals("Shoot"))
        {
            hit.GetComponent<ForceTech>().Action(-1);
            hit.GetComponent<Animator>().SetInteger("Active", 1);
        }
    }

    public void ClearShootTrace()
    {
        LineRenderer.enabled = false;
    }
    private IEnumerator HandleLineRenderer()
    {
        yield return _lineRendVisTime;
        ClearShootTrace();
    }

    private IEnumerator ShootPart(Vector3 shootPoint)
    {
        GameObject obj = Instantiate(ShootParticle, shootPoint, Quaternion.identity);
        yield return new WaitForSeconds(1);
        Destroy(obj);
    }


    // Выстрелы КОНЕЦ

    private void EnergyChanger()
    {
        if (EnergyTime < 2)
        {
            EnergyTime += Time.deltaTime;
            EnergyValue += EnergySpeedCurrent * Time.deltaTime;
        }
        else
        {
            EnergySpeedCurrent = EnergySpeedCharge;
            EnergyValue += EnergySpeedCurrent * Time.deltaTime;
        }

        if (EnergyValue == 0)
        {
            _reforce = true;
        }
        else if (EnergyValue <= 15)
        {
            EnergyFill.SetBool("Fill", false);
        }
        else
        {
            _reforce = false;
            EnergyFill.SetBool("Fill", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag.Equals("Gun"))
        {
            _score.Score -= 5;
            Health.value -= 40;
        }
        if (other.tag.Equals("SavePoint"))
        {
            Connect.SetBool("Active", true);
            _savePosition = other.transform.position;
        }
        if (other.tag.Equals("Durk"))
        {
            _score.Score -= 30;
            _vertSpeed = 0;
            _speed = 0;
            transform.position = _savePosition;
            //_camAnim.SetTrigger("Portal");
            camScript.ChangeAnimFieldOfView(162);
            EnergyValue -= 50;
            EnergySpeedCurrent = 0;
            EnergyTime = 0;
            Health.value -= 7;
        }
        if (other.tag.Equals("Rifle"))
        {
            _score.Score += 5;
            EnergyValue += 5;
            Destroy(other.gameObject);
        }
        if (other.tag.Equals("CameraChenger"))
        {
            InterfaceText.text = "Новая задача";
            Invoke("EmptyInterfaceText", 2);
            _score.Score += 100;
            MissionPoint MP = other.GetComponent<MissionPoint>();
            MissionText.text = MP.Message;
            if (MP.Clip != null)
            {
                MP.Anim.SetInteger("Active", 1);
                if (Music.isPlaying) Music.Stop();
                Music.clip = MP.Clip;
                Music.Play();
            }

            Destroy(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("SavePoint"))
        {
            Health.value += 10 * Time.deltaTime;
            EnergyValue += 5 * Time.deltaTime;
            EnergyTime = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("SavePoint"))
        {
            Connect.SetBool("Active", false);
        }
    }
}