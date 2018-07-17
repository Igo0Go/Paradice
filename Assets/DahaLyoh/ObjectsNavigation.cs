using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//скрипт заставляет объект патрулировать, т.е. тупо ходить туда-сюда по вейпоинтам в случайном порядке
public class ObjectsNavigation : MonoBehaviour
{
    [Tooltip("Перетащите сюда из иерархии родителя вейпоитов")]
    public Transform parentWaypoints; //создайте в иерархии пустой объект, и удочерите ему вейпоинты, по которым нужно ходить

    //Минимальная дистанция до вейпоита, когда объект посчитает, что дошел до нее
    float minDistance = 1f;
    //близко к 0 лучше не ставить, так как некоторые объекты никогда не достигнут вейпоинта, 
    //если вейпоинт на земле, а центр объекта выше указанного вами расстояния

    //создаем переменную с навигационным мешем
    UnityEngine.AI.NavMeshAgent agent;

    //указывает на какую из точек сейчас идти (выбирается случайным образом в ToRandWaypoint())
    int randomWaypoint = 0;

    void Start()
    {
        //получаем в переменную навигационный меш 
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        //яхз, какое-то включение агента, почему он выключен - яхз
        agent.enabled = true;

        //отправляем к первой случайной точке
        ToRandWaypoint();
    }

    void Update()
    {
        //если дистанция до точки меньше минимально необходимой (т.е. мы пришли к точке)
        if (Vector3.Distance(transform.position, parentWaypoints.GetChild(randomWaypoint).transform.position) <= minDistance)
        {
            //отправляем к случайной точке
            ToRandWaypoint();
        }
    }

    //Синтаксис в Update: "Vector3.Distance" возвращает дистанцию от одной точки до другой, в нее мы передаем 
    //"transform.position" - это позиция объекта, на котором висит скрипт и 
    //"parentWaypoints.GetChild(randomWaypoint).transform.position)" - это позиция случайно выбранного вейпоинта.
    //
    //Разберем второй элемент. "randomWaypoint" это номер случайной точки (число int), генерируется в функции ToRandWaypoint(), 
    //"parentWaypoints.GetChild(randomWaypoint)" это команда взять у объекта, который вы перенести в инспекторе в parentWaypoints, 
    //его дочерний по иерархии объект под номером randomWaypoint. После идет ".transform.position" это взять у этого дочернего
    //объекта его позицию. В итоге высчитывается расстояние от объекта до нужного вейпоинта.
    //Потом идет проверка, что это расстояние меньше или равно ("<= minDistance") минимальному расстоянию до вейпоинта,
    //и если так, то выбирается другая точка с помощью "ToRandWaypoint();".

    //отправляет объект идти к новой случайной точке
    void ToRandWaypoint()
    {
        //выбираем номер случайной точки от 0 до числа количества вейпоинтов (дочерних объектов) в parentWaypoints
        randomWaypoint = Random.Range(0, parentWaypoints.childCount);
        //направляем агента в эту точку
        agent.destination = parentWaypoints.GetChild(randomWaypoint).transform.position;
    }
}

//Советы: следите, чтобы вейпоинты были на земле (можно чуть выше уровня земли), иногда их случайно можно
//положить на дерево и т.п. И тогда объект не сможет дойти до вейпоинта и тупо будет стоять около него.
//Не забывайте запекать навигацию (Navigation > Bake > Bake). На некоторых поверхностях навигация
//может не запекаться в принципе, тогда нужно использовать Terrain (было у меня разок, уже не помню деталей)