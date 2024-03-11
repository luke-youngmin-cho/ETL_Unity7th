using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System;

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
    }
}