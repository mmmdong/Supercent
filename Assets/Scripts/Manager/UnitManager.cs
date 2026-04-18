using System;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    public Transform[] workerGenTrans;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        SpawnCop();
    }

    public void SpawnCop()
    {
        var deskZone = MapManager.instance.Desk.DeskZone;
        var spawnPos = deskZone.transform.position;
        var cop = ObjectPool.GetObject<Cop>(Define.PooledEnum.Cop, transform);
        cop.transform.position = spawnPos;
        cop.StartPatrol(spawnPos);
    }

    public void SpawnWorkers()
    {
        for (var i = 0; i < Define.WORKER_COUNT; i++)
        {
            var worker = ObjectPool.GetObject<Worker>(Define.PooledEnum.Worker, transform);
            var origin = workerGenTrans[i].position;
            worker.transform.position = origin;
            worker.StartPatrol(origin);
        }
    }
}