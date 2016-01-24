using System.Collections.Generic;
using Assets.Scripts.Simple.Utils.Extensions;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Simple.Entity
{
    public abstract class AbstractNetworkedEntity : NetworkBehaviour
    {
        protected List<GameObject> LocalOnlyObjects { get; private set; }
        protected List<GameObject> RemoteOnlyObjects { get; private set; }

        public override void OnStartClient()
        {
            base.OnStartClient();

            LocalOnlyObjects = gameObject.FindGameObjectsWithTag(Tags.LocalOnly);
            RemoteOnlyObjects = gameObject.FindGameObjectsWithTag(Tags.RemoteOnly);

            //Assume this is a remotly owned object.
            LocalOnlyObjects.ForEach(g => g.SetActive(false));
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            RemoteOnlyObjects.ForEach(g => g.SetActive(false));
            LocalOnlyObjects.ForEach(g => g.SetActive(true));
        }

        public override void OnStopAuthority()
        {
            base.OnStopAuthority();

            RemoteOnlyObjects.ForEach(g => g.SetActive(true));
            LocalOnlyObjects.ForEach(g => g.SetActive(false));
        }
    }
}
