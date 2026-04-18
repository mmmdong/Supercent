using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Player")] [SerializeField] private Player player;
    [Header("UI")] [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField] private float handleRange = 1f;

    private Vector2 inputDirection;
    private Vector2 center;
    private Canvas canvas;
    private float backgroundRadius;

    private CancellationTokenSource moveCts;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        backgroundRadius = background.sizeDelta.x * 0.5f;
        background.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.gameObject.SetActive(true);
        center = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, eventData.position);
        background.position = center;

        moveCts = new CancellationTokenSource();
        MoveLoopAsync().Forget();
    }

    public void OnDrag(PointerEventData eventData)
    {
        var delta = eventData.position - center;
        var maxRadius = backgroundRadius * handleRange;

        if (delta.magnitude > maxRadius)
            delta = delta.normalized * maxRadius;

        handle.localPosition = delta / canvas.scaleFactor;
        inputDirection = delta.magnitude > 0f ? delta.normalized : Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        moveCts?.Cancel();
        moveCts?.Dispose();
        moveCts = null;
        player.PlayAnimation(Define.ANIMATION_IDLE);

        background.gameObject.SetActive(false);
        inputDirection = Vector2.zero;
        handle.localPosition = Vector2.zero;
    }

    private async UniTask MoveLoopAsync()
    {
        player.PlayAnimation(Define.ANIMATION_RUN);
        while (true)
        {
            player.MoveDirection(GetMoveDirection());
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate, moveCts.Token);
        }
    }

    private Vector3 GetMoveDirection()
    {
        var vec = new Vector3(inputDirection.x, 0f, inputDirection.y);
        return vec;
    }
}