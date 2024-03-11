using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DiceGame.UI
{
    public class Test
    {
        string a = "A";
        string b = "B";
        string c = "C";
        string result;

        public void Sum()
        {
            result = a + b + c;
        }
    }

    public class UIPopUpBase : UIBase, IUIPopUp
    {
        [SerializeField] bool _hideWhenPointerDownOutside; // 바깥을 누르면 이 팝업 닫히게하는 옵션

        public override void InputAction()
        {
            base.InputAction();

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                // 유저가 다른 UI 와 상호작용하려고 시도했다면
                if (UIManager.instance.TryCastOther(this, out IUI other, out GameObject hovered))
                {
                    // 유저가 다른 Popup 을 선택했다면, 해당 Popup을 가장 앞에 보여주게 함.
                    if (other is IUIPopUp)
                        other.Show();
                }
            }
        }

        public override void Show()
        {
            base.Show();
            UIManager.instance.Push(this);
        }

        public override void Hide()
        {
            base.Hide();
            UIManager.instance.Pop(this);
        }

        protected override void Awake()
        {
            base.Awake();

            if (_hideWhenPointerDownOutside)
                CreateOutsidePanel();
        }

        /// <summary>
        /// 바깥 마우스누름 이벤트를 감지하여 현재 팝업을 숨기는 패널 생성
        /// </summary>
        private void CreateOutsidePanel()
        {
            GameObject panel = new GameObject("Outside"); // 빈 게임오브젝트 생성
            panel.transform.SetParent(transform); // 현재 캔버스 하위에 게임오브젝트 종속
            panel.transform.SetAsFirstSibling(); // 맨 앞 자식으로 순서 재설정
            panel.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.4f); // 빈 게임오브젝트에 이미지 추가 후 색 설정.

            RectTransform rectTransform = (RectTransform)panel.transform; // UI 컴포넌트 추가되면 transform 은 RectTransform 으로 바뀜.
            rectTransform.anchorMin = Vector2.zero; // 앵커프리셋 앵커 최솟값 0,0 ( 앵커 프리셋 Stretch width-height )
            rectTransform.anchorMax = Vector2.one; // 앵커프리셋 앵커 최댓값 1,1 ( 앵커 프리셋 Stretch width-height )
            rectTransform.pivot = new Vector2(0.5f, 0.5f); // 앵커프리셋 피봇 0.5, 0.5 ( 앵커 프리셋 Stretch width-height )
            rectTransform.localScale = Vector3.one; // 스케일 1,1,1

            EventTrigger trigger = panel.AddComponent<EventTrigger>(); // 빈 게임오브젝트에 이벤트트리거 추가
            EventTrigger.Entry entry = new EventTrigger.Entry(); // 트리거 진입점 생성
            entry.eventID = EventTriggerType.PointerDown; // 트리거 진입 타입 설정 : 마우스 누름
            entry.callback.AddListener(eventData => Hide()); // 트리거 진입되었을때 팝업 숨김 콜백 등록
            trigger.triggers.Add(entry); // 생성한 트리거 진입점을 추가.
        }
    }
}