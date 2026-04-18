using UnityEngine;

public class TrayTrigger : MonoBehaviour
{
    [SerializeField] private MachineController machine;

    private float deltaTime;

    private void OnTriggerStay(Collider other)
    {
        deltaTime += Time.deltaTime;
        if (deltaTime < Define.PROPSETTING_TIME) return;
        if (machine.HandCuffStk.Count == 0) return;

        if (other.TryGetComponent(out Unit unit))
        {
            machine.HandCuffStk.Pop().Release();
            unit.SetProp(Define.PooledEnum.Prop_Handcuff);
            deltaTime = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Unit _))
            deltaTime = 0f;
    }
}
