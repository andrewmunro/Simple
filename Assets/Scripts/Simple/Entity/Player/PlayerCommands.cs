using Assets.Scripts.Simple.Entity.Vehicle;
using UnityEngine.Networking;

namespace Assets.Scripts.Simple.Entity.Player
{
    public class PlayerCommands : NetworkBehaviour
    {
        public PlayerInfo Info { get; private set; }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Info = GetComponent<PlayerInfo>();
        }

        [Command]
        public void CmdShoot(NetworkInstanceId playerId)
        {
            var playerEntity = ClientScene.FindLocalObject(playerId).GetComponent<PlayerEntity>();
            var bulletSpawnPostion = playerEntity.Controller.CurrentWeapon.BulletSpawnPosition;
            var bulletSpawn = bulletSpawnPostion.position + bulletSpawnPostion.transform.up * 0.5f;

            var bullet = Instantiate(GameManager.Instance.BulletPrefab);
            var bulletEntity = bullet.GetComponent<BulletEntity>();

            bullet.transform.position = bulletSpawn;
            bulletEntity.BulletRotation = bulletSpawnPostion.rotation;
            bulletEntity.SpawnedByIdentity = playerId.Value;

            NetworkServer.Spawn(bullet);
        }

        [Command]
        public void CmdEnterVehicle(NetworkInstanceId playerId, NetworkInstanceId vehicleId)
        {
            var playerEntity = ClientScene.FindLocalObject(playerId).GetComponent<PlayerEntity>();
            var vehicle = NetworkServer.FindLocalObject(vehicleId);
            var vehicleIdentity = vehicle.GetComponent<NetworkIdentity>();

            var isOwner = vehicleIdentity.clientAuthorityOwner == null;
            if (isOwner) vehicleIdentity.AssignClientAuthority(playerEntity.connectionToClient);
            RpcOnEnteredVehicle(playerId, vehicleId, isOwner);
        }

        [Command]
        public void CmdLeaveVehicle(NetworkInstanceId playerId, NetworkInstanceId vehicleId)
        {
            var playerEntity = ClientScene.FindLocalObject(playerId).GetComponent<PlayerEntity>();
            var vehicle = NetworkServer.FindLocalObject(vehicleId);
            vehicle.GetComponent<NetworkIdentity>().RemoveClientAuthority(playerEntity.connectionToClient);
            RpcOnLeftVehicle(playerId, vehicleId);
        }

        [ClientRpc]
        private void RpcOnEnteredVehicle(NetworkInstanceId playerId, NetworkInstanceId vehicleId, bool isOwner)
        {
            var playerEntity = ClientScene.FindLocalObject(playerId).GetComponent<PlayerEntity>();
            var vehicleEntity = ClientScene.FindLocalObject(vehicleId).GetComponent<VehicleEntity>();
            playerEntity.Controller.OnEnteredVehicle(vehicleEntity);
            vehicleEntity.OnPassengerAdded(playerEntity, isOwner);
            Info.InVehicle = vehicleEntity;
        }

        [ClientRpc]
        private void RpcOnLeftVehicle(NetworkInstanceId playerId, NetworkInstanceId vehicleId)
        {
            var playerEntity = ClientScene.FindLocalObject(playerId).GetComponent<PlayerEntity>();
            var vehicleEntity = ClientScene.FindLocalObject(vehicleId).GetComponent<VehicleEntity>();
            playerEntity.Controller.OnLeftVehicle();
            vehicleEntity.OnPassengerRemoved(playerEntity);
            Info.InVehicle = null;
        }
    }
}
