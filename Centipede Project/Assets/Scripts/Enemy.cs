using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Centipede
{
    public class Enemy : MovementManager
    {
        [SerializeField]
        protected int speed = 40;

        private bool isCollide = false;
        private int horizontal = 1;
        private List<GameObject> tempMushroom = new List<GameObject>();

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (GameManager.instance.enemyCanMove == true && isCollide == false)
            {
                Move(horizontal, 0, speed);
            }
        }

        protected override void CollideObject()
        {
            isCollide = true;

            Vector2 startPosition = transform.position;
            Vector2 endPosition = startPosition + new Vector2(0, -1);

            StartCoroutine(MoveDown(endPosition));
            
        }

        IEnumerator MoveDown(Vector3 endPos)
        {
            float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

            while (sqrRemainingDistance > float.Epsilon)
            {
                Vector3 newPostion = Vector3.MoveTowards(base.rb2d.position, endPos, speed * Time.deltaTime);

                base.rb2d.MovePosition(newPostion);
                sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

                yield return null;
            }

            yield return new WaitForSeconds(0.1f);

            boxCollider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(transform.position, endPos, objectLayer);
            boxCollider.enabled = true;

            if(hit.transform != null)
            {
                GameObject temp = hit.transform.gameObject;
                objectHit = temp;
                temp.layer = 0;
                tempMushroom.Add(temp);
            }

            horizontal = -horizontal;
            isCollide = false;
        }

        private GameObject objectHit;

        void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.transform.gameObject == objectHit)
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
