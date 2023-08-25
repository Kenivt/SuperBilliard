using UnityEngine;
using UnityEngine.U2D;
using GameFramework.Fsm;
using GameFramework.Event;
using SuperBilliard.Constant;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public class ProcedureLoadMessage : ProcedureBase
    {
        public override bool UseNativeDialog => true;

        private PlayerDataBundle _playerDataBunlde = null;
        private SpriteAtlasDataBundle _spriteBundle = null;

        private FriendDataBundle _firendDataBundle = null;
        private readonly Dictionary<string, bool> _loadFlag = new Dictionary<string, bool>();

        private MyCoroutine _myCoroutine = new MyCoroutine();
        private bool _loadDone = false;

        protected override void OnEnter(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _loadDone = false;
            GameEntry.Event.Subscribe(UpdateFriendListEventArgs.EventId, OnUpdateFriendList);
            GameEntry.Event.Subscribe(UpdateFriendRequestListEventArgs.EventId, OnUpdateFriendRequest);
            GameEntry.Event.Subscribe(RecievePlayerMessageEventArgs.EventId, RecievePlayerMessageCallback);
            _myCoroutine.StartCoroutine(LoadMessage());
        }

        private void OnUpdateFriendRequest(object sender, GameEventArgs e)
        {
            Log.Info("更新好友请求列表");
            _loadFlag["UpdateFriendRequestList"] = true;
        }

        private void OnUpdateFriendList(object sender, GameEventArgs e)
        {
            Log.Info("更新好友列表");
            _loadFlag["UpdateFriendList"] = true;
        }

        protected override void OnUpdate(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            _myCoroutine.Update(elapseSeconds, realElapseSeconds);

            if (_loadDone == false)
            {
                return;
            }
            procedureOwner.SetData<VarInt32>(KeyNextSceneId, (int)SceneId.MainMenu);
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        private bool CheckLoadFlag()
        {
            foreach (var item in _loadFlag)
            {
                if (item.Value == false)
                {
                    return false;
                }
            }
            return true;
        }

        private IEnumerator<IMyCoroutineItem> LoadMessage()
        {
            //清空字典
            _loadFlag.Clear();

            _spriteBundle = GameEntry.DataBundle.GetData<SpriteAtlasDataBundle>();
            LoadSprite(_spriteBundle[EnumSpriteAtlas.SnokkerAtlas].Path
                , _spriteBundle[EnumSpriteAtlas.FaceAtlas].Path,
                _spriteBundle[EnumSpriteAtlas.BodyAtlas].Path,
                _spriteBundle[EnumSpriteAtlas.HairAtlas].Path,
                _spriteBundle[EnumSpriteAtlas.KitAtlas].Path,
                _spriteBundle[EnumSpriteAtlas.BilliardAtlas].Path);

            //加载材质
            BilliardDataBundle billiardDataBundle = GameEntry.DataBundle.GetData<BilliardDataBundle>();
            List<BilliardDataItem> billiardDataItems = new List<BilliardDataItem>();
            billiardDataBundle.GetAllBilliardData(billiardDataItems);
            string? lastPath = null;
            for (int i = 0; i < billiardDataItems.Count; i++)
            {
                if (lastPath != billiardDataItems[i].MaterialPath)
                {
                    lastPath = billiardDataItems[i].MaterialPath;
                    LoadMaterial(lastPath);
                }
            }

            //加载玩家信息
            _playerDataBunlde = GameEntry.DataBundle.GetData<PlayerDataBundle>();

            if (string.IsNullOrEmpty(_playerDataBunlde.UserName) == false)
            {
                GameEntry.Client.SendGetPlayerMessageRequest(_playerDataBunlde.UserName);
                _loadFlag.Add(_playerDataBunlde.UserName, false);
            }
            else
            {
                Log.Error("玩家的用户名为空....");
            }

            yield return new MyCoroutineWaitUtill(CheckLoadFlag);

            _loadFlag.Clear();
            //加载好友信息,因为好友信息依赖于Sprite,所以放在后面加载
            _firendDataBundle = GameEntry.DataBundle.GetData<FriendDataBundle>();
            _firendDataBundle.UpdateFriendData();
            _firendDataBundle.UpdateFriendRequestData();
            _loadFlag.Add("UpdateFriendList", false);
            _loadFlag.Add("UpdateFriendRequestList", false);

            yield return new MyCoroutineWaitUtill(CheckLoadFlag);
            _loadDone = true;
        }


        protected override void OnLeave(IFsm<GameFramework.Procedure.IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(UpdateFriendListEventArgs.EventId, OnUpdateFriendList);
            GameEntry.Event.Unsubscribe(UpdateFriendRequestListEventArgs.EventId, OnUpdateFriendRequest);
            GameEntry.Event.Unsubscribe(RecievePlayerMessageEventArgs.EventId, RecievePlayerMessageCallback);
        }

        private void LoadSpriteCallBack(string assetName, object asset, LoadResourceStatus status)
        {
            if (status == LoadResourceStatus.Success)
            {
                _loadFlag[assetName] = true;
                Log.Info("加载Sprite资源成功,名称为:{0}", assetName);
            }
            else
            {
                Log.Error("加载Sprite资源错误,名称为:{0},加载状态{1}", assetName, status.ToString());
            }
        }

        public void LoadSprite(params string[] paths)
        {
            AssetLoader<SpriteAtlas> loader = GameEntry.ResourceCache.SpriteAtlasLoader;
            foreach (var item in paths)
            {
                if (loader.HasAsset(item) == false)
                {
                    _loadFlag.Add(item, false);
                    loader.Load(item, LoadSpriteCallBack);
                }
            }
        }

        public void LoadMaterial(string paths)
        {
            AssetLoader<Material> loader = GameEntry.ResourceCache.BilliardMaterialLoader;
            if (loader.HasAsset(paths) == false)
            {
                loader.Load(paths, LoadMaterialCallBack);
            }
            else
            {
                Log.Warning("已经加载过该材质了,名称为:{0}", paths);
            }
        }

        private void LoadMaterialCallBack(string assetName, object asset, LoadResourceStatus status)
        {
            if (status == LoadResourceStatus.Success)
            {
                _loadFlag[assetName] = true;
                Log.Info("加载Material资源成功,名称为:{0}", assetName);
            }
            else
            {
                Log.Error("加载Material资源错误,名称为:{0},加载状态{1}", assetName, status.ToString());
            }
        }

        private void RecievePlayerMessageCallback(object sender, GameEventArgs e)
        {
            RecievePlayerMessageEventArgs args = (RecievePlayerMessageEventArgs)e;

            _loadFlag[_playerDataBunlde.UserName] = true;

            if (args.UserName != _playerDataBunlde.UserName)
            {
                Log.Error("收到的玩家信息的用户名与本地的玩家信息的用户名不一致...");
                return;
            }

            _playerDataBunlde.NickName = args.NickName;
            _playerDataBunlde.Level = args.Level;
            _playerDataBunlde.Description = args.Description;
            _playerDataBunlde.PlayerImage.BodyId = args.BodyID;
            _playerDataBunlde.PlayerImage.KitId = args.KitID;
            _playerDataBunlde.PlayerImage.HairId = args.HairID;
            _playerDataBunlde.PlayerImage.FacaId = args.FaceID;

            Log.Info("收到玩家信息...");
        }
    }
}