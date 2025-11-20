using System;
using UnityEngine;

public class ZzzCatTrigger : MonoBehaviour
{
    public static event Action ZzzCatDialogue = delegate { };

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.gameObject.layer == 10)
        {
            triggered = true;
            ZzzCatDialogue.Invoke();
        }
    }
}
