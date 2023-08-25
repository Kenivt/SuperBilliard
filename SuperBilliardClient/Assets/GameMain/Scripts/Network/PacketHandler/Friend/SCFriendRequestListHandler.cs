using GameMessage;
using GameFramework.Network;
using System.Collections.Generic;
using System.Diagnostics;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class SCFriendRequestListHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCFriendRequestList;

        public override void Handle(object sender, Packet packet)
        {
            SCFriendRequestList scPacket = (SCFriendRequestList)packet;
            Log.Warning("SCFriendRequestListHandler");

            List<FriendRequestData> datas = new List<FriendRequestData>(scPacket.RequestFriendList.Count);

            foreach (var item in scPacket.RequestFriendList)
            {
                FriendRequestData data = new FriendRequestData();
                data.userName = item.Username;
                data.nickName = item.NickName;
                data.description = item.Description;
                data.hair = GameEntry.ResourceCache.GetHairSprite(item.HairId);
                data.face = GameEntry.ResourceCache.GetFaceSprite(item.FaceId);
                data.body = GameEntry.ResourceCache.GetBodySprite(item.BodyId);
                data.kit = GameEntry.ResourceCache.GetKitSprite(item.KitId);
                data.handled = false;
                datas.Add(data);
            }

            GameEntry.Event.Fire(this, UpdateFriendRequestListEventArgs.Create(datas));
        }
    }
}