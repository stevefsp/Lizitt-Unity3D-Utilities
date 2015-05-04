using UnityEditor;
using UnityEngine;

namespace com.lizitt.u3d.editor
{
    public static class SimplePlaneAnnotationEditor
    {
        private static readonly Vector3 MarkerSize = new Vector3(1, 0.01f, 1);

        [DrawGizmo(GizmoType.NotSelected | GizmoType.SelectedOrChild)]
        static void DrawGizmo(SimplePlaneAnnotation item, GizmoType type)
        {
            if (!(AnnotationUtil.drawAlways || (type & GizmoType.SelectedOrChild) != 0))
                return;

            Gizmos.color = item.Color;
            Gizmos.matrix = item.transform.localToWorldMatrix;
            Gizmos.DrawCube(item.PositionOffset, MarkerSize);
        }
    }
}
