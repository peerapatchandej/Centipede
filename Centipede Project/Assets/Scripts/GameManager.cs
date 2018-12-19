using UnityEngine;

namespace Centipede
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [HideInInspector]
        public bool playerCanMove = true;

        [HideInInspector]
        public bool enemyCanMove = true;
        
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
