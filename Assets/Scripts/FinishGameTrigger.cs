using System;
using UnityEngine;

public class FinishGameTrigger : MonoBehaviour
{
    public static Action GameFinished = delegate { };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            GameFinished.Invoke();
        }
    }
}
