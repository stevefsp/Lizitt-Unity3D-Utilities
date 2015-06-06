/*
 * Copyright (c) 2015 Stephen A. Pratt
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using UnityEditor;
using UnityEngine;

namespace com.lizitt.u3d.editor
{
    public static class SimplePlaneAnnotationEditor
    {
        private static readonly Vector3 MarkerSize = new Vector3(1, 0.01f, 1);

#if UNITY_5_0_0 || UNITY_5_0_1
        [DrawGizmo(GizmoType.NotSelected | GizmoType.SelectedOrChild)]
#else
        [DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.InSelectionHierarchy)]
#endif
        static void DrawGizmo(SimplePlaneAnnotation item, GizmoType type)
        {
#if UNITY_5_0_0 || UNITY_5_0_1
            if (AnnotationUtil.drawAlways || (type & GizmoType.SelectedOrChild) != 0)
#else
            if (!(AnnotationUtil.drawAlways || (type & GizmoType.InSelectionHierarchy) != 0))
#endif
                return;

            Gizmos.color = item.Color;
            Gizmos.matrix = item.transform.localToWorldMatrix;
            Gizmos.DrawCube(item.PositionOffset, MarkerSize);
        }
    }
}
