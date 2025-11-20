using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configuració")]
    [Tooltip("Transform del jugador a seguir")]
    [SerializeField] private Transform Player;

    private bool gameFinished = false;

    private void OnEnable()
    {
        FinishGameTrigger.GameFinished += OnGameFinished;
    }

    private void OnDisable()
    {
        FinishGameTrigger.GameFinished -= OnGameFinished;
    }

    private void LateUpdate()
    {
        if (!gameFinished)
        {
            transform.position = new Vector3(Player.position.x, transform.position.y, transform.position.z);
        }
    }

    private void OnGameFinished()
    {
        gameFinished = true;
    }
}
