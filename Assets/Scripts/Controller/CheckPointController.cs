using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    #region Readonly Fields

    private readonly float QUE_SPACE = 1.5f;
    private readonly int QUE_FULL_COUNT = 2;

    #endregion

    [SerializeField] private DeskController desk;
    [SerializeField] private Transform genPoint;

    [Header("Queue")] [SerializeField] private Transform waitPoint;

    private Queue<Prisoner> waitQueue = new Queue<Prisoner>();
    private CancellationTokenSource queueCts;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        queueCts = new CancellationTokenSource();
        ProcessQueueAsync(queueCts.Token).Forget();
    }

    private void OnDestroy()
    {
        queueCts?.Cancel();
        queueCts?.Dispose();
    }

    private Prisoner SpawnPrisoner()
    {
        var prisoner = ObjectPool.GetObject<Prisoner>(Define.PooledEnum.Prisoner, null);
        prisoner.transform.position = genPoint.position;
        EnqueuePrisoner(prisoner);
        return prisoner;
    }

    private void EnqueuePrisoner(Prisoner prisoner)
    {
        prisoner.MoveTo(GetQueuePosition(waitQueue.Count));
        waitQueue.Enqueue(prisoner);
    }

    private async UniTaskVoid ProcessQueueAsync(CancellationToken ct)
    {
        while (true)
        {
            SpawnPrisoner();
            if (waitQueue.Count < QUE_FULL_COUNT)
                continue;

            await UniTask.WaitUntil(desk.CanTakePrisoner, cancellationToken: ct);

            var prisoner = waitQueue.Dequeue();
            desk.SetPrisoner(prisoner);
            animator.SetTrigger(Define.CHECKPOINT_OPEN);
            prisoner.MoveTo(desk.FrontDesk.position, () => prisoner.SetSpeechBubble(true));

            UpdateQueuePositions();
        }
    }

    private void UpdateQueuePositions()
    {
        var index = 0;
        foreach (var prisoner in waitQueue)
        {
            prisoner.MoveTo(GetQueuePosition(index));
            index++;
        }
    }

    private Vector3 GetQueuePosition(int index)
    {
        var lineDir = (genPoint.position - waitPoint.position).normalized;
        lineDir.y = 0f;
        return waitPoint.position + lineDir * (QUE_SPACE * index);
    }
}