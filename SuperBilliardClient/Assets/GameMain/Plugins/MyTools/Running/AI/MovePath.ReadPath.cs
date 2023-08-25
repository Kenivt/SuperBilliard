using UnityEngine;
using System.Collections.Generic;

namespace Knivt.Tools.AI
{
    public partial class MovePath : MonoBehaviour
    {
        private TargetPostionNodeReader _nodeReader;
        public TargetPositionNode GetNextTargetPosition()
        {
            if (_nodeReader.MoveNext())
            {
                return CurTargetPosition;
            }
            return null;
        }
        public TargetPositionNode CurTargetPosition
        {
            get
            {
                if (!_nodeReader.OnIndexRange)
                {
                    Debug.LogError("错误,索引越界了...");
                    return null;
                }
                return _nodeReader.Current;
            }
        }

        public void ResetTargetReader()
        {
            _nodeReader?.Reset();
        }

        internal class TargetPostionNodeReader
        {
            public TargetPositionNode Current => _movePaths[_index];
            public CycleOptions CycleOptions => _cycleOptions;
            public bool OnIndexRange => _index >= 0 && _index < _movePaths.Count;

            private CycleOptions _cycleOptions;
            private List<TargetPositionNode> _movePaths;
            private bool _isForward = true;
            private int _index = -1;
            public TargetPostionNodeReader(List<TargetPositionNode> paths, CycleOptions cycleOptions)
            {
                _movePaths = paths;
                _cycleOptions = cycleOptions;
            }
            public bool MoveNext()
            {
                if (_cycleOptions == CycleOptions.Loop)
                {
                    _index++;
                    _index %= _movePaths.Count;
                }
                else if (_cycleOptions == CycleOptions.BackAndForth)
                {
                    if (_isForward)
                    {
                        _index++;
                        //判断是最后一个节点
                        if (_index == _movePaths.Count)
                        {
                            _isForward = false;
                            _index = _index - 2;
                            if (_index < 0) _index = 0;
                        }
                    }
                    else
                    {
                        _index--;
                        if (_index == -1)
                        {
                            _isForward = true;
                            _index = Mathf.Min(1, _movePaths.Count - 1);
                        }
                    }
                }
                else if (_cycleOptions == CycleOptions.OnlyOnce)
                {
                    _index++;
                }
                return _index < _movePaths.Count;
            }
            public void Reset()
            {
                _index = -1;
                _isForward = true;
            }
        }
    }
}