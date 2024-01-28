using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ShowOnSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] float ShowHideTime = .1f;
    [SerializeField] ShowMode Mode;
    [SerializeField] float ShowDelay = 0f;
    [SerializeField] float HideDelay = 0f;

    [ShowIf("@Mode", ShowMode.FADE)] [SerializeField] float FadeShow = 1f;
    [ShowIf("@Mode", ShowMode.FADE)] [SerializeField] float FadeHide = 0f;

    [ShowIf("@Mode", ShowMode.COLOR)] [SerializeField] Color ColorShow = Color.white;
    [ShowIf("@Mode", ShowMode.COLOR)] [SerializeField] Color ColorHide = Color.clear;

    [ShowIf("@Mode", ShowMode.POSITION)] [SerializeField] Vector2 PositionShow = Vector2.one;
    [ShowIf("@Mode", ShowMode.POSITION)] [SerializeField] Vector2 PositionHide = Vector2.zero;

    [ShowIf("@Mode", ShowMode.ROTATION)] [SerializeField] float RotationShow = 0f;
    [ShowIf("@Mode", ShowMode.ROTATION)] [SerializeField] float RotationHide = 180f;

    [ShowIf("@Mode", ShowMode.SCALE)] [SerializeField] Vector3 ScaleShow = Vector3.one;
    [ShowIf("@Mode", ShowMode.SCALE)] [SerializeField] Vector3 ScaleHide = Vector3.one * .5f;


    [HideIf("@Mode", ShowMode.SCALE)] [SerializeField] List<Image> Targets;
    [HideIf("@Mode", ShowMode.SCALE)] [SerializeField] List<TMPro.TMP_Text> Texts;
    [ShowIf("@Mode", ShowMode.FADE)] [SerializeField] List<CanvasGroup> Canvases;
    [ShowIf("@Mode", ShowMode.SCALE)] [SerializeField] List<RectTransform> ScaleTargets;

    List<Tween> previousTweens = new List<Tween>();

    internal bool keepSelected = false;

    public void OnDeselect(BaseEventData eventData)
    {
        ShowHide(false || keepSelected);
        keepSelected = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowHide(true);
        keepSelected = false;
    }

    private void ShowHide(bool show)
    {
        float delay = show ? ShowDelay : HideDelay;

        foreach(var tween in previousTweens)
        {
            tween?.Kill();
        }
        previousTweens.Clear();

        switch (Mode)
        {
            case ShowMode.FADE:
                ShowHideFade(show ? FadeShow : FadeHide, delay);
                break;
            case ShowMode.COLOR:
                ShowHideColor(show ? ColorShow : ColorHide, delay);
                break;
            case ShowMode.POSITION:
                ShowHidePosition(show ? PositionShow : PositionHide, delay);
                break;
            case ShowMode.ROTATION:
                ShowHideRotation(show ? RotationShow : RotationHide, delay);
                break;
            case ShowMode.SCALE:
                ShowHideScale(show ? ScaleShow : ScaleHide, delay);
                break;
        }
    }

    private void ShowHideFade(float alpha, float delay)
    {
        foreach (var img in Targets)
            previousTweens.Add(img.DOFade(alpha, ShowHideTime).SetDelay(delay).SetUpdate(true));

        foreach (var txt in Texts)
            previousTweens.Add(txt.DOFade(alpha, ShowHideTime).SetDelay(delay).SetUpdate(true));

        foreach (var cnv in Canvases)
            previousTweens.Add(cnv.DOFade(alpha, ShowHideTime).SetDelay(delay).SetUpdate(true));
    }

    private void ShowHideColor(Color color, float delay)
    {
        foreach (var img in Targets)
            previousTweens.Add(img.DOColor(color, ShowHideTime).SetDelay(delay).SetUpdate(true));

        foreach (var txt in Texts)
            previousTweens.Add(txt.DOColor(color, ShowHideTime).SetDelay(delay).SetUpdate(true));
    }

    private void ShowHidePosition(Vector2 anchoredPosition, float delay)
    {
        foreach (var img in Targets)
            previousTweens.Add(img.rectTransform.DOAnchorPos(anchoredPosition, ShowHideTime).SetDelay(delay).SetUpdate(true));

        foreach (var txt in Texts)
            previousTweens.Add(txt.rectTransform.DOAnchorPos(anchoredPosition, ShowHideTime).SetDelay(delay).SetUpdate(true));
    }

    private void ShowHideRotation(float angle, float delay)
    {
        foreach (var img in Targets)
        {
            previousTweens.Add(img.rectTransform.DOLocalRotate(new Vector3(0, 0, angle), ShowHideTime, RotateMode.FastBeyond360).SetDelay(delay).SetUpdate(true));
        }

        foreach (var txt in Texts)
            previousTweens.Add(txt.rectTransform.DOLocalRotate(new Vector3(0, 0, angle), ShowHideTime, RotateMode.FastBeyond360).SetDelay(delay).SetUpdate(true));
    }

    private void ShowHideScale(Vector3 scale, float delay)
    {
        foreach (var rect in ScaleTargets)
        {
            previousTweens.Add(rect.DOScale(scale, ShowHideTime).SetDelay(delay).SetUpdate(true));
        }
    }

    [Button]
    internal void Preview(bool show)
    {
        foreach (var elm in Targets)
        {
            switch (Mode)
            {
                case ShowMode.FADE:
                    Color color = elm.color;
                    color.a = (show ? FadeShow : FadeHide);
                    elm.color = color;
                    break;
                case ShowMode.COLOR:
                    elm.color = (show ? ColorShow : ColorHide);
                    break;
                case ShowMode.POSITION:
                    elm.rectTransform.anchoredPosition = (show ? PositionShow : PositionHide);
                    break;
                case ShowMode.ROTATION:
                    elm.rectTransform.localEulerAngles = new Vector3(0,0,(show ? RotationShow : RotationHide));
                    break;
            }
        }

        foreach (var elm in Texts)
        {
            switch (Mode)
            {
                case ShowMode.FADE:
                    Color color = elm.color;
                    color.a = (show ? FadeShow : FadeHide);
                    elm.color = color;
                    break;
                case ShowMode.COLOR:
                    elm.color = (show ? ColorShow : ColorHide);
                    break;
                case ShowMode.POSITION:
                    elm.rectTransform.anchoredPosition = (show ? PositionShow : PositionHide);
                    break;
                case ShowMode.ROTATION:
                    elm.rectTransform.localEulerAngles = new Vector3(0, 0, (show ? RotationShow : RotationHide));
                    break;
            }
        }
    }

    private void OnEnable()
    {
        if(EventSystem.current.currentSelectedGameObject == gameObject)
        {
            OnSelect(null);
        }
    }

    private void OnDisable()
    {
        OnDeselect(null);
    }

    public enum ShowMode
    {
        FADE,
        COLOR,
        POSITION,
        ROTATION,
        SCALE
    }
}
