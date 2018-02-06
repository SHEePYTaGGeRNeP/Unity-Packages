using System;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class CustomParticleEffect : CustomEffect
    {
        protected ParticleSystem _particleSystem;

        protected virtual void Awake()
        {
            this._particleSystem = this.GetComponent<ParticleSystem>();
            if (this.playOnAwake)
                this.Play();
        }

        public override float GetLength()
        {
            return this._particleSystem.main.duration;
        }

        public override void Play()
        {
            this._particleSystem.Play();
            switch (this.destroyComponentAfterFinished)
            {
                case DestroyComponent.DestroyComponent:
                    Destroy(this, this._particleSystem.main.duration);
                    break;
                case DestroyComponent.DestroyGameObject:
                    Destroy(this.gameObject, this._particleSystem.main.duration);
                    break;
                case DestroyComponent.No:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
