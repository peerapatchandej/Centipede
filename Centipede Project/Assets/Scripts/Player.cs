using UnityEngine;

namespace Centipede
{
    public class Player : MovementManager
    {
        protected override void Start()
        {
            base.Start();
        }

        void Update()
        {
            if(Input.GetKeyUp(KeyCode.LeftArrow))
            {
                Move(- 1, 0);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                Move(1, 0);
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                Move(0, 1);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                Move(0, -1);
            }
        }
    }
}
