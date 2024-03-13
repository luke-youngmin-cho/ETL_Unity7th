using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame.UI
{
    public class UILobby : UIBase, IUIScreen, ILobbyCallbacks
    {
        [SerializeField] RectTransform _roomListContent;
        [SerializeField] RoomListSlot _roomListslotPrefab;
        private List<RoomListSlot> _roomListSlots;
        private int _selectedRoomListSlotIndex;

        public void OnJoinedLobby()
        {
            throw new System.NotImplementedException();
        }

        public void OnLeftLobby()
        {
            throw new System.NotImplementedException();
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            throw new System.NotImplementedException();
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            for (int i = _roomListSlots.Count - 1; i >= 0; i--)
                Destroy(_roomListSlots[i].gameObject);

            _roomListSlots.Clear();

            for (int i = 0; i < roomList.Count; i++)
            {
                RoomListSlot tmpSlot = Instantiate(_roomListslotPrefab, _roomListContent);
                tmpSlot.roomIndex = i;
                tmpSlot.onSelect += () => _selectedRoomListSlotIndex = tmpSlot.roomIndex;
            }
        }
    }
}