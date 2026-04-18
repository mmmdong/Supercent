using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DeskController : MonoBehaviour
{
    [SerializeField] private ZoneTrigger deskZone;
    public ZoneTrigger DeskZone => deskZone;
    [SerializeField] private Transform frontDesk;
    public Transform FrontDesk => frontDesk;
    [SerializeField] private Prisoner prisoner;

    private void Start()
    {
        ConsumeHandcuffsProcessAsync().Forget();
    }

    public void SetPrisoner(Prisoner prisoner)
    {
        this.prisoner = prisoner;
    }

    public bool CanTakePrisoner() => prisoner == null;

    private async UniTaskVoid ConsumeHandcuffsProcessAsync()
    {
        while (true)
        {
            await UniTask.WaitUntil(() => prisoner != null && prisoner.IsReady);
            if (deskZone.PropStack.TryPop(out var prop))
            {
                await prop.transform
                    .DOJump(prisoner.transform.position, 3, 1, 0.5f)
                    .OnComplete(() =>
                    {
                        prop.Release();
                        prisoner.GetHandcuff();
                        if (prisoner.CheckFullHandcuff())
                            prisoner = null;
                    })
                    .AsyncWaitForCompletion();
            }
            else
                await UniTask.WaitUntil(() => deskZone.PropStack.Count > 0);
        }
    }
}