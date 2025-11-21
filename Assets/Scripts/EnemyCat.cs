using UnityEngine;

[RequireComponent(typeof(MoveBehaviour), typeof(MoveAnimBehaviour))]
public class EnemyCat : MonoBehaviour
{
    private MoveBehaviour _mb;
    private MoveAnimBehaviour _mab;

    [SerializeField] private Transform _negativeLimit;
    [SerializeField] private Transform _positiveLimit;

    private float _direction = 1f;

    private void Awake()
    {
        _mb = GetComponent<MoveBehaviour>();
        _mab = GetComponent<MoveAnimBehaviour>();
    }

    private void Update()
    {
        _mb.MoveCharacter(new Vector2(_direction, 0f));
        _mab.RunAnimation(new Vector2(_direction, 0f));

        CheckLimits();
    }

    private void CheckLimits()
    {
        if (transform.position.x >= _positiveLimit.position.x)
        {
            _direction = -1f;
        }
        else if (transform.position.x <= _negativeLimit.position.x)
        {
            _direction = 1f;
        }
    }
}
