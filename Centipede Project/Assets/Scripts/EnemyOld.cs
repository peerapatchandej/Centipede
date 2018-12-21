using UnityEngine;
using System.Collections;

namespace Centipede
{
    public class EnemyOld : MovementManager
    {
        [SerializeField]
        private LayerMask edgeLayer;

        [SerializeField]
        private int speed = 80;

        private int horizontal = 1;
        private int vertical = -1;
        private bool isCollide = false;
        private bool isHead = false;
        private bool isLeader = false;
        private bool isFollower = false;
        private GameObject objectCollide;
        private Transform parent;
        private FollowMovement followMovement;
        private FollowMovement followerMovement;

        protected override void Start()
        {
            base.Start();
            parent = transform.parent;
            followMovement = GetComponent<FollowMovement>();

            int index = transform.GetSiblingIndex();

            if (index == 0)
            {
                isHead = true;
            }
            else
            {
                if(index != parent.childCount - 1)
                {
                    isLeader = true;
                }

                followerMovement = parent.transform.GetChild(index - 1).GetComponent<FollowMovement>();
                isFollower = true;
            }
        }

        void FixedUpdate()
        {
            if (GameManager.instance.gameStart)
            {
                if (isHead && objectCanMove && isCollide == false)
                {
                    objectCanMove = false;

                    if (!followMovement.canFollow)
                    {
                        followMovement.canFollow = true;
                    }

                    Movement(horizontal, 0, speed);
                }
                else
                {
                    if (isFollower && followerMovement.canFollow && isCollide == false)
                    {
                        followerMovement.canFollow = false;
                        Movement(horizontal, 0, speed);
                    }
                }
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
            //StartCoroutine(base.SmoothMovement(endPos, speed));

            //yield return new WaitUntil(() => objectCanMove == true);

            objectCanMove = false;

            if (isLeader)
            {
                if (!followMovement.canFollow)
                {
                    followMovement.canFollow = true;
                }
            }

            float sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

            while (sqrRemainingDistance > float.Epsilon)
            {
                Vector3 newPostion = Vector3.MoveTowards(rb2d.position, endPos, speed * Time.deltaTime);

                rb2d.MovePosition(newPostion);
                sqrRemainingDistance = (transform.position - endPos).sqrMagnitude;

                yield return null;
            }

            yield return new WaitForSeconds(0.05f);

            if (isCollide)
            {
                Transform overrideObj = DetectCollider(endPos, objectLayer);
                
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

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.gameObject == objectCollide)
            {
                objectCollide.layer = 8;
            }
        }
    }
}
