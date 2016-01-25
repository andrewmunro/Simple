using Assets.Scripts.Framework.Utils.Extensions;
using Assets.Scripts.Simple.Entity;
using Assets.Scripts.Simple.Vendor.Locations;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Simple
{
    [RequireComponent(typeof(WorldLocations))]
    class ServerManager : NetworkManager
    {
        public WorldLocations SpawnPoints { get; private set; }

        private GameObject CarPrefab { get { return spawnPrefabs.Find(p => p.name == "Car"); }  }


        public override void OnStartServer()
        {
#if UNITY_WEBGL
            useWebSockets = true;
#endif
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
            if (entity.InVehicle != null)
            {
                entity.CmdLeaveVehicle(entity.netId, entity.InVehicle.netId);
            }
            StopClient();
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            var spawnPoint = SpawnPoints.Locations.GetRandom();
            var playerObject = Instantiate(playerPrefab, spawnPoint.Position, Quaternion.Euler(spawnPoint.Rotation));
            NetworkServer.AddPlayerForConnection(conn, (GameObject)playerObject, playerControllerId);
        }

        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            NetworkServer.Destroy(player.gameObject);
        }
    }
}
