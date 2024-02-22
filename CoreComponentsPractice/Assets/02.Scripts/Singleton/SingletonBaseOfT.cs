using System;
using System.Reflection;

namespace DiceGame.Singleton
{
    public class SingletonBase<T>
        where T : SingletonBase<T>
    {
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    //ConstructorInfo constructorInfo = typeof(T).GetConstructor(new Type[] { });
                    //_instance = (T)constructorInfo.Invoke(new object[] { });

                    _instance = (T)Activator.CreateInstance(typeof(T));
                }

                return _instance;
            }
        }

        private static T _instance;
    }
}