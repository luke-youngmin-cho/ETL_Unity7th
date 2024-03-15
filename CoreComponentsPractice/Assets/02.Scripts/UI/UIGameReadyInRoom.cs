using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace DiceGame.UI
{
    public class UIGameReadyInRoom : UIBase, IUIScreen, IInRoomCallbacks
    {
        [SerializeField] Transform _playerStatusInGameReadyInRoomContent;
        [SerializeField] PlayerStatusInGameReadyInRoomSlot _playerStatusInGameReadyInRoomSlotPrefab;
        private Button _ready;
        private Button _start;
        private Dictionary<string, PlayerStatusInGameReadyInRoomSlot> _playerStatusInGameReadyInRoomSlots
             = new Dictionary<string, PlayerStatusInGameReadyInRoomSlot>();

        protected override void Awake()
        {
            base.Awake();

            _ready = transform.Find("Button - Ready").GetComponent<Button>();
            _start = transform.Find("Button - Start").GetComponent<Button>();

            _ready.onClick.AddListener(() =>
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
                {
                    { "isReady", !(bool)PhotonNetwork.LocalPlayer.CustomProperties["isReady"] }
                });
            });

            _start.onClick.AddListener(() =>
            {
                if (PhotonNetwork.IsMasterClient == false)
                    throw new System.Exception($"[UIgameReadyInRoom] : Tried to start game despite I'm not a master client.");

                // 모든 플레이어 준비됐는지 확인
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    // 방장은 준비 확인 할 필요 없음
                    if (player.IsMasterClient)
                        continue;

                    if (player.CustomProperties.TryGetValue("isReady", out bool isReady))
                    {
                        // 준비 안된 player 찾음
                        if (isReady == false)
                            return;
                    }
                    else
                    {
                        return; // 이 player 의 CustomProperty 가 아직 설정되지 못함..
                    }
                }

                PhotonNetwork.LoadLevel("GamePlay"); // 현재 방의 모든 클라이언트의 씬을 이동
            });
        }

        private void Start()
        {
            StartCoroutine(C_Init());
        }

        IEnumerator C_Init()
        {
            yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.Joined);
            yield return new WaitUntil(() => PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isReady"));

            _start.gameObject.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient == true);
            _ready.gameObject.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient == false);

            Player[] players = PhotonNetwork.PlayerList;

            for (int i = 0; i < players.Length; i++)
            {
                var slot = Instantiate(_playerStatusInGameReadyInRoomSlotPrefab, _playerStatusInGameReadyInRoomContent);
                slot.Refresh((bool)players[i].CustomProperties["isReady"]);
                _playerStatusInGameReadyInRoomSlots.Add(players[i].UserId, slot);
            }
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            _start.gameObject.SetActive(newMasterClient.IsLocal == true);
            _ready.gameObject.SetActive(newMasterClient.IsLocal == false);
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            var slot = Instantiate(_playerStatusInGameReadyInRoomSlotPrefab, _playerStatusInGameReadyInRoomContent);
            _playerStatusInGameReadyInRoomSlots.Add(newPlayer.UserId, slot);
            StartCoroutine(C_RefreshSlot(newPlayer));
        }

        IEnumerator C_RefreshSlot(Player player)
        {
            yield return new WaitUntil(() => player.CustomProperties.ContainsKey("isReady"));
            _playerStatusInGameReadyInRoomSlots[player.UserId].Refresh((bool)player.CustomProperties["isReady"]);
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (_playerStatusInGameReadyInRoomSlots.TryGetValue(otherPlayer.UserId, out PlayerStatusInGameReadyInRoomSlot slot))
            {
                Destroy(slot.gameObject);
                _playerStatusInGameReadyInRoomSlots.Remove(otherPlayer.UserId);
            }
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (_playerStatusInGameReadyInRoomSlots.TryGetValue(targetPlayer.UserId, out PlayerStatusInGameReadyInRoomSlot slot))
            {
                if (changedProps.TryGetValue("isReady", out bool value))
                {
                    slot.Refresh(value);
                }
            }
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }
    }
}