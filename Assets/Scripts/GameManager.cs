using System.Collections.Generic;
using Assets.Extensions.Locations;
using Assets.Scripts.Utils.Extensions;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class GameManager : NetworkManager
    {
        public WorldLocations SpawnPoints { get; private set; }

        [SerializeField]
        private GameObject CarPrefab;

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

            //Register all spawnable prefabs
            ClientScene.RegisterPrefab(CarPrefab);

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
            NetworkServer.AddPlayerForConnection(conn, (GameObject) playerObject, playerControllerId);
        }

        // called when a player is removed for a client
        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            NetworkServer.Destroy(player.gameObject);
        }
    }
}
