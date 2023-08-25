using GameMessage;
using UnityEngine;

namespace SuperBilliard
{
    public static class ClientComponentExtension
    {

        public static string GetKcpEndPointKey(this ClientComponent clientComponent)
        {
            if (clientComponent.KcpOwnEndPoint == null)
            {
                return null;
            }
            string key = clientComponent.KcpOwnEndPoint.Address.ToString() + "|" + clientComponent.KcpOwnEndPoint.Port;
            return key;
        }

        public static void SendGameResult(this ClientComponent clientComponent, GameMessage.GameResult gameResult)
        {
            CSGameResultPack packet = CSGameResultPack.Create(gameResult);
            clientComponent.Send(packet);
        }
        public static void SendEndTurn(this ClientComponent clientComponent, bool foul)
        {
            CSEndTurn packet = CSEndTurn.Create(foul);
            clientComponent.Send(packet);
        }
        public static void SendCueSync(this ClientComponent clientComponent, Vector3 position, float angleY)
        {
            CSCueSync packet = CSCueSync.Create(position, angleY);
            clientComponent.Send(packet);
        }
        public static void SendCueStorageSynce(this ClientComponent clientComponent, float fillAmount, Vector3 dir)
        {
            CSCueStorageSync packet = CSCueStorageSync.Create(fillAmount, dir);
            clientComponent.Send(packet);
        }
        public static void SendGetPlayerMessageRequest(this ClientComponent clientComponent, string accountName)
        {
            CSGetPlayerMessage packet = CSGetPlayerMessage.Create(accountName);
            clientComponent.Send(packet);
        }
        public static void SendBilliardSync(this ClientComponent clientComponent, params BilliardMessage[] billiardMessages)
        {
            if (billiardMessages.Length == 0)
            {
                return;
            }
            CSBilliardSync packet = CSBilliardSync.Create(billiardMessages);
            clientComponent.SendKcpMessage(packet);
        }
        public static void SendSyncSoundMessage(this ClientComponent clientComponent, EnumSound soundId, float volume, Vector3 position)
        {
            CSSyncSound packet = CSSyncSound.Create((int)soundId, volume, position);
            clientComponent.Send(packet);
        }
        public static void SendExitGameRoom(this ClientComponent clientComponent)
        {
            CSExitGameRoom packet = CSExitGameRoom.Create();
            clientComponent.Send(packet);
        }
        public static void SendBilliardTypeConfirme(this ClientComponent clientComponent, BilliardType billiardType)
        {
            CSBilliardTypeConfirm packet = CSBilliardTypeConfirm.Create(billiardType);
            clientComponent.Send(packet);
        }

        public static void SendSetBilliardActiveMessage(this ClientComponent clientComponent, int billiardId, bool active, bool physicsIsOpen)
        {
            CSSetBilliardState packet = CSSetBilliardState.Create(billiardId, active, physicsIsOpen);
            clientComponent.Send(packet);
        }
        public static void SendBattleEmptyEvent(this ClientComponent clientComponent, BattleEmptyEvent battleEmptyEvent)
        {
            CSBattleEmptyEvent packet = CSBattleEmptyEvent.Create(battleEmptyEvent);
            clientComponent.Send(packet);
        }

        public static void SendRequestFriend(this ClientComponent clientComponent, string accountName, string targetName)
        {
            CSRequestFriend packet = CSRequestFriend.Create(accountName, targetName);
            clientComponent.Send(packet);
        }

        public static void SendProcessingFriendRequest(this ClientComponent clientComponent, string accountName, string targetName, bool accept)
        {
            FriendRequestState state = FriendRequestState.Await;
            if (accept == true)
            {
                state = FriendRequestState.Agreen;
            }
            else
            {
                state = FriendRequestState.Refuse;
            }
            CSProcessingFriendRequest packet = CSProcessingFriendRequest.Create(accountName, targetName, state);
            clientComponent.Send(packet);
        }

        public static void SendGetFriendListMessage(this ClientComponent clientComponent, string accountName)
        {
            CSGetFriendList packet = CSGetFriendList.Create(accountName);
            clientComponent.Send(packet);
        }


        public static void SendCreateRoomPacket(this ClientComponent clientComponent, string username, GameType gameType)
        {
            CSCreateRoom packet = CSCreateRoom.Create();
            packet.Username = username;
            packet.GameType = gameType;
            clientComponent.Send(packet);
        }

        public static void SendReadyGamePacket(this ClientComponent clientComponent, bool isready)
        {
            string username = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
            CSReadyGame packet = CSReadyGame.Create(username, isready);
            clientComponent.Send(packet);
        }

        public static void SendLeaveGameRoom(this ClientComponent clientComponent)
        {
            string username = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;
            CSLeaveRoom cSLeaveRoom = CSLeaveRoom.Create(username);
            clientComponent.Send(cSLeaveRoom);
        }

        public static void SendSavePlayerMessageRequest(this ClientComponent clientComponent,
            string username, string nickName, string description,
            int hairId, int bodyId, int faceId, int kitId)
        {
            CSSavePlayerMessage packet = CSSavePlayerMessage.Create();
            packet.UserName = username;
            packet.SnikName = nickName;
            packet.Description = description;
            packet.HairId = hairId;
            packet.BodyId = bodyId;
            packet.FaceId = faceId;
            packet.KitId = kitId;
            clientComponent.Send(packet);
        }
    }
}
