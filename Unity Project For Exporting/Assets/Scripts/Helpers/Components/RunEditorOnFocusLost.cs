#if (UNITY_EDITOR)
using UnityEditor;
#endif
using UnityEngine;

namespace Helpers.Components
{
    public class RunEditorOnFocusLost : MonoBehaviour
    {
        [SerializeField]
        private bool _enabled = true;

#if (UNITY_EDITOR)

        private void OnApplicationFocus(bool focus)
        {
            // Does not work :(
            if (!this._enabled) return;
            if (!Application.isEditor) return;
            EditorApplication.isPaused = false;
            Time.timeScale = 1;
        }
#endif
    }
}