using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

#endif
namespace SuperBilliard
{

    [RequireComponent(typeof(PolygonCollider2D))]
    public class NonRectangularButtonImage : Image
    {
        private PolygonCollider2D areaPolygon;
        protected override void Start()
        {
            base.Start();
        }
        protected NonRectangularButtonImage()
        {
            useLegacyMeshGeneration = true;
        }

        private PolygonCollider2D Polygon
        {
            get
            {
                if (areaPolygon != null)
                    return areaPolygon;

                areaPolygon = GetComponent<PolygonCollider2D>();
                return areaPolygon;
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPoint);
            Vector2 worldPoint = rectTransform.TransformPoint(localPoint);
            bool flag = Polygon.OverlapPoint(worldPoint);
            return flag;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            transform.localPosition = Vector3.zero;
            var w = rectTransform.sizeDelta.x * 0.5f + 0.1f;
            var h = rectTransform.sizeDelta.y * 0.5f + 0.1f;
            Polygon.points = new[]
            {
            new Vector2(-w, -h),
            new Vector2(w, -h),
            new Vector2(w, h),
            new Vector2(-w, h)
        };
        }
#endif
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(NonRectangularButtonImage), true)]
    public class CustomRaycastFilterInspector : Editor
    {
        public override void OnInspectorGUI()
        {
        }
    }

    public class NonRectAngularButtonImageHelper
    {
        [MenuItem("GameObject/UI/Custom/NonRectangularButtonImage")]
        public static void CreateNonRectAngularButtonImage()
        {
            var goRoot = Selection.activeGameObject;
            if (goRoot == null)
                return;

            var button = goRoot.GetComponent<Button>();

            if (button == null)
            {
                Debug.Log("Selecting Object is not a button!");
                return;
            }

            // 关闭原来button的射线检测
            var graphics = goRoot.GetComponentsInChildren<Graphic>();
            foreach (var graphic in graphics)
            {
                graphic.raycastTarget = false;
            }

            var polygon = new GameObject("NonRectangularButtonImage");
            polygon.AddComponent<PolygonCollider2D>();
            polygon.AddComponent<NonRectangularButtonImage>();
            polygon.transform.SetParent(goRoot.transform, false);
            polygon.transform.SetAsLastSibling();
        }
    }

#endif
}