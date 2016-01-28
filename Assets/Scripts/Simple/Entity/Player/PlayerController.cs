using Assets.Scripts.Simple.Entity.Vehicle;
using Assets.Scripts.Simple.Utils.Extensions;
using Assets.Scripts.Simple.Vendor;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.Simple.Entity.Player
{
    public class PlayerController : NetworkBehaviour
    {
        public FirstPersonController Controller { get; private set; }
        public CharacterController CharacterController { get; private set; }
        public Camera Camera { get; private set; }
        public AudioListener AudioListener { get; private set; }
        public TextLabel TextLabel { get; private set; }
        public GameObject Model { get; private set; }
        public GunEntity CurrentWeapon { get; set; }

        public override void OnStartClient()
        {
            base.OnStartClient();

            Controller = GetComponent<FirstPersonController>();
            CharacterController = GetComponent<CharacterController>();
            Camera = gameObject.GetComponentInChildren<Camera>();
            AudioListener = gameObject.GetComponentInChildren<AudioListener>();
            Model = gameObject.FindGameObjectsWithTag(Tags.PlayerModel)[0];
            CurrentWeapon = gameObject.FindGameObjectsWithTag(Tags.Weapon)[0].GetComponent<GunEntity>();
            TextLabel = GetComponent<TextLabel>();
            TextLabel.Text = gameObject.transform.name;

            //Assume we dont have authority until told otherwise
            OnStopAuthority();
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            TextLabel.enabled = false;
            Controller.enabled = true;
            Camera.enabled = true;
            AudioListener.enabled = true;
        }

        public override void OnStopAuthority()
        {
            base.OnStopAuthority();
            Controller.enabled = false;
            Camera.enabled = false;
            AudioListener.enabled = false;
        }

        public void OnEnteredVehicle(VehicleEntity vehicle)
        {
            Model.SetActive(false);
            CharacterController.enabled = false;
            transform.SetParent(vehicle.transform);

            if (isLocalPlayer)
            {
                Controller.enabled = false;
                Camera.enabled = false;
                AudioListener.enabled = false;
            }
        }

        public void OnLeftVehicle()
        {
            Model.SetActive(true);
            CharacterController.enabled = true;
            transform.SetParent(null);

            if (isLocalPlayer)
            {
                Controller.enabled = true;
                Camera.enabled = true;
                AudioListener.enabled = true;
            }
        }
    }
}
