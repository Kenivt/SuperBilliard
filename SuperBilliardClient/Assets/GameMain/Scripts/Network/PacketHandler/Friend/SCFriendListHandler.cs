using GameMessage;
using GameFramework;
using GameFramework.Network;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuperBilliard
{
    public class SCFriendListHandler : PacketHandlerBase
    {
        public override int Id => Constant.PacketTypeId.SCFriendListMessage;

        public override void Handle(object sender, Packet packet)
        {
            SCFriendList sc = (SCFriendList)packet;

            var friendDataBundle = GameEntry.DataBundle.GetData<FriendDataBundle>();
            List<FriendMessage> friendMessages = friendDataBundle.FriendList;

            if (friendMessages == null)
            {
                friendMessages = new List<FriendMessage>();
                friendDataBundle.FriendList = friendMessages;
            }

            foreach (var item in sc.FriendMessages)
            {
                FriendMessage friendMessage = friendMessages.FirstOrDefault((m) => m.UserName == item.Username);
                if (friendMessage == null)
                {
                    friendMessage = new FriendMessage();
                    friendMessages.Add(friendMessage);
                }
                friendMessage.NickName = item.NickName;
                friendMessage.description = item.Description;
                friendMessage.status = item.Status;
                friendMessage.isLogin = item.IsLogin;
                friendMessage.UserName = item.Username;
                friendMessage.hair = GameEntry.ResourceCache.GetHairSprite(item.HairId);
                friendMessage.face = GameEntry.ResourceCache.GetFaceSprite(item.FaceId);
                friendMessage.body = GameEntry.ResourceCache.GetBodySprite(item.BodyId);
                friendMessage.kit = GameEntry.ResourceCache.GetKitSprite(item.KitId);
            }

            friendMessages.Sort((x, y) => -x.status.CompareTo(y.status));
            GameEntry.Event.Fire(this, UpdateFriendListEventArgs.Create(friendMessages));
        }
    }
}