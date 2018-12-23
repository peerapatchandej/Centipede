using System.Collections;
using UnityEngine;

namespace Centipede
{
    public abstract class MovementManager : MonoBehaviour
    {
        [SerializeField]
        protected LayerMask objectLayer;

        protected bool objectCanMove = true;

        protected Rigidbody2D rb2d;
        protected BoxCollider2D boxCollider;
        protected Animator anim;
        private RaycastHit2D hit;

        protected virtual void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();
        }

        protected void InitMovement(int horizontal, int vertical, float speed)
        {
            Vector2 startPosition = transform.position;
            Vector2 endPosition = startPosition + new Vector2(horizontal, vertical);

            if (DetectCollider(endPosition, objectLayer) == null)
            {
                StartCoroutine(Movement(endPosition));
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

        protected abstract IEnumerator Movement(Vector3 endPos);

        protected virtual void CollideObject()
        {
            return;
        }
    }
}
