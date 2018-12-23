using UnityEngine;
using UnityEngine.SceneManagement;

namespace Centipede
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public int maxPlayerLife = 3;
        public int maxEnemyLife = 15;

        [HideInInspector]
        public GridBoard gridBoard;

        [HideInInspector]
        public bool canPlay = false;

        [HideInInspector]
        public int playerLife;

        [HideInInspector]
        public int enemyLife;

        [HideInInspector]
        public int score = 0;

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

            playerLife = maxPlayerLife;
            enemyLife = maxEnemyLife;

            gridBoard = GetComponent<GridBoard>();
            gridBoard.SetupScene();
        }

        void Update()
        {
            if (enemyLife == 0)
            {
                canPlay = false;
                UIManager.instance.ShowGameOver();
            }

            if (Input.GetKeyDown(KeyCode.Space) && !canPlay && playerLife == 3 && enemyLife != 0)
            {
                canPlay = true;
            }
            else if(Input.GetKeyDown(KeyCode.Space) && !canPlay && (playerLife == 0 || enemyLife == 0))
            {
                Restart();
            } 
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (instance.playerLife < 3)
            {
                instance.canPlay = true;
            }

            instance.gridBoard.SetupScene();
        }

        private void Restart()
        {
            playerLife = maxPlayerLife;
            enemyLife = maxEnemyLife;
            score = 0;
            canPlay = true;
            SceneManager.LoadScene("Centipede Game");
        }
    }
}
