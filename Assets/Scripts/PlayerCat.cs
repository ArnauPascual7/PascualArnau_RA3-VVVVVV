using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MoveBehaviour), typeof(GravityChangeBehaviour), typeof(MoveAnimBehaviour))]
[RequireComponent(typeof(ExIdleAnimBehaviour), typeof(SpriteExplosionBehaviour))]
public class PlayerCat : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    private MoveBehaviour _mb;
    private GravityChangeBehaviour _gcb;
    private MoveAnimBehaviour _mab;
    private ExIdleAnimBehaviour _xab;
    private SpriteExplosionBehaviour _sxb;

    private bool OnFloor = true;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);

        _mb = GetComponent<MoveBehaviour>();
        _gcb = GetComponent<GravityChangeBehaviour>();
        _mab = GetComponent<MoveAnimBehaviour>();
        _xab = GetComponent<ExIdleAnimBehaviour>();
        _sxb = GetComponent<SpriteExplosionBehaviour>();
    }

    private void OnEnable()
    {
        DisableInputs(false);
        PauseMenu.GamePausedEvent += DisableInputs;
    }
    private void OnDisable()
    {
        DisableInputs(true);
        PauseMenu.GamePausedEvent -= DisableInputs;
    }
    private void DisableInputs(bool disable)
    {
        if (disable)
        {
            inputActions.Disable();
        }
        else
        {
            inputActions.Enable();
        }
    }

    private void Update()
    {
        if (_mab.idle)
        {
            _mab.idle = _xab.CheckIdleTime(_mab.idle);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (OnFloor)
        {
            _gcb.ChangeGravity();
            _xab.CancelAnimations();

            _mab.idle = false;

            if (context.canceled)
            {
                _mab.idle = true;
                _xab.SetIdleTime();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();

        _mb.MoveCharacter(direction);
        _mab.RunAnimation(direction);
        _xab.CancelAnimations();

        _mab.idle = false;

        if (context.canceled)
        {
            _mab.idle = true;
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

        if (collision.gameObject.layer == 21)
        {
            _sxb.Explode();
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
