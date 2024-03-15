using DiceGame.UI;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame.Network
{
    [RequireComponent(typeof(PhotonView))]
    public abstract class ClientBehaviour : MonoBehaviour, IPunObservable
    {
        public float horizontal { get; set; }
        public float vertical { get; set; }
        public Vector3 velocity { get; set; }

        protected PhotonView photonView;
        private Vector3 _cachedPosition;
        private Quaternion _cachedRotation;
        private float _interpolation = 0.5f;
        private float _extrapolation = 0.5f;
        private float _predictedLatency = 0.0f;
        private float _cachedServerTimestamp;

        protected virtual void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }


        private void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                velocity = new Vector3(horizontal, 0, vertical);
                transform.position += velocity * Time.fixedDeltaTime;
            }
            else
            {
                // interpolation
                //transform.position = Vector3.Lerp(transform.position, _cachedPosition, _interpolation);
                //transform.rotation = Quaternion.Lerp(transform.rotation, _cachedRotation, _interpolation);

                // extrapolation
                //transform.position = _cachedPosition + (_cachedPosition - transform.position).normalized * _predictedLatency * _extrapolation;
                
                // none
                transform.position = _cachedPosition;
                transform.rotation = _cachedRotation;
            }
        }


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                // 시간지연 = 틱 차이 / (전송갯수 / 시간) = 시간 * 틱차이 / 전송갯수
                _predictedLatency = (info.SentServerTimestamp - _cachedServerTimestamp) / PhotonNetwork.SendRate;
                _cachedServerTimestamp = info.SentServerTimestamp;
                _cachedPosition = (Vector3)stream.ReceiveNext();
                _cachedRotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}