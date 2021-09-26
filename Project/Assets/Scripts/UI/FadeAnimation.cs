using UnityEngine;
using UnityEngine.Events;

public class FadeAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] [Range(0.0f, 1.0f)] private float initialAlpha;
    [SerializeField] [Range(0.0f, 1.0f)] private float finalAlpha;
    [SerializeField] private LeanTweenType type = LeanTweenType.easeOutSine;
    [SerializeField] private float time;
    [SerializeField] private float startDelay;
    [SerializeField] private float endDelay;
    [SerializeField] private bool ignoreTimeScale;
    [SerializeField] private UnityEvent onStart;
    [SerializeField] private UnityEvent onComplete;

    [Header("Dependencies")] 
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        AnimateIn();
    }

    private void AnimateIn()
    {
        canvasGroup.alpha = initialAlpha;
        LeanTween.alphaCanvas(canvasGroup, finalAlpha, time).setEase(type).setDelay(startDelay).setIgnoreTimeScale(ignoreTimeScale).setOnComplete(onStart.Invoke);
    }

    public void AnimateOut()
    {
        LeanTween.alphaCanvas(canvasGroup, initialAlpha, time).setEase(type).setDelay(endDelay).setIgnoreTimeScale(ignoreTimeScale).setOnComplete(onComplete.Invoke);
    }
}
