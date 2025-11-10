using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Player;

    private void LateUpdate()
    {
        //transform.position = Player.transform.position + new Vector3(0, 1f, -10f);
        transform.position = new Vector3(Player.position.x, transform.position.y, transform.position.z);
    }
}
