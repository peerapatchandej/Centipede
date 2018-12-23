using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Random = UnityEngine.Random;

namespace Centipede
{
    /// <summary>
    /// Class inherits from MovementManager class.
    /// </summary>
    public class Enemy : MovementManager
    {
        [SerializeField]
        private LayerMask edgeLayer;

        [SerializeField]
        private float speed = 0.05f;

        [SerializeField]
        private List<Transform> tail = new List<Transform>();

        private int horizontal = 1;
        private int vertical = -1;
        private Enemy headEnemy;
        private GameObject objectCollide;
        private bool isCollide = false;
        private bool isShooted = false;
        private bool isDivide = false;
        private bool isUpdate = false;

        /// <summary>
        /// Called awake method of class base.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            if (!isUpdate)
            {
                UpdateHead();
                transform.localScale = new Vector2(-1, 1);
            }
            anim.Play("Enemy_Head");
        }

        private void FixedUpdate()
        {
            if (GameManager.instance.canPlay)
            {
                if (objectCanMove && !isCollide && !isDivide && !isShooted)
                {
                    InitMovement(horizontal, 0, speed);
                }
            }
        }

        /// <summary>
        /// This class overrides this method for movement of the enemy.
        /// </summary>
        /// <param name="endPos">It's end position that enemy will move.</param>
        /// <returns></returns>
        protected override IEnumerator Movement(Vector3 endPos)
        {
            try
            {
                objectCanMove = false;

                Vector2 tempPos = transform.position;

                if (tail.Count > 0)
                {
                    tail.Last().localScale = new Vector2(-horizontal, 1);
                    tail.Last().position = tempPos;
                    tail.Insert(0, tail.Last());
                    tail.RemoveAt(tail.Count - 1);
                }

                boxCollider.enabled = false;
                rb2d.MovePosition(endPos);
                boxCollider.enabled = true;

                if (objectCollide != null)
                {
                    objectCollide.layer = 8;
                }
            }
            catch (Exception e)
            {
                UIManager.instance.ShowException(e);
            }

            yield return new WaitForSeconds(speed);

            if (isCollide)
            {
                Transform overrideObj = DetectCollider(transform.position, objectLayer);

                if (overrideObj != null)
                {
                    objectCollide = overrideObj.transform.gameObject;
                    objectCollide.layer = 0;
                }

                horizontal = -horizontal;
                transform.Rotate(new Vector3(0, 0, vertical * 90));
                isCollide = false;
            }

            objectCanMove = true;
        }

        /// <summary>
        /// Enemy will move down and reverse direct when it collide with edge or mushroom.
        /// </summary>
        protected override void CollideObject()
        {
            isCollide = true;

            Vector2 startPosition = transform.position;
            Vector2 endPosition = startPosition + new Vector2(0, vertical);

            if (DetectCollider(endPosition, edgeLayer) != null)
            {
                vertical = -vertical;
                endPosition = startPosition + new Vector2(0, vertical);
            }

            transform.Rotate(new Vector3(0, 0, vertical * 90));
            StartCoroutine(Movement(endPosition));
        }

        /// <summary>
        /// Random speed of movement
        /// </summary>
        void RandomSpeed()
        {
            speed = Random.Range(0.03f, 0.05f);
        }

        /// <summary>
        /// When some part has turned new head. It's will update data to tails that It's new head.
        /// </summary>
        void UpdateHead()
        {
            try
            {
                headEnemy = GetComponent<Enemy>();

                if (tail.Count > 0)
                {
                    for (int i = 0; i < tail.Count; i++)
                    {
                        tail[i].GetComponent<Enemy>().headEnemy = headEnemy;
                    }
                }

                isUpdate = true;
            }
            catch (Exception e)
            {
                UIManager.instance.ShowException(e);
            }
        }

        /// <summary>
        /// Deviding part of the enemy when some part is shooted
        /// </summary>
        /// <param name="collision">Use get additional information of the bullet</param>
        void OnTriggerEnter2D(Collider2D collision)
        {
            try
            {
                if (collision.CompareTag("Bullet"))
                {
                    if (!isShooted)
                    {
                        bool isHead = false;
                        isShooted = true;
                        headEnemy.isDivide = true;

                        if (collision.transform.position.x != transform.position.x)
                        {
                            isShooted = false;
                            isDivide = false;
                            return;
                        }

                        Transform newHead = null;
                        Enemy newHeadEnemy = null;

                        int thisIndex = headEnemy.tail.IndexOf(transform);

                        if (thisIndex == -1)
                        {
                            if (headEnemy.tail.Count > 0)
                            {
                                isHead = true;
                                
                                for (int i = 0;i < headEnemy.tail.Count; i++)
                                {
                                    headEnemy.tail[i].GetComponent<BoxCollider2D>().enabled = false;
                                }
                                
                                newHead = headEnemy.tail.Last();
                                newHeadEnemy = newHead.GetComponent<Enemy>();

                                headEnemy.tail.RemoveAt(headEnemy.tail.Count - 1);
                                newHeadEnemy.tail = headEnemy.tail;
                                newHeadEnemy.tail.Reverse();
                            }
                        }
                        else if (thisIndex != headEnemy.tail.Count - 1)
                        {
                            for (int i = 0; i < headEnemy.tail.Count; i++)
                            {
                                headEnemy.tail[i].GetComponent<BoxCollider2D>().enabled = false;
                            }

                            newHead = headEnemy.tail.Last();
                            newHeadEnemy = newHead.GetComponent<Enemy>();

                            int nextIndex = thisIndex + 1;                      

                            if (nextIndex != headEnemy.tail.Count)
                            {
                                newHeadEnemy.tail = headEnemy.tail.GetRange(nextIndex, headEnemy.tail.Count - (nextIndex + 1)).ToList();

                                headEnemy.tail.RemoveAt(nextIndex - 1);
                                headEnemy.tail.RemoveAt(headEnemy.tail.Count - 1);
                                headEnemy.tail = headEnemy.tail.Except(newHeadEnemy.tail).ToList();
                                newHeadEnemy.tail.Reverse();
                            }
                        }
                        else    
                        {
                            headEnemy.tail.RemoveAt(thisIndex);
                            headEnemy.isDivide = false;                         
                        }

                        if (newHead != null && newHeadEnemy != null)
                        {
                            newHeadEnemy.horizontal = -headEnemy.horizontal;    
                            newHeadEnemy.vertical = headEnemy.vertical;         
                            newHeadEnemy.UpdateHead();
                            newHeadEnemy.RandomSpeed();

                            newHead.transform.localScale = new Vector2(-newHeadEnemy.horizontal, 1);

                            for (int i = 0; i < newHeadEnemy.tail.Count; i++)
                            {
                                newHeadEnemy.tail[i].GetComponent<BoxCollider2D>().enabled = true;
                                newHeadEnemy.tail[i].transform.localScale = new Vector2(-1, 1);
                            }

                            for (int i = 0; i < headEnemy.tail.Count; i++)
                            {
                                headEnemy.tail[i].GetComponent<BoxCollider2D>().enabled = true;
                            }                      
                            
                            if (!isHead)
                            {
                                headEnemy.isDivide = false;                     
                            }
                            newHeadEnemy.enabled = true;                        
                        }

                        GameManager.instance.score += 10;
                        GameManager.instance.enemyLife--;
                        UIManager.instance.UpdateScore();
                        anim.Play("Enemy_Dead");
                    }
                }
            }
            catch (Exception e)
            {
                UIManager.instance.ShowException(e);
            }
        }
    }
}
