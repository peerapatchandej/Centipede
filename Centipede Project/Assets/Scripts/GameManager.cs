using UnityEngine;
using System.Collections.Generic;

namespace Centipede
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public bool gameStart = false;

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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameStart = true;
            }
        }
    }
}
