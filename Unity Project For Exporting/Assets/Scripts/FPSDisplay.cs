using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Helpers
{
    [ExecuteInEditMode]
    [System.Serializable]

    public class FPSDisplay : MonoBehaviour
    {
        Text _fpsText;

        int _frames = 0;
        float _time = 0.0f;

        [SerializeField]
        private int _targetFrameRate = 60;

        // Length in seconds of time "buffer" to average.
        [SerializeField]
        private float _updateTime = 1.0f;

        // String format.
        [SerializeField]
        private string _textFormat = "FPS (X/Xs-AVG): 00.00";

        void Awake()
        {
            Application.targetFrameRate = this._targetFrameRate;
        }


        void Start()
        {
            this._fpsText = this.GetComponent<Text>();
            if (FindObjectsOfType<FPSDisplay>().Length > 1)
                Destroy(this.gameObject);
        }


        void Update()
        {
            this._time += Time.deltaTime;

            this._frames++;

            if (this._time < this._updateTime) return;
            this._fpsText.text = (1.0f / (this._time / this._frames)).ToString(this._textFormat);

            this._time = 0.0f;
            this._frames = 0;
        }

    }
}