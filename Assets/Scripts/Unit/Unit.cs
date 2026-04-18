using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Unit : PooledObject
{
    [SerializeField] protected float moveSpeed = 5f;
    protected Vector3 dir;
    protected Animator animator;

    private CancellationTokenSource moveCts;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public void MoveTo(Vector3 targetPos, Action onArrived = null)
    {
        moveCts?.Cancel();
        moveCts?.Dispose();
        moveCts = new CancellationTokenSource();

        MoveAsync(new Vector3(targetPos.x, transform.position.y, targetPos.z), onArrived, moveCts.Token).Forget();
    }

    private async UniTaskVoid MoveAsync(Vector3 targetPos, Action onArrived, CancellationToken ct)
    {
        PlayAnimation(Define.ANIMATION_RUN);

        while (!ct.IsCancellationRequested)
        {
            var diff = targetPos - transform.position;
            diff.y = 0f;

            if (diff.magnitude <= Define.ARRIVE_THRESHOLD)
            {
                transform.position = targetPos;
                break;
            }

            MoveDirection(diff);
            await UniTask.Yield(PlayerLoopTiming.Update, ct);
        }

        if (ct.IsCancellationRequested) return;

        PlayAnimation(Define.ANIMATION_IDLE);
        onArrived?.Invoke();
    }

    public virtual void MoveDirection(Vector3 direction)
    {
        dir = SetDirection(direction);
        transform.LookAt(transform.position + dir);
        transform.position += dir.normalized * moveSpeed * Time.deltaTime;
    }

    public void PlayAnimation(string animationName) => animator.SetTrigger(animationName);
    protected virtual Vector3 SetDirection(Vector3 direction) => direction;

    protected virtual void OnDestroy()
    {
        moveCts?.Cancel();
        moveCts?.Dispose();
    }
}
