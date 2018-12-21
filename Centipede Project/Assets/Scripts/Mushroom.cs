using System.Collections.Generic;
using UnityEngine;

namespace Centipede
{
    public class Mushroom : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> mushroomDamage = null;

        private int damageIndex = 0;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Bullet"))
            {
                mushroomDamage[damageIndex].SetActive(false);
                damageIndex++;

                if (damageIndex != mushroomDamage.Count)
                {
                    mushroomDamage[damageIndex].SetActive(true);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
