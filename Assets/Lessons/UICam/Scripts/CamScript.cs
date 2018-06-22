using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CamScript : MonoBehaviour
{
    public float speed;

    private Camera _camera;
    private bool _key;
    private float _target;
    private int _lage;

    private void Start()
    {
        _target = 0;
        _lage = 1;
        _key = false;
        _camera = GetComponent<Camera>();
        _camera.fieldOfView = LoadLevel.FieldOfView;
        LoadLevel.FieldOfViewChanged += FieldOfViewChanged;
    }

    private void Update()
    {
        if (_key && _camera.fieldOfView != _target)
        {
            if (speed < Math.Abs(_camera.fieldOfView - _target))
            {
                _camera.fieldOfView += speed * _lage;
            }
            else
            {
                _camera.fieldOfView = _target;

                if (_camera.fieldOfView == LoadLevel.FieldOfView)
                {
                    _key = false;
                }
                else
                {
                    _target = LoadLevel.FieldOfView;
                    _lage = -1;
                }
            }
        }
    }

    public void ChangeAnimFieldOfView(float target)
    {
        _key = true;
        _target = target;
        if (_camera.fieldOfView <= target)
        {
            _lage = 1;
        }
        else
        {
            _lage = -1;
        }
    }
    private void FieldOfViewChanged(float value)
    {
        _camera.fieldOfView = LoadLevel.FieldOfView;
    }
}