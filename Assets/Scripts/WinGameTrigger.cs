using System;
using UnityEngine;

public class WinGameTrigger : MonoBehaviour
{
    public static Action GameWin = delegate { };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            GameWin.Invoke();
        }
    }
}
