using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class CustomSoundEffect : CustomEffect
    {
        [SerializeField]
        private ulong _delay = 0;

        private AudioSource _audioSource;
        private AudioClip _audioClip;

        protected override void Awake()
        {
            base.Awake();
            this._audioSource = this.GetComponent<AudioSource>();
            this._audioClip = this._audioSource.clip;
            if (this.PlayOnAwake)
                this.Play();
        }

        public override float GetLength() { return this._audioClip.length; }
        public override void Play()
        {
            this._audioSource.Play(this._delay);
            switch (this.DestroyComponentAfterFinished)
            {
                case DestroyComponent.DestroyComponent:
                    Destroy(this, this._audioClip.length);
                    break;
                case DestroyComponent.DestroyGameObject:
                    Destroy(this.gameObject, this._audioClip.length);
                    break;
                case DestroyComponent.No:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
