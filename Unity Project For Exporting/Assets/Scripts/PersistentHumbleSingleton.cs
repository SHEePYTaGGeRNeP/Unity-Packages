namespace Assets
{
    using UnityEngine;

    /// <summary>
    /// Persistent humble singleton, basically a classic singleton but will destroy any other older components of the same type it finds on awake
    /// The GameObject will be destroyed on load
    /// </summary>
    public class PersistentHumbleSingleton<T> : MonoBehaviour where T : Component
    {
        protected static T instance;
        public float InitializationTime { get; private set; }

        /// <summary>
        /// Singleton design pattern
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = FindObjectOfType<T>();
                if (instance != null) return instance;
                GameObject obj = new GameObject
                {
                    name = typeof(T).Name,
                    //hideFlags = HideFlags.DontSave
                };
                instance = obj.AddComponent<T>();
                return instance;
            }
        }

        /// <summary>
        /// On awake, we check if there's already a copy of the object in the scene. If there's one, we destroy it.
        /// </summary>
        protected virtual void Awake()
        {
            this.InitializationTime = Time.time;

            // DontDestroyOnLoad(this.gameObject);
            // we check for existing objects of the same type
            T[] check = FindObjectsOfType<T>();
            foreach (T searched in check)
            {
                if (searched == this) continue;
                float searchedIniTime = searched.GetComponent<PersistentHumbleSingleton<T>>().InitializationTime;
                // if we find another object of the same type (not this), and if it's older than our current object, we destroy it.
                if (searchedIniTime < this.InitializationTime)
                {
                    Destroy(searched.gameObject);
                }
            }

            if (instance == null)
            {
                instance = this as T;
            }
        }
    }
}
