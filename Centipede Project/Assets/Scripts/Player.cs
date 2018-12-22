using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Centipede
{
    public class Player : MovementManager
    {
        [SerializeField]
        private int speed = 60;

        [SerializeField]
        private GameObject bullet = null;

        [SerializeField]
        private GameObject bulletSpawnPoint = null;

        private Animator anim;
        private float timer = 0f;
        private float maxPositionY = 0;

        protected override void Start()
        {
            maxPositionY = ((GameManager.instance.gridBoard.gridCell.rows - 3) * (15f / 100f)) + transform.position.y - 1;
            base.Start();
            anim = GetComponent<Animator>();
        }

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

        protected override IEnumerator Movement(Vector3 endPos)
        {
            objectCanMove = false;

            rb2d.MovePosition(endPos);

            yield return new WaitForSeconds(0.05f);

            objectCanMove = true;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Enemy"))
            {
                GameManager.instance.playerLife--;
                GameManager.instance.canPlay = false;
                GameManager.instance.isDead = true;
                anim.Play("Player_Dead");
                Invoke("Restart", 3f);
            }
        }

        void Restart()
        {
            if (GameManager.instance.playerLife > 0)
            {
                SceneManager.LoadScene("Centipede Game");
            }
        }
    }
}
