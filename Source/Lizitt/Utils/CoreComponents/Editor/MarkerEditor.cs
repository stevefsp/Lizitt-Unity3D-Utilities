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
    public class MarkerEditor
    {
        [DrawGizmo(GizmoType.NotSelected | GizmoType.SelectedOrChild | GizmoType.Pickable)]
        private static void DrawGizmo(Marker marker, GizmoType type)
        {
            /*
             * Design note:
             * 
             * It is unusally preferred to place all gizmo code in an editor class.  But the current
             * Unity behavior is that this method is called on all classes that have
             * Marker as their base.  So both Marker and NavigationMarker result in a
             * call to this method.  That is why the drawing code is located in the marker classes
             * rather than an editor class.
             * 
             * Unity Bug: The content of 'type' is broken in Unity 5.0+ (bug submitted and 
             * accepted). So the 'type' test below isn't currently functional.  Until the bug
             * is fixed, the gizmo will only display when the marker is drectly selelected or 
             * drawAlways is true.
             */

            if (AnnotationUtil.drawAlways || (type & GizmoType.SelectedOrChild) != 0)
                marker.DrawGizmo();
        }
    }
}
