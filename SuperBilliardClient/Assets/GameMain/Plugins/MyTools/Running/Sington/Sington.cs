using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knivt.Tools
{
    public class Sington<T> : MonoBehaviour where T : Sington<T>
    {
        public static T Instance
        {
            get
            {
                return _instance;
            }
        }
        private static T _instance;
        protected virtual void Awake()
        {
            _instance = this as T;
        }
    }
}
