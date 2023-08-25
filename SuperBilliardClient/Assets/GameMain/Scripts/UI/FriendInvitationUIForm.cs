using GameMessage;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace SuperBilliard
{
    public class InvitationMessage : IReference
    {
        public string inviterName;
        public int roomId;
        public GameType gameType;

        //对应的控制类
        public float timer = 0f;
        public int? uiSerilid;

        public void Clear()
        {
            timer = 0f;
            roomId = 0;
            inviterName = null;
            gameType = GameType.None;
            uiSerilid = null;
        }

        public static InvitationMessage Create(string inviterName, int roomId, GameType gameType)
        {
            InvitationMessage invitationMessage = ReferencePool.Acquire<InvitationMessage>();
            invitationMessage.inviterName = inviterName;
            invitationMessage.roomId = roomId;
            invitationMessage.gameType = gameType;
            invitationMessage.timer = 0f;
            return invitationMessage;
        }
    }

    public class FriendInvitationUIForm : UILogicBase
    {
        [SerializeField] private Button _refuseBtn;
        [SerializeField] private Button _acceptBtn;
        [SerializeField] private Image _waitBar;

        [SerializeField] private Image face;
        [SerializeField] private Image hair;
        [SerializeField] private Image kit;
        [SerializeField] private Image body;
        [SerializeField] private Text nickName;

        public Queue<InvitationMessage> InvitationMessageQueue { get; private set; } = new Queue<InvitationMessage>();

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            _refuseBtn.onClick.AddListener(OnRefuseBtnClick);
            _acceptBtn.onClick.AddListener(OnAcceptBtnClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(RecievePlayerMessageEventArgs.EventId, OnRecievePlayerMessage);

            InvitationMessage invitationMessage = userData as InvitationMessage;
            if (invitationMessage == null)
            {
                Log.Error("InvitationMessage is null");
            }
            InvitationMessageQueue.Enqueue(invitationMessage);
            GameEntry.Client.SendGetPlayerMessageRequest(invitationMessage.inviterName);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(RecievePlayerMessageEventArgs.EventId, OnRecievePlayerMessage);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (InvitationMessageQueue.TryPeek(out InvitationMessage message))
            {
                message.timer += elapseSeconds;
                if (message.timer >= 5)
                {
                    ReferencePool.Release(message);
                    InvitationMessageQueue.Dequeue();
                    //如果队列中还有消息，那么就显示下一条消息
                    if (InvitationMessageQueue.TryPeek(out InvitationMessage message2))
                    {
                        //获取玩家信息
                        GameEntry.Client.SendGetPlayerMessageRequest(message2.inviterName);
                    }
                }
                else
                {
                    _waitBar.fillAmount = Mathf.Clamp01(1f - message.timer / 5f);
                }
            }
            else
            {
                _waitBar.fillAmount = 0f;
                //关闭界面
                Close();
            }
        }

        private void OnAcceptBtnClick()
        {
            if (InvitationMessageQueue.TryPeek(out InvitationMessage message))
            {
                //获取自己的用户名
                string ownUsername = GameEntry.DataBundle.GetData<PlayerDataBundle>().UserName;

                //发送同意邀请的消息
                CSAcceptGameInvitation acceptInvitation = CSAcceptGameInvitation.
                  Create(message.gameType, message.inviterName, ownUsername, message.roomId);
                GameEntry.Client.Send(acceptInvitation);
            }
            else
            {
                Log.Error("InvitationMessage is null");
            }
            Close();
        }

        private void OnRecievePlayerMessage(object sender, GameEventArgs e)
        {
            RecievePlayerMessageEventArgs ne = (RecievePlayerMessageEventArgs)e;
            if (InvitationMessageQueue.TryPeek(out InvitationMessage message))
            {
                if (ne.UserName != message.inviterName)
                {
                    return;
                }
                //显示对应的玩家信息
                face.sprite = GameEntry.ResourceCache.GetFaceSprite(ne.FaceID);
                hair.sprite = GameEntry.ResourceCache.GetHairSprite(ne.HairID);
                kit.sprite = GameEntry.ResourceCache.GetKitSprite(ne.KitID);
                body.sprite = GameEntry.ResourceCache.GetBodySprite(ne.BodyID);
                nickName.text = ne.NickName;
            }
        }
        private void OnRefuseBtnClick()
        {
            if (InvitationMessageQueue.Count > 0)
            {
                InvitationMessage message = InvitationMessageQueue.Dequeue();
                if (InvitationMessageQueue.TryPeek(out InvitationMessage mess))
                {
                    //获取玩家信息
                    GameEntry.Client.SendGetPlayerMessageRequest(mess.inviterName);
                }
                else
                {
                    //关闭界面
                    Close();
                }
            }
            else
            {
                Log.Error("InvitationMessage is null,When close the uiForm.");
            }
        }
    }
}