using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ExIdleAnimBehaviour : MonoBehaviour
{
    private Animator _animator;

    [Header("Configuració")]
    [Tooltip("Llista dels noms dels paràmetres d'animació d'idle")]
    [SerializeField] private List<string> idleParams = new List<string>();

    [Tooltip("Temps d'espera abans de reproduir una animació d'idle")]
    [SerializeField] private float idleWaitTime = 5f;
    private float currentIdleTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        currentIdleTime = idleWaitTime + Time.time;
    }

    public bool CheckIdleTime(bool idle)
    {
        if (Time.time > currentIdleTime)
        {
            idle = false;
            RunAnimation();
        }
        return idle;
    }

    public void RunAnimation()
    {
        int rand = Random.Range(0, idleParams.Count);
        
        if (rand == 0)
        {
            _animator.SetBool(idleParams[0], true);
        }
        else if (rand == 1)
        {
            _animator.SetBool(idleParams[1], true);
        }
    }

    public void CancelAnimations()
    {
        foreach (string param in idleParams)
        {
            _animator.SetBool(param, false);
        }
    }

    public void SetIdleTime()
    {
        currentIdleTime = Time.time + idleWaitTime;
    }
}
