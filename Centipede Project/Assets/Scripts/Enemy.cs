using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Centipede
{
    public class Enemy : MovementManager
    {
        [SerializeField]
        private LayerMask edgeLayer;

        [SerializeField]
        private float speed = 0.05f;

        [SerializeField]
        private AnimationClip animationClip;

        public List<Transform> tail = new List<Transform>();

        [HideInInspector]
        public Enemy headEnemy;

        [HideInInspector]
        public int horizontal = 1;

        private Animator anim;
        private GameObject objectCollide;
        private int vertical = -1;
        private bool isCollide = false;
        private bool isShooted = false;
        private bool isDivide = false;

        protected override void Start()
        {
            base.Start();
            anim = GetComponent<Animator>();

            anim.Play("Enemy_Head");

            //ส่วนไหนที่เป็นหัว จะบอกทุก ๆ ส่วนว่า ตัวนี้คือหัวนะ
            headEnemy = GetComponent<Enemy>();

            if (tail.Count > 0)
            {
                for (int i = 0; i < tail.Count; i++)
                {
                    tail[i].GetComponent<Enemy>().headEnemy = headEnemy;
                }
            }
        }

        private void FixedUpdate()
        {
            if (GameManager.instance.canPlay)
            {
                if (objectCanMove && !isCollide && !isDivide)
                {
                    InitMovement(horizontal, 0, speed);
                }
            }
        }

        protected override IEnumerator Movement(Vector3 endPos)
        {
            objectCanMove = false;

            Vector2 tempPos = transform.position;

            if (tail.Count > 0)
            {
                tail.Last().position = tempPos;
                tail.Insert(0, tail.Last());
                tail.RemoveAt(tail.Count - 1);
            }

            boxCollider.enabled = false;

            rb2d.MovePosition(endPos);

            boxCollider.enabled = true;

            //When enemy pass mushroom while head change direction. 
            if (objectCollide != null)
            {
                objectCollide.layer = 8;
            }

            yield return new WaitForSeconds(speed);

            //It's case enemy overlap with mushroom when movement of enemy finished;
            if (isCollide)
            {
                Transform overrideObj = DetectCollider(transform.position, objectLayer);

                if (overrideObj != null)
                {
                    objectCollide = overrideObj.transform.gameObject;
                    objectCollide.layer = 0;
                }

                horizontal = -horizontal;
                isCollide = false;
            }

            objectCanMove = true;
        }

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

            StartCoroutine(Movement(endPosition));
        }

        void RandomSpeed()
        {
            speed = Random.Range(0.03f, 0.05f);
        }

        /// <summary>
        /// Deviding part of enemy
        /// </summary>
        /// <param name="collision">Use get additional information of bullet</param>
        void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Bullet"))
            {
                if (!isShooted)
                {
                    isShooted = true;
                    GameManager.instance.score += 10;
                    GameManager.instance.enemyLife--;
                    UIManager.instance.UpdateScore();

                    headEnemy.isDivide = true;  //Suspended movement 

                    Transform newHead = null;
                    Enemy newHeadEnemy = null;

                    
                    int thisIndex = headEnemy.tail.IndexOf(transform);  //Get this index of list

                    if (thisIndex == -1)    //It is the case player shoot head;
                    {
                        if (headEnemy.tail.Count > 0)
                        {
                            //Set new head
                            newHead = headEnemy.tail.Last();
                            newHeadEnemy = newHead.GetComponent<Enemy>();

                            //Transfer all of the data from current head list to new head list
                            headEnemy.tail.RemoveAt(headEnemy.tail.Count - 1);
                            newHeadEnemy.tail = headEnemy.tail;
                            newHeadEnemy.tail.Reverse();
                        }
                    }
                    else if (thisIndex != headEnemy.tail.Count - 1)
                    {
                        //Set new head
                        newHead = headEnemy.tail.Last();
                        newHeadEnemy = newHead.GetComponent<Enemy>();

                        //Get next index from this index
                        int nextIndex = thisIndex + 1;

                        //Devide data of current head list into new head list
                        if (nextIndex != headEnemy.tail.Count)
                        {
                            newHeadEnemy.tail = headEnemy.tail.GetRange(nextIndex, headEnemy.tail.Count - (nextIndex + 1)).ToList();

                            headEnemy.tail.RemoveAt(nextIndex - 1);
                            headEnemy.tail.RemoveAt(headEnemy.tail.Count - 1);
                            headEnemy.tail = headEnemy.tail.Except(newHeadEnemy.tail).ToList();
                            newHeadEnemy.tail.Reverse();
                        }
                    }
                    else    //It is the case player shoot last part; 
                    {
                        headEnemy.tail.RemoveAt(thisIndex);

                        //Continued movement
                        headEnemy.isDivide = false;
                    }

                    if(newHead != null && newHeadEnemy != null)
                    {
                        //Set new direction for new head
                        newHeadEnemy.horizontal = -headEnemy.horizontal;

                        //Continued movement
                        headEnemy.isDivide = false;

                        //Random speed for new head movement
                        newHeadEnemy.RandomSpeed();

                        //Enable script of new head
                        newHeadEnemy.enabled = true;
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}
