using UnityEngine;
using Firebase.Firestore;
using System;
using Firebase.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;
using DiceGame.Game;
using DiceGame.UI;
using Firebase;
using Firebase.Auth;

namespace DiceGame.Data
{
    public static class LoginInformation
    {
        public static bool loggedIn => string.IsNullOrEmpty(s_id) == false;
        public static string userKey { get; private set; }
        public static ProfileDataModel profile { get; set; }
        private static string s_id;


        public static async Task<bool> RefreshInformationAsync(string id)
        {
            bool result = false;

            if (GameManager.instance.isTesting)
            {
                id = "tester";
            }

            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            userKey = id.Replace("@", "").Replace(".", "");
            DocumentReference docRef = db.Collection("users").Document(userKey);

            await docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                Dictionary<string, object> documentDictionary = task.Result.ToDictionary();

                if (documentDictionary != null)
                {
                    profile = new ProfileDataModel
                    {
                        nickname = (string)documentDictionary["nickname"],
                    };
                    result = true;
                }
            });

            s_id = id;
            return result;
        }
    }
}