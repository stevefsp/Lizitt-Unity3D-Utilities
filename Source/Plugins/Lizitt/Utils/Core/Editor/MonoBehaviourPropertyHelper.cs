/*
 * Copyright (c) 2016 Stephen A. Pratt
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
using System.Collections.Generic;

namespace com.lizitt.editor
{
    /// <summary>
    /// Provides features useful for creating simple custom editors without having to hard code field names. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// Expected user case:
    /// </para>
    /// <para>
    /// <ol>
    /// <li>Create the helper during the custom editor's initialization.  (E.g. OnEnable())</li>
    /// <li>Call <see cref="LoadProperties"/> at the beginning of every draw call.  (E.g. OnInspectorGUI())</li>
    /// <li>Call <see cref="ExtractProperty[U]"/> to extract the properties that need to be drawn manually.</li>
    /// <li>Manually draw extracted properties. (All or some.)</li>
    /// <li>Draw the unextracted properties by calling <see cref="DrawProperties"/>.</li>
    /// <li>Draw any remaining extracted properties.</li>
    /// </ol>
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The MonoBehaviour.</typeparam>
    public class BehaviourPropertyHelper<T> where T : MonoBehaviour
    {
        private readonly List<SerializedFieldInfo> m_FieldInfo = new List<SerializedFieldInfo>();
        private readonly List<SerializedProperty> m_Properties = new List<SerializedProperty>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public BehaviourPropertyHelper()
        {
            LizittEditorUtil.AddSerializedFields<T>(m_FieldInfo);
        }

        /// <summary>
        /// Load/reload all visible properties of the serialized object.  (Does not enter children.) 
        /// </summary>
        /// <param name="serializedObject">The serialized property of the Monobehaviour.</param>
        /// <param name="includeScript">If true, include the script reference property.  Otherwise ignore it.</param>
        public void LoadProperties(SerializedObject serializedObject, bool includeScript = true)
        {
            var prop = serializedObject.GetIterator();
            if (!includeScript)
                prop.NextVisible(true);

            bool didFirst = !includeScript;

            m_Properties.Clear();
            while (prop.NextVisible(didFirst ? false : true))
            {
                didFirst = true;
                m_Properties.Add(serializedObject.FindProperty(prop.propertyPath));
            }
        }

        /// <summary>
        /// Extract the first property of the specified type.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Any properties extracted by this method will not be drawn when calling <see cref="DrawProperties"/>.
        /// </para>
        /// </remarks>
        /// <typeparam name="U">The field type of the property to extract.</typeparam>
        /// <returns>The first property of the specified type, or null if none exists.</returns>
        public SerializedProperty ExtractProperty<U>()
        {
            for (int i = 0; i < m_FieldInfo.Count; i++)
            {
                var info = m_FieldInfo[i];

                if (!info.isHidden && info.typ == typeof(U))
                {
                    for (int j = 0; j < m_Properties.Count; j++)
                    {
                        var prop = m_Properties[j];
                        if (prop.propertyPath == info.name)
                        {
                            m_Properties.RemoveAt(j);
                            return prop;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the property information if it was found, or a default information object if not found.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The property information if it was found, or a default information object if not found.</returns>
        public SerializedFieldInfo GetPropertyInfo(SerializedProperty property)
        {
            if (property != null)
            {
                foreach (var info in m_FieldInfo)
                {
                    if (property.propertyPath == info.name)
                        return info;
                }
            }

            return new SerializedFieldInfo();
        }

        /// <summary>
        /// The current property.  (The next one that will be drawn.)
        /// </summary>
        public SerializedProperty CurrentProperty
        {
            get { return m_Properties.Count == 0 ? null : m_Properties[0]; }
        }

        /// <summary>
        /// Draw and remove the current property.
        /// </summary>
        /// <returns>The property that was drawn, or null if there are no more properties.</returns>
        public SerializedProperty DrawCurrentProperty()
        {
            if (m_Properties.Count > 0)
            {
                var prop = m_Properties[0];
                m_Properties.RemoveAt(0);  // In case of error.  Don't want to get struck here.
                EditorGUILayout.PropertyField(prop);
                return prop;
            }

            return null;
        }

        /// <summary>
        /// Draw the GUI controls for all properties remaining in the property list.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Properties extracted via <see cref="ExtractProperty[U]"/> will not be drawn.
        /// </para>
        /// <para>
        /// Calling this method empties the property list.  <see cref="LoadProperties"/> must be used to reload 
        /// the list.
        /// </para>
        /// </remarks>
        public void DrawProperties()
        {
            foreach (var item in m_Properties)
                EditorGUILayout.PropertyField(item);

            m_Properties.Clear();
        }
    }
}