using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MoveBehaviour), typeof(GravityChangeBehaviour), typeof(MoveAnimBehaviour))]
[RequireComponent(typeof(ExIdleAnimBehaviour))]
public class PlayerCat : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    private MoveBehaviour _mb;
    private GravityChangeBehaviour _gcb;
    private MoveAnimBehaviour _ab;
    private ExIdleAnimBehaviour _xab;

    private bool OnFloor = true;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);

        _mb = GetComponent<MoveBehaviour>();
        _gcb = GetComponent<GravityChangeBehaviour>();
        _ab = GetComponent<MoveAnimBehaviour>();
        _xab = GetComponent<ExIdleAnimBehaviour>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        if (_ab.idle)
        {
            _ab.idle = _xab.CheckIdleTime(_ab.idle);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (OnFloor)
        {
            _gcb.ChangeGravity();
            _xab.CancelAnimations();

            _ab.idle = false;

            if (context.canceled)
            {
                _ab.idle = true;
                _xab.SetIdleTime();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();

        _mb.MoveCharacter(direction);
        _ab.RunAnimation(direction);
        _xab.CancelAnimations();

        _ab.idle = false;

        if (context.canceled)
        {
            _ab.idle = true;
            _xab.SetIdleTime();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            OnFloor = true;

            _gcb.FinishJump();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            OnFloor = false;
        }
    }
}
