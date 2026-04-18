using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    [SerializeField] protected Define.PooledEnum propType;
    [SerializeField] protected Transform[] propTray;

    protected Stack<Prop> propStack = new Stack<Prop>();
    public Stack<Prop> PropStack => propStack;

    private float deltaTime = 0f;

    protected virtual void Awake()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cop unit))
        {
            OnEnterCallback(unit);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        deltaTime += Time.deltaTime;
        if (deltaTime < Define.PROPSETTING_TIME)
            return;

        if (other.TryGetComponent(out Cop unit))
        {
            if (unit.ConsumeProp(propType))
            {
                deltaTime = 0f;
                OnStayCallback(unit);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Cop unit))
        {
            deltaTime = 0f;
            OnExitCallback(unit);
        }
    }

    protected virtual void OnEnterCallback(Unit unit)
    {
    }

    public virtual void OnStayCallback(Unit unit)
    {
    }

    protected virtual void OnExitCallback(Unit unit)
    {
    }
}