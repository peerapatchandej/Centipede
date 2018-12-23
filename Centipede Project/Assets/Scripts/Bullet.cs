﻿using UnityEngine;

namespace Centipede
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private int speed = 60;

        void Update()
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Shootable"))
            {
                Destroy(gameObject);
            }
            else if (collision.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }
}
