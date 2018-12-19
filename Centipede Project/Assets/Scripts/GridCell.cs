using UnityEngine;

namespace Centipede
{
    public class GridCell : MonoBehaviour
    {
        public static GridCell instance;

        private int _columns { get; set; }
        private int _rows { get; set; }

        public GridCell()
        {
            _columns = 40;
            _rows = 40;
        }

        public int columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public int rows
        {
            get { return _rows; }
            set { _rows = value; }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
