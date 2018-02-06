using System;
using System.Runtime.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Helpers.Classes
{
    public class MathematicalCurve
    {
        [System.Serializable]
        public class Curve
        {
            [DataMember]
            //[JsonConverter(typeof(StringEnumConverter))]
            public WaveType waveType;

            [DataMember]
            public float startOffset = 0f, amp = 1, length = 30, becomesOneAt = 0.5f;

            //[JsonIgnore]
            public float MaxValue { get { return this.startOffset + this.amp; } }

            //[JsonIgnore]
            public float MinValue { get { return this.startOffset - this.amp; } }
        }

        public enum WaveType
        {
            Sinus,
            Square,
            Triangle,
            Sawtooth,
            WhiteNoise
        }

        public static float DoWave(Curve curve, float t)
        {
            return DoWave(curve.waveType, t, curve.startOffset, curve.amp, curve.length, curve.becomesOneAt);
        }

        public static float DoWave(WaveType waveType, float t, float startOffset = 0, float amp = 1, float length = 1,
            float becomesOneAt = 0)
        {
            switch (waveType)
            {
                case WaveType.Sinus: return Sin(t, startOffset, amp, length);
                case WaveType.Square: return Square(t, startOffset, amp, length, becomesOneAt);
                case WaveType.Triangle: return Triangle(t, startOffset, amp, length);
                case WaveType.Sawtooth: return Sawtooth(t, startOffset, amp, length);
                case WaveType.WhiteNoise: return WhiteNoise(startOffset, amp);
                default:
                    throw new ArgumentOutOfRangeException("waveType", waveType, null);
            }
        }

        public static float Sin(float t, float startOffset = 0, float amp = 1, float length = 1)
        {
            // position on wave between 0 and 1 
            float pos = Mathf.Repeat(t, length) / length;
            float sin = Mathf.Sin(pos * 2f * Mathf.PI);
            float result = startOffset + sin * amp;
            return result;
        }


        /// <param name="t">time in the wave</param>
        /// <param name="startOffset">start value</param>
        /// <param name="amp">amplitude</param>
        /// <param name="length">length of the wave</param>
        /// <param name="becomesOneAt">Mathf.Sin(pos) >= becomesOneAt ? 1 : 0</param>
        public static float Square(float t, float startOffset = 0, float amp = 1, float length = 1,
            float becomesOneAt = -0)
        {
            // position on wave between 0 and 1
            float sin = Sin(t, startOffset, amp, length);
            float add = sin >= becomesOneAt ? 1 : 0;
            float result = startOffset + add * amp;
            return result;
        }

        public static float Triangle(float t, float startOffset = 0, float amp = 1, float length = 1)
        {
            // position on wave between 0 and 1
            float pos = Mathf.Repeat(t, length) / length;
            float result;
            if (pos < 0.5f)
                result = startOffset + Mathf.Lerp(startOffset - amp, startOffset + amp, pos * 2f);
            else
                result = startOffset + Mathf.Lerp(startOffset + amp, startOffset - amp, (pos - .5f) * 2f);
            return result;
        }

        public static float Sawtooth(float t, float startOffset = 0, float amp = 1, float length = 1)
        {
            // position on wave between 0 and 1
            float pos = Mathf.Repeat(t, length) / length;
            float result = startOffset + (pos - Mathf.Floor(pos)) * amp;
            return result;
        }

        public static float WhiteNoise(float startOffset = 0, float amp = 1)
        {
            float result = Random.Range(startOffset - amp, startOffset + amp);
            return result;
        }
    }
}