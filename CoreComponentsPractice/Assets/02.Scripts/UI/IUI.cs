using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace DiceGame.UI
{
    /// <summary>
    /// 모든 UI 의 기본 상호작용 인터페이스
    /// </summary>
    public interface IUI
    {
        /// <summary>
        /// Canvas 의 SortOrder 
        /// </summary>
        int sortingOrder { get; set; }

        /// <summary>
        /// InputAction 을 수행할지 여부
        /// </summary>
        bool inputActionEnable { get; set; }

        /// <summary>
        /// 이 UI 와 유저가 상호작용 할 때 실행할 내용
        /// </summary>
        void InputAction();
        
        /// <summary>
        /// 이 UI 를 보여줌
        /// </summary>
        void Show();

        /// <summary>
        /// 이 UI 를 숨김
        /// </summary>
        void Hide();

        /// <summary>
        /// 현재 Canvas에 GraphicsRaycaster 모듈로 RaycastTarget 을 감지함
        /// </summary>
        /// <param name="results"> 감지된 결과를 캐싱해 둘 리스트 </param>
        void Raycast(List<RaycastResult> results);
    }
}