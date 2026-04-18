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