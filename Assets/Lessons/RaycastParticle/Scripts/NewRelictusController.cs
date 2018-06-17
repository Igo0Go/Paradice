using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ShootHandler(Vector3 pos);

public class NewRelictusController : MonoBehaviour
{
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
    public int EnergySpeed;
    public float WaitShootTime = 0;
    public bool Reload;
    public bool Fast;
    public GameObject ShootParticle;
    
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
    private bool _reforce;
    private int _defaultLayerMask;


    //гравитация
    private float _grav;
    private float _jumpSpeed;
    private float _vertSpeed;

    void Start()
    {
        _score = GetComponent<ScoreScript>();
        _defaultLayerMask = Cam.cullingMask;
        _camAnim = Cam.GetComponent<Animator>();
        VisorPanel.SetActive(false);
        _lineRendVisTime = new WaitForSeconds(WaitShootTime);
        LineRenderer.positionCount = 2;
        _reforce = false;
        Reload = false;
        Health.value = 100;
        _savePosition = transform.position;
        _speed = Speed;
        _anim = GetComponent<Animator>();
        _grav = -9.8f;
        _jumpSpeed = 5;
        InterfaceText.text = string.Empty;
        Energy.value = 100;
        EnergySpeed = 2;
        _controller = GetComponent<CharacterController>();
        _rotationX = _rotationY = 0;
        Cursor.lockState = CursorLockMode.Locked;
        foreach (var c in KeysImages)
        {
            c.SetActive(false);
        }
    }

    void Update()
    {
        Shoot();

        if (Health.value > 0)
        {
            RelictusMove();
            Rotate();
            MaxSpeed();
            Visor();
        }
        else
        {
            FatlError();
        }

        EnergyChanger();
    }

    private void RelictusMove()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        _moveVector = transform.right * x + transform.forward * z;
        _anim.SetFloat("RunWalk", Mathf.Clamp(_moveVector.magnitude * _speed / (Speed * 2), 0, 1));
        if (_controller.isGrounded)
        {
            _vertSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _vertSpeed = _jumpSpeed;
                EnergyTime = 0;
            }
        }

        _vertSpeed += _grav * Time.deltaTime;
        _moveVector = new Vector3(_moveVector.x * _speed * Time.fixedDeltaTime, _vertSpeed * Time.deltaTime,
            _moveVector.z * _speed * Time.fixedDeltaTime);
        if (_moveVector != Vector3.zero)
        {
            _controller.Move(_moveVector);
        }
    }

    private void Rotate()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            var h = Input.GetAxis("Mouse X");
            var v = Input.GetAxis("Mouse Y");
            _rotationX = transform.localEulerAngles.y + h * RotateSpeed;
            _rotationY += v * RotateSpeed;
            _rotationY = Mathf.Clamp(_rotationY, MinVert, MaxVert);
            transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0);
        }
    }

    private void MaxSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !_reforce)
        {
            Fast = true;
            _speed = Speed * 2;
            EnergySpeed = -5;
            EnergyTime = 0;
        }
        else
        {
            Fast = false;
            _speed = Speed;
            EnergySpeed = 1;
        }
    }

    private void FatlError()
    {
        _anim.SetTrigger("Fatal");
        Cursor.lockState = CursorLockMode.None;
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

    public void Visor()
    {
        if (VisorPanel.activeSelf)
        {
            Energy.value -= 0.1f;
            EnergyTime = 0;
            if(Energy.value <= 0)
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
        if (Input.GetKeyDown(KeyCode.V))
        {
            VisorPanel.SetActive(!VisorPanel.activeSelf);
        }
    }

    public void MakeSound()
    {
        if(ShootSound != null)
        {
            ShootSound.Invoke(transform.position);
        }
    }

//выстрелы НАЧАЛО
    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && !Reload && Energy.value > 15)
        {
            MakeSound();
            Energy.value -= 12;
            _anim.SetFloat("RunWalk", 0);
            _anim.SetTrigger("Shoot");
            EnergyTime = 0;
            DrowShoot();
        }
    }

    private void DrowShoot()
    {
        Ray ray = new Ray(GunEnd.position, transform.forward);
        RaycastHit hit;

        //Debug.DrawRay(transform.position, transform.forward * 10, Color.magenta, 0.2f);

        if (Physics.Raycast(ray, out hit, 1000, ~(1 << 2)))
        {
            MakeShoot(hit.point, -hit.normal, hit.collider.GetComponent<Rigidbody>(), hit);
        }
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
        
        if(hit.tag.Equals("Bullet"))
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
        GameObject obj = Instantiate(ShootParticle,shootPoint, Quaternion.identity);
        yield return new WaitForSeconds(1);
        Destroy(obj);
    }


// Выстрелы КОНЕЦ

    private void EnergyChanger()
    {
        if (EnergyTime < 2)
        {
            EnergyTime += Time.deltaTime;
            Energy.value += EnergySpeed * Time.deltaTime;
        }
        else
        {
            EnergySpeed = 7;
            Energy.value += EnergySpeed * Time.deltaTime;
        }

        if (Energy.value == 0)
        {
            _reforce = true;
        }
        else if (Energy.value <= 15)
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
        if(other.tag.Equals("Gun"))
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
            _camAnim.SetTrigger("Portal");
            Energy.value -= 50;
            EnergySpeed = 0;
            EnergyTime = 0;
            Health.value -= 7;
        }
        if (other.tag.Equals("Rifle"))
        {
            _score.Score += 5;
            Energy.value += 5;
            Destroy(other.gameObject);
        }
        if (other.tag.Equals("CameraChenger"))
        {
            _score.Score += 100;
            MissionPoint MP = other.GetComponent<MissionPoint>();
            MissionText.text = MP.Message;
            if (MP.Clip != null)
            {
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
            Health.value += Time.deltaTime;
            EnergyTime = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("SavePoint"))
        {
            Connect.SetBool("Active", false);
        }
        if (other.tag.Equals("Finish"))
        {
            other.GetComponent<PlatformForBox>().Weight -= 1f;
        }
    }
}