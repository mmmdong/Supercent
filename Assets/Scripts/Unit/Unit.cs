using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : PooledObject
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 dir;
    protected Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public virtual void MoveDirection(Vector3 direction)
    {
        dir = SetDirection(direction);
        transform.LookAt(transform.position + dir);
        transform.position += dir.normalized * moveSpeed * Time.deltaTime;
    }

    public void PlayAnimation(string animationName) => animator.SetTrigger(animationName);
    protected virtual Vector3 SetDirection(Vector3 direction) => direction;
}