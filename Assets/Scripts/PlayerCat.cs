using System;
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

    public static event Action PlayerDeath = delegate { };

    private bool onFloor = true;

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

        UI.GamePausedEvent += DisableInputs;
        UI.GameStarted += b => DisableInputs(!b);
    }
    private void OnDisable()
    {
        DisableInputs(true);

        UI.GamePausedEvent -= DisableInputs;
        UI.GameStarted -= b => DisableInputs(!b);
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
        if (_mab.Idle)
        {
            _mab.Idle = _xab.CheckIdleTime(_mab.Idle);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (onFloor)
        {
            _gcb.ChangeGravity();
            _xab.CancelAnimations();

            _mab.Idle = false;

            if (context.canceled)
            {
                _mab.Idle = true;
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

        _mab.Idle = false;

        if (context.canceled)
        {
            _mab.Idle = true;
            _xab.SetIdleTime();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            onFloor = true;

            _gcb.FinishJump();
        }

        if (collision.gameObject.layer == 21)
        {
            PlayerDeath.Invoke();

            _sxb.Explode();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            onFloor = false;
        }
    }
}
