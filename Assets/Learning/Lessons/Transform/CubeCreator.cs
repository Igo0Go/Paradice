using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Coordinate      //тип координат, в которых будет вставлен кубик
{
    Local,
    Global
}
public enum Cube            //тип кубика
{
    Grass,
    Stone,
    Sand
}
public class CubeCreator : MonoBehaviour {

    public Coordinate Coordinate = Coordinate.Global; //можно проводить инициализацию по умолчанию и без старта,
                                                            //но мы привыкли всё же там
    public Cube TypeCube = Cube.Grass;  
    public GameObject SandCube;         
    public GameObject StoneCube;
    public GameObject GrassCube;
    public GameObject Parent;//объект, относительно которого будут считаться локальные координаты (та большая чёрно-зелёная штука)
    public Vector3 Position;// позиция, которую мы задаём для вставки объекта
    public bool Create;         //ключ вставить выбранный объект в выбранные координаты указанного типа
    public bool Destroy;        //удалить последний вставленный объект

    GameObject obj;

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
            CreateCube(Coordinate, TypeCube);  //метод создания куба указанного типа в указанных координатах
            Create = !Create;
        }
        if (Destroy)
        {
            DestroyObject(obj);         //метод удаления указанной сущности типа GameObject(стандартный в Unity)
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
                    obj = Instantiate(GrassCube, Position, Quaternion.identity); //вставка префаба куба в указанной позиции
                    break;                                                  //в глобальных координатах со стандартным поворотом
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
                    obj.transform.parent = Parent.transform;        //назначение вставленного куба дочерним от объекта Parent
                    obj.transform.localPosition = Position;         //перемещение в позицию, расчитанную от родителя
                    break;
                case Cube.Sand:
                    obj = Instantiate(SandCube,Parent.transform);  //вставка куба доченим от Parent
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
