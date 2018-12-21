using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Snake : MonoBehaviour
{
    private float speed = 0.1f;
    Vector2 vector = Vector2.up;
    Vector2 moveVector;

    bool vertical = false;
    bool horizontal = true;

    [SerializeField]
    private List<Transform> tail = new List<Transform>();

    void Start()
    {
        //InvokeRepeating("Movement", 0.1f, speed);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) && horizontal)
        {
            horizontal = false;
            vertical = true;
            vector = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && vertical)
        {
            horizontal = true;
            vertical = false;
            vector = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && vertical)
        {
            horizontal = true;
            vertical = false;
            vector = -Vector2.up;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && horizontal) 
        {
            horizontal = false;
            vertical = true;
            vector = -Vector2.right;
        }
        moveVector = vector / 1f;
    }

    void Movement()
    {
        Vector2 ta = transform.position;

        tail.Last().position = ta;
        tail.Insert(0, tail.Last());
        tail.RemoveAt(tail.Count - 1);

        transform.Translate(moveVector);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("test");
    }
}
