using UnityEngine;

namespace Centipede
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameManager = null;

        [SerializeField]
        private GameObject uiManager = null;

        /// <summary>
        /// If instance of class that implement according to principle of singleton pattern is null, It's instantiate objects that atteched these class.
        /// </summary>
        void Awake()
        {
            if (GameManager.instance == null)
            {
                Instantiate(gameManager);
            }

            if (UIManager.instance == null)
            {
                Instantiate(uiManager);
            }
        }
    }
}
