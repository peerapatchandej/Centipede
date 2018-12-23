using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Centipede
{
    /// <summary>
    /// Class inherits from MovementManager class.
    /// </summary>
    public class Player : MovementManager
    {
        [SerializeField]
        private int speed = 60;

        [SerializeField]
        private GameObject bullet = null;

        [SerializeField]
        private GameObject bulletSpawnPoint = null;

        private float timer = 0f;
        private float maxPositionY = 0;

        /// <summary>
        /// Called awake method of base class.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// Setting max y axis position that player can move up and down. 
        /// </summary>
        void Start()
        {
            maxPositionY = ((GameManager.instance.gridBoard.rows - 3) * (15f / 100f)) + transform.position.y - 1;
        }

        /// <summary>
        /// Input management for movement and shooting bullet.
        /// </summary>
        void FixedUpdate()
        {
            if (GameManager.instance.canPlay)
            {
                if (objectCanMove)
                {
                    int horizontal = 0;
                    int vertical = 0;

                    horizontal = (int)(Input.GetAxisRaw("Horizontal"));
                    vertical = (int)(Input.GetAxisRaw("Vertical"));

                    if (transform.position.y + vertical > maxPositionY || transform.position.y + vertical < -21)
                    {
                        return;
                    }

                    if (horizontal != 0)
                    {
                        vertical = 0;
                    }

                    if (horizontal != 0 || vertical != 0)
                    {
                        InitMovement(horizontal, vertical, speed);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);
                    timer = 0;
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    if (timer < 0.1f)
                    {
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);
                        timer = 0;
                    }
                }
            }
        }

        /// <summary>
        /// This class overrides this method for movement of the player.
        /// </summary>
        /// <param name="endPos">It's end position that player will move.</param>
        /// <returns></returns>
        protected override IEnumerator Movement(Vector3 endPos)
        {
            objectCanMove = false;
            rb2d.MovePosition(endPos);

            yield return new WaitForSeconds(0.05f);

            objectCanMove = true;
        }

        /// <summary>
        /// Player will die when player collide with the enemy.
        /// </summary>
        /// <param name="collision">Use get additional information of the enemy</param>
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                GameManager.instance.playerLife--;
                GameManager.instance.canPlay = false;

                UIManager.instance.UpdateLife();
                anim.Play("Player_Dead");

                if (GameManager.instance.playerLife != 0)
                {
                    Invoke("Retry", 1f); /*************/
                }
                else
                {
                    UIManager.instance.ShowGameOver();
                }
            }
        }

        /// <summary>
        /// For restart game when player has died.
        /// </summary>
        void Retry()
        {
            GameManager.instance.enemyLife = 15;
            SceneManager.LoadScene("Centipede Game");
        }
    }
}
