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
    public abstract class TrsInterpolatorEditor
        : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (serializedObject.isEditingMultipleObjects)
                return;

            if (!Application.isPlaying)
                return;

            EditorGUILayout.Separator();

            var targ = target as TrsInterpolator;

            EditorGUILayout.Separator();

            if (GUILayout.Button("Reset"))
                targ.Reset();

            switch (targ.Status)
            {
                case InterpolationStatus.Inactive:
                case InterpolationStatus.Paused:
                case InterpolationStatus.Complete:

                    if (GUILayout.Button("Play"))
                        targ.Play();

                    break;

                case InterpolationStatus.Playing:

                    if (GUILayout.Button("Pause"))
                        targ.Pause();

                    break;
            }
        }
    }

    [CustomEditor(typeof(SmoothFollowPosition))]
    public class SmoothFollowPositionEditor
        : TrsInterpolatorEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.HelpBox("This interpolator never completes on its own.", MessageType.Info);
        }
    }

    [CustomEditor(typeof(SmoothFollowRotation))]
    public class SmoothFollowRotationEditor
        : TrsInterpolatorEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.HelpBox("This interpolator never completes on its own.", MessageType.Info);
        }
    }

    [CustomEditor(typeof(SmoothLookAt))]
    public class SmoothLookAtEditor
        : TrsInterpolatorEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.HelpBox("This interpolator never completes on its own.", MessageType.Info);
        }
    }

    [CustomEditor(typeof(SmoothStepMoveTo))]
    public class SmoothStepMoveToEditor
        : TrsInterpolatorEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var message = "This interpolator is guarenteed to reach its match position within"
                + " the specified duration.";

            var targ = target as SmoothStepMoveTo;

            if (targ.CompletionAction == EaseCompletionType.ContinueUpdating)
            {
                message += "\n\nWill continue tracking after completion.";
            }
            else if (targ.CompletionAction == EaseCompletionType.StopUpdating)
            {
                message += "\n\nWill stop tracking once at the match position.";
            }

            EditorGUILayout.HelpBox(message, MessageType.Info);
        }
    }

    public abstract class TrsEaseEditor
        : TrsInterpolatorEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (serializedObject.isEditingMultipleObjects)
                return;

            var message = "This interpolator is guarenteed to complete within the specified duration.";

            var targ = target as TrsEase;

            if (!targ.MatchIsDynamic)
            {
                message += "\n\nThe match target is fixed once interpolation starts. Changes to the"
                    + " match target will be ignored.";
            }
            else if (targ.CompletionAction == EaseCompletionType.ContinueUpdating)
            {
                message += "\n\nWill continue tracking match target after completion.";
            }

            EditorGUILayout.HelpBox(message, MessageType.Info);

            if (targ.MatchIsDynamic)
            {
                var warn = "Dynamic target tracking is more expensive.  Other interpolators are"
                    + " more efficient.";

                if (!targ.MatchItem)
                {
                    warn += "\n\nDynamic tracking will only have an"
                        + " effect if the match offset is being manually updated.";
                }

                if (targ.CompletionAction == EaseCompletionType.StopUpdating)
                {
                    warn += "\n\nInterpolation will stop as soon at the match target is reached,"
                        + " even if the match target is still moving.";
                }

                EditorGUILayout.HelpBox(warn, MessageType.Warning);
            }
            else if (targ.CompletionAction == EaseCompletionType.ContinueUpdating)
            {
                EditorGUILayout.HelpBox("Continuing to update after completion will have no effect.",
                    MessageType.Warning);
            }
        }
    }

    [CustomEditor(typeof(EaseMoveTo))]
    [CanEditMultipleObjects]
    public class EaseMoveToEditor
        : TrsEaseEditor
    {
    }

    [CustomEditor(typeof(EaseRotateTo))]
    [CanEditMultipleObjects]
    public class EaseRotateToEditor
        : TrsEaseEditor
    {
    }
}
