using System.Collections.Generic;
using UnityEngine;

public class Cop : Unit
{
    [Header("Object")] [SerializeField] protected Transform[] propMainPar;
    [SerializeField] protected Transform handCuffPar;

    protected Dictionary<Define.PooledEnum, Stack<PooledObject>> propStack = new();

    public override void SetProp(Define.PooledEnum prop)
    {
        if (!propStack.ContainsKey(prop)) return;

        var propObj = ObjectPool.GetObject<Prop>(prop, handCuffPar);
        propObj.SetLocalPosition(Vector3.up * Define.STACK_GAP * propStack[prop].Count);
        propObj.SetLocalRotation(Quaternion.identity);
        propStack[prop].Push(propObj);
    }

    public override bool ConsumeProp(Define.PooledEnum propType)
    {
        if (!propStack.ContainsKey(propType)) return false;
        if (!propStack[propType].TryPop(out var propObj)) return false;

        propObj.Release();
        return true;
    }
}