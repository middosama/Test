using Commons;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace VisualizeFXs
{
    public class DampingFX : MonoBehaviour
    {
        [SerializeField] Vector2 finalValue;
        [SerializeField] float duration = 0.5f;
        RectTransform content;
        Tweener dampingTween;

        private void Awake()
        {
            content = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            if (dampingTween != null)
            {
                dampingTween.Kill();
                dampingTween = null;
            }
            content.anchoredPosition = Vector2.zero;
            dampingTween = content.DOAnchorPos(finalValue, duration).SetEase(Ease.OutBack).SetAutoKill(false);
        }
        
        public void BackToOrigin()
        {
            if (!gameObject.activeSelf) return;
            if (dampingTween != null)
            {
                dampingTween.PlayBackwards();
                this.SetTimeout(()=>gameObject.SetActive(false), duration);
            }
        }

    }
}