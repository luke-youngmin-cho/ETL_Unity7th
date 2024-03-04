using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DiceGame.UI
{
    public class InventorySlot : MonoBehaviour
    {
        public int slotID { get; set; }
        [SerializeField] Image _icon;
        [SerializeField] TMP_Text _num;


        public void Refresh(int itemID, int itemNum)
        {
            if (itemID > 0 && itemNum > 0)
            {
                _icon.sprite = ItemInfoResources.instance[itemID].icon;
                _num.text = itemNum > 1 ? itemNum.ToString() : string.Empty;
            }
            else
            {
                _icon.sprite = null;
                _num.text = string.Empty;
            }
        }
    }
}