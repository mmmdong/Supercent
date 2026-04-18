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
        if (other.TryGetComponent(out Player player))
        {
            OnEnterCallback(player);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        deltaTime += Time.deltaTime;
        if (deltaTime < Define.PROPSETTING_TIME)
            return;

        if (other.TryGetComponent(out Player player))
        {
            if (player.DropProp(propType))
            {
                OnStayCallback(player);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            OnExitCallback(player);
        }
    }

    protected virtual void OnEnterCallback(Player player)
    {
    }

    protected virtual void OnStayCallback(Player player)
    {
        deltaTime = 0f;
    }

    protected virtual void OnExitCallback(Player player)
    {
        deltaTime = 0f;
    }
}