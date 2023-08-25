using GameMessage;
using UnityEngine;
using GameFramework.Network;
using GameFramework;

namespace SuperBilliard
{
    public class SCCueSyncHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.SCCueSync;

        public override void Handle(object sender, Packet packet)
        {
            SCCueSync scCueSync = (SCCueSync)packet;
            if (scCueSync == null)
            {
                return;
            }
            Vector3 position = new Vector3(scCueSync.Position.X, scCueSync.Position.Y, scCueSync.Position.Z);
            GameEntry.Event.Fire(this, CueSyncEventArgs.Create(position, scCueSync.AngltY));
        }
    }
}