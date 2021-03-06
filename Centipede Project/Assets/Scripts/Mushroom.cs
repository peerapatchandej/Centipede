﻿using System.Collections.Generic;
using UnityEngine;

namespace Centipede
{
    public class Mushroom : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> mushroomDamage = null;

        private int damageIndex = 0;

        /// <summary>
        /// Mushroom will take damage when it collide with some bullet. 
        /// </summary>
        /// <param name="collision"></param>
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Bullet"))
            {
                if (damageIndex != mushroomDamage.Count - 1)
                {
                    mushroomDamage[damageIndex].SetActive(false);
                    damageIndex++;
                    mushroomDamage[damageIndex].SetActive(true);
                }
                else
                {
                    GameManager.instance.score++;
                    UIManager.instance.UpdateScore();
                    Destroy(gameObject);
                }
            }
        }
    }
}
