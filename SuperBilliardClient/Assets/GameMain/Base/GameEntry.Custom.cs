//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static ClientComponent Client;
        public static InputComponent Input;
        public static PhysicSimulateComponent PhysicSimulate;
        public static DataComponent DataBundle;
        public static ResourceCacheComponent ResourceCache;

        private static void InitCustomComponents()
        {
            Client = UnityGameFramework.Runtime.GameEntry.GetComponent<ClientComponent>();
            Input = UnityGameFramework.Runtime.GameEntry.GetComponent<InputComponent>();
            PhysicSimulate = UnityGameFramework.Runtime.GameEntry.GetComponent<PhysicSimulateComponent>();
            DataBundle = UnityGameFramework.Runtime.GameEntry.GetComponent<DataComponent>();
            ResourceCache = UnityGameFramework.Runtime.GameEntry.GetComponent<ResourceCacheComponent>();
        }
    }
}
