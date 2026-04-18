using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Player : Unit
{
    [Header("PlayerData")] public int level;
    [Header("Object")] [SerializeField] private Transform[] propMainPar;
    [SerializeField] private Transform handCuffPar;
    [SerializeField] private TMP_Text maxText;
    [SerializeField] private GameObject humanBase, drill, tractor;

    private Dictionary<Define.PooledEnum, Stack<PooledObject>> propStack = new();
    public Dictionary<Define.PooledEnum, Stack<PooledObject>> PropStack => propStack;

    // propMainPar 슬롯에 배정된 prop 타입 순서
    private List<Define.PooledEnum> _parOrder = new List<Define.PooledEnum>();

    private RockDestroyController rockDestroyController;
    private Camera mainCam;
    private bool isOnMax;

    protected override void Awake()
    {
        base.Awake();
        mainCam = Camera.main;
        rockDestroyController = GetComponentInChildren<RockDestroyController>(true);

        propStack.Add(Define.PooledEnum.Prop_Rock, new Stack<PooledObject>());
        propStack.Add(Define.PooledEnum.Prop_Money, new Stack<PooledObject>());
        propStack.Add(Define.PooledEnum.Prop_Handcuff, new Stack<PooledObject>());
    }

    public override void MoveDirection(Vector3 direction)
    {
        base.MoveDirection(direction);
        mainCam.transform.position = transform.position + Define.CAM_POS;
    }

    protected override Vector3 SetDirection(Vector3 direction)
        => Quaternion.Euler(0, mainCam.transform.rotation.eulerAngles.y, 0) * base.SetDirection(direction);

    public void SetPickingMode(bool picking)
    { 
        switch (level)
        {
            case 1:
                rockDestroyController.gameObject.SetActive(picking);
                animator.SetBool(Define.ANIMATION_PICKING, picking);
                return;
            case 2:
                drill.SetActive(picking);
                return;
        }

        // Lv 3 이상
        humanBase.SetActive(!picking);
        tractor.SetActive(picking);
    }

    public void SetTakeMode(bool take)
    {
        animator.SetBool(Define.ANIMATION_TAKE, take);
    }

    public override void OnPickingEvent()
    {
        rockDestroyController.RockDestroy();
    }

    public override void SetProp(Define.PooledEnum prop)
    {
        base.SetProp(prop);
        if (prop == Define.PooledEnum.Prop_Handcuff)
        {
            GetObject(prop, handCuffPar);
            return;
        }

        if (propStack[prop].Count >= level * 10)
        {
            ShowMaxText();
            return;
        }

        // 처음 획득하는 종류면 다음 빈 par 슬롯 배정
        if (!_parOrder.Contains(prop))
        {
            if (_parOrder.Count >= propMainPar.Length)
            {
                ShowMaxText();
                return;
            }

            _parOrder.Add(prop);
        }

        GetObject(prop, propMainPar[_parOrder.IndexOf(prop)]);
    }

    private void GetObject(Define.PooledEnum prop, Transform par)
    {
        var propObj = ObjectPool.GetObject<Prop>(prop, par);
        propObj.SetLocalPosition(Vector3.zero + Vector3.up * Define.STACK_GAP * propStack[prop].Count);
        var quaternion = Quaternion.identity;
        switch (prop)
        {
            case Define.PooledEnum.Prop_Rock:
                quaternion = Quaternion.Euler(Vector3.up * 90f);
                break;
            case Define.PooledEnum.Prop_Money:
                quaternion = Quaternion.Euler(Vector3.up * 90f);
                PlayerManager.instance.GetMoney();
                break;
        }

        propObj.SetLocalRotation(quaternion);

        propStack[prop].Push(propObj);
    }

    public bool DropProp(Define.PooledEnum propType)
    {
        if (!propStack[propType].TryPop(out var propObj))
            return false;

        if (propType == Define.PooledEnum.Prop_Money)
            PlayerManager.instance.ConsumeMoney();

        propObj.Release();

        if (propStack[propType].Count == 0)
            UpdateStackPar();

        return true;
    }

    public void UpdateStackPar()
    {
        for (var i = 0; i < _parOrder.Count; i++)
        {
            if (propStack[_parOrder[i]].Count > 0) continue;

            _parOrder.RemoveAt(i);

            // i번 이후 슬롯을 한 칸 앞 par로 이동
            for (var j = i; j < _parOrder.Count; j++)
            {
                foreach (var propObj in propStack[_parOrder[j]])
                    propObj.transform.SetParent(propMainPar[j], false);
            }

            i--;
        }
    }

    private void ShowMaxText()
    {
        if (isOnMax)
            return;
        isOnMax = true;
        var startPos = maxText.transform.localPosition;

        // 초기 상태 복원
        maxText.DOKill();
        var color = maxText.color;
        color.a = 1f;
        maxText.color = color;
        maxText.gameObject.SetActive(true);

        // 위로 올라가면서 FadeOut (두 트윈을 동시에 실행하고 완료까지 대기)
        maxText.transform.DOLocalMoveY(startPos.y + 2f, 1f).SetEase(Ease.OutCubic);
        maxText.DOFade(0f, 1f).SetEase(Ease.InQuad).OnUpdate(() =>
        {
            maxText.transform.rotation =
                Quaternion.LookRotation(maxText.transform.position - mainCam.transform.position);
        }).OnComplete(() =>
        {
            maxText.gameObject.SetActive(false);
            maxText.transform.localPosition = startPos;
            isOnMax = false;
        });
    }

    public void LevelUp()
    {
        level++;
        var effect =
            ObjectPool.GetObject<Pool_Particle>(Define.PooledEnum.LevelUp_Particle, GameManager.instance.EffectPar);
        effect.SetGlobalPosition(transform.position + Vector3.up * 0.1f);
    }
}