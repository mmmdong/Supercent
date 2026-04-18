using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Prop : PooledObject
{
    private Vector3 oriScale;

    public override void Init(Define.PooledEnum id)
    {
        base.Init(id);
        oriScale = transform.localScale;
        SetInitEffect();
    }

    public override void SetInitEffect()
    {
        base.SetInitEffect();
        transform.localScale = Vector3.zero;
        transform
            .DOScale(oriScale, 0.5f)
            .SetEase(Ease.OutBack);
    }

    public void DotweenSkip()
    {
        transform.DOKill();
        transform.localScale = oriScale;
    }

    public override void Release()
    {
        base.Release();
        DotweenSkip();
    }
}