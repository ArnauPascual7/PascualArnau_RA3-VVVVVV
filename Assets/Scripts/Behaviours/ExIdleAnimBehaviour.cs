using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ExIdleAnimBehaviour : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private List<string> IdleParams = new List<string>();

    [SerializeField] private float IdleWaitTime = 5f;
    private float CurrentIdleTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        CurrentIdleTime = IdleWaitTime;
    }

    public bool CheckIdleTime(bool idle)
    {
        if (Time.time > CurrentIdleTime)
        {
            idle = false;
            RunAnimation();
        }
        return idle;
    }

    public void RunAnimation()
    {
        int rand = Random.Range(0, IdleParams.Count);
        
        if (rand == 0)
        {
            _animator.SetBool(IdleParams[0], true);
        }
        else if (rand == 1)
        {
            _animator.SetBool(IdleParams[1], true);
        }
    }

    public void CancelAnimations()
    {
        foreach (string param in IdleParams)
        {
            _animator.SetBool(param, false);
        }
    }

    public void SetIdleTime()
    {
        CurrentIdleTime = Time.time + IdleWaitTime;
    }
}
