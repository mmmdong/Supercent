using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MachineController : InteractController
{
    #region Dotween resource

    private readonly float JUMP_POWER = 2.5f;
    private readonly int JUMP_COUNT = 1;
    private readonly float ROCK_JUMP_DURATION = 1f;
    private readonly float HANDCUFF_JUMP_DURATION = 0.5f;
    private readonly float SCROLL_DURATION = 2.5f;
    private readonly float RAIL_MOVE_DISTANCE = 3f;
    private readonly Vector3 TRAY_LOCAL_POS = new Vector3(-3.75f, 0.25f, 0f);

    #endregion

    [SerializeField] private MeshRenderer belt;
    [SerializeField] private ZoneTrigger zone;

    private static readonly int MainTexSt = Shader.PropertyToID("_MainTex_ST");

    private MaterialPropertyBlock mpb;
    private Vector2 offset = Vector2.one;
    private Vector2 tiling = Vector2.one;
    private Stack<Prop> handCuffStk = new Stack<Prop>();
    public Stack<Prop> HandCuffStk => handCuffStk;

    private bool isOn;

    protected override void Awake()
    {
        base.Awake();
        mpb = new MaterialPropertyBlock();
    }

    protected override void Start()
    {
        base.Start();
        StartRailScroll();
        InstantiateHandCuffAsync().Forget();
    }

    private void StartRailScroll()
    {
        var uvOffset = RAIL_MOVE_DISTANCE / (belt.localBounds.size.x * belt.transform.localScale.x);
        DOTween.To(() => offset.y, y => offset.y = y, offset.y - uvOffset, SCROLL_DURATION * 2.125f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .OnUpdate(() =>
            {
                belt.GetPropertyBlock(mpb);
                mpb.SetVector(MainTexSt, new Vector4(tiling.x, tiling.y, offset.x, offset.y));
                belt.SetPropertyBlock(mpb);
            });
    }

    private async UniTaskVoid InstantiateHandCuffAsync()
    {
        while (true)
        {
            await UniTask.WaitUntil(() => zone.PropStack.Count > 0);

            var prop = zone.PropStack.Pop();
            await prop.transform
                .DOJump(propEntry.position, JUMP_POWER, JUMP_COUNT, ROCK_JUMP_DURATION)
                .SetEase(Ease.Linear)
                .AsyncWaitForCompletion();
            prop.Release();

            var handCuff = ObjectPool.GetObject<Prop_Handcuff>(Define.PooledEnum.Prop_Handcuff, transform);
            handCuff.SetLocalPosition(propEntry.localPosition);
            handCuff.SetLocalRotation(Quaternion.identity);
            handCuff.transform.DOLocalMoveX(-RAIL_MOVE_DISTANCE, SCROLL_DURATION)
                .SetRelative()
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    handCuff.transform.DOScale(Vector3.one * 0.5f, 0.1f)
                        .SetRelative()
                        .SetEase(Ease.Linear);
                    handCuff.transform.DOLocalJump(
                            TRAY_LOCAL_POS + Vector3.up * 0.1f * handCuffStk.Count,
                            JUMP_POWER,
                            JUMP_COUNT,
                            HANDCUFF_JUMP_DURATION)
                        .SetEase(Ease.Linear)
                        .OnComplete(() => { handCuffStk.Push(handCuff); });
                });
        }
    }
}