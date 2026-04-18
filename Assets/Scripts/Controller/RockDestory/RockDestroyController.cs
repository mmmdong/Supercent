using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDestroyController : MonoBehaviour
{
    [SerializeField] private Player player;
    private Stack<Rock> rockStack = new Stack<Rock>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Rock rock))
        {
            OnTriggerEnterCallBack(rock);
        }
    }

    public void RockDestroy()
    {
        if (rockStack.TryPeek(out var rock))
        {
            if (!rock.IsInit)
                return;

            rock.Release();
            player.SetProp(Define.PooledEnum.Prop_Rock);
        }

        rockStack.Clear();
    }

    protected virtual void OnTriggerEnterCallBack(Rock rock)
    {
        rockStack.Push(rock);
    }
}