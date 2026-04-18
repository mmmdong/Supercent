using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop_Handcuff : Prop
{
    public override void Init(Define.PooledEnum id)
    {
        transform.localScale = Vector3.one;
        base.Init(id);
    }
}