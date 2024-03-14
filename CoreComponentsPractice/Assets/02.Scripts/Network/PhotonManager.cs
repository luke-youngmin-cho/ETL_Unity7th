using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace DiceGame.Network
{
    public class PhotonManager : MonoBehaviourPunCallbacks
    {
        #region Singleton
        public static PhotonManager instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new GameObject().AddComponent<PhotonManager>();
                }

                return s_instance;
            }
        }
        private static PhotonManager s_instance;
        #endregion
        public event Action onConnectedToMaster;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (PhotonNetwork.IsConnected == false)
            {
                if (PhotonNetwork.ConnectUsingSettings())
                    Debug.Log("[PhotonManager] : Connected to photon server.");
                else
                    throw new Exception("[PhotonManager] :Failed to connect to photon server.");
            }
        }

        private void OnApplicationQuit()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            onConnectedToMaster?.Invoke();
            Debug.Log("[PhotonManager] : Conntected to master");
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            
            Debug.Log($"[PhotonManager] : Created room.");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.Log($"[PhotonManager] : Failed to create room, returnCode : {returnCode}, {message}");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            SceneManager.LoadScene("GameReady");
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
            {
                { "isReady", false }
            });
        }
    }
}