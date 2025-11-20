using System;
using UnityEngine;

public class ZzzCatTrigger : MonoBehaviour
{
    public static event Action<bool> ZzzCatDialogue = delegate { };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            ZzzCatDialogue.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            ZzzCatDialogue.Invoke(false);
        }
    }
}
