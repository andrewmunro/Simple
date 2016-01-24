using Assets.Scripts.Framework.Utils.Extensions;
using Assets.Scripts.Simple.Vendor;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.Simple.Entity
{
    [RequireComponent(typeof (FirstPersonController))]
    [RequireComponent(typeof (Camera))]
    [RequireComponent(typeof (AudioListener))]
    public abstract class AbstractPlayerEntity : AbstractNetworkedEntity
    {
        public FirstPersonController Controller { get; private set; }
        public CharacterController CharacterController { get; private set; }
        public Camera Camera { get; private set; }
        public AudioListener AudioListener { get; private set; }
        public TextLabel TextLabel { get; private set; }
        public GameObject Model { get; private set; }

        public override void OnStartClient()
        {
            base.OnStartClient();

            Controller = GetComponent<FirstPersonController>();
            CharacterController = GetComponent<CharacterController>();
            Camera = gameObject.GetComponentInChildren<Camera>();
            AudioListener = gameObject.GetComponentInChildren<AudioListener>();
            TextLabel = GetComponent<TextLabel>();
            Model = gameObject.FindGameObjectsWithTag(Tags.PlayerModel)[0];
            SetPlayerState(false);

            TextLabel.Text = "NetworkId: " + netId.Value;
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            TextLabel.enabled = false;
            SetPlayerState(true);
        }

        public override void OnStopAuthority()
        {
            base.OnStopAuthority();

            SetPlayerState(false);
        }

        protected void SetPlayerState(bool enabled)
        {
            Controller.enabled = enabled;
            Camera.enabled = enabled;
            AudioListener.enabled = enabled;
        }
    }
}