using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace DiceGame.Data
{
    public static class LoginInformation
    {
        public static bool loggedIn => profile == null ? false : profile.IsValid;
        public static ProfileDataModel profile { get; private set; }
        public static bool isTesting = true;

        public static bool TryLogin(string id, string pw)
        {
            if (isTesting)
            {
                profile = new ProfileDataModel
                {
                    id = "tester",
                    pw = "0000",
                    nickname = ""
                };
            }

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