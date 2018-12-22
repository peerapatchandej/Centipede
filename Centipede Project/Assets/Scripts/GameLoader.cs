using UnityEngine;

namespace Centipede
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameManager = null;

        [SerializeField]
        private GameObject uiManager = null;

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
