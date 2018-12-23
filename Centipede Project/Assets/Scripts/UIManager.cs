using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace Centipede
{
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// Creating instance of this class according to the principle of singleton pattern.
        /// </summary>
        public static UIManager instance;

        [HideInInspector]
        public GameObject firstPlayText;

        private Text scoreText;
        private Text lifeText;
        private Text gameoverText;
        private Image exceptionPanel;
        private Text exceptionText;
        private Button exceptionButton;

        /// <summary>
        /// Begin find UI object of the scene.
        /// </summary>
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

        /// <summary>
        /// It's method for update score value.
        /// </summary>
        public void UpdateScore()
        {
            scoreText.text = "Score : " + GameManager.instance.score;
        }

        /// <summary>
        /// It's method for update life value.
        /// </summary>
        public void UpdateLife()
        {
            lifeText.text = "Life : " + GameManager.instance.playerLife;
        }

        /// <summary>
        /// It's method for showing the text of game over.
        /// </summary>
        public void ShowGameOver()
        {
            gameoverText.gameObject.SetActive(true);
        }

        /// <summary>
        /// It's method for showing message of error.
        /// </summary>
        /// <param name="e"></param>
        public void ShowException(Exception e)
        {
            exceptionText.text = e.Message;
            exceptionPanel.gameObject.SetActive(true);
        }

        /// <summary>
        /// Finding UI object on the scene.
        /// </summary>
        private void FindUI()
        {
            scoreText = GameObject.Find("Score Text").GetComponent<Text>();
            lifeText = GameObject.Find("Life Text").GetComponent<Text>();
            gameoverText = GameObject.Find("Game Over Text").GetComponent<Text>();
            firstPlayText = GameObject.Find("First play Text");
            exceptionPanel = GameObject.Find("Exception Panel").GetComponent<Image>();
            exceptionText = exceptionPanel.transform.GetChild(0).GetComponent<Text>();
            exceptionButton = exceptionPanel.transform.GetChild(1).GetComponent<Button>();

            exceptionButton.onClick.AddListener(() => GameManager.instance.Restart());

            gameoverText.gameObject.SetActive(false);
            exceptionPanel.gameObject.SetActive(false);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// It's finding UI object and updating some values when scene has loaded.
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            instance.FindUI();
            instance.firstPlayText.SetActive(false);
            instance.UpdateScore();
            instance.UpdateLife();
        }
    }
}
