using UnityEngine;
using UnityEditor;
using Knivt.Tools.AI;

namespace Knivt.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(MovePath), true), InitializeOnLoad]
    public class MovePathEditor : Editor
    {
        private void OnSceneGUI()
        {
            Handles.color = Color.green;
            MovePath t = (target as MovePath);
            if (t.NeedResetOrignalPosition)
            {
                return;
            }
            for (int i = 0; i < t.MovePathNodeList.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 oldPoint = t.OriginalTransformPosition + t.MovePathNodeList[i].PathNodePosition;

                GUIStyle style = new GUIStyle();

                style.normal.textColor = Color.yellow;
                Handles.Label(t.OriginalTransformPosition + t.MovePathNodeList[i].PathNodePosition + 0.4f * new Vector3(1, -1, 0), i.ToString(), style);

                Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, .5f, new Vector3(0.25f, 0.25f, 0.25f), Handles.CircleHandleCap);
                newPoint = ApplyAxisLock(oldPoint, newPoint);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Free Move Handle");
                    t.MovePathNodeList[i].PathNodePosition = newPoint - t.OriginalTransformPosition;
                }
            }
        }
        protected virtual Vector3 ApplyAxisLock(Vector3 oldPoint, Vector3 newPoint)
        {
            MovePath t = (target as MovePath);
            if (t.LockHandlesOnXAxis)
            {
                newPoint.x = oldPoint.x;
            }
            if (t.LockHandlesOnYAxis)
            {
                newPoint.y = oldPoint.y;
            }
            if (t.LockHandlesOnZAxis)
            {
                newPoint.z = oldPoint.z;
            }
            return newPoint;
        }
    }
}