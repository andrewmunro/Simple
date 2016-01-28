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

        [SyncVar(hook = "OnCurrentHealthChanged")]
        public int CurrentHealth;

        [SyncVar(hook = "OnNameChanged")]
        public string Name;

        public VehicleEntity InVehicle { get; set; }

        public List<VehicleEntity> EnterableVehicles { get; private set; }

        private void Awake()
        {
            SetDefaultValues();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            OnCurrentHealthChanged(CurrentHealth);
            OnNameChanged(Name);
        }

        private void SetDefaultValues()
        {
            CurrentHealth = MaxHealth;
            EnterableVehicles = new List<VehicleEntity>();
        }

        private void OnCurrentHealthChanged(int value)
        {
            if (isLocalPlayer)
            {
                Debug.Log("I took damage! New HP: " + value);
            }
        }

        private void OnNameChanged(string value)
        {
            transform.name = value;
        }
    }
}
