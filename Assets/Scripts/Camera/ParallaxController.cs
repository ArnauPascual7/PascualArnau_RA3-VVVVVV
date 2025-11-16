using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [Tooltip("Velocitat del Parallax (1 és la velocitat normal)")]
    [Range(0f, 1f)]
    [SerializeField] private float parallaxEffect;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private float spriteWidth, startPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        startPosition = transform.position.x;
    }

    private void FixedUpdate()
    {
        float deltaX = (cameraTransform.position.x - previousCameraPosition.x) * parallaxEffect;
        float moveAmount = cameraTransform.position.x * (1 - parallaxEffect);

        transform.Translate(new Vector3(deltaX, 0, 0));
        previousCameraPosition = cameraTransform.position;

        if (moveAmount > startPosition + spriteWidth)
        {
            transform.Translate(new Vector3(spriteWidth, 0, 0));
            startPosition += spriteWidth;
        }
        else if (moveAmount < startPosition - spriteWidth)
        {
            transform.Translate(new Vector3(-spriteWidth, 0, 0));
            startPosition -= spriteWidth;
        }
    }
}
