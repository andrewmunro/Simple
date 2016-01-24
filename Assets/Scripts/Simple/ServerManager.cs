using Assets.Scripts.Simple.Utils.Extensions;
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
            StopClient();
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            var spawnPoint = SpawnPoints.Locations.GetRandom();

            var prefab = spawnPoint.Name == "Car" ? CarPrefab : playerPrefab;

            var playerObject = Instantiate(prefab, spawnPoint.Position, Quaternion.Euler(spawnPoint.Rotation));
            NetworkServer.AddPlayerForConnection(conn, (GameObject)playerObject, playerControllerId);
        }

        // called when a player is removed for a client
        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            NetworkServer.Destroy(player.gameObject);
        }
    }
}
