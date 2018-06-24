using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidScript : MonoBehaviour {
    
    public Light BBLight;                       //фонарик
    public List<GameObject> CameraPoints;       //Коллекция объектов для позиции камеры
    public Transform SavePoint;                 //точка сохранения
    public GameObject Cam;                      //камера
    public GameObject Head;                     //голова робота
    public float Speed;                 
    public float JumpForce;
    public Rigidbody Body;                      //компонент rigidbody, установленный на шарике робота (посмотрите про компонент rigidbody)



    private Vector3 _savePosition;              //позиция сохранения
    private Vector3 _moveVector;                
    private Vector3 _ofset;                     //смещение головы робота относительно тела
    private Quaternion _headRotatoin;           //поворот головы робота в мировых координатах
    private int _pointNumber;
    private int _saveCamPoint;

    
    void Start()
    {
        BBLight.enabled = false;
        _moveVector = transform.position - Cam.transform.position;
        _moveVector = new Vector3(_moveVector.x, 0, _moveVector.z);
        _ofset = Head.transform.position - transform.position;
        _headRotatoin = Head.transform.rotation;
        _pointNumber = 0;
        Cam.transform.position = CameraPoints[_pointNumber].transform.position;
    }

    //метод вызывается каждые 0.02с
    void FixedUpdate()
    {
        MoveBB();
        Head.transform.position = transform.position + _ofset;
        Cam.transform.LookAt(transform.position);
        Head.transform.forward = _moveVector;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))//нажали один раз
        {
            BBLight.enabled = !BBLight.enabled;
        }
        if(Input.GetKeyUp(KeyCode.Q))//отпустили
        {
            PreviosCam();
        }
        if(Input.GetKeyUp(KeyCode.E))
        {
            NextCam();
        }
    }

    private void SetSavePosition(GameObject point)
    {
        SavePoint = point.transform;
        var pos = SavePoint.position;
        _savePosition = new Vector3(pos.x, pos.y + 3, pos.z);
        _saveCamPoint = _pointNumber;
    }

    private void MoveBB()
    {   //получение значения характеристики от двух кнопок [-1;1]
        if(Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical")!=0)
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            _moveVector = Cam.transform.right*x + Cam.transform.forward*z;
            _moveVector = new Vector3(_moveVector.x, 0, _moveVector.z);
            if(_moveVector != Vector3.zero)
            {
                //прикладываем определённую силу в определённом направлении
                Body.AddForce(_moveVector.normalized * Speed * 0.01f, ForceMode.Force);
                Head.transform.forward = _moveVector.normalized;
            }
        }
    }

    private void NextCam()
    {
        if (_pointNumber < CameraPoints.Count-1)
        {
            _pointNumber++;
        }
        else
        {
            _pointNumber = 0;
        }
        Cam.transform.position = CameraPoints[_pointNumber].transform.position;
    }

    private void PreviosCam()
    {
        if (_pointNumber > 0)
        {
            _pointNumber--;
        }
        else
        {
            _pointNumber = CameraPoints.Count-1;
        }
        Cam.transform.position = CameraPoints[_pointNumber].transform.position;
    }

    //при ВХОЖДЕНИИ в триггер
    private void OnTriggerEnter(Collider other)
    {
        //проверка по тегам
        if(other.tag.Equals("CameraChenger"))
        {
            if(_pointNumber < CameraPoints.Count)
            {
                NextCam();
                Destroy(other.gameObject);
            }
        }
        if (other.tag.Equals("JumpPoint"))
        {
            Body.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
        if (other.tag.Equals("Durk"))
        {
            transform.position = _savePosition;
            Cam.transform.position = CameraPoints[_saveCamPoint].transform.position;
            _pointNumber = _saveCamPoint;
        }
        if(other.tag.Equals("SavePoint"))
        {
            SetSavePosition(other.gameObject);
        }
    }

    //пока находимся в триггере
    private void OnTriggerStay(Collider other)
    {                                   //удерживание
        if(other.tag.Equals("Finish") && Input.GetKey(KeyCode.U))
        {
            other.gameObject.transform.position += new Vector3(0, 0, 1) * Time.deltaTime;
        }
    }
}
