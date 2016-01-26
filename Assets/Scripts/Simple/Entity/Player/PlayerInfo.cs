using System.Collections.Generic;
using Assets.Scripts.Simple.Entity.Vehicle;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Simple.Entity.Player
{
    public class PlayerInfo : NetworkBehaviour
    {
        [SerializeField]
        public int MaxHealth = 100;

        [SyncVar]
        public int CurrentHealth;

        public VehicleEntity InVehicle { get; set; }

        public List<VehicleEntity> EnterableVehicles { get; private set; }

        private void Awake()
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            CurrentHealth = MaxHealth;
            EnterableVehicles = new List<VehicleEntity>();
        }
    }
}
