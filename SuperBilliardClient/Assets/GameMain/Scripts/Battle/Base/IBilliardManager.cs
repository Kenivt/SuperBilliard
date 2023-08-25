using UnityEngine;
using System.Collections.Generic;

namespace SuperBilliard
{
    public interface IBilliardManager
    {
        float Deceleration { get; set; }

        IBilliard WhiteBilliard { get; }

        void RigisterBilliard(IBilliard billiard);
        void UnrigisterAllBilliard();
        bool AllUsingBilliardStop();
        void UsingBilliardReset();
        void AddUsingBilliard(IBilliard billiard);
        bool RemoveUsingBilliard(IBilliard billiard);

        void RemoveAllUsingBilliard();

        BilliardMessage[] GetAllUsingBilliardMessage();
        List<BilliardMessage> GetAllRollingBilliardMessage();

        void SyncUsingBilliard(int ballId, Vector3 position, Vector3 rotation);
    }
}