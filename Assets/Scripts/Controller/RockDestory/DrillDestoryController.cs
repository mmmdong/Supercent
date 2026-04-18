using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DrillDestoryController : RockDestroyController
{
    protected override void OnTriggerEnterCallBack(Rock rock)
    {
        base.OnTriggerEnterCallBack(rock);
        RockDestroy();
    }

    private void OnEnable()
    {
        transform.DOLocalMoveZ(0.1f, 0.1f).SetRelative().SetLoops(-1, LoopType.Yoyo);
    }
}