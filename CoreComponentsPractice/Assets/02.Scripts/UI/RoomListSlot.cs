using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DiceGame.UI
{
    public class RoomListSlot : MonoBehaviour
    {
        public int roomIndex;
        private Button _select;
        public event UnityAction onSelect
        {
            add
            {
                _select.onClick.AddListener(value);
            }
            remove
            {
                _select.onClick.RemoveListener(value);
            }
        }


        private void Awake()
        {
            _select = GetComponent<Button>();
        }
    }
}