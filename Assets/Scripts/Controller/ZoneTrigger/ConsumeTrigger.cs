using System;
using UnityEngine;
using UnityEngine.UI;

public class ConsumeTrigger : ZoneTrigger
{
    [SerializeField] private Define.ConsumeDest consumeDestType;
    [SerializeField] private Image backGround;
    [SerializeField] private Image gauge;
    [SerializeField] private int cost = 50;
    [SerializeField] private Text costText;

    private int accumulatedCost;
    private Action onConsumeComplete;

    public void SetCallback(Action callback)
    {
        onConsumeComplete = callback;
    }

    protected override void Awake()
    {
        base.Awake();
        propType = Define.PooledEnum.Prop_Money;
        gauge.fillAmount = 0f;
    }

    private void Start()
    {
        costText.text = $"{cost}";
    }

    private void UpdateCostText()
    {
        cost *= 2;
        costText.text = $"{cost}";
    }

    protected override void OnEnterCallback(Player player)
    {
        base.OnEnterCallback(player);
        backGround.color = Color.green;
        switch (consumeDestType)
        {
            case Define.ConsumeDest.Hire_Cop:
                break;
            case Define.ConsumeDest.Hire_Worker:
                break;
            case Define.ConsumeDest.LevelUp:
                SetCallback(player.LevelUp);
                break;
        }
    }

    protected override void OnStayCallback(Player player)
    {
        base.OnStayCallback(player);
        accumulatedCost += Define.MONEY_COST;
        gauge.fillAmount = (float)accumulatedCost / cost;

        if (accumulatedCost >= cost)
        {
            accumulatedCost = 0;
            gauge.fillAmount = 0f;
            onConsumeComplete?.Invoke();
            switch (consumeDestType)
            {
                case Define.ConsumeDest.Hire_Cop:
                case Define.ConsumeDest.Hire_Worker:
                    gameObject.SetActive(false);
                    break;
                case Define.ConsumeDest.LevelUp:
                    UpdateCostText();
                    break;
            }
        }
    }

    protected override void OnExitCallback(Player player)
    {
        base.OnExitCallback(player);
        backGround.color = Color.white;
    }
}