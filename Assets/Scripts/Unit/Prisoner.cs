using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Prisoner : Unit
{
    #region MyRegion

    private const float ARRIVE_THRESHOLD = 0.15f;
    private const int MONEY_COUNT = 2;

    #endregion

    public int needHandcuff = 4;

    [Header("UI")] [SerializeField] private GameObject canvas;
    [SerializeField] private Text countTxt;
    [SerializeField] private Image countGauge;

    private CancellationTokenSource _moveCts;
    private int curHandcuff = 0;

    private bool isReady = false;
    public bool IsReady => isReady;

    public override void Init(Define.PooledEnum id)
    {
        base.Init(id);
        canvas.SetActive(false);
    }

    public void MoveTo(Vector3 targetPos, Action onArrived = null)
    {
        // 기존 이동 취소 후 새 이동 시작
        _moveCts?.Cancel();
        _moveCts?.Dispose();
        _moveCts = new CancellationTokenSource();

        MoveAsync(new Vector3(targetPos.x, transform.position.y, targetPos.z), onArrived, _moveCts.Token).Forget();
    }

    private async UniTaskVoid MoveAsync(Vector3 targetPos, Action onArrived, CancellationToken ct)
    {
        PlayAnimation(Define.ANIMATION_RUN);

        while (!ct.IsCancellationRequested)
        {
            var diff = targetPos - transform.position;
            diff.y = 0f;

            if (diff.magnitude <= ARRIVE_THRESHOLD)
            {
                transform.position = targetPos;
                break;
            }

            MoveDirection(diff);
            await UniTask.Yield(PlayerLoopTiming.Update, ct);
        }

        if (ct.IsCancellationRequested) return;

        PlayAnimation(Define.ANIMATION_IDLE);
        onArrived?.Invoke();
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
        
        for (var i = 0; i < MONEY_COUNT; i++)
            MapManager.instance.Bank.AddMoney();
        
        MoveTo(transform.position + Vector3.right * 10f, () => Release());

        return true;
    }

    private void OnDestroy()
    {
        _moveCts?.Cancel();
        _moveCts?.Dispose();
    }
}