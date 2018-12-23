using UnityEngine;

namespace Centipede
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private int speed = 60;

        /// <summary>
        /// Movement of bullet
        /// </summary>
        void Update()
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }

        /// <summary>
        /// Bullets will be destroy when it collides with the object. 
        /// </summary>
        /// <param name="collision"></param>
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
