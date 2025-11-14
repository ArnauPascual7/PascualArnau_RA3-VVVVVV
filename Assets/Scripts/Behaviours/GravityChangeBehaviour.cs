using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator),typeof(SpriteRenderer))]
public class GravityChangeBehaviour : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _sr;

    private const string JumpParam = "Jumping";

    [Header("Configuració")]
    public bool jumping;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();

        jumping = _animator.GetBool(JumpParam);
    }

    public void ChangeGravity()
    {
        _animator.SetBool(JumpParam, true);
        jumping = _animator.GetBool(JumpParam);

        _sr.flipY = _rb.gravityScale > 0;
        _rb.gravityScale *= -1;
    }

    public void FinishJump()
    {
        _animator.SetBool(JumpParam, false);
        jumping = _animator.GetBool(JumpParam);
    }
}
