using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MovementType // тип движения
{
    Teleport,
    Steps,
    Smooth
}

public class LessonTransform : MonoBehaviour { //MonoBehavior - стандартный родитель для всех скриптов
    
	//публичные поля и свойства отображаются в редакторе


    public GameObject Sphere; 				//Метка, которую мы используем для обозначения координат, куда пойдём
    public MovementType MoveType; 			//пределение типа движения в нашем объекте
    public float XposTarget; 				//координата х целевой позиции
    public float ZposTarget; 				//координата z целевой позиции
    public float Speed;						//скорость перемещения
    public float RotateSpeed;				//скорость вращения
    public bool Move;						//ключ для движения
    public bool Rotate;						//ключ для поворота
    
    private bool keyRotate;					//временный ключ для вращения
    private const float y = 1f;				//высота игрока над центром кубика
    private bool keyMove;					//временный ключ для движения
    private Vector3 TargetPosition;			//целевая позиция (с учётом подъёма над центром)
    private Vector3 moveVector;				//вектор в направлении цели


	//метод вызывается, когда объект становится активным на сцене
    private void Start()
    {
        MoveType = MovementType.Teleport;
        keyRotate = true;
        keyMove = true;
        Rotate = false;
        Move = false;
    }

	//метод вызывается каждый кадр
    private void Update()
    {
        if(Rotate)
        {
            PlayerRotate(); 	//метод вращения
        }
        if (Move)
        {
            PlayerMove();		//метод движения
        }
    }

    private void PlayerRotate()
    {
        if (MoveType == MovementType.Teleport)
        {
            Vector3 Target = new Vector3(XposTarget, y, ZposTarget); 	//задаём целевую позицию по указанным координатам
            transform.LookAt(Target);									//резко поворачиваем объект, чтобы он смотрел на целевую позицию
            Rotate = false;
        }
        else if (MoveType == MovementType.Steps)
        {
            Vector3 TargetPosition = new Vector3(XposTarget, y, ZposTarget);
            if (keyRotate)
            {
                Instantiate(Sphere, TargetPosition, Quaternion.identity);		//устанавливаем префаб в сцену в целевую позицию в глобальных координатах
                keyRotate = false;											
            }
            Vector3 moveVector = TargetPosition - transform.position;			//расчитываем вектор до целевой позиции
            int c = GetSign(moveVector);
            if (Vectors(transform.forward, moveVector))//метод проверки угла				
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVector),0.5f); //сложное вращение от нашего разворота
            }																									//к вектору до целевой позиции
            else
            {
                transform.forward = moveVector;
                Rotate = false;
            }
        }
        else if (MoveType == MovementType.Smooth)
        {
            Vector3 TargetPosition = new Vector3(XposTarget, y, ZposTarget);
            if (keyRotate)
            {
                Instantiate(Sphere, TargetPosition, Quaternion.identity);
                keyRotate = false;
            }
            Vector3 moveVector = TargetPosition - transform.position;
            int c = GetSign(moveVector);
            if (Vectors(transform.forward, moveVector))
            {				//	  ось вращения,	скорость вращения
                transform.Rotate(transform.up, c * RotateSpeed * Time.deltaTime); //метод поворачивающий объект вокруг оси вращения с заданной 
            }																				//скоростью поротив часовой стрелки (вроде, или наоборот)
            else																			//в общем, направление поворотам меняется знаком "-"
            {
                transform.forward = moveVector;
                Rotate = false;
            }
        }
    }

    private void PlayerMove()
    {
        if (MoveType == MovementType.Teleport)
        {
            transform.position = new Vector3(XposTarget, y, ZposTarget); 				//резкое присваивание целевой позиции координатам игрока
            Move = false;
        }
        else if (MoveType == MovementType.Steps)
        {
            TargetPosition = new Vector3(XposTarget, y, ZposTarget);
            moveVector = TargetPosition - transform.position;
            if (Vector3.Distance(transform.position, TargetPosition) > 0.5f) //проверка дистанции между игроком и целевой позицией
            {
                transform.position += moveVector*0.5f; 								//прибовление координатам позиции игрока координат вектора направления,
            }																//умноженных на 0.5    Учитывая, что это происходит каждый кадр, получается не такое
            else																		//резкое движение, плавность которого зависит от множителя
            {
                transform.position = TargetPosition;						//"довод" игрока до цели
                Move = false;
            }
        }
        else if (MoveType == MovementType.Smooth)
        {
            
            if (keyMove)
            {
                TargetPosition = new Vector3(XposTarget, y, ZposTarget);			//просчёт вектора проходит только в первый раз, чтобы не уменьшать его
                moveVector = TargetPosition - transform.position;					//длины по мере приближения игрока к цели. Ведь из-за этого и прибавлять
                keyMove = !keyMove;													//к его позиции мы будем с каждым разом всё меньше
            }
            float x = Vector3.Distance(transform.position, TargetPosition);		//проверка оставшегося до целевой позиции состояния
            if (Speed * Time.deltaTime < x)										//проверка, покроет ли наш "шаг" расстояние до цели и не превысит ли его
            {
                transform.position += moveVector.normalized * Speed * Time.deltaTime; //normalized оставляет нам только направление вектора, поэтому величина
            }																			//"шага" будет зависеть только от остальных множителей
            else																		//Time.deltaTime - время с предыдущего кадра (сотые доли секунды)
            {
                keyMove = !keyMove;
                Move = false;
                transform.position += moveVector.normalized * x;						//последний "шаг" игрока на расстояние, равное дистанции до цели
            }
        }
    }

    private int GetSign(Vector3 moveVector)		//метод получения знака угла между векторами
    {
        if (transform.forward.x > moveVector.x) //если цель "западнее" (просто чтоб понятно, что это относительно мира, а не игрока) взора
        {
            Debug.Log("Я слева"); //Debug.Log() - лучшиый друг на все времена. Выводит в консоль всё, что думает
            return -1;
        }
        else if (transform.forward.x < moveVector.x)		//если цель "восточнее" взора
        {
            Debug.Log("Я справа");
            return +1;
        }
        else if (transform.forward.z > moveVector.z)		//если цель "южнее" взора
        {
            Debug.Log("Я снизу");
            return -1;
        }
        else if (transform.forward.z < moveVector.z)		//если цель "севернее" взора
        {
            Debug.Log("Я сверху");
            return +1;
        }
        return 1;
    }

    private bool Vectors(Vector3 Face, Vector3 move)  				//метод нахождения угла между векторами, переведёнными в плоскость карты
    {
        Vector3 FaceVector = new Vector3(Face.x, 0, Face.z);		//вектор взгляда персонажа
        Vector3 MoveVector = new Vector3(move.x, 0, move.z);		//вектор до цели
        Debug.Log(Vector3.Angle(FaceVector, MoveVector));			//да, можно и результаты чего-нибудь выводить. Тут - числа будут покааны
        if (Vector3.Angle(FaceVector, MoveVector) < 1)				//в градусах
        {
            return false;
        }
        return true;
    }
}
