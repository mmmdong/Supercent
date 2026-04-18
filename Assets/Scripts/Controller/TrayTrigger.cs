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
        if (other.TryGetComponent(out Player player))
        {
            SetProp(player).Forget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            cts.Cancel();
        }
    }

    private async UniTaskVoid SetProp(Player player)
    {
        await UniTask.WaitUntil(() => machine.HandCuffStk.Count > 0, cancellationToken: cts.Token);
        player.SetTakeMode(true);
        while (true)
        {
            await UniTask.WaitUntil(() => machine.HandCuffStk.Count > 0, cancellationToken: cts.Token);

            var handCuff = machine.HandCuffStk.Pop() as Prop_Handcuff;
            handCuff.Release();
            player.SetProp(Define.PooledEnum.Prop_Handcuff);
            await UniTask.Delay(TimeSpan.FromSeconds(Define.PROPSETTING_TIME), cancellationToken: cts.Token);
        }
    }
}