using UnityEngine;
using System.Collections.Generic;



namespace Knivt.Tools
{
    public static class TransformShortCut
    {
        /// <summary>
        /// 寻找对应名称的transform,
        /// 要子代没有重名的
        /// </summary>
        /// <param name="transform">根treansform</param>
        /// <param name="tfName">物体的名称</param>
        /// <returns></returns>
        public static Transform FindOffspringBFS(this Transform transform, string tfName)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(transform);
            //广度优先搜索
            while (queue.Count > 0)
            {
                Transform curTf = queue.Dequeue();
                for (int i = 0; i < curTf.childCount; i++)
                {
                    if (curTf.GetChild(i).name == tfName)
                    {
                        return curTf.GetChild(i);
                    }
                    else
                    {
                        queue.Enqueue(curTf.GetChild(i));
                    }
                }
            }
            return null;
        }
        public static Transform FindOffspringDFS(this Transform transform, string tfName)
        {
            if (transform.name == tfName)
            {
                return transform;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform result = FindOffspringDFS(transform.GetChild(i), tfName);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
        public static T GetComponentFromOffspring<T>(this Transform transform, string tfName) where T : Component
        {
            Transform target = transform.FindOffspringDFS(tfName);
            if (target != null)
            {
                return target.GetComponent<T>();
            }
            else
            {
                Debug.LogError($"没有找到对应{tfName}的物体");
            }
            return null;
        }
    }
}

