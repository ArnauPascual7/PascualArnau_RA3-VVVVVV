using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void LateUpdate()
    {
        transform.position = player.transform.position + new Vector3(0, 1f, -10f);
    }
}
