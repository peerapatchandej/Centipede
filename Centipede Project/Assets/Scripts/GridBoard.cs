using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Centipede
{
    public class GridBoard : MonoBehaviour
    {
        [Serializable]
        protected class CountMushroom
        {
            public int minimum;
            public int maximum;

            public CountMushroom(int min, int max)
            {
                minimum = min;
                maximum = max;
            }
        }

        [Space]
        [Header("Count of Mushroom")]

        [SerializeField]
        private CountMushroom countMushroom = new CountMushroom(30, 45);

        [Space]
        [Header("Object for Spawn")]

        [SerializeField]
        private GameObject mushroom = null;

        [SerializeField]
        private GameObject background = null;
        private Transform boardParent, mushroomParent;
        private List<Vector2> gridPosition = new List<Vector2>();

        void InitGridPositionList()
        {
            gridPosition.Clear();

            for(int x = GridCell.instance.columns / -2; x < (GridCell.instance.columns / 2) + 1; x++)
            {
                for(int y = (GridCell.instance.rows / -2) + 1; y < (GridCell.instance.rows / 2) - 2; y++)
                { 
                    gridPosition.Add(new Vector2(x, y));
                }
            }
        }

        void GridBoardSetup()
        {
            boardParent = new GameObject("Grid Board").transform;

            for(int x = GridCell.instance.columns / -2; x < (GridCell.instance.columns / 2) + 1; x++)
            {
                for (int y = GridCell.instance.rows / -2; y < (GridCell.instance.rows / 2) + 1; y++)
                {
                    GameObject backgroundInstance = Instantiate(background, new Vector2(x, y), Quaternion.identity) as GameObject;

                    backgroundInstance.transform.SetParent(boardParent);
                }
            }
        }

        Vector2 RandomPosition()
        {
            int randomIndex = Random.Range(0, gridPosition.Count);
            Vector2 randomPosition = gridPosition[randomIndex];

            gridPosition.RemoveAt(randomIndex);

            return randomPosition;
        }

        void CreateMushroom(GameObject mushroom, int min, int max)
        {
            int mushroomCount = Random.Range(min, max + 1);
            mushroomParent = new GameObject("Mushrooms").transform;

            for (int i = 0; i < mushroomCount; i++)
            {
                Vector2 randomPosition = RandomPosition();
                GameObject mushroomInstance = Instantiate(mushroom, randomPosition, Quaternion.identity) as GameObject;

                mushroomInstance.transform.SetParent(mushroomParent);
            }
        }

        public void SetupScene()
        {

            InitGridPositionList();
            GridBoardSetup();
            CreateMushroom(mushroom, countMushroom.minimum, countMushroom.maximum);
        }
    }
}
