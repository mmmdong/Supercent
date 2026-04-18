using Cysharp.Threading.Tasks;
using UnityEngine;

public class Worker : Unit
{
    private RockDestroyController rockDestroyController;
    private Vector3 origin;
    private bool isPatrolling;

    protected override void Awake()
    {
        base.Awake();
        rockDestroyController = GetComponentInChildren<RockDestroyController>(true);
    }

    public override void Init(Define.PooledEnum id)
    {
        base.Init(id);
        rockDestroyController.gameObject.SetActive(true);
        animator.SetBool(Define.ANIMATION_PICKING, true);
    }

    public void StartPatrol(Vector3 origin)
    {
        this.origin = origin;
        isPatrolling = true;
        MoveToForward();
    }

    private void MoveToForward()
    {
        if (!isPatrolling) return;
        MoveTo(origin + Vector3.forward * Define.WORKER_PATROL_DISTANCE, MoveToOrigin);
    }

    private void MoveToOrigin()
    {
        if (!isPatrolling) return;
        MoveTo(origin, MoveToForward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Rock>(out _)) return;
        isPatrolling = false;
        StopMove();
        PlayRockAnimation().Forget();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Rock>(out _)) return;
        isPatrolling = true;
        MoveToForward();
    }

    private async UniTaskVoid PlayRockAnimation()
    {
        PlayAnimation(Define.ANIMATION_IDLE);
        await UniTask.Yield();
        PlayAnimation(Define.ANIMATION_PICKING);
    }

    public override void OnPickingEvent()
    {
        base.OnPickingEvent();
        rockDestroyController.RockDestroy();
    }

    public override void SetProp(Define.PooledEnum prop)
    {
        base.SetProp(prop);

        MapManager.instance.Machine.Zone.OnStayCallback(null);
    }
}