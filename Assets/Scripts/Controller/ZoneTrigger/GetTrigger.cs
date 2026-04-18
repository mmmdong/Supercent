using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTrigger : ZoneTrigger
{
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void OnEnterCallback(Player player)
    {
        base.OnEnterCallback(player);
        spriteRenderer.color = Color.green;
    }

    protected override void OnStayCallback(Player player)
    {
        base.OnStayCallback(player);
        if (propTray.Length > 1)
        {
            var index = propStack.Count / propTray.Length;
            var prop = ObjectPool.GetObject<Prop>(propType, propTray[propStack.Count % propTray.Length]);
            prop.SetLocalPosition(Vector3.zero + Vector3.up * Define.STACK_GAP * index);
            prop.SetLocalRotation(Quaternion.identity);
            prop.SetInitEffect();
            propStack.Push(prop);
        }
        else
        {
            var prop = ObjectPool.GetObject<Prop>(propType, propTray[0]);
            prop.SetLocalPosition(Vector3.zero + Vector3.up * Define.STACK_GAP * propStack.Count);
            prop.SetLocalRotation(Quaternion.identity);
            prop.SetInitEffect();
            propStack.Push(prop);
        }
    }

    protected override void OnExitCallback(Player player)
    {
        base.OnExitCallback(player);
        spriteRenderer.color = Color.white;
    }
}