using UnityEngine;

namespace DiceGame.Game.Effects
{
    public class DamagePopUpFactory
    {
        public DamagePopUpFactory()
        {
            _normal = Resources.Load<DamagePopUpNormal>(nameof(DamagePopUpNormal));
            _critical = Resources.Load<DamagePopUpCritical>(nameof(DamagePopUpCritical));
        }


        private DamagePopUpNormal _normal;
        private DamagePopUpCritical _critical;


        public DamagePopUp Create(Vector3 position, Quaternion rotation, float damage, DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Normal:
                    {
                        var popUp = GameObject.Instantiate(_normal, position, rotation);
                        popUp.Show(damage);
                        return popUp;
                    }
                case DamageType.Critical:
                    {
                        var popUp = GameObject.Instantiate(_critical, position, rotation);
                        popUp.Show(damage);
                        return popUp;
                    }
                default:
                    throw new System.Exception($"{damageType} 이라는 데미지 타입 없는데요...");
            }
        }
    }
}