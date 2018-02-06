using System;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class CustomSoundEffect : CustomEffect
    {
        [SerializeField]
        protected ulong _delay = 0;

        protected AudioSource _audioSource;

        protected virtual void Awake()
        {
            this._audioSource = this.GetComponent<AudioSource>();
            if (this.playOnAwake)
                this.Play();
        }

        public override float GetLength() { return this._audioSource.clip.length; }
        public override void Play()
        {
            this._audioSource.Play(this._delay);
            switch (this.destroyComponentAfterFinished)
            {
                case DestroyComponent.DestroyComponent:
                    Destroy(this, this._audioSource.clip.length + this._delay);
                    break;
                case DestroyComponent.DestroyGameObject:
                    Destroy(this.gameObject, this._audioSource.clip.length + this._delay);
                    break;
                case DestroyComponent.No:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
