using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PooledObject : MonoBehaviour
{
    public PooledEnum ID => id;
    private PooledEnum id;

    protected virtual void Awake()
    {
    }

    public virtual void Init(PooledEnum id)
    {
        this.id = id;
    }

    public void SetLocalPosition(Vector3 pos) => transform.localPosition = pos;
    public void SetGlobalPosition(Vector3 pos) => transform.position = pos;
    public void SetLocalRotation(Quaternion rot) => transform.localRotation = rot;
    public void SetGlobalRotation(Quaternion rot) => transform.rotation = rot;

    public virtual void SetInitEffect()
    {
    }

    public virtual void Release()
    {
        ObjectPool.ReleaseObject(this);
    }
}