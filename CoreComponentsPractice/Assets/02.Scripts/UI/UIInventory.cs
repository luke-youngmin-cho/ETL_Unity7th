using DiceGame.Data;
using DiceGame.Game;
using DiceGame.Game.Character;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DiceGame.Game.Interactables;

namespace DiceGame.UI
{
    /// <summary>
    /// InventorySlot 들에 대한 데이터를 보여주는 UI.
    /// </summary>
    public class UIInventory : UIPopUpBase
    {
        [SerializeField] InventorySlot _slotPrefab; // 슬롯 데이터를 보여줄 기본 단위 원본
        [SerializeField] Transform _slotContent; // 슬롯 생성 위치
        private List<InventorySlot> _slots; // 생성된 슬롯들
        [SerializeField] ItemController _itemControllerPrefab; // 슬롯의 아이템을 버릴때 월드에 생성할 컨트롤러
        private IRepository<InventorySlotDataModel> _repository; // 슬롯 데이터를 가지고있는 저장소
        private int _selectedSlotID = NOT_SELECTED;
        private const int NOT_SELECTED = -1;
        [SerializeField] Image _selectedFollowingItemIcon;
        private List<RaycastResult> _results = new List<RaycastResult>();


        protected override void Awake()
        {
            base.Awake();

            _repository = GameManager.instance.unitOfWork.inventoryRepository; // 저장소 주소 참조 받아옴 (의존성 주입)
            _slots = new List<InventorySlot>(_repository.GetAllItems().Count()); // 저장소에있는 데이터 갯수만큼 슬롯이 들어갈 자리 생성
            InventorySlot tmpSlot = null;
            int tmpSlotID = 0;

            // 저장소에 있는 모든 데이터 순회하면서 슬롯 생성 및 갱신.
            foreach (var slotData in _repository.GetAllItems())
            {
                tmpSlot = Instantiate(_slotPrefab, _slotContent);
                tmpSlot.slotID = tmpSlotID++;
                tmpSlot.Refresh(slotData.itemID, slotData.itemNum);
                _slots.Add(tmpSlot);
            }

            // 저장소의 데이터가 변경될 때마다 해당 슬롯을 갱신.
            _repository.onItemUpdated += (slotID, slotData) =>
            {
                _slots[slotID].Refresh(slotData.itemID, slotData.itemNum);
            };
        }

        public override void InputAction()
        {
            base.InputAction();

            if (Input.GetMouseButtonDown(0))
            {
                // 선택된 슬롯이 없다면
                if (_selectedSlotID == NOT_SELECTED)
                {
                    _results.Clear();
                    Raycast(_results);

                    // 뭔가 상호작용 할만한게 이 캔버스에 있다..!
                    if (_results.Count > 0)
                    {
                        foreach (var result in _results)
                        {
                            // 캐스팅된 타겟중에 슬롯이 있다면 해당슬롯을 선택
                            if (result.gameObject.TryGetComponent(out InventorySlot slot))
                            {
                                if (_repository.GetItemByID(slot.slotID).isEmpty == false)
                                {
                                    Select(slot.slotID);
                                    return;
                                }
                            }
                        }
                    }
                }
                // 선택된 슬롯이 있다면
                else
                {
                    _results.Clear();
                    Raycast(_results);

                    // 뭔가 상호작용 할만한게 이 캔버스에 있다..!
                    if (_results.Count > 0)
                    {
                        foreach (var result in _results)
                        {
                            // 캐스팅된 타겟중에 슬롯이 있다면 해당슬롯을 선택
                            if (result.gameObject.TryGetComponent(out InventorySlot slot))
                            {
                                // 다른 슬롯이 선택 됐다면 스왑.
                                if (_selectedSlotID != slot.slotID)
                                {
                                    var selectedSlotData = _repository.GetItemByID(_selectedSlotID);
                                    var castedSlotData = _repository.GetItemByID(slot.slotID);
                                    _repository.UpdateItem(slot.slotID, new InventorySlotDataModel(selectedSlotData));
                                    _repository.UpdateItem(_selectedSlotID, castedSlotData);
                                    Deselect();
                                    return;
                                }
                            }
                        }
                    }
                    // 다른 UI 에 상호작용 할만한 것이 있다..!
                    else if (UIManager.instance.TryCastOther(this, out IUI other, out GameObject hovered))
                    {

                    }
                    // 상호작용할만한 UI 가 감지되지 않았다 (그냥 World 에 누름)
                    else
                    {
                        // 대리자를 전달할 때(CallBack 등) 람다식내에서 지역변수가 아닌 멤버변수를 사용하면,  
                        // 해당 대리자가 호출되기 전까지 멤버변수의 값이 수정되었을때 람다식은 해당 멤버변수위치를 참조하므로 
                        // 의도하지 않은 동작을 할 수 있으므로, 값이 변할 여지가 있는 변수를 참조해야할 경우, 지역변수를 참조하도록 작성한다.
                        int tmpSlotID = _selectedSlotID;
                        UIManager.instance.Get<UIConfirmWindow>()
                                          .Show(message: "Sure to throw away ?",
                                                onConfirm: () =>
                                                {
                                                    InventorySlotDataModel slotData = _repository.GetItemByID(tmpSlotID);
                                                    _repository.UpdateItem(tmpSlotID, new InventorySlotDataModel(0, 0));

                                                    Instantiate(_itemControllerPrefab,
                                                                PlayerController.instance.transform.position,
                                                                Quaternion.identity)
                                                        .SetUp(slotData);
                                                });
                        
                        Deselect();
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (_selectedSlotID != NOT_SELECTED)
                    Deselect();
            }

            // 현재 선택된 슬롯이 있다면, 아이콘이 마우스포인터 따라다니게 함.
            if (_selectedSlotID != NOT_SELECTED)
                _selectedFollowingItemIcon.transform.position = Input.mousePosition;
        }

        private void Select(int slotID)
        {
            _selectedSlotID = slotID;
            _selectedFollowingItemIcon.sprite = _slots[slotID].sprite;
            _selectedFollowingItemIcon.enabled = true;
            _selectedFollowingItemIcon.transform.position = Input.mousePosition;
        }

        private void Deselect()
        {
            _selectedSlotID = NOT_SELECTED;
            _selectedFollowingItemIcon.enabled = false;
        }
    }
}