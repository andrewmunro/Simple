using Assets.Scripts.Framework.Utils.Extensions;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Simple.Entity
{
    public class PlayerEntity : AbstractPlayerEntity
    {
        private VehicleEntity EnterableVehicle { get; set; }
        public VehicleEntity InVehicle { get; set; }

        public void SetEnterableVehicle(VehicleEntity vehicleEntity)
        {
            EnterableVehicle = vehicleEntity;
        }

        [Command]
        public void CmdEnterVehicle(NetworkInstanceId playerId, NetworkInstanceId vehicleId)
        {
            var player = NetworkServer.FindLocalObject(playerId);
            var playerConnection = player.GetComponent<NetworkIdentity>().connectionToClient;
            var vehicle = NetworkServer.FindLocalObject(vehicleId);
            var vehicleIdentity = vehicle.GetComponent<NetworkIdentity>();

            var isOwner = vehicleIdentity.clientAuthorityOwner == null;
            if (isOwner) vehicleIdentity.AssignClientAuthority(playerConnection);
            RpcOnEnteredVehicle(playerId, vehicleId, isOwner);
        }

        [Command]
        public void CmdLeaveVehicle(NetworkInstanceId playerId, NetworkInstanceId vehicleId)
        {
            var player = NetworkServer.FindLocalObject(playerId);
            var playerConnection = player.GetComponent<NetworkIdentity>().connectionToClient;
            var vehicle = NetworkServer.FindLocalObject(vehicleId);
            vehicle.GetComponent<NetworkIdentity>().RemoveClientAuthority(playerConnection);
            RpcOnLeftVehicle(playerId);
        }

        [ClientRpc]
        private void RpcOnEnteredVehicle(NetworkInstanceId playerId, NetworkInstanceId vehicleId, bool isOwner)
        {
            var player = ClientScene.FindLocalObject(playerId);
            var entity = player.GetComponent<PlayerEntity>();
            entity.Model.SetActive(false);
            entity.CharacterController.enabled = false;
            player.transform.SetParent(ClientScene.FindLocalObject(vehicleId).transform);

            if (entity.isLocalPlayer)
            {
                EnterableVehicle.SetVehicleState(true, isOwner);
                entity.InVehicle = EnterableVehicle;
                entity.EnterableVehicle = null;
                SetPlayerState(false);
            }
        }

        [ClientRpc]
        private void RpcOnLeftVehicle(NetworkInstanceId playerId)
        {
            var player = ClientScene.FindLocalObject(playerId);
            var entity = player.GetComponent<PlayerEntity>();
            entity.Model.SetActive(true);
            entity.CharacterController.enabled = true;
            player.transform.SetParent(null);

            if (entity.isLocalPlayer)
            {
                entity.InVehicle.SetVehicleState(false);
                entity.InVehicle = null;
                entity.EnterableVehicle = null;
                SetPlayerState(true);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(EnterableVehicle != null) CmdEnterVehicle(netId, EnterableVehicle.netId);
                else if(InVehicle != null) CmdLeaveVehicle(netId, InVehicle.netId);
            }
        }
    }
}
