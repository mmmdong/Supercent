using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [SerializeField] private Player player;
    public Player Player => player;
    [SerializeField] private int money;
    public int Money => money;

    [Header("UI")]
    [SerializeField] private Text moneyText;

    private void Start()
    {
        UpdateMoney();
    }

    public void GetMoney()
    {
        money += Define.MONEY_COST;
        UpdateMoney();
    }

    public void ConsumeMoney()
    {
        money -= Define.MONEY_COST;
        UpdateMoney();
    }

    private void UpdateMoney() => moneyText.text = $"{money}";
}