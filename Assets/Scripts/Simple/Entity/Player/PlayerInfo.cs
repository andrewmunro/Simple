using System.Collections.Generic;
using Assets.Scripts.Simple.Entity.Vehicle;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Simple.Entity.Player
{
    public class PlayerInfo : NetworkBehaviour
    {
        [SyncVar]
        public int CurrentHealth = 100;

        public VehicleEntity InVehicle;

        public List<VehicleEntity> EnterableVehicles = new List<VehicleEntity>();
    }
}
