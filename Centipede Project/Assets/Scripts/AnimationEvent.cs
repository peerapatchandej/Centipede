using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    /// <summary>
    /// Method will work when part of the enemy is shooted;
    /// </summary>
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
