using UnityEngine;
using System;

namespace Location
{
	[Serializable]
	public class WorldLocation
    {
		public string Name;
		public Vector3 Position;
		public Vector3 Rotation;

		public WorldLocation( string n, Vector3 p, Vector3 r )
        {
			Name = n;
			Position = p;
			Rotation = r;
		}
	}
}
