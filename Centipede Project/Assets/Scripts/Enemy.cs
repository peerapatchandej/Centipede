using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Centipede
{
    public class Enemy : MovementManager
    {
        [SerializeField]
        private LayerMask edgeLayer;

        [SerializeField]
        protected int speed = 60;

        private bool isCollide = false;
        private int horizontal = 1;
        private int vertical = -1;
        private List<GameObject> tempMushroom = new List<GameObject>();
        private GameObject objectCollide;
        
        protected override void Start()
        {
            base.Start();
        }

        void FixedUpdate()
        {
            if (GameManager.instance.enemyCanMove == true && isCollide == false)
            {
                Movement(horizontal, 0, speed);
            }
        }

        protected override void CollideObject()
        {
            isCollide = true;

            Vector2 startPosition = transform.position;
            Vector2 endPosition = startPosition + new Vector2(0, vertical);

            Transform edgeTopAndBottom = DetectCollider(endPosition, edgeLayer);

            if (edgeTopAndBottom != null)
            {
                vertical = -vertical;
                endPosition = startPosition + new Vector2(0, vertical);
            }
            
            StartCoroutine(SmoothMovement(endPosition, speed));
            
        }

        protected override IEnumerator SmoothMovement(Vector3 endPos, int speed)
        {
            StartCoroutine(base.SmoothMovement(endPos, speed));

            if (isCollide)
            {
                Transform overrideObj = DetectCollider(endPos, objectLayer);
                
                if (overrideObj != null)
                {
                    GameObject temp = overrideObj.transform.gameObject;
                    objectCollide = temp;
                    temp.layer = 0;
                    tempMushroom.Add(temp);
                }

                horizontal = -horizontal;
                isCollide = false;
            }

            yield return null;
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.transform.gameObject == objectCollide)
            {
                if (tempMushroom.Count > 0)
                {
                    for (int i = 0; i < tempMushroom.Count; i++)
                    {
                        tempMushroom[i].layer = 8;
                    }

                    tempMushroom.Clear();
                }
            }
        }
    }
}
