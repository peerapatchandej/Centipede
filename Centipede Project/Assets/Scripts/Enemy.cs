using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Centipede
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private LayerMask objectLayer;

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

        /*[HideInInspector]
        public GameObject newHead;*/

        private Rigidbody2D rb2d;
        private BoxCollider2D boxCollider;
        private RaycastHit2D hit;
        private Animator anim;
        private GameObject objectCollide;
        private int vertical = -1;
        private bool isCollide = false;
        private bool objectCanMove = true;
        private bool isShooted = false;
        private bool isDivide = false;

        void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
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
            if (GameManager.instance.gameStart)
            {
                if (objectCanMove && !isCollide && !isDivide)
                {
                    InitMovement();
                }
            }
        }

        void InitMovement()
        {
            Vector2 startPosition = transform.position;
            Vector2 endPosition = startPosition + new Vector2(horizontal, 0);
                
            if (DetectCollider(endPosition, objectLayer) == null)
            {
                StartCoroutine(StartMovement(endPosition));
            }
            else
            {
                CollideObject();
            }
        }

        void CollideObject()
        {
            isCollide = true;

            Vector2 startPosition = transform.position;
            Vector2 endPosition = startPosition + new Vector2(0, vertical);

            if (DetectCollider(endPosition, edgeLayer) != null)
            {
                vertical = -vertical;
                endPosition = startPosition + new Vector2(0, vertical);
            }

            StartCoroutine(StartMovement(endPosition));
        }

        IEnumerator StartMovement(Vector2 endPos)
        {
            objectCanMove = false;

            Vector2 tempPos = transform.position;

            if (tail.Count > 0)
            {
                tail.Last().position = tempPos;
                tail.Insert(0, tail.Last());
                tail.RemoveAt(tail.Count - 1);
            }

            rb2d.MovePosition(endPos);

            if (objectCollide != null)
            {
                objectCollide.layer = 8;
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
                isCollide = false;
            }

            objectCanMove = true;
        }

        Transform DetectCollider(Vector2 endPos, LayerMask layer)
        {
            boxCollider.enabled = false;
            hit = Physics2D.Linecast(transform.position, endPos, layer);
            boxCollider.enabled = true;

            return hit.transform;
        }

        void RandomSpeed()
        {
            speed = Random.Range(0.01f, 0.05f);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Bullet"))
            {
                if (!isShooted)
                {
                    isShooted = true;

                    Destroy(collision.gameObject);

                    ///Suspended movement 
                    headEnemy.isDivide = true;

                    Transform newHead;
                    Enemy newHeadEnemy;

                    ///Get this index of list
                    int thisIndex = headEnemy.tail.IndexOf(transform);

                    ///It is the case player shoot head;
                    if (thisIndex == -1)
                    {
                        if (headEnemy.tail.Count > 0)
                        {
                            ///Set new head
                            newHead = headEnemy.tail.Last();
                            newHeadEnemy = newHead.GetComponent<Enemy>();

                            headEnemy.tail.RemoveAt(headEnemy.tail.Count - 1);
                            newHeadEnemy.tail = headEnemy.tail;
                            newHeadEnemy.tail.Reverse();

                            ///Set new direction for new head
                            newHeadEnemy.horizontal = -headEnemy.horizontal;

                            ///Continued movement
                            headEnemy.isDivide = false;

                            ///Random speed for new head movement
                            newHeadEnemy.RandomSpeed();

                            ///Enable script of new head
                            newHeadEnemy.enabled = true;
                        }

                        Destroy(gameObject);
                        return;
                    }

                    if (thisIndex != headEnemy.tail.Count - 1)
                    {
                        ///Set new head
                        newHead = headEnemy.tail.Last();
                        newHeadEnemy = newHead.GetComponent<Enemy>();

                        ///Get next index from this index
                        int nextIndex = thisIndex + 1;

                        ///Devide current head list into new head list
                        if (nextIndex != headEnemy.tail.Count)
                        {
                            newHeadEnemy.tail = headEnemy.tail.GetRange(nextIndex, headEnemy.tail.Count - (nextIndex + 1)).ToList();

                            headEnemy.tail.RemoveAt(nextIndex - 1);
                            headEnemy.tail.RemoveAt(headEnemy.tail.Count - 1);
                            headEnemy.tail = headEnemy.tail.Except(newHeadEnemy.tail).ToList();
                            newHeadEnemy.tail.Reverse();
                        }

                        ///Set new direction for new head
                        newHeadEnemy.horizontal = -headEnemy.horizontal;

                        ///Continued movement
                        headEnemy.isDivide = false;

                        ///Random speed for new head movement
                        newHeadEnemy.RandomSpeed();

                        ///Enable script of new head
                        newHeadEnemy.enabled = true;
                    }
                    else
                    {
                        ///It is the case player shoot last part;
                        headEnemy.tail.RemoveAt(thisIndex);

                        ///Continued movement
                        headEnemy.isDivide = false;
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}
