using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Firebase.Extensions;
using System.Text;
using System.Linq;

namespace DiceGame.Data
{
    public class InGameContext
    {
        public InGameContext()
        {
            Load();
        }


        public bool hasInitialized { get; private set; }
        public List<InventorySlotDataModel> inventorySlotDataModels { get; private set; } // dependency source
        private FirebaseDatabase _realtimeDB = FirebaseDatabase.DefaultInstance;

        public event Action<int, InventorySlotDataModel> onInventorySlotDataChanged;

        public async void Load()
        {
            DatabaseReference inventorySlotsRef =
                _realtimeDB.GetReference($"users/{LoginInformation.userKey}/inventorySlots");

            DataSnapshot snapshot = await inventorySlotsRef.GetValueAsync();

            if (snapshot.Exists == false)
            {
                int defaultSlotTotal = 30;
                string slotDataJson = "{\r\n          \"itemID\": 0,\r\n          \"itemNum\": 0\r\n        },\r\n        ";
                string head = " {\r\n      \"inventorySlots\": [\r\n        ";
                string tail = "]\r\n    }\r\n  ";
                StringBuilder stringBuilder = new StringBuilder(head.Length + tail.Length + slotDataJson.Length * defaultSlotTotal);
                stringBuilder.Append(head);
                stringBuilder.Insert(stringBuilder.Length, slotDataJson, defaultSlotTotal);
                stringBuilder.Append(tail);
                await FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(LoginInformation.userKey).SetRawJsonValueAsync(stringBuilder.ToString());
                snapshot = await inventorySlotsRef.GetValueAsync();
            }

            inventorySlotDataModels = new List<InventorySlotDataModel>();

            foreach (var item in snapshot.Children)
            {
                inventorySlotDataModels.Add(JsonConvert.DeserializeObject<InventorySlotDataModel>(item.GetRawJsonValue()));
            }

            // DB 에서 InventorySlot 각각의 데이터가 변경되었을때 알림을 통지할 이벤트
            inventorySlotsRef.ChildChanged += (sender, childEventArgs) =>
            {
                DataSnapshot childSnapshot = childEventArgs.Snapshot;
                int slotID = int.Parse(childSnapshot.Key); // 바뀐 슬롯 ID
                InventorySlotDataModel dataChanged =
                    JsonConvert.DeserializeObject<InventorySlotDataModel>(childSnapshot.GetRawJsonValue()); // 바뀐 슬롯 데이터
                inventorySlotDataModels[slotID] = dataChanged; // DependencySource 갱신
                onInventorySlotDataChanged?.Invoke(slotID, dataChanged); // 슬롯 데이터 변경 통지
            };

            hasInitialized = true;
        }

        /// <summary>
        /// InventorySlotsData 의 DependencySource 의 특정 슬롯 데이터를 DB에 저장
        /// </summary>
        /// <param name="slotID"> 저장할 DependencySource의 인덱스 </param>
        /// <param name="onCompleted"> 저장 완료후 실행할 행동 </param>
        public void SaveInventorySlotDataModel(int slotID, InventorySlotDataModel newData, Action<InventorySlotDataModel> onCompleted)
        {
            string json = JsonConvert.SerializeObject(newData); // 저장할 객체를 json 으로 직렬화
            _realtimeDB.GetReference($"users/{LoginInformation.userKey}/inventorySlots/{slotID}") // 저장할 위치 참조 
                       .SetRawJsonValueAsync(json) // 해당 위치에 저장
                       .ContinueWithOnMainThread(task => 
                       {
                           if (task.IsCompleted)
                           {
                               inventorySlotDataModels[slotID] = newData;
                               onCompleted?.Invoke(newData);
                           }
                       }); // 저장 끝나고나서 추가 내용 수행
        }

        /// <summary>
        /// InventorySlotsData 의 DependencySource 의 특정 슬롯 데이터를 DB로부터 읽어서 갱신
        /// </summary>
        /// <param name="slotID"> 읽을 DependencySource의 인덱스 </param>
        /// <param name="onCompleted"> 읽기 완료후 실행할 행동 </param>
        public void LoadInventorySlotDataModel(int slotID, Action<InventorySlotDataModel> onCompleted)
        {
            _realtimeDB.GetReference($"users/{LoginInformation.userKey}/inventorySlots/{slotID}") // 읽을 위치 참조 
                       .GetValueAsync() // 해당 위치에서 읽기
                       .ContinueWithOnMainThread(task =>
                       {
                           DataSnapshot snapshot = task.Result;

                           // 해당 위치에서 정상적으로 데이터를 읽었으면
                           if (snapshot.Exists)
                           {
                               InventorySlotDataModel data =
                                    JsonConvert.DeserializeObject<InventorySlotDataModel>(snapshot.GetRawJsonValue()); // 데이터 역직렬화
                               inventorySlotDataModels[slotID] = data; // DependencySource 갱신
                               onCompleted?.Invoke(data); // 완료 액션
                           }
                       }); // 읽기 끝난 후 추가 수행
        }
    }
}