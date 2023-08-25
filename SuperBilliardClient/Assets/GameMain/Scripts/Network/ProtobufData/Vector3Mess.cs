using GameFramework;
using UnityEngine;

namespace GameMessage
{
    public partial class Vector3Mess : IReference
    {
        public void Clear()
        {
            X = 0; Y = 0; Z = 0;
        }
        public static Vector3Mess Create(Vector3 value)
        {
            Vector3Mess vector3Mess = ReferencePool.Acquire<Vector3Mess>();
            vector3Mess.X = value.x;
            vector3Mess.Y = value.y;
            vector3Mess.Z = value.z;
            return vector3Mess;
        }
    }
}