using UnityEngine;
using UnityEngine.UI;
using GameFramework.Event;
using System;

namespace SuperBilliard
{
    public class SnokkerPlayerDataUIForm : UILogicBase
    {
        public SnokkerPlayerDataUIItem Own;
        public SnokkerPlayerDataUIItem Opponent;

        public Text timerText;
        private float _timecount = 40;
        private bool _startTimeCount = false;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            SetMessage();
            _timecount = 40;
            _startTimeCount = false;
            GameEntry.Event.Subscribe(StartTimeCountEventArgs.EventId, StartTimeCount);
            GameEntry.Event.Subscribe(StopTimeCountEventArgs.EventId, StopTimeCount);
            GameEntry.Event.Subscribe(UpdatePlayerScoreEvenrArgs.EventId, OnUpdateScore);
            GameEntry.Event.Subscribe(UpdateActiveBilliardTypeEventArgs.EventId, OnUpdateActiveBilliardType);
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Subscribe(StopTimeCountEventArgs.EventId, StopTimeCount);
            GameEntry.Event.Unsubscribe(StartTimeCountEventArgs.EventId, StartTimeCount);
            GameEntry.Event.Unsubscribe(UpdatePlayerScoreEvenrArgs.EventId, OnUpdateScore);
            GameEntry.Event.Unsubscribe(UpdateActiveBilliardTypeEventArgs.EventId, OnUpdateActiveBilliardType);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (_startTimeCount == false)
            {
                return;
            }
            _timecount -= realElapseSeconds;
            if (_timecount > 0)
            {
                timerText.text = ((int)_timecount).ToString();
            }
            else
            {
                timerText.text = 0.ToString();
            }
        }

        private void StopTimeCount(object sender, GameEventArgs e)
        {
            _startTimeCount = false;
        }

        private void StartTimeCount(object sender, GameEventArgs e)
        {
            StartTimeCountEventArgs args = (StartTimeCountEventArgs)e;
            _timecount = args.Time;
            _startTimeCount = true;
        }

        private void OnUpdateActiveBilliardType(object sender, GameEventArgs e)
        {
            UpdateActiveBilliardTypeEventArgs args = (UpdateActiveBilliardTypeEventArgs)e;
            if (args.IsOwnTurn)
            {
                //得到对应的图片
                Opponent.UpdateActiveBilliard(null);
                Own.UpdateActiveBilliard(GameEntry.ResourceCache.GetSnokkerBilliardSprite(args.BilliardType));
            }
            else
            {
                Own.UpdateActiveBilliard(null);
                Opponent.UpdateActiveBilliard(GameEntry.ResourceCache.GetSnokkerBilliardSprite(args.BilliardType));
            }
        }

        private void OnUpdateScore(object sender, GameEventArgs e)
        {
            UpdatePlayerScoreEvenrArgs args = (UpdatePlayerScoreEvenrArgs)e;
            if (args.IsOwn)
            {
                Own.UpdateScore(args.Score);
            }
            else
            {
                Opponent.UpdateScore(args.Score);
            }
        }

        private void SetMessage()
        {
            var playerDataBundle = GameEntry.DataBundle.GetData<PlayerDataBundle>();
            PlayerDataItem ownData = playerDataBundle.Own;

            Sprite bodySprite = GameEntry.ResourceCache.GetBodySprite(ownData.PlayerImageData.BodyId);
            Sprite hairSprite = GameEntry.ResourceCache.GetHairSprite(ownData.PlayerImageData.HairId);
            Sprite faceSprite = GameEntry.ResourceCache.GetFaceSprite(ownData.PlayerImageData.FacaId);
            Sprite kitSprite = GameEntry.ResourceCache.GetKitSprite(ownData.PlayerImageData.KitId);
            Own.DisplayMessage(hairSprite, bodySprite, kitSprite, faceSprite, ownData.NickName);
            Own.UpdateScore(0);

            Sprite bodySprite1 = GameEntry.ResourceCache.GetBodySprite(playerDataBundle.Opponent.PlayerImageData.BodyId);
            Sprite hairSprite1 = GameEntry.ResourceCache.GetHairSprite(playerDataBundle.Opponent.PlayerImageData.HairId);
            Sprite faceSprite1 = GameEntry.ResourceCache.GetFaceSprite(playerDataBundle.Opponent.PlayerImageData.FacaId);
            Sprite kitSprite1 = GameEntry.ResourceCache.GetKitSprite(playerDataBundle.Opponent.PlayerImageData.KitId);
            Opponent.DisplayMessage(hairSprite1, bodySprite1, kitSprite1, faceSprite1, playerDataBundle.Opponent.NickName);
            Opponent.UpdateScore(0);
        }
    }
}