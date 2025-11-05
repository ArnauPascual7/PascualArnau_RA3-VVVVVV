using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class AnimationBehaviour : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _sr;

    private const string VELOCITY_PARAM = "Velocity";

    public bool idle;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();

        idle = _animator.GetFloat(VELOCITY_PARAM) == 0;
    }

    public void RunAnimation(Vector2 direction)
    {
        if (direction.x < 0)
        {
            _sr.flipX = true;
        }
        else if (direction.x > 0)
        {
            _sr.flipX = false;
        }

        _animator.SetFloat(VELOCITY_PARAM, direction.magnitude);

        idle = _animator.GetFloat(VELOCITY_PARAM) == 0;
    }
}
