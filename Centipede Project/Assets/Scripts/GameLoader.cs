using UnityEngine;

namespace Centipede
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameManager;

        private GridBoard gridBoard;

        void Awake()
        {
            if (GameManager.instance == null)
            {
                Instantiate(gameManager);
            }
            
            gridBoard = GetComponent<GridBoard>();
            gridBoard.SetupScene();
        }
    }
}
