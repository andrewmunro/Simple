using Assets.Scripts.Simple.Entity.Vehicle;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Simple.Entity.Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerInfo))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerCommands))]
    public class PlayerEntity : AbstractNetworkedEntity
    {
        public PlayerController Controller { get; private set; }
        public PlayerCommands Commands { get; private set; }
        public PlayerInfo Info { get; private set; }

        public NetworkIdentity Identity { get; private set; }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Controller = GetComponent<PlayerController>();
            Commands = GetComponent<PlayerCommands>();
            Info = GetComponent<PlayerInfo>();

            Identity = GetComponent<NetworkIdentity>();
        }

        public void EnterVehicle(VehicleEntity vehicle)
        {
            Commands.CmdEnterVehicle(netId, vehicle.netId);
        }

        public void LeaveVehicle()
        {
            Commands.CmdLeaveVehicle(netId, Info.InVehicle.netId);
        }

        public void Shoot()
        {
            Commands.CmdShoot(netId);
        }
    }
}
