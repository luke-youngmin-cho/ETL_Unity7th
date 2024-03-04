using System;

namespace DiceGame.Data
{
    [Serializable]
    public class ProfileDataModel
    {
        public bool IsValid => (string.IsNullOrEmpty(id) == false) && (string.IsNullOrEmpty(pw) == false);

        public string id;
        public string pw;
        public string nickname;
    }
}