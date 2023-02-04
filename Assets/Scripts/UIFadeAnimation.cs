using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class UIFadeAnimation : MonoBehaviour
{
    [Header("UI Fade Settings")]
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;
    public float fadeTime = 1f;
    public Ease easeIn;
    public Ease easeOut;

    public void PanelFadeIn()
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = new Vector3(rectTransform.transform.localPosition.x, -1000f, 0);

        rectTransform.DOAnchorPos(new Vector2(rectTransform.transform.localPosition.x, 0f), fadeTime, false).SetEase(easeIn);
        canvasGroup.DOFade(1, fadeTime);
    }

    public void PanelFadeOut()
    {
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = new Vector3(rectTransform.transform.localPosition.x, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(rectTransform.transform.localPosition.x, -1000f), fadeTime, false).SetEase(easeOut);
        canvasGroup.DOFade(0, fadeTime);
        StartCoroutine(DeActivePanel(fadeTime));
    }

    IEnumerator DeActivePanel(float delay)
    {

        yield return new WaitForSeconds(delay);
        canvasGroup.gameObject.SetActive(false);
    }
}
