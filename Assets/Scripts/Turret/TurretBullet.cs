using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    private Rigidbody2D _rb;

    public float speed;
    public Turret spawner;

    Camera mainCamera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        mainCamera = Camera.main;
    }

    private void Update()
    {
        CheckIfOutOfCamera();
        OnMove();
    }

    private void CheckIfOutOfCamera()
    {
        if (mainCamera == null)
            return;

        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);

        // Només comprovar l'eix X perquè la bala només es mou horitzontalment
        // Si surt per l'esquerra retorna al spawner
        if (viewportPoint.x < -0.1f/* || viewportPoint.x > 1.1f*/)
        {
            ReturnToSpawner();
        }
    }

    private void OnMove()
    {
        _rb.linearVelocityX = -speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReturnToSpawner();
    }

    private void ReturnToSpawner()
    {
        gameObject.SetActive(false);
        spawner.SpawnerStackPush(gameObject);
    }
}
