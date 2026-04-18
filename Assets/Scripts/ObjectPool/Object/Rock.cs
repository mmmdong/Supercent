using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Rock : PooledObject
{
    public bool IsInit => isInit;
    private bool isInit = true;

    public override void SetInitEffect()
    {
        base.SetInitEffect();
        transform.localPosition += Vector3.down * 2f;

        transform.DOLocalMoveY(0f, 0.5f, false)
            .SetEase(Ease.OutBack)
            .OnComplete(() => { isInit = true; });
    }

    public override void Release()
    {
        base.Release();
        isInit = false;
        MapManager.instance.Respawn(transform.localPosition);
        var hit = ObjectPool.GetObject<Pool_Particle>(Define.PooledEnum.Hit_Particle);
        hit.SetLocalPosition(transform.localPosition + Vector3.up * 0.5f);
    }
}