using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MoveBehaviour))]
[RequireComponent(typeof(AnimationBehaviour))]
[RequireComponent(typeof(ExIdleAnimBehaviour))]
public class PlayerCat : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    private MoveBehaviour _mb;
    private AnimationBehaviour _ab;
    private ExIdleAnimBehaviour _xab;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);

        _mb = GetComponent<MoveBehaviour>();
        _ab = GetComponent<AnimationBehaviour>();
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
            _xab.CheckIdleTime();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
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
}
