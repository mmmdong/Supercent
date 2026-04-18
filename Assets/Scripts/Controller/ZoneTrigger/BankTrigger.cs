using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BankTrigger : MonoBehaviour
{
    [SerializeField] private Transform[] propTray;

    private bool isProcessing;
    private Stack<Prop_Money> moneyStack = new Stack<Prop_Money>();

    private void OnTriggerStay(Collider other)
    {
        if (isProcessing) return;
        if (moneyStack.Count == 0) return;

        if (other.TryGetComponent(out Player player))
        {
            TransferMoneyToPlayer(player).Forget();
        }
    }

    private async UniTask TransferMoneyToPlayer(Player player)
    {
        isProcessing = true;
        while (moneyStack.TryPop(out var money))
        {
            money.Release();
            player.SetProp(Define.PooledEnum.Prop_Money);
            await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
        }

        isProcessing = false;
    }

    public void AddMoney()
    {
        if (propTray.Length > 1)
        {
            var index = moneyStack.Count / propTray.Length;
            var par = propTray[moneyStack.Count % propTray.Length];
            var prop = ObjectPool.GetObject<Prop_Money>(Define.PooledEnum.Prop_Money, par);
            prop.SetGlobalPosition(par.position + Vector3.up * 0.25f * index);
            prop.SetGlobalRotation(Quaternion.identity);
            prop.SetInitEffect();
            moneyStack.Push(prop);
        }
        else
        {
            var prop = ObjectPool.GetObject<Prop_Money>(Define.PooledEnum.Prop_Money, propTray[0]);
            prop.SetGlobalPosition(propTray[0].position + Vector3.up * 0.25f * moneyStack.Count);
            prop.SetGlobalRotation(Quaternion.identity);
            prop.SetInitEffect();
            moneyStack.Push(prop);
        }
    }
}