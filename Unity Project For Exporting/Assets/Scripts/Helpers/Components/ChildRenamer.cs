using System;
using UnityEngine;

namespace Helpers.Components
{
    public class ChildRenamer :MonoBehaviour
    {
        [SerializeField]
        [Header("E.g. Name{0}")]
        private string _nameFormat;

        [SerializeField]
        private int _startAt = 1;

        public void Rename()
        {
            int count = this._startAt;
            foreach (Transform t in this.transform)
                t.name = String.Format(this._nameFormat, count++);
        }
    }
}