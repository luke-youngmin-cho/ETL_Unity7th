using TMPro;
using UnityEngine;

namespace DiceGame.UI
{
    public class PlayerStatusInGameReadyInRoomSlot : MonoBehaviour
    {
        private TMP_Text _status;


        public void Refresh(bool isReady)
        {
            _status.enabled = isReady;
        }

        private void Awake()
        {
            _status = transform.Find("Text (TMP) - Status").GetComponent<TMP_Text>();
        }
    }
}