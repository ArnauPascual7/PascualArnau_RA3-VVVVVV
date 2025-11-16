using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator),typeof(SpriteRenderer))]
public class GravityChangeBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _sr;

    private const string JumpParam = "Jumping";

    public bool Jumping;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();

        Jumping = _animator.GetBool(JumpParam);
    }

    public void ChangeGravity()
    {
        _animator.SetBool(JumpParam, true);
        Jumping = _animator.GetBool(JumpParam);

        _sr.flipY = _rb.gravityScale > 0;
        _rb.gravityScale *= -1;
    }

    public void FinishJump()
    {
        _animator.SetBool(JumpParam, false);
        Jumping = _animator.GetBool(JumpParam);
    }
}
