using UnityEngine;
using System.Collections.Generic;

namespace Centipede
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [HideInInspector]
        public bool playerCanMove = true;

        [HideInInspector]
        public bool enemyCanMove = false;
        //public List<bool> enemyCanMove = new List<bool>();

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            /*for(int i = 0; i < 15; i++)
            {
                enemyCanMove.Add(true);
            }*/
        }

        public void SetObjectCanMove(string objectTag, bool canMove)
        {
            if(objectTag == "Player")
            {
                playerCanMove = canMove;
            }
            else if(objectTag == "Enemy")
            {
                enemyCanMove = canMove;
            }
        }
    }
}
