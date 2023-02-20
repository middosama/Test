using Commons;
using GameSystems;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Assets.Scripts.VisualizeFXs
{
    
    public class TextBubble : DynamicObjectPooling<TextBubble>
    {
        public RectTransform content;
        [SerializeField] TMP_Text txtContentText;
        public float duration = 1f;
        public float distance = -100f;
        public override Transform PoolContainer => GameMaster.PoolingObjectContainer;

        public override void OnSpawn()
        {
            gameObject.SetActive(true);
            content.anchoredPosition = Vector2.zero;
            content.DOAnchorPosY(distance, duration).OnComplete(Destroy);
        }

        public void SetData(string text)
        {
            txtContentText.text = text;
        }

    }
}