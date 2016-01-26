using Assets.Scripts.Simple.Entity.Player;
using Assets.Scripts.Simple.Utils.Extensions;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Simple.Entity
{
    public class BulletEntity : NetworkBehaviour
    {
        private const int BULLET_DAMAGE = 10;
        private const float BULLET_SPEED = 0.1f;

        private void Start()
        {
            var rigidBody = GetComponent<Rigidbody>().velocity = Vector3.forward * BULLET_SPEED;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isServer) return;

            if (other.gameObject.HasTag(Tags.Player))
            {
                var playerEntity = other.GetComponent<PlayerEntity>();
                playerEntity.Info.CurrentHealth -= BULLET_DAMAGE;
                Debug.Log("Bullet hit " + playerEntity.name);
            }
            else
            {
                //NetworkServer.Destroy(gameObject);
            }
        }
    }
}