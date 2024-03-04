using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public class ItemInfoResources : MonoBehaviour
    {
        #region Singleton
        public static ItemInfoResources instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = Instantiate(Resources.Load<ItemInfoResources>(nameof(ItemInfoResources)));
                }

                return s_instance;
            }
        }
        private static ItemInfoResources s_instance;
        #endregion

        public ItemInfo this[int itemID] => _dictionary[itemID];
        Dictionary<int, ItemInfo> _dictionary;
        [SerializeField] List<ItemInfo> _list;


        private void Awake()
        {
            _dictionary = new Dictionary<int, ItemInfo>();

            foreach (var item in _list)
            {
                _dictionary.TryAdd(item.id, item);
            }
        }
    }
}