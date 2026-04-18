using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    #region Readonly Fields

    private readonly Vector3 BASE_ROCK_POSITION = new Vector3(-3.5f, 0, 4.5f);
    private readonly int ROW_COUNT = 8;
    private readonly int COLUMN_COUNT = 14;
    private readonly float RESPAWN_INTERVAL = 4f;

    #endregion

    [SerializeField] private Transform rockPar;
    [SerializeField] private BankTrigger bank;
    public BankTrigger Bank => bank;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        SpawnAllRocks();
    }

    private void SpawnAllRocks()
    {
        for (var i = 0; i < ROW_COUNT; i++)
        {
            for (var j = 0; j < COLUMN_COUNT; j++)
            {
                var pos = BASE_ROCK_POSITION + new Vector3(i, 0, j);
                var rock = ObjectPool.GetObject<Rock>(Define.PooledEnum.Rock, rockPar);
                rock.SetLocalPosition(pos);
            }
        }
    }

    public void Respawn(Vector3 pos)
    {
        RespawnAsync(pos).Forget();
    }

    private async UniTaskVoid RespawnAsync(Vector3 pos)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(RESPAWN_INTERVAL));
        
        var rock = ObjectPool.GetObject<Rock>(Define.PooledEnum.Rock, rockPar);
        rock.SetLocalPosition(pos);
        rock.SetInitEffect();
    }
}