using System.Collections.Generic;
using Assets.Scripts.Simple.Entity.Player;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

namespace Assets.Scripts.Simple.Entity.Vehicle
{
    [RequireComponent(typeof(CarUserControl))]
    [RequireComponent(typeof(CarController))]
    [RequireComponent(typeof(CarAudio))]
    [RequireComponent(typeof(Camera))]
    public class VehicleEntity : AbstractNetworkedEntity
    {
        public CarController Controller { get; private set; }
        public CarUserControl Input { get; private set; }
        public CarAudio CarAudio { get; private set; }
        public Camera Camera { get; private set; }

        public PlayerEntity Driver { get; private set; }
        public List<PlayerEntity> Occupants { get; private set; }  

        public override void OnStartClient()
        {
            base.OnStartClient();

            Occupants = new List<PlayerEntity>();
            Controller = GetComponent<CarController>();
            Input = GetComponent<CarUserControl>();
            (CarAudio = GetComponent<CarAudio>()).maxRolloffDistance = 0;
            Camera = gameObject.GetComponentInChildren<Camera>();

            SetVehicleState(false);
        }

        public void SetVehicleState(bool localPlayerInCar, bool localPlayerCanControl = false)
        {
            if(Controller == null) return;

            Controller.enabled = localPlayerInCar && localPlayerCanControl;
            Input.enabled = localPlayerInCar && localPlayerCanControl;
            Camera.gameObject.SetActive(localPlayerInCar);
        }

        public void OnPassengerAdded(PlayerEntity player, bool isDriver)
        {
            Occupants.Add(player);
            if (player.isLocalPlayer) SetVehicleState(true, isDriver);

            if (isDriver)
            {
                Driver = player;
                CarAudio.maxRolloffDistance = 100;
            }
        }

        public void OnPassengerRemoved(PlayerEntity player)
        {
            Occupants.Remove(player);
            if(player.isLocalPlayer) SetVehicleState(false);

            if (player == Driver)
            {
                Driver = null;
                CarAudio.maxRolloffDistance = 0;
            }
        }
    }
}