using UnityEngine;

namespace Centipede
{
    public class Enemy : MovementManager
    {
        [SerializeField]
        protected int speed = 40;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(GameManager.instance.enemyCanMove == true)
            {
                Move(1, 0, speed);
            }
        }

        protected override void CollideObject()
        {
            /*Do something when it collide with obj*/
        }
    }
}
