using UnityEngine;

namespace Logic.Managers
{
    public abstract class BaseManager<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    _instance.transform.SetParent(GetOrAddManagerHolder().transform);
                }

                return _instance;
            }
        }

        private static GameObject GetOrAddManagerHolder()
        {
            var managerHolderName = "ManagerHolder";
            var managerHolder = GameObject.FindGameObjectWithTag(managerHolderName);
            if (!managerHolder)
            {
                managerHolder = new GameObject(managerHolderName)
                {
                    tag = managerHolderName
                };
                managerHolder.AddComponent<DontDestroyOnLoad>();
            }
            return managerHolder;
        }

        public abstract void Initialize();
    }
}
