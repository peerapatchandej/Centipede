using UnityEngine;
using UnityEngine.SceneManagement;

namespace Centipede
{
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Creating instance of this class according to the principle of singleton pattern.
        /// </summary>
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

        /// <summary>
        /// Generating board of the game.
        /// </summary>
        void Awake()
        {
            Application.targetFrameRate = 60;

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

        /// <summary>
        /// Game starting management
        /// </summary>
        void Update()
        {
            if (enemyLife == 0)
            {
                canPlay = false;
                UIManager.instance.ShowGameOver();
            }

            if (Input.GetKeyDown(KeyCode.Space) && !canPlay && playerLife == 3 && enemyLife != 0)
            {
                UIManager.instance.firstPlayText.SetActive(false);
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

        /// <summary>
        /// Scene will be set when scene has loaded.
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (instance.playerLife < 3)
            {
                instance.canPlay = true;
            }

            instance.gridBoard.SetupScene();
        }

        /// <summary>
        /// Restarting game
        /// </summary>
        public void Restart()
        {
            playerLife = maxPlayerLife;
            enemyLife = maxEnemyLife;
            score = 0;
            canPlay = true;
            SceneManager.LoadScene("Centipede Game");
        }
    }
}
