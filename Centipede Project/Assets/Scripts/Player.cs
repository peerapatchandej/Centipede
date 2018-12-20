using UnityEngine;

namespace Centipede
{
    public class Player : MovementManager
    {
        [SerializeField]
        protected int speed = 20;

        [SerializeField]
        private GameObject bullet = null;

        [SerializeField]
        private GameObject bulletSpawnPoint = null;

        private float timer = 0f;
        private float maxPositionY = 0;

        protected override void Start()
        {
            maxPositionY = (GridCell.instance.rows * (15f / 100f)) + transform.position.y - 1;
            base.Start();
        }

        void FixedUpdate()
        {
            if(GameManager.instance.playerCanMove == true)
            {
                int horizontal = 0; 
                int vertical = 0;

                horizontal = (int)(Input.GetAxisRaw("Horizontal"));
                vertical = (int)(Input.GetAxisRaw("Vertical"));

                if (transform.position.y + vertical > maxPositionY)
                {
                    return;
                }
                
                if (horizontal != 0)
                {
                    vertical = 0;
                }

                if (horizontal != 0 || vertical != 0)
                {
                    Movement(horizontal, vertical, speed);
                }
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);
                timer = 0;
            }
            else if(Input.GetKey(KeyCode.Space))
            {
                if(timer < 0.1f)
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
}
