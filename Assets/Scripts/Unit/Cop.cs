using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Cop : Unit
{
    [Header("Object")]
    [SerializeField] protected Transform[] propMainPar;
    [SerializeField] protected Transform handCuffPar;
    
    protected Dictionary<Define.PooledEnum, Stack<PooledObject>> propStack = new();
}
