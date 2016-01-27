using Assets.Scripts.Simple.Entity.Player;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Simple.Entity
{
    public class BulletEntity : NetworkBehaviour
    {
        public const int BULLET_DAMAGE = 10;
        public const float BULLET_SPEED = 10f;

        [SyncVar(hook="OnRotationSet")]
        public Quaternion BulletRotation;

        public PlayerEntity SpawnedBy { get; set; }
    
        private void OnRotationSet(Quaternion rotation)
        {
            transform.rotation = rotation;
            GetComponent<Rigidbody>().velocity = transform.forward * BULLET_SPEED;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isServer) return;

            var playerEntity = other.transform.root.GetComponent<PlayerEntity>();

            if (playerEntity != null && playerEntity != SpawnedBy)
            {
                playerEntity.Info.CurrentHealth -= BULLET_DAMAGE;
                Debug.Log("Bullet hit " + playerEntity.name);
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}