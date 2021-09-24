using UnityEngine;
using UnityEngine.Events;

public class ScaleAnimation : MonoBehaviour
{
    [Header("Animation")] 
    [SerializeField] private LeanTweenType type;
    [SerializeField] private Vector2 initialScale;
    [SerializeField] private Vector2 finalScale;
    [SerializeField] private float time;
    [SerializeField] private float startDelay;
    [SerializeField] private float endDelay;
    [SerializeField] private bool ignoreTimeScale;
    [SerializeField] private bool animateAlpha;
    [SerializeField] private UnityEvent onAnimateIn;
    [SerializeField] private UnityEvent onAnimateOut;

    [Header("Dependencies")] 
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        AnimateIn();
    }

    public void AnimateIn()
    {
        rectTransform.localScale = initialScale;
        canvasGroup.alpha = 0.0f;

        LeanTween.scale(rectTransform, finalScale, time).setEase(type).setDelay(startDelay).setIgnoreTimeScale(ignoreTimeScale).setOnComplete(onAnimateIn.Invoke);
        LeanTween.alphaCanvas(canvasGroup, 1.0f, time).setEase(type).setDelay(startDelay).setIgnoreTimeScale(ignoreTimeScale);
    }

    public void AnimateOut()
    {
        LeanTween.scale(rectTransform, initialScale, time).setEase(type).setDelay(endDelay).setIgnoreTimeScale(ignoreTimeScale).setOnComplete(onAnimateOut.Invoke);
        LeanTween.alphaCanvas(canvasGroup, 0.0f, time).setEase(type).setDelay(endDelay).setIgnoreTimeScale(ignoreTimeScale);
    }
}
