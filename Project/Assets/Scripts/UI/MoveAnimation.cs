using UnityEngine;
using UnityEngine.Events;

public class MoveAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Vector2 initialPos;
    [SerializeField] private Vector2 finalPos;
    [SerializeField] private LeanTweenType type = LeanTweenType.easeOutSine;
    [SerializeField] private float time;
    [SerializeField] private float startDelay;
    [SerializeField] private float endDelay;
    [SerializeField] private bool ignoreTimeScale;
    [SerializeField] private bool animateAlpha;
    [SerializeField] private UnityEvent onAnimateIn;
    [SerializeField] private UnityEvent onAnimateOut;

    [Header("Dependencies")] 
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        AnimateIn();
    }

    public void AnimateIn()
    {
        rectTransform.anchoredPosition = initialPos;
        LeanTween.move(rectTransform, finalPos, time).setEase(type).setDelay(startDelay).setIgnoreTimeScale(ignoreTimeScale).setOnComplete(onAnimateIn.Invoke);

        if (animateAlpha)
        {
            canvasGroup.alpha = animateAlpha ? 0.0f : canvasGroup.alpha;
            LeanTween.alphaCanvas(canvasGroup, 1.0f, time).setEase(type).setDelay(startDelay).setIgnoreTimeScale(ignoreTimeScale);
        }
    }

    public void AnimateOut()
    {
        LeanTween.move(rectTransform, initialPos, time).setEase(type).setDelay(endDelay).setIgnoreTimeScale(ignoreTimeScale).setOnComplete(onAnimateOut.Invoke);
        
        if (animateAlpha)
            LeanTween.alphaCanvas(canvasGroup, 0.0f, time).setEase(type).setDelay(endDelay).setIgnoreTimeScale(ignoreTimeScale);
    }
}
