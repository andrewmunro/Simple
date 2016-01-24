using Assets.Scripts.Framework.Utils.Extensions;
using Assets.Scripts.Simple.Entity;
using Assets.Scripts.Simple.Vendor;
using UnityEngine;

namespace Assets.Scripts.Simple.Triggers
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
                    playerEntity.SetEnterableVehicle(Vehicle);
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
                    playerEntity.SetEnterableVehicle(null);
                }
            }
        }
    }
}
