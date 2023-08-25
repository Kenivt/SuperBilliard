using UnityEngine;

namespace Knivt.Tools.Layer
{
    public static class LayerTools
    {
        public static bool IsLayer(int layer, LayerMask layerMask)
        {
            int value = (1 << layer);
            return (layerMask.value & value) == value;
        }
    }
}