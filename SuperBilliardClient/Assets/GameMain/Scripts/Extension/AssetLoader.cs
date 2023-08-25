using System;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public delegate void AssetLoaderHandler(string assetName, object asset, LoadResourceStatus status);

    public class AssetLoader<T> where T : UnityEngine.Object
    {
        private readonly Dictionary<string, T> _assetDic = new Dictionary<string, T>();
        private readonly Dictionary<string, AssetLoaderHandler> _loadingFlags = new Dictionary<string, AssetLoaderHandler>();
        private readonly LoadAssetCallbacks _loadAssetCallback;
        private Action<AssetLoader<T>> _loadCompleteCallback;

        public bool LoadFinish => _loadingFlags.Count == 0;

        public AssetLoader(Action<AssetLoader<T>> loadCompleteCallback = null)
        {
            _loadAssetCallback = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetFailureCallBack);
            _loadCompleteCallback = loadCompleteCallback;
        }

        public bool HasAsset(string assetName)
        {
            return _assetDic.ContainsKey(assetName);
        }

        public bool AssetLoading(string assetName)
        {
            return _loadingFlags.ContainsKey(assetName);
        }

        public bool AddLoadedCallback(string assetName, AssetLoaderHandler loadCompleteCallback)
        {
            if (_loadingFlags.ContainsKey(assetName) == true)
            {
                _loadingFlags[assetName] += loadCompleteCallback;
                return true;
            }
            return false;
        }

        public bool TryGetAsset(string assetName, out T value)
        {
            value = null;
            if (_assetDic.ContainsKey(assetName))
            {
                value = _assetDic[assetName];
            }
            return value != null;
        }

        public T GetAsset(string assetName)
        {
            if (_assetDic.ContainsKey(assetName))
            {
                return _assetDic[assetName];
            }
            return null;
        }

        public void Load(string assetPath, AssetLoaderHandler assetLoaderHandler)
        {
            if (_assetDic.ContainsKey(assetPath))
            {
                Log.Info("已经加载了对应{0}的Asset.", assetPath);
                return;
            }
            if (_loadingFlags.ContainsKey(assetPath))
            {
                _loadingFlags[assetPath] += assetLoaderHandler;
                Log.Info("该名称为：{0}的Asset.正在加载中.", assetPath);
                return;
            }
            _loadingFlags.Add(assetPath, assetLoaderHandler);
            GameEntry.Resource.LoadAsset(assetPath, _loadAssetCallback);
        }

        public void Unload(string assetName)
        {
            if (_assetDic.ContainsKey(assetName))
            {
                _assetDic.Remove(assetName);
                GameEntry.Resource.UnloadAsset(assetName);
            }
        }

        public void UnloadAll()
        {
            foreach (var item in _assetDic)
            {
                GameEntry.Resource.UnloadAsset(item);
            }
            _assetDic.Clear();
        }

        private void LoadAssetSuccessCallback(string assetName, object asset, float duration, object userData)
        {
            if (_assetDic.ContainsKey(assetName) == false)
            {
                _loadingFlags[assetName]?.Invoke(assetName, asset, LoadResourceStatus.Success);
                _loadingFlags.Remove(assetName);
                _assetDic.Add(assetName, asset as T);
                if (_loadingFlags.Count == 0)
                {
                    _loadCompleteCallback?.Invoke(this);
                }
            }
            else
            {
                Log.Error("Repeate load same asset.");
            }
        }

        private void LoadAssetFailureCallBack(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            _loadingFlags[assetName].Invoke(null, null, status);
            _loadingFlags.Remove(assetName);
            Log.Error("Load asset failure. assetName:{0}, status:{1}, errorMessage:{2}", assetName, status, errorMessage);
        }
    }
}