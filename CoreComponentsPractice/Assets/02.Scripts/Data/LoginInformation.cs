using UnityEngine;
using Firebase.Firestore;
using System;
using Firebase.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;
using DiceGame.Game;
using DiceGame.UI;
using Firebase;

namespace DiceGame.Data
{
    public static class LoginInformation
    {
        public static bool loggedIn => profile == null ? false : profile.IsValid;
        public static ProfileDataModel profile { get; private set; }


        public static async Task<bool> TryLogin(string id, string pw)
        {
            var dependencyState = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyState != DependencyStatus.Available)
            {
                throw new Exception();
            }

            if (GameManager.instance.isTesting)
            {
                id = "tester";
                pw = "0000";
            }

            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

            CollectionReference usersRef = db.Collection("users");

            await usersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                // Snapshot : 읽은 데이터, 읽어서 받아본 데이터는 사실 이미 지난 과거의 데이터기때문에 Snapshot 이라고 함.
                QuerySnapshot snapshot = task.Result;

                foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
                {
                    Dictionary<string, object> documentDictionary = documentSnapshot.ToDictionary();

                    if (documentDictionary.ContainsKey("id"))
                    {
                        if (id.Equals((string)documentDictionary["id"]))
                        {
                            if (pw.Equals((string)documentDictionary["pw"]))
                            {
                                profile = new ProfileDataModel
                                {
                                    id = (string)documentDictionary["id"],
                                    pw = (string)documentDictionary["pw"],
                                    nickname = (string)documentDictionary["nickname"]
                                };
                            }
                            else
                            {
                                UIManager.instance.Get<UIWarningWindow>()
                                                  .Show("Wrong Password.");
                            }

                            return;
                        }
                    }
                }

                UIManager.instance.Get<UIWarningWindow>()
                                  .Show("Wrong ID.");
            });

            if (loggedIn)
            {
                Debug.Log($"[LoginInformation] : Logged in with {profile.id}");
                return true;
            }
                
            Debug.Log($"[LoginInformation] : Failed to Login with {profile.id}");
            return false;
        }
    }
}