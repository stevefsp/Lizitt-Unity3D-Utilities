using UnityEditor;
using UnityEngine;
using com.lizitt.u3d;
using System.Collections.Generic;

namespace com.lizitt.u3d.editor
{
    [CustomPropertyDrawer(typeof(RendererMaterialPtrAttribute))]
    public class RendererMaterialPtrDrawer
        : PropertyDrawer
    {
        // Can't support multi-object editing because renderer values can't be the same
        // across multiple objects.

        // TODO: Merge common methods with MaterialOverrideGroupDrawer.  A lot of code is
        // the same.  Specifically PrototInfo and the GUI drawing methods.

        private const string RendererPropName = "m_Renderer";
        private const string IndexPropName = "m_MaterialIndex";

        private static readonly GUIContent RendererLabel = new GUIContent(
            "Renderer", "The renderer that contains the material.");

        private static readonly GUIContent TargetLabel = new GUIContent(
            "Material", "The material.");

        private struct Props
        {
            public SerializedProperty renderer;
            public SerializedProperty index;

            public Props(SerializedProperty property)
            {
                renderer = property.FindPropertyRelative(RendererPropName);
                index = property.FindPropertyRelative(IndexPropName);
            }
        }

        private class ProtoInfo
        {
            public GameObject prototype = null;

            public readonly List<Renderer> renderers = new List<Renderer>(5);
            public GUIContent[] rendererLabels;

            public readonly List<Material[]> materials = new List<Material[]>(5);
            public readonly List<GUIContent[]> materialLabels = new List<GUIContent[]>(5);

            public void Refresh(GameObject proto)
            {
                // This process is desinged to support both scene objects and prefab assets.
                // (GetComponent() and related methods don't work with assets.)

                this.prototype = proto;

                renderers.Clear();
                materials.Clear();
                materialLabels.Clear();
                var sl = new List<GUIContent>(5);

                renderers.Add(null);
                materials.Add(null);
                materialLabels.Add(null);

                sl.Add(new GUIContent("Select Renderer..."));

                for (int i = 0; i < proto.transform.childCount; i++)
                {
                    var r = proto.transform.GetChild(i).GetComponent<Renderer>();

                    if (!r)
                        continue;

                    renderers.Add(r);
                    sl.Add(new GUIContent(r.name));

                    var mats = r.sharedMaterials;

                    var labels = new GUIContent[mats.Length];

                    for (int j = 0; j < mats.Length; j++)
                    {
                        labels[j] = new GUIContent(mats[j].name);
                    }

                    materials.Add(mats);
                    materialLabels.Add(labels);
                }

                if (renderers.Count == 1)
                {
                    renderers.Clear();
                    materials.Clear();
                    materialLabels.Clear();
                    sl.Clear();
                }

                rendererLabels = sl.ToArray();  // << Must be an array for editor purposes.
            }
        }

        private ProtoInfo m_ProtoInfo;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3 // Extra for name label.
                + EditorGUIUtility.standardVerticalSpacing * 4
                + 8;  // Improves separation of elements.  Looks better.
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //var attr = attribute as RendererMaterialPtrAttribute;
            var go = ((Component)property.serializedObject.targetObject).gameObject;

            if (m_ProtoInfo == null || m_ProtoInfo.prototype != go)
            {
                m_ProtoInfo = new ProtoInfo();
                m_ProtoInfo.Refresh(go);
            }

            label = EditorGUI.BeginProperty(position, label, property);

            var rect = new Rect(position.x, position.y, 
                position.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.LabelField(rect, label);

            rect = new Rect(rect.x, rect.yMax + EditorGUIUtility.standardVerticalSpacing,
                rect.width, position.height - rect.height - EditorGUIUtility.standardVerticalSpacing);

            if (m_ProtoInfo == null || m_ProtoInfo.renderers.Count == 0)
                StandardGUI(rect, property);
            else
                PrototypeGUI(rect, property);

            EditorGUI.EndProperty();
        }

        private void PrototypeGUI(Rect position, SerializedProperty property)
        {
            var origLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 80;

            var props = new Props(property);
            var info = m_ProtoInfo;

            // Renderer

            int iOrig = 0;

            if (props.renderer.objectReferenceValue)
            {
                iOrig = info.renderers.IndexOf(props.renderer.objectReferenceValue as Renderer);
                iOrig = iOrig == -1 ? 0 : iOrig;
            }

            var space = EditorGUIUtility.standardVerticalSpacing;
            var height = EditorGUIUtility.singleLineHeight;

            var rect = new Rect(position.x, position.y + space, position.width, height);
            int iSel = EditorGUI.Popup(rect, RendererLabel, iOrig, info.rendererLabels);

            if (iSel != iOrig)
                props.renderer.objectReferenceValue = info.renderers[iSel];

            int iRen = iSel;

            // Target Index

            rect = new Rect(rect.xMin, rect.yMax + space, rect.width, rect.height);

            if (iRen == 0)
                EditorGUI.LabelField(rect, TargetLabel, new GUIContent("None"));
            else
            {
                GUIContent[] labels = info.materialLabels[iRen];

                iOrig = Mathf.Clamp(props.index.intValue, 0, labels.Length);

                // Don't bother.  Warning isn't helpful because clamping happens frequently as 
                // renderers are switched back and forth.
                //if (iOrig != props.index.intValue)
                //{
                //    if (props.index.intValue != -1)
                //        Debug.LogWarning("Clamped target material index for " + label);
                //}

                iSel = EditorGUI.Popup(rect, TargetLabel, iOrig, labels);

                if (iSel != iOrig)
                    props.index.intValue = iSel;
            }

            EditorGUIUtility.labelWidth = origLabelWidth;
        }

        private void StandardGUI(Rect position, SerializedProperty property)
        {
            var props = new Props(property);

            var space = EditorGUIUtility.standardVerticalSpacing;
            var height = EditorGUIUtility.singleLineHeight;

            var rect = new Rect(position.x, position.y + space, position.width, height);
            EditorGUI.PropertyField(rect, props.renderer);

            rect = new Rect(rect.xMin, rect.yMax + space, rect.width, rect.height);
            EditorGUI.PropertyField(rect, props.index);
        }
    }
}
