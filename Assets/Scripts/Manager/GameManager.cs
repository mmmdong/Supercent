using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Transform effectPar;
    public Transform EffectPar => effectPar;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        ObjectPool.Init();
    }
}