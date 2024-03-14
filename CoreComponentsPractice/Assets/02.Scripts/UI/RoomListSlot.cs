using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace DiceGame.UI
{
    public class RoomListSlot : MonoBehaviour
    {
        public int roomIndex;
        private TMP_Text _roomName;
        private TMP_Text _playerRatio;
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


        public void Refresh(string roomName, int currentPlayersInRoom, int maxPlayerInRoom)
        {
            _roomName.text = roomName;
            _playerRatio.text = $"{currentPlayersInRoom} / {maxPlayerInRoom}";
        }

        private void Awake()
        {
            _roomName = transform.Find("Text (TMP) - RoomName").GetComponent<TMP_Text>();
            _playerRatio = transform.Find("Text (TMP) - PlayerRatio").GetComponent<TMP_Text>();
            _select = GetComponent<Button>();
        }
    }
}