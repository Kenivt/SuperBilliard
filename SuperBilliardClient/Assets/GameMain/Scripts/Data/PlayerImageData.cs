using UnityEngine;

namespace SuperBilliard
{
    public class PlayerImageData
    {
        public int BodyId
        {
            get
            {
                return _bodyId;
            }
            set
            {
                if (_bodyId != value)
                {
                    _bodyId = value;
                }
            }
        }

        public int HairId
        {
            get
            {
                return _hairId;
            }
            set
            {
                if (_hairId != value)
                {
                    _hairId = value;
                }
            }
        }

        public int FacaId
        {
            get
            {
                return _faceId;
            }
            set
            {
                if (_faceId != value)
                {
                    _faceId = value;
                }
            }
        }

        public int KitId
        {
            get
            {
                return _kitId;
            }
            set
            {
                if (_kitId != value)
                {
                    _kitId = value;
                }
            }
        }

        private int _bodyId;
        private int _hairId;
        private int _faceId;
        private int _kitId;

        public PlayerImageData()
        {
            _bodyId = 1;
            _faceId = 1;
            _hairId = 1;
            _kitId = 1;
        }
    }
}