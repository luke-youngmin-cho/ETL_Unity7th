using UnityEngine;
using UnityEngine.EventSystems;

namespace DiceGame.UI
{
    public class DraggablePanel : MonoBehaviour, IDragHandler
    {
        [SerializeField] bool _resetPositionOnEnable = true;
        private RectTransform _rectTransform;
        private Vector2 _origin;


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _origin = _rectTransform.anchoredPosition;

            if (transform.root.TryGetComponent(out IUI ui))
            {
                ui.onShow += () =>
                {
                    if (_resetPositionOnEnable)
                        _rectTransform.anchoredPosition = _origin;
                };
            }
        }

        private void OnEnable()
        {
            if (_resetPositionOnEnable)
                _rectTransform.anchoredPosition = _origin;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta;
        }
    }
}