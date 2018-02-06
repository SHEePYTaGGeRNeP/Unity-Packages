using Assets.Scripts.Helpers.Classes;
using UnityEngine;

namespace Helpers.Components
{
    public class MultiplayerDisabler : MonoBehaviour //: Photon.PunBehaviour
    {
        private enum Options
        {
            IsMine,
            IsMaster,
            Never
        }

        private enum DisableOn
        {
            Awake,
            Start,
            Manual
        }

        [SerializeField]
        private Options _ignoreDisable;

        [SerializeField]
        private DisableOn _disableOn;

        [SerializeField]
        private MonoBehaviour[] _componentsToDisable;

        [SerializeField]
        private GameObject[] _gameObjectsToDisable;
        [SerializeField]
        private Component[] _componentsToDestroy;

        [SerializeField]
        private GameObject[] _gameObjectsToDestroy;

        private void Awake()
        {
            if (this._disableOn == DisableOn.Awake)
                this.CheckDisable();
        }

        private void Start()
        {
            if (this._disableOn == DisableOn.Start)
                this.CheckDisable();
        }

        public void CheckDisable()
        {
            if (this._ignoreDisable == Options.Never) return;
//            if (PhotonNetwork.isMasterClient && this._ignoreDisable == Options.IsMaster) return;
//            if (this.photonView.isMine && this._ignoreDisable == Options.IsMine) return;
            this.SetThings(false);
        }

        public void CheckEnable()
        {
            if (this._ignoreDisable == Options.Never) return;
//            if (!PhotonNetwork.isMasterClient && this._ignoreDisable == Options.IsMaster) return;
//            if (!this.photonView.isMine && this._ignoreDisable == Options.IsMine) return;
            this.SetThings(true);
        }

        private void SetThings(bool isEnabled)
        {
            foreach (MonoBehaviour c in this._componentsToDisable)
                c.enabled = isEnabled;
            foreach (GameObject go in this._gameObjectsToDisable)
                go.SetActive(isEnabled);
            if (isEnabled) return;
            foreach (Component c in this._componentsToDestroy)
                Destroy(c);
            this._componentsToDestroy = new Component[0];
            foreach (GameObject go in this._gameObjectsToDestroy)
                Destroy(go);            
            this._gameObjectsToDestroy = new GameObject[0];
        }


//        public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
//        {
//            LogHelper.Log(typeof(MultiplayerDisabler), "Master switched! Am I master: " + PhotonNetwork.isMasterClient);
//            this.CheckEnable();
//        }
    }
}