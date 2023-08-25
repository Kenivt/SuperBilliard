using GameMessage;
using UnityEngine;
using GameFramework.Network;

namespace SuperBilliard
{
    public class SCSetBilliardActiveHandler : PacketHandlerBase
    {
        public override int Id => SuperBilliard.Constant.PacketTypeId.SCSetBilliardState;

        public override void Handle(object sender, Packet packet)
        {
            SCSetBilliardState packetImpl = (SCSetBilliardState)packet;
            //因为网络包赋值为0时容易出错,所以这里用888代替白球
            if (packetImpl.BilliardId == 888)
            {
                if (BilliardManager.Instance != null)
                {
                    MonoBehaviour mono = (MonoBehaviour)BilliardManager.Instance.WhiteBilliard;
                    IBilliard billiard = mono.GetComponent<IBilliard>();
                    mono.gameObject.SetActive(packetImpl.Active);
                    billiard.SetRigidbodyEnable(packetImpl.PhysicsIsOpen);
                }
            }
            else
            {
                IBilliard billiard = BilliardManager.Instance.GetBilliard(packetImpl.BilliardId);
                billiard.SetRigidbodyEnable(packetImpl.PhysicsIsOpen);
                ((MonoBehaviour)billiard).gameObject.SetActive(packetImpl.Active);
            }
        }
    }
}