using System.Collections.Generic;
using System.Linq;
using Location;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Assets.Extensions.Locations
{
    public class WorldLocations : MonoBehaviour
    {
        public string LocationGroupName = "Locations";
        public List<WorldLocation> Locations = new List<WorldLocation>();

        #if UNITY_EDITOR
        private Transform SceneCamera { get { return SceneView.GetAllSceneCameras()[0].transform; } }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Locations.ForEach(l => Gizmos.DrawIcon(l.Position, "PlayerSpawn"));
        }

        public void MoveUp(WorldLocation loc)
        {
            var index = Locations.IndexOf(loc);
            if (index > 0)
            {
                Locations.Remove(loc);
                Locations.Insert(index - 1, loc);
            }
        }

        public void MoveDown(WorldLocation loc)
        {
            var index = Locations.IndexOf(loc);
            if (index < Locations.Count - 1)
            {
                Locations.Remove(loc);
                Locations.Insert(index + 1, loc);
            }
        }

        public void CreateWarpPoint(string name, Vector3 pos, Vector3 eulerRot)
        {
            var newloc = new WorldLocation(name, pos, eulerRot);
            Locations.Add(newloc);
        }

        public void CreateWarpPoint()
        {
            var newloc = new WorldLocation("New Warp", SceneCamera.position, SceneCamera.rotation.eulerAngles);
            Locations.Add(newloc);
        }

        public void RemoveWarpPoint(WorldLocation loc)
        {
            Locations.Remove(loc);
        }

        public void WarpTo(string NameOfWarp)
        {
            var warp = Locations.SingleOrDefault(w => w.Name == NameOfWarp);
            if (warp != null) WarpTo(warp);
        }

        public void WarpTo(WorldLocation loc)
        {
            var temp = new GameObject();
            temp.transform.position = loc.Position;
            temp.transform.rotation = Quaternion.Euler(loc.Rotation);
            SceneView.lastActiveSceneView.AlignViewToObject(temp.transform);
            DestroyImmediate(temp);
        }

        public void Overwrite(WorldLocation loc)
        {
            loc.Position = SceneCamera.position;
            loc.Rotation = SceneCamera.rotation.eulerAngles;
        }

        public bool WarpMatch(WorldLocation loc)
        {
            var match = !(RoundVec3(loc.Position) != RoundVec3(SceneCamera.position));
            if (RoundVec3(loc.Rotation) != RoundVec3(SceneCamera.rotation.eulerAngles)) match = false;
            return match;
        }

        Vector3 RoundVec3(Vector3 v)
        {
            Vector3 rounded = Vector3.zero;
            rounded.x = (int)v.x;
            rounded.y = (int)v.y;
            rounded.z = (int)v.z;
            return rounded;
        }
        #endif
    }
}