using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TrayTrigger : MonoBehaviour
{
    [SerializeField] private MachineController machine;

    private CancellationTokenSource cts;

    private void OnTriggerEnter(Collider other)
    {
        cts = new CancellationTokenSource();
        if (other.TryGetComponent(out Unit unit))
        {
            SetProp(unit).Forget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Unit unit))
        {
            cts.Cancel();
        }
    }

    private async UniTaskVoid SetProp(Unit Unit)
    {
        await UniTask.WaitUntil(() => machine.HandCuffStk.Count > 0, cancellationToken: cts.Token);
        while (true)
        {
            await UniTask.WaitUntil(() => machine.HandCuffStk.Count > 0, cancellationToken: cts.Token);

            var handCuff = machine.HandCuffStk.Pop() as Prop_Handcuff;
            handCuff.Release();
            Unit.SetProp(Define.PooledEnum.Prop_Handcuff);
            await UniTask.Delay(TimeSpan.FromSeconds(Define.PROPSETTING_TIME), cancellationToken: cts.Token);
        }
    }
}