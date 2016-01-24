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
using UnityEngine;

namespace com.lizitt.u3d
{
    /// <summary>
    /// A Physics raycaster.
    /// </summary>
    [System.Serializable]
    public class Raycaster
    {
        [SerializeField]
        [Tooltip("The object to cast from. (Must be set before use.)")]
        private Transform m_Source;

        [SerializeField]
        [Tooltip("The default object to cast the ray towards. (If not set, the value"
            + " of direction will be used.)")]
        private Transform m_Target;

        [SerializeField]
        [Tooltip("The direction to cast the ray, or source forward if set to Vector3.zero."
            + " (Ignored if target is specified.)")]
        private Vector3 m_Direction = Vector3.forward;

        [SerializeField]
        [Tooltip("The space of the direction.  (Ignored if target is specified.)")]
        private Space m_DirectionSpace = Space.Self;

        [SerializeField]
        [Tooltip("The radius of the cast.  (Any non-zero value will result in a sphere cast.")]
        [ClampMinimum(0)]
        private float m_Radius = 0;

        [SerializeField]
        [Tooltip("The length of the ray. (0 for infinity.)")]
        [ClampMinimum(0)]
        private float m_Distance = 50f;

        [SerializeField]
        [Tooltip("Pick only from these layers.")]
        private LayerMask m_LayerMask = Physics.IgnoreRaycastLayer;

        [SerializeField]
        [Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
        private bool m_InvertMask = true;

        private RaycastHit m_Hit;

        /// <summary>
        /// True if <see cref="Hit"/> has hit information from a raycast.
        /// </summary>
        public bool HasHit { get; private set; }

        /// <summary>
        /// Current hit information.  (Value is undefined if <see cref="HasHit"/> is false.)
        /// </summary>
        public RaycastHit Hit
        {
            get { return m_Hit; }
        }

        /// <summary>
        /// Clears all hit information.
        /// </summary>
        public void ResetHit()
        {
            HasHit = false;
            m_Hit = new RaycastHit();
        }

        /// <summary>
        /// The object to cast from. (Must be set before use.)
        /// </summary>
        public Transform Source
        {
            get { return m_Source; }
            set { m_Source = value; }
        }

        /// <summary>
        /// The default object to cast the ray towards. (If null, the value of  
        /// <see cref="Direction"/> will be used.)
        /// </summary>
        public Transform Target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }

        /// <summary>
        /// The direction to cast the ray if <see cref="Target"/> is null.
        /// (A value of Vector3.zero will result in <see cref="Source"/>.forward being used.)
        /// </summary>
        public Vector3 Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }

        /// <summary>
        /// The cast direction based on the configuration. (<see cref="Source"/> must assigned.)
        /// </summary>
        public Vector3 CurrentDirection
        {
            get
            {
                if (m_Target)
                    return m_Target.position - m_Source.position;
                else if (m_Direction == Vector3.zero)
                    return m_Source.forward;
                else if (m_DirectionSpace == Space.Self)
                    return m_Source.TransformDirection(m_Direction);
                else
                    return m_Direction;
            }
        }

        /// <summary>
        /// Performs the default raycast.
        /// </summary>
        /// <returns>
        /// True if a hit was detected.  (Value will equal <see cref="HasHit"/>.)
        /// </returns>
        public bool Raycast()
        {
            if (!m_Source)
            {
                HasHit = false;
                return false;
            }

            HasHit = RaycastToward(CurrentDirection, out m_Hit);

            return HasHit;
        }

        /// <summary>
        /// Performs a raycast to a custom position.  (Configured target/direction is ignored.)
        /// </summary>
        /// <remarks>
        /// <para>Does not update internal state.</para>
        /// </remarks>
        /// <param name="position">The position to raycast toward.</param>
        /// <param name="hit">The result of the raycast.</param>
        /// <returns>
        /// True if a hit occurred and <paramref name="hit"/> contains hit information.
        /// </returns>
        public bool RaycastTo(Vector3 position, out RaycastHit hit)
        {
            return RaycastToward(position - m_Source.position, out hit);
        }

        /// <summary>
        /// Performs a raycast in a custom direction.  (Configured target/direction is ignored.)
        /// </summary>
        /// <remarks>
        /// <para>Does not update internal state.</para>
        /// </remarks>
        /// <param name="hit">The result of the raycast.</param>
        /// <returns>
        /// True if a hit occurred and <paramref name="hit"/> contains hit information.
        /// </returns>
        private bool RaycastToward(Vector3 direction, out RaycastHit hit)
        {
            if (!m_Source)
            {
                hit = new RaycastHit();
                return false;
            }

            var distance = m_Distance == 0 ? Mathf.Infinity : m_Distance;
            var mask = m_InvertMask ? ~m_LayerMask.value : m_LayerMask.value;

            bool hasHit;

            if (m_Radius == 0)
                hasHit = Physics.Raycast(m_Source.position, direction, out hit, distance, mask);
            else
            {
                hasHit = Physics.SphereCast(
                    m_Source.position, m_Radius, direction, out hit, distance, mask);
            }
            
            return hasHit;
        }

        /// <summary>
        /// Displays debug information if called from MonoBehaviour.OnRenderObject().
        /// </summary>
        public void RenderDebug()
        {
            if (!m_Source)
                return;

            DebugDraw.BeginAppend();

            var pos = m_Source.position;
            Vector3 targ;
            float scale = 0.05f;

            DebugDraw.SetColor(Color.yellow);

            if (HasHit)
            {
                targ = m_Hit.point;
                DebugDraw.AppendArrow(targ, targ + m_Hit.normal * 0.15f, 0, 0.025f);

                DebugDraw.SetColor(Color.red);
            }
            else
            {
                targ = pos + CurrentDirection * m_Distance;
                scale = 0.1f;
            }

            DebugDraw.AppendArrow(pos, targ, 0, scale);

            DebugDraw.SetColor(Color.blue);

            if (m_Radius > 0)
                DebugDraw.AppendCircle(pos, m_Radius);
            else
                DebugDraw.AppendXMarker(pos, 0.02f);

            DebugDraw.EndAppend();
        }
    }
}
