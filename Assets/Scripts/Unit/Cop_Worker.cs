using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Cop_Worker : Cop
{
    private Vector3 spawnPos;
    private CancellationTokenSource patrolCts;

    public void StartPatrol(Vector3 spawnPos)
    {
        this.spawnPos = spawnPos;

        patrolCts?.Cancel();
        patrolCts?.Dispose();
        patrolCts = new CancellationTokenSource();

        PatrolAsync(patrolCts.Token).Forget();
    }

    private async UniTaskVoid PatrolAsync(CancellationToken ct)
    {
        var machine = MapManager.instance.Machine;
        var trayPos = machine.Tray.transform.position;
        var deskPos = MapManager.instance.Desk.transform.position;

        while (!ct.IsCancellationRequested)
        {
            LookAt(deskPos);
            await UniTask.Delay(TimeSpan.FromSeconds(Define.COP_SPAWN_WAIT_TIME), cancellationToken: ct);

            await MoveToTask(trayPos, ct);

            if (machine.HandCuffStk.Count < Define.COP_HANDCUFF_THRESHOLD)
            {
                await UniTask.WhenAny(
                    UniTask.WaitUntil(() => machine.HandCuffStk.Count >= Define.COP_HANDCUFF_THRESHOLD, cancellationToken: ct),
                    UniTask.Delay(TimeSpan.FromSeconds(Define.COP_HANDCUFF_WAIT_TIME), cancellationToken: ct)
                );
            }

            await MoveToTask(spawnPos, ct);
        }
    }

    private void LookAt(Vector3 targetPos)
    {
        var lookDir = targetPos - transform.position;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }

    private UniTask MoveToTask(Vector3 targetPos, CancellationToken ct)
    {
        var tcs = new UniTaskCompletionSource();
        ct.Register(() => tcs.TrySetCanceled());
        MoveTo(targetPos, () => tcs.TrySetResult());
        return tcs.Task;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        patrolCts?.Cancel();
        patrolCts?.Dispose();
    }
}
