using Assets.Scripts.Simple.Entity.Player;
using Assets.Scripts.Simple.Utils.Extensions;
using Assets.Scripts.Simple.Vendor;
using UnityEngine;

namespace Assets.Scripts.Simple.Entity.Vehicle
{
    [RequireComponent(typeof(BoxCollider))]
    public class VehicleEntrance : MonoBehaviour
    {
        public VehicleEntity Vehicle;
        public TextLabel TextLabel;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.HasTag(Tags.Player))
            {
                var playerEntity = other.GetComponent<PlayerEntity>();

                if (playerEntity.isLocalPlayer)
                {
                    TextLabel.enabled = true;
                    playerEntity.Info.EnterableVehicles.Add(Vehicle);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.HasTag(Tags.Player))
            {
                var playerEntity = other.GetComponent<PlayerEntity>();

                if (playerEntity.isLocalPlayer)
                {
                    TextLabel.enabled = false;
                    playerEntity.Info.EnterableVehicles.Remove(Vehicle);
                }
            }
        }
    }
}
