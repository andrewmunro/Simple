using Assets.Scripts.Simple.Entity.Player;
using Assets.Scripts.Simple.Utils.Extensions;
using Assets.Scripts.Simple.Vendor.Locations;
using UnityEngine;
using UnityEngine.Networking;
using PlayerController = UnityEngine.Networking.PlayerController;

namespace Assets.Scripts.Simple
{
    [RequireComponent(typeof(WorldLocations))]
    class ServerManager : NetworkManager
    {
        public WorldLocations SpawnPoints { get; private set; }

        public override void OnStartServer()
        {
            base.OnStartServer();

            SpawnPoints = GetComponent<WorldLocations>();
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            NetworkServer.SetClientReady(conn);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            ClientScene.Ready(conn);
            ClientScene.AddPlayer(0);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            NetworkServer.DestroyPlayersForConnection(conn);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            var player = conn.playerControllers[0].gameObject;
            var entity = player.GetComponent<PlayerEntity>();
            if (entity.Info.InVehicle != null)
            {
                entity.LeaveVehicle();
            }
            StopClient();
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            var spawnPoint = SpawnPoints.Locations.GetRandom();
            var playerObject = (GameObject)Instantiate(playerPrefab, spawnPoint.Position, Quaternion.Euler(spawnPoint.Rotation));
            var playerEntity = playerObject.GetComponent<PlayerEntity>();

            NetworkServer.AddPlayerForConnection(conn, playerObject, playerControllerId);
            GameManager.Instance.AddPlayer(playerEntity.netId.ToString(), playerEntity);
        }

        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            var playerEntity = conn.playerControllers[0].gameObject.GetComponent<PlayerEntity>();

            GameManager.Instance.RemovePlayer(playerEntity.netId.ToString());
            NetworkServer.Destroy(player.gameObject);
        }
    }
}
