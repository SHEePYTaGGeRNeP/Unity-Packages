using System;
using Helpers.Classes;
using UnityEngine;

namespace Helpers.Components
{
    [ExecuteInEditMode]
    public class MathematicalCurveTester : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve _curve;

        [SerializeField]
        private MathematicalCurve.WaveType _waveType;

        [SerializeField]
        private float _startOffset = 0;

        [SerializeField]
        private float _amp = 1;

        [SerializeField]
        private float _length = 1;

        [SerializeField]
        private float _becomesPositiveat = 0;

        [SerializeField]
        private int _nrOfPoints = 10;

        [SerializeField]
        private int _nrOfWaves = 3;

        [SerializeField]
        private bool _update;

        private void Update()
        {
            if (!this._update) return;
            this.UpdateCurve();
        }

        private void UpdateCurve()
        {
            for (int i = this._curve.length - 1; i > 0; i--)
                this._curve.RemoveKey(i);
            float step = this._length / this._nrOfPoints;
            for (int wave = 0; wave < this._nrOfWaves; wave++)
            {
                float t = 0;
                for (int i = 0; i < this._nrOfPoints; i++)
                {
                    float val = MathematicalCurve.DoWave(this._waveType, t, this._startOffset, this._amp, this._length,
                        this._becomesPositiveat);
                    if (wave == 0 && i == 0 && this._curve.keys.Length == 1)
                        this._curve.keys[0] = new Keyframe(t, val);
                    else
                        this._curve.AddKey(t, val);
                    t += step;
                }
            }
        }
    }
}