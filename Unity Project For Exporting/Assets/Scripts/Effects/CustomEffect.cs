namespace Assets.Scripts.Effects
{
    using UnityEngine;

    public abstract class CustomEffect  : MonoBehaviour
    {
        protected enum DestroyComponent { DestroyGameObject, DestroyComponent, No };
        [SerializeField]
        protected DestroyComponent destroyComponentAfterFinished;

        [SerializeField]
        protected bool playOnAwake;

        public abstract float GetLength();
        public abstract void Play();
    }
}
