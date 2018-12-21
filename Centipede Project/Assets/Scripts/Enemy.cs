using UnityEngine;
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
        private float speed = 0.1f;

        [SerializeField]
        private List<Transform> tail = new List<Transform>();

        private Rigidbody2D rb2d;
        private BoxCollider2D boxCollider;
        private RaycastHit2D hit;
        private GameObject objectCollide;
        private int horizontal = 1;
        private int vertical = -1;

        void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();

            InvokeRepeating("InitMovement", 0.1f, speed);
        }

        void InitMovement()
        {
            if (GameManager.instance.gameStart)
            {
                Vector2 startPosition = transform.position;
                Vector2 endPosition = startPosition + new Vector2(horizontal, 0);

                if (DetectCollider(endPosition, objectLayer) == null)
                {
                    Movement(endPosition);
                }
                else
                {
                    CollideObject();
                }
            }
        }

        void Movement(Vector2 endPos)
        {
            Vector2 tempPos = transform.position;

            if (tail.Count > 0)
            {
                tail.Last().position = tempPos;
                tail.Insert(0, tail.Last());
                tail.RemoveAt(tail.Count - 1);
            }

            transform.position = endPos;
        }

        Transform DetectCollider(Vector2 endPos, LayerMask layer)
        {
            boxCollider.enabled = false;
            hit = Physics2D.Linecast(transform.position, endPos, layer);
            boxCollider.enabled = true;

            return hit.transform;
        }

        protected void CollideObject()
        {
            Vector2 startPosition = transform.position;
            Vector2 endPosition = startPosition + new Vector2(0, vertical);

            if (DetectCollider(endPosition, edgeLayer) != null)
            {
                vertical = -vertical;
                endPosition = startPosition + new Vector2(0, vertical);
            }

            Movement(endPosition);

            horizontal = -horizontal;

            startPosition = transform.position;
            endPosition = startPosition + new Vector2(horizontal, 0);

            if (DetectCollider(transform.position, objectLayer) != null)
            {
                Transform overrideObj = DetectCollider(transform.position, objectLayer);
                objectCollide = overrideObj.transform.gameObject;
                objectCollide.layer = 0;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.transform.gameObject == objectCollide)
            {
                objectCollide.layer = 0;
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.gameObject == objectCollide)
            {
                objectCollide.layer = 8;
            }
        }
    }
}
