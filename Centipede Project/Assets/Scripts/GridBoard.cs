using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Centipede
{
    public class GridBoard : MonoBehaviour
    {
        [Space]
        [Header("Size of Grid Cell")]

        public int columns = 40;
        public int rows = 43;

        [Space]
        [Header("Count of Mushroom")]

        [SerializeField]
        private int minimum = 35;

        [SerializeField]
        private int maximum = 45;

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

            for(int x = columns / -2; x < (columns / 2) + 1; x++)
            {
                for(int y = (rows / -2) + 1; y < (rows / 2) - 2; y++)
                { 
                    gridPosition.Add(new Vector2(x, y));
                }
            }
        }

        void GridBoardSetup()
        {
            boardParent = new GameObject("Grid Board").transform;

            for(int x = columns / -2; x < (columns / 2) + 1; x++)
            {
                for (int y = rows / -2; y < (rows / 2) + 1; y++)
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
            CreateMushroom(mushroom, minimum, maximum);
        }
    }
}
