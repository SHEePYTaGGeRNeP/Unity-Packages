using UnityEngine;

namespace Helpers.Components
{
    public class TimedEnabler : MonoBehaviour
    {
        [SerializeField]
        private float _timeInSeconds;

        [SerializeField]
        private MonoBehaviour[] _components;

        [SerializeField]
        private bool _enable = true;

        private void Start()
        {
            this.Invoke("Enable", this._timeInSeconds);
        }

        private void Enable()
        {
            for (int i = 0; i < this._components.Length; i++)
                this._components[i].enabled = this._enable;
        }
    }
}