using System.Collections;
using UnityEngine;

namespace Centipede
{
    public abstract class MovementManager : MonoBehaviour
    {
        [SerializeField]
        protected LayerMask objectLayer;

        protected Rigidbody2D rb2d;
        protected BoxCollider2D boxCollider;
        protected Animator anim;
        protected bool objectCanMove = true;

        private RaycastHit2D hit;

        /// <summary>
        /// Inheritors get components of them;
        /// </summary>
        protected virtual void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();
        }

        /// <summary>
        /// Initialization of movement which will check next position that It's have object or not.
        /// </summary>
        /// <param name="horizontal">Horizontal direction for movement</param>
        /// <param name="vertical">Veritcal direction for movement</param>
        /// <param name="speed">Speed of movement</param>
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

        /// <summary>
        /// It's method for checking the object.
        /// </summary>
        /// <param name="endPos">End position of checking the object.</param>
        /// <param name="layer">Layer for checking the object.</param>
        /// <returns></returns>
        protected Transform DetectCollider(Vector2 endPos, LayerMask layer)
        {
            boxCollider.enabled = false;
            hit = Physics2D.Linecast(transform.position, endPos, layer);
            boxCollider.enabled = true;

            return hit.transform;
        }

        /// <summary>
        /// Inheritor will implement this method for use it for movement.
        /// </summary>
        /// <param name="endPos">It's end position that the object will go.</param>
        /// <returns></returns>
        protected abstract IEnumerator Movement(Vector3 endPos);

        /// <summary>
        /// Inheritor can override this method for use it for doing something about object collision.
        /// </summary>
        protected virtual void CollideObject()
        {
            return;
        }
    }
}
