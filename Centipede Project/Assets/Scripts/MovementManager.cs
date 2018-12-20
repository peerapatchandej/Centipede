﻿using UnityEngine;
using System.Collections;

namespace Centipede
{
    public abstract class MovementManager : MonoBehaviour
    {
        [SerializeField]
        protected LayerMask objectLayer;
        
        protected Rigidbody2D rb2d;
        protected BoxCollider2D boxCollider;
        private RaycastHit2D hit;

        protected virtual void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        protected void Movement(int horizontal, int vertical, int speed)
        {
            Vector2 startPosition = transform.position;
            Vector2 endPosition = startPosition + new Vector2(horizontal, vertical);
            
            if (DetectCollider(endPosition, objectLayer) == null)
            {
                StartCoroutine(SmoothMovement(endPosition, speed));
            }
            else
            {
                CollideObject();
            }
        }

        protected Transform DetectCollider(Vector2 endPos, LayerMask layer)
        {
            boxCollider.enabled = false;
            hit = Physics2D.Linecast(transform.position, endPos, layer);
            boxCollider.enabled = true;

            return hit.transform;
        }

        protected virtual IEnumerator SmoothMovement(Vector3 endPos, int speed)
        {
            GameManager.instance.SetObjectCanMove(transform.tag, false);

            float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

            while (sqrRemainingDistance > float.Epsilon)
            {
                Vector3 newPostion = Vector3.MoveTowards(rb2d.position, endPos, speed * Time.deltaTime);

                rb2d.MovePosition(newPostion);
                sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;
                
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);

            GameManager.instance.SetObjectCanMove(transform.tag, true);
        }

        protected virtual void CollideObject()
        {
            return;
        }
    }
}
