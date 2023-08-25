using UnityEngine;


namespace Knivt.Tools
{
    public class PersistentSington<T> : MonoBehaviour where T : PersistentSington<T>
    {
        //这个变量可能会被意向不到地改变，这样，编译器就不会去假设这个变量的值了，
        //优化器在用这个变量是必须每次都小心地重新读取这个变量的值,而不是使用保存在寄存器里的备份
        private volatile static T _instance;

        private static object locker = new object();
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locker)
                    {
                        if (_instance == null)
                        {
                            GameObject psObject = new GameObject(typeof(T).Name + "_PersitentSington");
                            _instance = psObject.AddComponent<T>();
                            DontDestroyOnLoad(_instance.gameObject);
                        }
                    }
                }
                return _instance;
            }
        }
        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Debug.LogError("错误,场景内已经有了该持久单例！！！");
                DontDestroyOnLoad(_instance);
            }
        }
    }
}

