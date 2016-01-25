using Assets.Scripts.Simple.Entity.Vehicle;
using Assets.Scripts.Simple.Utils.Extensions;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Simple.Entity.Player
{
    public class PlayerInput : NetworkBehaviour
    {
        public PlayerEntity PlayerEntity { get; private set; }
        public PlayerController Controller { get; private set; }
        public PlayerInfo Info { get; private set; }
        public NetworkIdentity Identity { get; private set; }

        public override void OnStartClient()
        {
            base.OnStartClient();
            PlayerEntity = GetComponent<PlayerEntity>();
            Controller = GetComponent<PlayerController>();
            Info = GetComponent<PlayerInfo>();
            Identity = GetComponent<NetworkIdentity>();
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Info.InVehicle) PlayerEntity.LeaveVehicle();
                else
                {
                    //Raycast center of screen
                    var ray = Controller.Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        var vehicleEntity = hit.transform.GetComponent<VehicleEntity>();
                        if (vehicleEntity != null && Info.EnterableVehicles.Contains(vehicleEntity))
                        {
                            PlayerEntity.EnterVehicle(vehicleEntity);
                        }
                    }
                }
            }
        }
    }
}
