using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace DiceGame.UI
{
    public class UILobby : UIBase, IUIScreen, ILobbyCallbacks
    {
        public int selectedRoomListSlotIndex
        {
            get => _selectedRoomListSlotIndex;
            set
            {
                _selectedRoomListSlotIndex = value;
                _join.interactable = value >= 0;
            }
        }

        private Button _join;
        private Button _create;
        [SerializeField] RectTransform _roomListContent;
        [SerializeField] RoomListSlot _roomListslotPrefab;
        private List<RoomListSlot> _roomListSlots = new List<RoomListSlot>();
        private int _selectedRoomListSlotIndex;
        private List<RoomInfo> _localRoomInfos;


        protected override void Awake()
        {
            base.Awake();

            _join = transform.Find("Button - Join").GetComponent<Button>();
            _create = transform.Find("Button - Create").GetComponent<Button>();

            _join.onClick.AddListener(() =>
            {
                if (PhotonNetwork.JoinRoom(_localRoomInfos[_selectedRoomListSlotIndex].Name))
                {
                    
                }
                else
                {
                    UIManager.instance.Get<UIWarningWindow>()
                                      .Show("The room is invalid.");
                }
            });
            _create.onClick.AddListener(() => UIManager.instance.Get<UICreatingRoom>().Show());
        }


        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this); // PhotonNetwork interface 상속받았으면, 콜백 호출 대상으로 등록해야 구현한 콜백함수들이 호출됨.
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void Start()
        {
            StartCoroutine(C_JoinLobbyAtTheVeryFirstTime());   
        }

        IEnumerator C_JoinLobbyAtTheVeryFirstTime()
        {
            yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer);
            PhotonNetwork.JoinLobby();
        }

        public void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");
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
            _localRoomInfos = roomList;

            Debug.Log("Room list updated...");
            for (int i = _roomListSlots.Count - 1; i >= 0; i--)
                Destroy(_roomListSlots[i].gameObject);

            _roomListSlots.Clear();

            for (int i = 0; i < roomList.Count; i++)
            {
                RoomListSlot tmpSlot = Instantiate(_roomListslotPrefab, _roomListContent);
                tmpSlot.roomIndex = i;
                tmpSlot.Refresh(roomList[i].Name, roomList[i].PlayerCount, roomList[i].MaxPlayers);
                tmpSlot.onSelect += () => selectedRoomListSlotIndex = tmpSlot.roomIndex;
                _roomListSlots.Add(tmpSlot);
            }
        }
    }
}