using UnityEngine;
using GameFramework.Event;

namespace SuperBilliard
{
    public class GameStartUIForm : UILogicBase
    {
        public PlayerDataUI own;
        public PlayerDataUI opponent;

        private string opponentAccountName;
        private PlayerDataBundle _playerDataBundle;

        private bool _gameStartFlag = false;
        private float _closeTime = 2f;
        private float _timer = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            opponentAccountName = (string)userData;

            _timer = 0;
            _gameStartFlag = false;

            //获得自己的信息
            _playerDataBundle = GameEntry.DataBundle.GetData<PlayerDataBundle>();
            Sprite hairSprite = GameEntry.ResourceCache.GetHairSprite(_playerDataBundle.PlayerImage.HairId);
            Sprite faceSprite = GameEntry.ResourceCache.GetFaceSprite(_playerDataBundle.PlayerImage.FacaId);
            Sprite bodySprite = GameEntry.ResourceCache.GetBodySprite(_playerDataBundle.PlayerImage.BodyId);
            Sprite kitSprite = GameEntry.ResourceCache.GetKitSprite(_playerDataBundle.PlayerImage.KitId);
            own.Display(_playerDataBundle.NickName, _playerDataBundle.Level.ToString(), bodySprite, hairSprite, faceSprite, kitSprite);

            //获得敌方的信息
            GameEntry.Client.SendGetPlayerMessageRequest(opponentAccountName);
            GameEntry.Event.Subscribe(RecievePlayerMessageEventArgs.EventId, OnRecievePlayerMessage);
            GameEntry.Event.Subscribe(GameStartEventArgs.EventId, OnGameStart);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(RecievePlayerMessageEventArgs.EventId, OnRecievePlayerMessage);
            GameEntry.Event.Unsubscribe(GameStartEventArgs.EventId, OnGameStart);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (_gameStartFlag == false)
            {
                return;
            }
            _timer += elapseSeconds;
            if (_timer >= _closeTime)
            {
                _gameStartFlag = false;
                Close();
            }
        }

        //游戏开始
        private void OnGameStart(object sender, GameEventArgs e)
        {
            GameStartEventArgs args = (GameStartEventArgs)e;
            //游戏开始,延迟关闭界面
            _gameStartFlag = true;
            _timer = 0;
            _closeTime = args.DelayStartTime;
        }

        private void OnRecievePlayerMessage(object sender, GameEventArgs e)
        {
            RecievePlayerMessageEventArgs args = (RecievePlayerMessageEventArgs)e;

            if (args.UserName == opponentAccountName)
            {
                PlayerDataItem opponentData = _playerDataBundle.Opponent;
                opponentData.UserName = args.UserName;
                opponentData.NickName = args.NickName;
                opponentData.Level = args.Level;
                opponentData.Description = args.Description;

                //设置完成后
                opponentData.PlayerImageData.FacaId = args.FaceID;
                opponentData.PlayerImageData.HairId = args.HairID;
                opponentData.PlayerImageData.BodyId = args.BodyID;
                opponentData.PlayerImageData.KitId = args.KitID;

                //获取对应的Sprite
                Sprite hairSprite = GameEntry.ResourceCache.GetHairSprite(opponentData.PlayerImageData.HairId);
                Sprite faceSprite = GameEntry.ResourceCache.GetFaceSprite(opponentData.PlayerImageData.FacaId);
                Sprite bodySprite = GameEntry.ResourceCache.GetBodySprite(opponentData.PlayerImageData.BodyId);
                Sprite kitSprite = GameEntry.ResourceCache.GetKitSprite(opponentData.PlayerImageData.KitId);

                opponent.Display(args.NickName, args.Level.ToString(), hairSprite, faceSprite, bodySprite, kitSprite);
            }
        }

    }
}
