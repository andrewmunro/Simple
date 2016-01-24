using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

namespace Assets.Scripts.Simple.Entity
{
    [RequireComponent(typeof(CarUserControl))]
    [RequireComponent(typeof(CarController))]
    public class VehicleNetworkedEntity : AbstractNetworkedEntity
    {
        public CarController Controller { get; private set; }
        public CarUserControl CarUserControl { get; private set; }
        public Camera Camera { get; private set; }

        public override void OnStartClient()
        {
            base.OnStartClient();

            Controller = GetComponent<CarController>();
            CarUserControl = GetComponent<CarUserControl>();
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
            CarUserControl.enabled = local;
            Camera.gameObject.SetActive(local);
        }
    }
}