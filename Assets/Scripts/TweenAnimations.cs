using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TweenAnimations : MonoBehaviour
{
    public bool DEBUG;
    public TweenAnimation[] animations;
    private Dictionary<string, TweenAnimation> animDict;

    private void OnEnable()
    {
        Initialize();
    }

    private void Update()
    {
        if (DEBUG)
        {
            PlayAllAnimations();
            DEBUG = false;
        }
    }

    private void Initialize()
    {
        animDict = new Dictionary<string, TweenAnimation>();
        foreach (var animation in animations)
        {
            if (animation.autoStart)
                animation.Play(this);

            animDict.Add(animation.uniqueName, animation);
        }
    }

    public void PlayAllAnimations()
    {
        foreach (var animation in animations)
        {
            animation.Play(this);
        }
    }

    public void PlayAnimation(string name)
    {
        animDict[name].Play(this);
    }

}

[System.Serializable]
public class TweenAnimation
{
    public string uniqueName;
    public RectTransform transform;
    public bool autoStart, enabledOnStart, disabledAtEnd;
    public float executionDelay = 0;
    public float animationDuration = 0.5f;
    public Ease easing;
    public float elasticAmplitude, elasticPeriod;
    public TweenType animationType;
    public bool punchAnimation;
    public bool percentSizeVector;
    public Vector3 fromVector, toVector;
    public float fromFloat, toFloat;
    public bool useTransforms;
    public RectTransform fromTransform, toTransform;
    public bool relative, invertDirection;

    private MonoBehaviour _context;
    private Action _onComplete;

    public void Play(MonoBehaviour context, Action onComplete = null)
    {
        if (!transform)
            return;

        _context = context;
        _onComplete = onComplete;

        transform.gameObject.SetActive(enabledOnStart);
        switch (animationType)
        {
            case TweenType.scale:
                context.StartCoroutine(InvokeAfterSeconds(ScaleAnimation, executionDelay));
                break;
            case TweenType.translate:
                context.StartCoroutine(InvokeAfterSeconds(TranslateAnimtion, executionDelay));
                break;
            case TweenType.resize:
                context.StartCoroutine(InvokeAfterSeconds(ResizeAnimation, executionDelay));
                break;
            case TweenType.fade:
                context.StartCoroutine(StartCoroutineAfterSeconds(FadeAnimation, executionDelay));
                break;
            default:
                break;
        }
    }

    private void OnComplete (){
        transform.gameObject.SetActive(!disabledAtEnd);

        if (_onComplete != null)
            _onComplete.Invoke();
    }

    private IEnumerator InvokeAfterSeconds(Action method, float delay)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(delay < 0 ? 0 : delay);

