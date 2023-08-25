using GameMessage;
using UnityEngine;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCCueStorageSyncHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.SCCueStorageSync;

        public override void Handle(object sender, Packet packet)
        {
            SCCueStorageSync sCCueStorageSync = packet as SCCueStorageSync;
            if (sCCueStorageSync == null)
            {
                return;
            }
            Vector3 dir = new Vector3(sCCueStorageSync.DirX, sCCueStorageSync.DirY, sCCueStorageSync.DirZ);
            //GameEntry.Event.Fire(this, FireBilliardEventArgs.Create(sCCueStorageSync.FillAmount, dir));
        }
    }
}