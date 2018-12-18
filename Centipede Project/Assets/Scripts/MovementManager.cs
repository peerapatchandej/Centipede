using UnityEngine;
using System.Collections;

namespace Centipede
{
    public abstract class MovementManager : MonoBehaviour
    {
        private Rigidbody2D rb2d;
        private BoxCollider2D boxCollider;

        protected virtual void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        protected void Move(int horizontal, int vertical)
        {
            rb2d.MovePosition(new Vector2(transform.position.x + horizontal, transform.position.y + vertical));
        }
    }
}
