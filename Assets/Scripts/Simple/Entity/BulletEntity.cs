using System.Linq;
using Assets.Scripts.Simple.Entity.Player;
using Assets.Scripts.Simple.Entity.Vehicle;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Simple.Entity
{
    public class BulletEntity : NetworkBehaviour
    {
        public const int BULLET_DAMAGE = 10;
        public const float BULLET_SPEED = 10;

        [SyncVar(hook="OnRotationSet")]
        public Quaternion BulletRotation;

        [SyncVar(hook = "OnSpawnedBySet")]
        public uint SpawnedByIdentity;

        public PlayerEntity SpawnedBy { get; set; }

        public override void OnStartClient()
        {
            base.OnStartClient();

            OnSpawnedBySet(SpawnedByIdentity);
            OnRotationSet(BulletRotation);
        }

        private void OnSpawnedBySet(uint identity)
        {
            SpawnedBy = ClientScene.FindLocalObject(new NetworkInstanceId(identity)).GetComponent<PlayerEntity>();

            if (SpawnedBy.Info.InVehicle != null)
            {
                SpawnedBy.Info.InVehicle.Colliders.ForEach(c => Physics.IgnoreCollision(c, GetComponent<Collider>()));
            }
        }

        private void OnRotationSet(Quaternion rotation)
        {
            transform.rotation = rotation;
            GetComponent<Rigidbody>().velocity = transform.up * BULLET_SPEED;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!isServer || isCollidingWithLocalPlayer(other)) return;

            var playerEntity = other.transform.root.GetComponent<PlayerEntity>();

            if (playerEntity != null)
            {
                playerEntity.Info.CurrentHealth -= BULLET_DAMAGE;
                Debug.Log("Bullet hit " + playerEntity.name);
            }

            NetworkServer.Destroy(gameObject);
        }

        private bool isCollidingWithLocalPlayer(Collision other)
        {
            var playerEntity = other.transform.root.GetComponent<PlayerEntity>();
            var vehicleEntity = other.transform.root.GetComponent<VehicleEntity>();
            return (playerEntity != null && playerEntity == SpawnedBy) || (vehicleEntity != null && vehicleEntity == SpawnedBy.Info.InVehicle);
        }
    }
}