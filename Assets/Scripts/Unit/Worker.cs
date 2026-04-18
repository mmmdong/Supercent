using UnityEngine;

public class Worker : Unit
{
    private Vector3 _origin;

    public void StartPatrol(Vector3 origin)
    {
        _origin = origin;
        MoveToForward();
    }

    private void MoveToForward()
    {
        MoveTo(_origin + Vector3.forward * Define.WORKER_PATROL_DISTANCE, MoveToOrigin);
    }

    private void MoveToOrigin()
    {
        MoveTo(_origin, MoveToForward);
    }
}
