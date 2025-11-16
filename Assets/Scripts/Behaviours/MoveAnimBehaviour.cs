using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MoveAnimBehaviour : MonoBehaviour
{
    private Animator _animator;

    private const string VelocityParam = "Velocity";

    public bool Idle;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        Idle = _animator.GetFloat(VelocityParam) == 0;
    }

    public void RunAnimation(Vector2 direction)
    {
        _animator.SetFloat(VelocityParam, direction.magnitude);

        Idle = _animator.GetFloat(VelocityParam) == 0;
    }
}
