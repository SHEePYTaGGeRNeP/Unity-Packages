using UnityEngine;

namespace Assets.Scripts
{
    public abstract class CustomEffect : CustomBase
    {
        protected enum DestroyComponent { DestroyGameObject, DestroyComponent, No };
        [SerializeField]
        protected DestroyComponent DestroyComponentAfterFinished;

        [SerializeField]
        protected bool PlayOnAwake;

        public abstract float GetLength();
        public abstract void Play();
    }
}
