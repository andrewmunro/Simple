using Assets.Scripts.Framework.Utils.Extensions;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Vehicles.Car;

namespace Assets.Scripts.Simple.Entity
{
    [RequireComponent(typeof(CarUserControl))]
    [RequireComponent(typeof(CarController))]
    public class VehicleEntity : AbstractNetworkedEntity
    {
        public CarController Controller { get; private set; }
        public CarUserControl CarUserControl { get; private set; }
        public Camera Camera { get; private set; }

        public NetworkConnection Owner { get { return GetComponent<NetworkIdentity>().clientAuthorityOwner; } }
        public bool Occupied { get { return Owner != null; } }

        public override void OnStartClient()
        {
            base.OnStartClient();

            Controller = GetComponent<CarController>();
            CarUserControl = GetComponent<CarUserControl>();
            Camera = gameObject.GetComponentInChildren<Camera>();
            SetVehicleState(false);
        }

        public void SetVehicleState(bool isOn, bool canControl = false)
        {
            if(Controller == null) return;
            Controller.enabled = isOn && canControl;
            CarUserControl.enabled = isOn && canControl;
            Camera.gameObject.SetActive(isOn);
        }
    }
}