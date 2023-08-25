using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace SuperBilliard
{
    [System.Serializable]
    public class PlayerImageToggle
    {
        public EnumSpriteAtlas PlayerImageType;
        public SelectToggle SelectBtn;
    }

    //这里的逻辑写的不行,应该重新写一个表去读表进而来配置
    public class PlayerImageCostomUIForm : UILogicBase
    {
        [SerializeField] private Image _body;
        [SerializeField] private Image _kit;
        [SerializeField] private Image _face;
        [SerializeField] private Image _hair;
        [SerializeField] private InputField _nickNameInput;

        [SerializeField] private PlayerImageScrollView _playerImageScrollView;
        [SerializeField] private PlayerImageToggle[] _playerImageToggles;

        [SerializeField] private Button _backMainMenuBtn;

        [SerializeField] private Button _saveBtn;
        private PlayerImageData _playerImage;
        private PlayerDataBundle _playerData;

        private EnumSpriteAtlas _curPlayerImage;
        private readonly Dictionary<EnumSpriteAtlas, Sprite[]> _dic = new Dictionary<EnumSpriteAtlas, Sprite[]>();

        //缓存玩家形象
        private int _hairId;
        private int _faceId;
        private int _bodyId;
        private int _kitId;
        private string _nickName;
        private bool _saved;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            _playerData = GameEntry.DataBundle.GetData<PlayerDataBundle>();
            _playerImage = _playerData.PlayerImage;
            LoadSprite(_playerImageToggles);
            InitSelectBtn();
            _backMainMenuBtn.onClick.AddListener(() => { Close(); });
            _nickNameInput.onEndEdit.AddListener((str) =>
            {
                _nickName = str;
            });
            _saveBtn.onClick.AddListener(SavePlayerImage);
        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(ClickPlayerImageSlotEventArgs.EventId, OnClickPlayerImage);

            _curPlayerImage = _playerImageToggles[0].PlayerImageType;
            _playerImageToggles[0].SelectBtn.IsSelecting = true;
            _playerImageScrollView.Initlize(_dic[_playerImageToggles[0].PlayerImageType]);

            for (int i = 1; i < _playerImageToggles.Length; i++)
            {
                _playerImageToggles[i].SelectBtn.IsSelecting = false;
            }
            //更新玩家形象
            _hair.sprite = GameEntry.ResourceCache.GetHairSprite(_playerImage.HairId);
            _body.sprite = GameEntry.ResourceCache.GetBodySprite(_playerImage.BodyId);
            _face.sprite = GameEntry.ResourceCache.GetFaceSprite(_playerImage.FacaId);
            _kit.sprite = GameEntry.ResourceCache.GetKitSprite(_playerImage.KitId);
            _nickNameInput.text = _playerData.NickName;
            //玩家备份数据
            _hairId = _playerImage.HairId;
            _faceId = _playerImage.FacaId;
            _bodyId = _playerImage.BodyId;
            _kitId = _playerImage.KitId;
            _nickName = _playerData.NickName;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(ClickPlayerImageSlotEventArgs.EventId, OnClickPlayerImage);
        }

        private void InitSelectBtn()
        {
            //设置Button事件
            for (int i = 0; i < _playerImageToggles.Length; i++)
            {
                int index = i;
                PlayerImageToggle playerImageToggle = _playerImageToggles[index];
                SelectToggle selectToggle = _playerImageToggles[index].SelectBtn;
                selectToggle.onClick.AddListener(() =>
                {
                    if (selectToggle.IsSelecting == false)
                    {
                        selectToggle.IsSelecting = true;
                        _curPlayerImage = playerImageToggle.PlayerImageType;
                        _playerImageScrollView.Initlize(_dic[playerImageToggle.PlayerImageType]);
                    }
                    //关闭一下
                    for (int j = 0; j < _playerImageToggles.Length; j++)
                    {
                        if (j != index)
                        {
                            _playerImageToggles[j].SelectBtn.IsSelecting = false;
                        }
                    }
                });
            }
        }

        private void LoadSprite(PlayerImageToggle[] playerImageToggles)
        {
            SpriteAtlasDataBundle dataBundle = GameEntry.DataBundle.GetData<SpriteAtlasDataBundle>();
            for (int i = 0; i < playerImageToggles.Length; i++)
            {
                if (_dic.ContainsKey(playerImageToggles[i].PlayerImageType) == true)
                {
                    Log.Warning("重复加载角色形象");
                    continue;
                }

                SpriteAtlas spriteAtlas = GameEntry.ResourceCache.SpriteAtlasLoader.GetAsset(dataBundle[playerImageToggles[i].PlayerImageType].Path);
                if (spriteAtlas == null)
                {
                    Log.Error("加载角色形象失败,没有对应的{0}SpriteAtlas", playerImageToggles[i].PlayerImageType.ToString());
                    continue;
                }
                Sprite[] sprites = new Sprite[spriteAtlas.spriteCount];
                spriteAtlas.GetSprites(sprites);
                _dic.Add(playerImageToggles[i].PlayerImageType, sprites);
            }
        }

        private void OnClickPlayerImage(object sender, GameEventArgs e)
        {
            ClickPlayerImageSlotEventArgs args = (ClickPlayerImageSlotEventArgs)e;

            switch (_curPlayerImage)
            {
                case EnumSpriteAtlas.BodyAtlas:
                    _bodyId = args.PlayerImageID;
                    _body.sprite = args.PlayerImageSprite;
                    break;
                case EnumSpriteAtlas.FaceAtlas:
                    _faceId = args.PlayerImageID;
                    _face.sprite = args.PlayerImageSprite;
                    break;
                case EnumSpriteAtlas.HairAtlas:
                    _hairId = args.PlayerImageID;
                    _hair.sprite = args.PlayerImageSprite;
                    break;
                case EnumSpriteAtlas.KitAtlas:
                    _kitId = args.PlayerImageID;
                    _kit.sprite = args.PlayerImageSprite;
                    break;
                default:
                    _saved = true;
                    Log.Error("是不是混入了其他类型的图片？{0}", _curPlayerImage);
                    break;
            }
        }

        private void SavePlayerImage()
        {
            _playerImage.HairId = _hairId;
            _playerImage.FacaId = _faceId;
            _playerImage.BodyId = _bodyId;
            _playerImage.KitId = _kitId;
            _playerData.NickName = _nickName;
            //保存到服务器
            _playerData.SavePlayerMessage();
        }
    }
}