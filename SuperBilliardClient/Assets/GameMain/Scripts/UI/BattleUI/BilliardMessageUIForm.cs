using GameMessage;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class BilliardMessageUIForm : UILogicBase
    {
        public BattleMessageUI OwnSlot;
        public BattleMessageUI OpponentSlot;
        [Header("====计时====")]
        public Text timerText;
        [Header("当前回合的显示...")]
        public Image ownIconBcg;
        public Image opponentIconBcg;
        [SerializeField] private Color _orignColor;
        [SerializeField] private Color _endColor;
        [SerializeField] private float _colorChangeTime = 0.5f;

        private bool[] billiardIds = new bool[16];

        /// <summary>
        /// 指的是我方或者左侧玩家的球色...
        /// </summary>
        private BilliardType billiardType;
        private bool _isconfirm;
        //当前回合的标志...
        private float timer;
        private Color _colorA;
        private Color _colorB;
        private Image _targetBcg;

        //计时
        private float _timecount;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            //初始化闪烁颜色
            _colorA = _orignColor;
            _colorB = _endColor;
            //bool isFirstMove = (bool)userData;

            //if (isFirstMove)
            //{
            //    opponentIconBcg.color = _orignColor;
            //    _targetBcg = ownIconBcg;
            //}
            //else
            //{
            //    ownIconBcg.color = _orignColor;
            //    _targetBcg = opponentIconBcg;
            //}

            _isconfirm = false;



            for (int i = 0; i < 16; i++)
            {
                billiardIds[i] = false;
            }
            OwnSlot.ClearAllBilliard();
            OpponentSlot.ClearAllBilliard();

            //更新玩家形象
            var playerDataBundle = GameEntry.DataBundle.GetData<PlayerDataBundle>();
            PlayerImageData own = playerDataBundle.PlayerImage;
            Sprite bodySprite = GameEntry.ResourceCache.GetBodySprite(own.BodyId);
            Sprite hairSprite = GameEntry.ResourceCache.GetHairSprite(own.HairId);
            Sprite faceSprite = GameEntry.ResourceCache.GetFaceSprite(own.FacaId);
            Sprite kitSprite = GameEntry.ResourceCache.GetKitSprite(own.KitId);
            OwnSlot.DisplayPlayerImage(hairSprite, faceSprite, bodySprite, kitSprite);

            Sprite bodySprite1 = GameEntry.ResourceCache.GetBodySprite(playerDataBundle.Opponent.PlayerImageData.BodyId);
            Sprite hairSprite1 = GameEntry.ResourceCache.GetHairSprite(playerDataBundle.Opponent.PlayerImageData.HairId);
            Sprite faceSprite1 = GameEntry.ResourceCache.GetFaceSprite(playerDataBundle.Opponent.PlayerImageData.FacaId);
            Sprite kitSprite1 = GameEntry.ResourceCache.GetKitSprite(playerDataBundle.Opponent.PlayerImageData.KitId);

            OpponentSlot.DisplayPlayerImage(hairSprite1, faceSprite1, bodySprite1, kitSprite1);

            GameEntry.Event.Subscribe(StartTurnEventArgs.EventId, StartTurnEventCallBack);
            GameEntry.Event.Subscribe(EndTurnEventArgs.EventId, EndTurnEventCallBack);

            //进球事件
            GameEntry.Event.Subscribe(BilliardGoalEventArgs.EventId, BilliardGoalCallBack);
            GameEntry.Event.Subscribe(ConfirmBilliardTypeEventArgs.EventId, ConfirmBilliardTypeCallBack);
            //计时事件
            GameEntry.Event.Subscribe(StartTimeCountEventArgs.EventId, StartTimeCountCallback);
            GameEntry.Event.Subscribe(StopTimeCountEventArgs.EventId, StopTimeCountCallback);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(StartTurnEventArgs.EventId, StartTurnEventCallBack);
            GameEntry.Event.Unsubscribe(EndTurnEventArgs.EventId, EndTurnEventCallBack);
            GameEntry.Event.Unsubscribe(BilliardGoalEventArgs.EventId, BilliardGoalCallBack);
            GameEntry.Event.Unsubscribe(ConfirmBilliardTypeEventArgs.EventId, ConfirmBilliardTypeCallBack);
            //计时事件
            GameEntry.Event.Unsubscribe(StartTimeCountEventArgs.EventId, StartTimeCountCallback);
            GameEntry.Event.Unsubscribe(StopTimeCountEventArgs.EventId, StopTimeCountCallback);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (_targetBcg != null)
            {
                //颜色闪烁
                timer += Time.deltaTime / _colorChangeTime;

                if (timer < 1f)
                {
                    _targetBcg.color = Color.Lerp(_colorA, _colorB, timer);
                }
                else
                {
                    timer = 0;
                    Color temp = _colorA;
                    _colorA = _colorB;
                    _colorB = temp;
                }
            }
            _timecount -= Time.deltaTime;
            if (_timecount > 0)
            {
                timerText.text = ((int)_timecount).ToString();
            }
        }

        private void StopTimeCountCallback(object sender, GameEventArgs e)
        {
            _timecount = 0;
        }

        private void StartTimeCountCallback(object sender, GameEventArgs e)
        {
            StartTimeCountEventArgs args = (StartTimeCountEventArgs)e;
            _timecount = args.Time;
        }

        private void BilliardGoalCallBack(object sender, GameEventArgs e)
        {
            BilliardGoalEventArgs ne = (BilliardGoalEventArgs)e;
            int index = ne.Billiard.BilliardId;

            if (index == 0 || index == 8)
            {
                return;
            }

            if (billiardIds[index] == true)
            {
                Log.Error("为什么重复进球...");
            }
            else
            {
                billiardIds[index] = true;
            }
            if (_isconfirm == false)
            {
                return;
            }
            if (billiardType == BilliardType.Single)
            {
                UpdateBilliardSlot(OwnSlot, OpponentSlot);
            }
            else if (billiardType == BilliardType.Double)
            {
                UpdateBilliardSlot(OpponentSlot, OwnSlot);
            }
            else
            {
                Log.Error("球色类型错误...");
            }
        }

        private void UpdateBilliardSlot(BattleMessageUI singlon, BattleMessageUI doublon)
        {
            singlon.ClearAllBilliard();
            doublon.ClearAllBilliard();

            //是否为决胜球（除了黑8其他球都进了）
            int displayIndex = 0;
            for (int i = 1; i < 8; i++)
            {
                if (billiardIds[i] == false)
                {
                    singlon.DisplayBilliard(displayIndex++, GameEntry.ResourceCache.GetFancyBilliardSprite(i));
                }
            }
            if (displayIndex == 0)
            {
                //显示黑8
                if (billiardIds[7] == false)
                    singlon.DisplayBilliard(displayIndex, GameEntry.ResourceCache.GetFancyBilliardSprite(8));
            }
            displayIndex = 0;
            for (int i = 9; i < 16; i++)
            {
                if (billiardIds[i] == false)
                {
                    doublon.DisplayBilliard(displayIndex++, GameEntry.ResourceCache.GetFancyBilliardSprite(i));
                }
            }
            if (displayIndex == 0)
            {
                //显示黑8
                if (billiardIds[15] == false)
                    doublon.DisplayBilliard(0, GameEntry.ResourceCache.GetFancyBilliardSprite(8));
            }
        }

        private void ConfirmBilliardTypeCallBack(object sender, GameEventArgs e)
        {
            ConfirmBilliardTypeEventArgs ne = (ConfirmBilliardTypeEventArgs)e;
            _isconfirm = true;
            billiardType = ne.billiardType;
            if (billiardType == BilliardType.Single)
            {
                UpdateBilliardSlot(OwnSlot, OpponentSlot);
            }
            else if (billiardType == BilliardType.Double)
            {
                UpdateBilliardSlot(OpponentSlot, OwnSlot);
            }
            else
            {
                Log.Error("球色类型错误...");
            }
        }

        private void EndTurnEventCallBack(object sender, GameEventArgs e)
        {
            ownIconBcg.color = _orignColor;
            _targetBcg = opponentIconBcg;
        }

        private void StartTurnEventCallBack(object sender, GameEventArgs e)
        {
            opponentIconBcg.color = _orignColor;
            _targetBcg = ownIconBcg;
        }
    }
}