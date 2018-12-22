using UnityEngine;
using UnityEngine.SceneManagement;

namespace Centipede
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GridBoard gridBoard;
        public bool canPlay = false;
        public bool isDead = false;
        public int playerLife = 3;
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

            gridBoard = GetComponent<GridBoard>();
        }

        void Start()
        {
            gridBoard.SetupScene();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !canPlay && playerLife == 3)
            {
                canPlay = true;
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
            instance.isDead = false;
        }
    }
}
