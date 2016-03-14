using Assets.Scripts;
using UnityEngine;

namespace Assets
{
    public class CustomBase : MonoBehaviour
    {
        //protected GameManager GameManager { get; private set; }
        private static GameObject DynamicObjects { get; set; }


        // Can be filled in later to set references.
        protected virtual void Awake()
        {
            //this.GameManager = GameObject.FindObjectOfType<GameManager>();
        }
        protected virtual void Start()
        { }

        public static void DestroyDynamicObjects()
        {
            if (DynamicObjects != null)
            {
                Destroy(DynamicObjects);
                DynamicObjects = null;
            }
            //else
                //LogHelper.LogWarning(typeof(GnomeBase), "DestroyDynamicObjects", "DynamicObjects does not exist");
        }
        public static void MakeDynamicObjects()
        {
            while (true)
            {
                if (DynamicObjects == null)
                    DynamicObjects = new GameObject("Dynamic Objects") { hideFlags = HideFlags.None };
                else
                {
                    //LogHelper.LogWarning(typeof(GnomeBase), "MakeDynamicObjects", "DynamicObjects already exists, made a new one");
                    DestroyDynamicObjects();
                    continue;
                }
                break;
            }
        }

        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        protected GameObject InstantiateGnomeObject(GameObject objectToInstantiate, Transform parent = null)
        {
            return InstantiateGnomeObjectStatic(objectToInstantiate, Vector3.zero, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        protected GameObject InstantiateGnomeObject(GameObject objectToInstantiate, Vector3 pos, Transform parent = null)
        {
            return InstantiateGnomeObjectStatic(objectToInstantiate, pos, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        protected GameObject InstantiateGnomeObject(GameObject objectToInstantiate, Vector3 pos, Quaternion quaternion, Transform parent = null)
        {
            return InstantiateGnomeObjectStatic(objectToInstantiate, pos, quaternion, parent);
        }

        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        protected T InstantiateGnomeObjectOfType<T>(T objectToInstantiate, Transform parent = null) where T : MonoBehaviour
        {
            return InstantiateGnomeObjectStaticOfType(objectToInstantiate, Vector3.zero, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        protected T InstantiateGnomeObjectOfType<T>(T objectToInstantiate, Vector3 pos, Transform parent = null) where T : MonoBehaviour
        {
            return InstantiateGnomeObjectStaticOfType(objectToInstantiate, pos, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        protected T InstantiateGnomeObjectOfType<T>(T objectToInstantiate, Vector3 pos, Quaternion quaternion, Transform parent = null) where T : MonoBehaviour
        {
            return InstantiateGnomeObjectStaticOfType(objectToInstantiate, pos, quaternion, parent);
        }


        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        public static GameObject InstantiateGnomeObjectStatic(GameObject objectToInstantiate, Transform parent = null)
        {
            return InstantiateGnomeObjectStatic(objectToInstantiate, Vector3.zero, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        public static GameObject InstantiateGnomeObjectStatic(GameObject objectToInstantiate, Vector3 pos, Transform parent = null)
        {
            return InstantiateGnomeObjectStatic(objectToInstantiate, pos, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        public static GameObject InstantiateGnomeObjectStatic(GameObject objectToInstantiate, Vector3 pos, Quaternion quaternion, Transform parent = null)
        {
            GameObject newGameObject = (GameObject)GameObject.Instantiate(objectToInstantiate, pos, quaternion);
            if (parent != null)
                newGameObject.transform.SetParent(parent);
#if UNITY_EDITOR
            else
            {
                if (DynamicObjects == null)
                    MakeDynamicObjects();
                newGameObject.transform.SetParent(DynamicObjects.transform);
            }
#endif
            return newGameObject;
        }

        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        public static T InstantiateGnomeObjectStaticOfType<T>(T objectToInstantiate, Transform parent = null) where T : MonoBehaviour
        {
            return InstantiateGnomeObjectStaticOfType(objectToInstantiate, Vector3.zero, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        public static T InstantiateGnomeObjectStaticOfType<T>(T objectToInstantiate, Vector3 pos, Transform parent = null) where T : MonoBehaviour
        {
            return InstantiateGnomeObjectStaticOfType(objectToInstantiate, pos, Quaternion.identity, parent);
        }
        /// <summary>
        /// Also sets the parent of the GameObject to the DynamicObject if we are in the Unity Editor.
        /// </summary>
        public static T InstantiateGnomeObjectStaticOfType<T>(T objectToInstantiate, Vector3 pos, Quaternion quaternion, Transform parent = null) where T : MonoBehaviour
        {
            T newGameObject = GameObject.Instantiate<T>(objectToInstantiate);
            if (parent != null)
                newGameObject.transform.SetParent(parent);
#if UNITY_EDITOR
            else
            {
                if (DynamicObjects == null)
                    MakeDynamicObjects();
                newGameObject.transform.SetParent(DynamicObjects.transform);
            }
#endif
            newGameObject.transform.rotation = quaternion;
            newGameObject.transform.position = pos;
            return newGameObject;
        }


    }

}