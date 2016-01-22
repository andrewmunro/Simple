using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.Entity
{
    [RequireComponent(typeof (FirstPersonController))]
    public class PlayerEntity : AbstractEntity
    {
        public FirstPersonController Controller { get; private set; }
        public Camera Camera { get; private set; }

        public override void OnStartClient()
        {
            base.OnStartClient();

            Controller = GetComponent<FirstPersonController>();
            Camera = gameObject.GetComponentInChildren<Camera>();
            SetPlayerState(false);
        }

        // Use this for initialization
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            SetPlayerState(true);
        }

        public override void OnStopAuthority()
        {
            base.OnStopAuthority();

            SetPlayerState(false);
        }

        private void SetPlayerState(bool local)
        {
            Controller.enabled = local;
            Camera.enabled = local;
        }
    }
}