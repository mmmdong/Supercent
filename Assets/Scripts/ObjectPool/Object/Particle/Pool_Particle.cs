using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Particle : PooledObject
{
    private ParticleSystem particle;

    protected override void Awake()
    {
        base.Awake();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    public override void Init(Define.PooledEnum id)
    {
        base.Init(id);
        
        particle.Play();
    }

    private void OnParticleSystemStopped()
    {
        ObjectPool.ReleaseObject(this);
    }
}
