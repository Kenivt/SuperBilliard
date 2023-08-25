using UnityEngine;

namespace SuperBilliard
{
    public interface IBilliard
    {
        BilliardData BilliardData { get; }
        /// <summary>
        /// 台球的Id
        /// </summary>
        int BilliardId { get; }
        /// <summary>
        /// 规定-2为碰撞到不可反弹的边界,-1为没有碰到任何碰撞体,0-15为碰撞到的球的编号
        /// </summary>
        int FirestCollideId { get; }
        /// <summary>
        /// 是否发送进球事件,联网时不需要发送进球事件
        /// </summary>
        Vector3 Velocity { get; set; }

        Vector3 Position { get; set; }

        Vector3 Rotation { get; set; }

        Vector3 LastTurnPosition { get; }

        float Radius { get; }

        void SetActive(bool active);
        bool Decelerate(float decelerate);

        void SetRigidbodyEnable(bool active);

        void OnSyncTransform(Vector3 positon, Vector3 euler);

        /// <summary>
        /// 重新设置小球
        /// </summary>
        void TurnReset();
    }
}