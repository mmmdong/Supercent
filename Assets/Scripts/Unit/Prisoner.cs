using UnityEngine;
using UnityEngine.UI;

public class Prisoner : Unit
{
    public int needHandcuff = 4;

    [Header("UI")] [SerializeField] private GameObject canvas;
    [SerializeField] private Text countTxt;
    [SerializeField] private Image countGauge;

    private int curHandcuff = 0;

    private bool isReady = false;
    public bool IsReady => isReady;

    public override void Init(Define.PooledEnum id)
    {
        base.Init(id);
        canvas.SetActive(false);
    }

    public void SetSpeechBubble(bool isOn)
    {
        if (isOn)
        {
            countTxt.text = $"{needHandcuff}";
        }
        else
        {
            curHandcuff = 0;
            countGauge.fillAmount = 0f;
        }

        isReady = isOn;
        canvas.SetActive(isOn);
    }

    public void GetHandcuff()
    {
        curHandcuff++;
        countGauge.fillAmount = (float)curHandcuff / needHandcuff;
    }

    public bool CheckFullHandcuff()
    {
        if (curHandcuff < needHandcuff)
            return false;

        SetSpeechBubble(false);

        for (var i = 0; i < Define.PRISONER_MONEY_COUNT; i++)
            MapManager.instance.Bank.AddMoney();

        MoveTo(transform.position + Vector3.right * 10f, () => Release());

        return true;
    }
}
