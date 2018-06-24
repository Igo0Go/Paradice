using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType
{
Active=0,
Stop=1,
Action=2,
Light=3
}

public class ButtonScript : MonoBehaviour {

    public ButtonType Type;

    private IMechanic _mech;

	// Use this for initialization
	void Start () {
        _mech = GetComponentInParent<IMechanic>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        switch (Type)
        {
            case ButtonType.Stop:
                _mech.Stop();
                break;
            case ButtonType.Active:
                _mech.Activate();
                break;
            case ButtonType.Action:
                _mech.Action();
                break;
            case ButtonType.Light:
                _mech.LightActivator();
                break;
        }
    }
}
