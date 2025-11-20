using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MoveBehaviour), typeof(GravityChangeBehaviour), typeof(MoveAnimBehaviour))]
[RequireComponent(typeof(ExIdleAnimBehaviour), typeof(SpriteExplosionBehaviour), typeof(MeowBehaviour))]
public class PlayerCat : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    private MoveBehaviour _mb;
    private GravityChangeBehaviour _gcb;
    private MoveAnimBehaviour _mab;
    private ExIdleAnimBehaviour _xab;
    private SpriteExplosionBehaviour _sxb;
    private MeowBehaviour _meowb;
    private Vector2 _moveDirection = Vector2.zero;

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
        _meowb = GetComponent<MeowBehaviour>();
    }

    private void OnEnable()
    {
        DisableInputs(false);

        UI.GamePausedEvent += DisableInputs;
        UI.GameStarted += b => DisableInputs(!b);
        FinishGameTrigger.GameFinished += OnGameFinished;
    }
    private void OnDisable()
    {
        DisableInputs(true);

        UI.GamePausedEvent -= DisableInputs;
        UI.GameStarted -= b => DisableInputs(!b);
        FinishGameTrigger.GameFinished -= OnGameFinished;
    }
    private void DisableInputs(bool disable)
    {
        if (disable)
        {
            inputActions.Disable();

            _mab.Idle = false;
        }
        else
        {
            inputActions.Enable();

            StartIdle();
        }
    }

    private void Update()
    {
        if (_mab.Idle)
        {
            _mab.Idle = _xab.CheckIdleTime(_mab.Idle);
        }

        _mb.MoveCharacter(_moveDirection);
        _meowb.SetDirection(_moveDirection);
        _mab.RunAnimation(_moveDirection);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (onFloor)
        {
            if (context.performed)
            {
                AudioManager.Instance.PlayClip(AudioClipType.GravitySwitch);
            }

            _meowb.FlipY();
            _gcb.ChangeGravity();
            _xab.CancelAnimations();

            _mab.Idle = false;

            if (context.canceled)
            {
                StartIdle();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();

        _xab.CancelAnimations();

        _mab.Idle = false;

        if (context.canceled)
        {
            StartIdle();
        }
    }

    public void OnMiau(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _meowb.Meow();
        }

        _mab.Idle = false;
        _xab.CancelAnimations();

        if (context.canceled)
        {
            StartIdle();
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

        if (collision.gameObject.layer == 11)
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

    private void StartIdle()
    {
        _mab.Idle = true;
        _xab.SetIdleTime();
    }

    private void OnGameFinished()
    {
        DisableInputs(true);

        _moveDirection = Vector2.right;
    }
}
