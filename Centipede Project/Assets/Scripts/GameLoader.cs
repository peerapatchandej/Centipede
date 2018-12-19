using UnityEngine;

namespace Centipede
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameManager = null;

        [SerializeField]
        private GameObject gridCell = null;

        private GridBoard gridBoard;

        void Awake()
        {
            if (GameManager.instance == null)
            {
                Instantiate(gameManager);
            }

            if(GridCell.instance == null)
            {
                Instantiate(gridCell);
            }
            
            gridBoard = GetComponent<GridBoard>();
            gridBoard.SetupScene();
        }
    }
}
