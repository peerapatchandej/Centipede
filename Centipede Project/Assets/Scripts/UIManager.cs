using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Centipede
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        private Text scoreText;
        private Text lifeText;
        private Text gameoverText;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            instance.FindUI();
            instance.UpdateLife();
        }

        public void UpdateScore()
        {
            scoreText.text = "Score : " + GameManager.instance.score;
        }

        public void UpdateLife()
        {
            lifeText.text = "Life : " + GameManager.instance.playerLife;
        }

        public void ShowGameOver()
        {
            gameoverText.gameObject.SetActive(true);
        }

        private void FindUI()
        {
            scoreText = GameObject.Find("Score Text").GetComponent<Text>();
            lifeText = GameObject.Find("Life Text").GetComponent<Text>();
            gameoverText = GameObject.Find("Game Over Text").GetComponent<Text>();
            gameoverText.gameObject.SetActive(false);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            instance.FindUI();
            instance.UpdateScore();
            instance.UpdateLife();
        }
    }
}
