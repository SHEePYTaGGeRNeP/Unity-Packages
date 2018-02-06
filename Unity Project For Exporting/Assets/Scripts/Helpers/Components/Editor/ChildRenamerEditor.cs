using UnityEditor;
using UnityEngine;

namespace Helpers.Components.Editor
{
    [CustomEditor(typeof(ChildRenamer))]
    public class ChildRenamerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();
        
            ChildRenamer childRenamer = (ChildRenamer) this.target;
            if(GUILayout.Button("Rename childs"))
            {
                childRenamer.Rename();
            }
        }
    }
}