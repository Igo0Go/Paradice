﻿using System;
using System.Collections;
using UnityEngine;

namespace Lessons.SilaArtema
{
    public class ForceReaction : MonoBehaviour
    {
        private Rigidbody _rb;
        public float attractionForce = 10;
        public int Forcing = 1;
        public ParticleSystem Particle;
        public ParticleSystem.EmissionModule BoostAvailable;


        void Awake()
        {
            BoostAvailable = Particle.emission;
            BoostAvailable.enabled = false;
        }

        // Use this for initialization
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }


        private IEnumerator MoveToTarget(Vector3 target)
        {
            yield return new WaitForFixedUpdate();

            // Находим общее расстояние между кубом и целевой точкой
            float distance = Vector3.Distance(transform.position, target);

            // Заводим переменные для хранения значения текущего расстояния и процента пройденного пути (от 0.0 до 1.0)
            float currentDistance = distance;
            float percentage = 0f;

            // Выполняем цикл до тех пор, пока куб не приблизится к точке на 0.005м
            while (currentDistance > 0.005)
            {
                // Находим текущее расстояние и процент пройденного пути
                currentDistance = Vector3.Distance(transform.position, target);
                percentage = (distance - currentDistance) / distance;

                //Debug.Log("currentDistance = " + currentDistance.ToString());

                // Прикладываем к телу силу, направленную в сторону точки
                GetComponent<Rigidbody>().AddForce((target - transform.position).normalized * attractionForce,
                    ForceMode.Acceleration);
                // Прикладываем к телу силу противодействия, направленную в противоположную сторону, с величиной, пропорциональной пройденному пути (чем меньше осталось до точки, тем выше сила)
                GetComponent<Rigidbody>().AddForce(
                    -(target - transform.position).normalized * attractionForce * percentage * 2,
                    ForceMode.Acceleration);

                yield return new WaitForFixedUpdate();
            }

            // Когда куб оказывается в районе 0.005м от точки, останавливаем его и присваиваем ему координаты целевой точки, чтобы всё точно было
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = target;

            Debug.Log("End of coroutine");
            yield return null;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag.Equals("Force"))
            {
                Vector3 forceVector = Vector3.zero;
                if (Input.GetKeyDown(KeyCode.Q))
                {
//                    StopAllCoroutines();
//                    StartCoroutine(MoveToTarget(Player.transform.position));.
                    forceVector = other.transform.up;
                    BoostAvailable.enabled = true;

                    Invoke("Poff", 0.5f);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
//                    StopAllCoroutines();
                    forceVector = -other.transform.up;
                    BoostAvailable.enabled = true;

                    Invoke("Poff", 0.5f);
                }

                _rb.AddForce(forceVector.normalized * Forcing, ForceMode.Impulse);
            }
        }

        void Poff()
        {
            BoostAvailable.enabled = false;
        }
    }
}