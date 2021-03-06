﻿
/*
 * Copyright (c) 2015-2016 Stephen A. Pratt
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

namespace com.lizitt.editor
{
    /// <summary>
    /// Draws the GUI element for <see cref="RendererMaterialPtr"/> fields with
    /// <see cref="RendererMaterialPtrAttribute"/> applied.
    /// </summary>
    [CustomPropertyDrawer(typeof(RendererMaterialPtrAttribute))]
    public class RendererMaterialPtrDrawer
        : PropertyDrawer
    {
        private RendererMaterialPtrControl m_GuiControl;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CheckInitialized();

            return m_GuiControl.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CheckInitialized();

            m_GuiControl.OnGUI(position, property, label);
        }

        private void CheckInitialized()
        {
            if (m_GuiControl == null)
            {
                var attr = attribute as RendererMaterialPtrAttribute;

                m_GuiControl = attr.RequireLocal
                    ? new RendererMaterialPtrControl(attr.SearchPropertyPath)
                    : new RendererMaterialPtrControl();
            }
        }
    }
}
