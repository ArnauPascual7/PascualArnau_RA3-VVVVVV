using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configuració")]
    [SerializeField] private Transform Player;

    private void LateUpdate()
    {
        //transform.position = Player.transform.position + new Vector3(0, 1f, -10f);
        transform.position = new Vector3(Player.position.x, transform.position.y, transform.position.z);
    }
}
