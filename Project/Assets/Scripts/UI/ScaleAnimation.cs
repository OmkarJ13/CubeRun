using UnityEngine;
using UnityEngine.Events;

public class ScaleAnimation : MonoBehaviour
{
    [Header("Animation")] 
    [SerializeField] private Vector2 initialScale;
    [SerializeField] private Vector2 finalScale;
    [SerializeField] private LeanTweenType type = LeanTweenType.easeOutSine;
    [SerializeField] private float time;
    [SerializeField] private float startDelay;
    [SerializeField] private float endDelay;
    [SerializeField] private bool ignoreTimeScale;
    [SerializeField] private bool animateAlpha;
    [SerializeField] private bool playOnAwake;
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
        if (playOnAwake) 
            AnimateIn();
    }

    public void AnimateIn()
    {
        rectTransform.localScale = initialScale;
        LeanTween.scale(rectTransform, finalScale, time).setEase(type).setDelay(startDelay).setIgnoreTimeScale(ignoreTimeScale).setOnComplete(onAnimateIn.Invoke);

        if (animateAlpha)
        {
            canvasGroup.alpha = 0.0f;
            LeanTween.alphaCanvas(canvasGroup, 1.0f, time).setEase(type).setDelay(startDelay).setIgnoreTimeScale(ignoreTimeScale);
        }
    }

    public void AnimateOut()
    {
        LeanTween.scale(rectTransform, initialScale, time).setEase(type).setDelay(endDelay).setIgnoreTimeScale(ignoreTimeScale).setOnComplete(onAnimateOut.Invoke);
        
        if (animateAlpha)
            LeanTween.alphaCanvas(canvasGroup, 0.0f, time).setEase(type).setDelay(endDelay).setIgnoreTimeScale(ignoreTimeScale);
    }
}
