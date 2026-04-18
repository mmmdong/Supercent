using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Worker : Unit
{
    private RockDestroyController rockDestroyController;
    private Vector3 origin;
    private bool isPatrolling;
    private bool isHeadingForward;

    private readonly HashSet<Rock> rocksInTrigger = new();

    protected override void Awake()
    {
        base.Awake();
        rockDestroyController = GetComponentInChildren<RockDestroyController>(true);
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
        isHeadingForward = true;
        animator.SetBool(Define.ANIMATION_PICKING, false);
        MoveTo(origin + Vector3.forward * Define.WORKER_PATROL_DISTANCE, MoveToOrigin);
    }

    private void MoveToOrigin()
    {
        if (!isPatrolling) return;
        isHeadingForward = false;
        animator.SetBool(Define.ANIMATION_PICKING, false);
        MoveTo(origin, MoveToForward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Rock>(out var rock) || !rock.IsInit) return;

        rocksInTrigger.Add(rock);

        if (!isPatrolling) return;

        isPatrolling = false;
        StopMove();
        PlayRockAnimation();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Rock>(out var rock)) return;
        rocksInTrigger.Remove(rock);
    }

    private void PlayRockAnimation()
    {
        PlayAnimation(Define.ANIMATION_IDLE);
        animator.SetBool(Define.ANIMATION_PICKING, true);
    }

    public override void OnPickingEvent()
    {
        base.OnPickingEvent();
        rockDestroyController.RockDestroy();
        CheckRockAndResume();
    }

    private void CheckRockAndResume()
    {
        rocksInTrigger.RemoveWhere(r => r == null || !r.gameObject.activeInHierarchy || !r.IsInit);

        if (rocksInTrigger.Count == 0)
        {
            isPatrolling = true;
            if (isHeadingForward) MoveToForward();
            else MoveToOrigin();
        }
        else
        {
            PlayRockAnimation();
        }
    }

    public override void SetProp(Define.PooledEnum prop)
    {
        base.SetProp(prop);
        MapManager.instance.Machine.Zone.OnStayCallback(null);
    }
}