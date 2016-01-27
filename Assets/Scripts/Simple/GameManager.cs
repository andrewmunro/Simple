﻿using System.Collections.Generic;
using Assets.Scripts.Simple.Entity.Player;
using UnityEngine;

namespace Assets.Scripts.Simple
{
    [RequireComponent(typeof(ServerManager))]
    public class GameManager : MonoBehaviour
    {
        private const string PLAYER_ID_PREFIX = "Player ";

        public static GameManager Instance { get; private set; }

        public GameObject BulletPrefab;

        public PlayerEntity LocalPlayer { get; private set; }

        private Dictionary<string, PlayerEntity> Players { get; set; }

        private void Awake()
        {
            Instance = this;

            Players = new Dictionary<string, PlayerEntity>();
        }

        public void AddPlayer(string networkId, PlayerEntity player)
        {
            Players.Add(networkId, player);
            if (player.isLocalPlayer) LocalPlayer = player;
            player.Info.Name = PLAYER_ID_PREFIX + networkId;
        }

        public void RemovePlayer(string networkId)
        {
            Players.Remove(networkId);
        }
    }
}
