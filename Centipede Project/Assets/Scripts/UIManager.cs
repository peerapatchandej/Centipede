using UnityEngine;
using UnityEngine.UI;

namespace Centipede
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        private Text scoreText;
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

            scoreText = GameObject.Find("Score Text").GetComponent<Text>();
            gameoverText = GameObject.Find("Game Over Text").GetComponent<Text>();
            gameoverText.gameObject.SetActive(false);
        }

        void UpdateScore()
        {
            scoreText.text = "Score : " + GameManager.instance.score;
        }

        void ShowGameOver()
        {
            gameoverText.gameObject.SetActive(true);
        }
    }
}
