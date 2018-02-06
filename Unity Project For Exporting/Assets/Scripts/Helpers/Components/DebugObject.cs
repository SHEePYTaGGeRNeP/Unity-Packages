using System;
using UnityEngine.Assertions;

namespace Assets.Scripts.Helpers.Components
{
    using UnityEngine;

    public class DebugObject : MonoBehaviour
    {
        // ReSharper disable once NotAccessedField.Global
        public static Transform Instance;

        [SerializeField]
        private float _seconds = 11.441f;

        // these fields are only used for debugging.
#pragma warning disable 414
        [SerializeField]
        private string _result;
#pragma warning restore 414
        
        private void Awake()
        {
            Instance = this.transform;
        }


        private void OnDrawGizmos()
        {
            int intTime = (int)_seconds;
            int minutes = intTime / 60;
            int seconds = intTime % 60;
            float fraction = _seconds * 1000;
            fraction = (fraction % 1000);
            this._result = String.Format ("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);            
        }
    }
}
