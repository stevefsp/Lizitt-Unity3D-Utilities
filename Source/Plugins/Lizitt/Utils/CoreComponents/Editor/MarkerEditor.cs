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

namespace com.lizitt.u3d.editor
{
    /// <summary>
    /// Providers marker features for the editor.
    /// </summary>
    public class MarkerEditor
    {
#if UNITY_5_0_0 || UNITY_5_0_1
        [DrawGizmo(GizmoType.NotSelected | GizmoType.SelectedOrChild | GizmoType.Pickable)]
#else
        [DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.InSelectionHierarchy | GizmoType.Pickable)]        
#endif
        private static void DrawGizmo(Marker marker, GizmoType type)
        {
            /*
             * Design note:
             * 
             * It is unusally preferred to place all gizmo code in an editor class.  
             * But Unity behavior is to call the gizmo method for all classes in 
             * an objects heirarchy.  E.g.  NavigationMarker is a sub-class of Marker.  If
             * both have DrawGizmo methods, then both methods will be called.  So each concrete 
             * class needs to know how to draw itself with only this one draw gizmo method
             * defined.
             * 
             * Unity Bug (Pre-5.0.2): The content of 'type' is broken.  The 'type' test below 
             * isn't currently functional. 
             */

#if UNITY_5_0_0 || UNITY_5_0_1
            if (AnnotationUtil.drawAlways || (type & GizmoType.SelectedOrChild) != 0)
#else
            if (AnnotationUtil.drawAlways || (type & GizmoType.InSelectionHierarchy) != 0)
#endif
                marker.DrawGizmo();
        }
    }
}
