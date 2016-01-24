using UnityEngine;

namespace com.lizitt
{
    /// <summary>
    /// Provides user friendly dropdown selections for renderers on/under the object the
    /// field's component is attached to.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This attribute can be used when the field will only refer to renders on the component's
    /// GameObject or one of its children.  It searches for renders and provides dropdowns
    /// for user-friendly selection.  If no renderers are found a basic GUI will be provided.
    /// </para>
    /// </remarks>
    public class RendererMaterialPtrAttribute
        : PropertyAttribute
    {
    }
}
