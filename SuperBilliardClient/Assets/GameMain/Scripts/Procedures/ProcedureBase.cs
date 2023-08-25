using System.Collections.Generic;

namespace SuperBilliard
{
    public abstract class ProcedureBase : GameFramework.Procedure.ProcedureBase
    {
        public const string KeyNextSceneId = "NextScene";
        public const string KeyBattle = "Battle";

        /// <summary>
        /// 忽略的UI,在改变场景的时候会遍历此表,忽略对应的UI,请把对应的Serilizeid,加入其中
        /// </summary>
        protected static Queue<int> _splashUIQueue = new Queue<int>();
        protected static Queue<int> _ignoreUIQueue = new Queue<int>();

        // 获取流程是否使用原生对话框
        // 在一些特殊的流程（如游戏逻辑对话框资源更新完成前的流程）中，可以考虑调用原生对话框进行消息提示行为
        public abstract bool UseNativeDialog
        {
            get;
        }
    }
}
