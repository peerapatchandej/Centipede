using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Stay");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
    }
}