        if (method != null)
            method.Invoke();
        transform.gameObject.SetActive(true);
    }

    private IEnumerator StartCoroutineAfterSeconds(Func<IEnumerator> method, float delay)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(delay < 0 ? 0 : delay);

        if (method != null)
            _context.StartCoroutine(method.Invoke());
        transform.gameObject.SetActive(true);
    }

    private IEnumerator FadeAnimation()
    {
        CanvasGroup cg;

        if (!transform.gameObject.GetComponent<CanvasGroup>())
            cg = transform.gameObject.AddComponent<CanvasGroup>();
        else
            cg = transform.gameObject.GetComponent<CanvasGroup>();

        cg.interactable = false;
        float timeElapsed = 0;
        while (timeElapsed <= animationDuration)
        {
            if (invertDirection)
                cg.alpha = DOVirtual.EasedValue(toFloat, fromFloat, timeElapsed / animationDuration, easing);
            else
                cg.alpha = DOVirtual.EasedValue(fromFloat, toFloat, timeElapsed / animationDuration, easing);

            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cg.interactable = true;
        UnityEngine.Object.Destroy(cg);
        OnComplete();
    }

    private void ScaleAnimation()
    {
        Vector3 scaleTo = Vector3.zero;

        if (relative)
        {
            if (useTransforms)
            {
                scaleTo = toTransform.localScale;
            }
            else
            {
                scaleTo = toVector;
            }
        }
        else
        {
            if (useTransforms)
            {
                transform.localScale = fromTransform.localScale;
                scaleTo = toTransform.localScale;
            }
            else
            {
                transform.localScale = fromVector;
                scaleTo = toVector;
            }
        }

        if (punchAnimation)
        {
            if (invertDirection)
                transform.DOPunchScale(scaleTo, animationDuration).From().SetEase(easing, elasticAmplitude, elasticPeriod).OnComplete(OnComplete);
            else
                transform.DOPunchScale(scaleTo, animationDuration).SetEase(easing, elasticAmplitude, elasticPeriod).OnComplete(OnComplete);
        }
        else
        {
            if (invertDirection)
                transform.DOScale(scaleTo, animationDuration).From().SetEase(easing, elasticAmplitude, elasticPeriod).OnComplete(OnComplete);
            else
                transform.DOScale(scaleTo, animationDuration).SetEase(easing, elasticAmplitude, elasticPeriod).OnComplete(OnComplete);
        }
    }

    private void TranslateAnimtion()
    {
        Vector3 translateTo = Vector3.zero;

        if (relative)
        {
            if (useTransforms)
            {
                translateTo = toTransform.anchoredPosition3D;
            }
            else
            {
                if (percentSizeVector)
                    translateTo = transform.anchoredPosition3D + new Vector3(transform.rect.size.x * toVector.x,
                                                                             transform.rect.size.y * toVector.y);
                else
                    translateTo = toVector;
            }
        }
        else
        {
            if (useTransforms)
            {
                transform.anchoredPosition3D = fromTransform.anchoredPosition3D;
                translateTo = toTransform.anchoredPosition3D;
            }
            else
            {
                transform.anchoredPosition3D = transform.anchoredPosition3D + new Vector3(transform.rect.size.x * fromVector.x,
                                                                                          transform.rect.size.y * fromVector.y);
                
                translateTo = transform.anchoredPosition3D + new Vector3(transform.rect.size.x * toVector.x,
                                                                         transform.rect.size.y * toVector.y);
            }
        }

        if (punchAnimation)
        {
            if (invertDirection)
                transform.DOPunchAnchorPos(translateTo, animationDuration).From().SetEase(easing, elasticAmplitude, elasticPeriod).OnComplete(OnComplete);
            else
                transform.DOPunchAnchorPos(translateTo, animationDuration).SetEase(easing, elasticAmplitude, elasticPeriod).OnComplete(OnComplete);
        }
        else
        {
            if (invertDirection)
                transform.DOAnchorPos(translateTo, animationDuration).From().SetEase(easing, elasticAmplitude, elasticPeriod).OnComplete(OnComplete);
            else
                transform.DOAnchorPos(translateTo, animationDuration).SetEase(easing, elasticAmplitude, elasticPeriod).OnComplete(OnComplete);
        }
    }

    private void ResizeAnimation()
    {
        Vector3 resizeTo = Vector3.zero;

        if (relative)
        {
            if (useTransforms)
            {
                resizeTo = toTransform.sizeDelta;
            }
            else
            {
                resizeTo = toVector;
            }
        }
        else
        {
            if (useTransforms)
            {
                transform.sizeDelta = fromTransform.sizeDelta;
                resizeTo = toTransform.sizeDelta;
            }
            else
            {
                transform.sizeDelta = fromVector;
                resizeTo = toVector;
            }
        }

        if (invertDirection)
            transform.DOSizeDelta(resizeTo, animationDuration).From().SetEase(easing, elasticAmplitude, elasticPeriod).OnComplete(OnComplete);
        else
            transform.DOSizeDelta(resizeTo, animationDuration).SetEase(easing, elasticAmplitude, elasticPeriod).OnComplete(OnComplete);
    }
}

public enum TweenType
{
    scale, translate, resize, fade
}