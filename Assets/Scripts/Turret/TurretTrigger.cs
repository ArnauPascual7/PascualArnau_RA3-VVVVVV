using UnityEngine;

public class TurretTrigger : MonoBehaviour
{
    [SerializeField] private GameObject turret;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            turret.SetActive(true);
        }
    }
}
