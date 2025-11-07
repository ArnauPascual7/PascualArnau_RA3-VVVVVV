using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class MoveBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    [SerializeField] private float speed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    public void MoveCharacter(Vector2 direction)
    {
        _rb.linearVelocityX = direction.x * speed;

        if (direction.x != 0)
        {
            _sr.flipX = direction.x < 0;
        }
    }
}
