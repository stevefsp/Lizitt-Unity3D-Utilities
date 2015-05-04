using UnityEditor;
using UnityEngine;

namespace com.lizitt.u3d.editor
{
    [InitializeOnLoad]
    public static class AnnotationEditorUtil
    {
        private const string DrawPref = "com.lizitt.u3d.DrawAnnotations";

        static AnnotationEditorUtil()
        {
            AnnotationUtil.drawAlways = EditorPrefs.GetBool(DrawPref);
        }

        [MenuItem(EditorUtil.ViewMenu + "Toggle Annotations", false, EditorUtil.ViewGroup)]
        public static void ToggleDrawAll()
        {
            AnnotationUtil.drawAlways = !AnnotationUtil.drawAlways;
            EditorPrefs.SetBool(DrawPref, AnnotationUtil.drawAlways);

            //var items = (AnnotationMarker[])GameObject.FindObjectsOfType(typeof(AnnotationMarker));

            //foreach (var item in items)
            //{
            //    item.SetDisplay(AnnotationUtil.drawAlways);
            //}

            //SceneView.RepaintAll();
        }
    }
}