using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Coordinate
{
    Local,
    Global
}
public enum Cube
{
    Grass,
    Stone,
    Sand
}
public class CubeCreator : MonoBehaviour {

    public Coordinate Coordinate = Coordinate.Global;
    public Cube TypeCube = Cube.Grass;
    public GameObject SandCube;
    public GameObject StoneCube;
    public GameObject GrassCube;
    public GameObject Parent;
    public Vector3 Position;
    GameObject obj;
    public bool Create;
    public bool Destroy;

	// Use this for initialization
	void Start () {
        obj = new GameObject();
        Position = new Vector3();
        Create = false;
        Destroy = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (Create)
        {
            CreateCube(Coordinate, TypeCube);
            Create = !Create;
        }
        if (Destroy)
        {
            DestroyObject(obj);
            Destroy = !Destroy;
        }
	}

    public void CreateCube(Coordinate coordinate, Cube cube)
    {
      
        if (coordinate == Coordinate.Global)
        {
            switch (cube)
            {
                case Cube.Grass:
                    obj = Instantiate(GrassCube, Position, Quaternion.identity);
                    break;
                case Cube.Sand:
                    obj = Instantiate(SandCube);
                    obj.transform.position = Position;
                    break;
                case Cube.Stone:
                    obj = Instantiate(StoneCube,Position,Quaternion.identity);
                    break;
            }
        }
        if (coordinate == Coordinate.Local)
        {
            switch (cube)
            {
                case Cube.Grass:
                    obj = Instantiate(GrassCube);
                    obj.transform.parent = Parent.transform;
                    obj.transform.localPosition = Position;
                    break;
                case Cube.Sand:
                    obj = Instantiate(SandCube,Parent.transform);
                    obj.transform.localPosition = Position;
                    break;
                case Cube.Stone:
                    obj = Instantiate(StoneCube,Parent.transform);
                    obj.transform.localPosition = Position;
                    break;
            }
        }
    }
}
