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
    /// A base class for collider related settings.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Useful for defining configurations for colliders that will be created a runtime.
    /// </para>
    /// </remarks>
    [System.Serializable]
    public abstract class ColliderParams
    {
        [Header("Transform Settings")]

        [SerializeField]
        [Tooltip("If true, the collider should be added to a new transform."
            + " Otherwise it should be added directly to its parent object.")]
        private bool m_NewTransform = false;
        
        [SerializeField]
        [Tooltip("If adding to new transform, the transform should be offset by this position.")]
        private Vector3 m_PositionOffset = Vector3.zero;

        [SerializeField]
        [Tooltip(
            "If adding a to new transform, the child transform should be offset by this rotation.")]
        private Vector3 m_RotationOffset = Vector3.zero;

        [Header("Collider Settings")]

        [SerializeField]
        [Tooltip("True if the collider is a trigger.")]
        private bool m_IsTrigger = false;

        [SerializeField]
        [Tooltip("The collider's physics material. (Can be null.)")]
        private PhysicMaterial m_Material = null;

        /// <summary>
        /// If true, the collider should be added to a new transform.
        /// Otherwise it should be added directly to its parent object.
        /// </summary>
        public bool NewTransform
        {
            get { return m_NewTransform; }
            set { m_NewTransform = value; }
        }

        /// <summary>
        /// If adding a new transform, the transform should be offset by this position.
        /// </summary>
        public Vector3 PositionOffset
        {
            get { return m_PositionOffset; }
            set { m_PositionOffset = value; }
        }

        /// <summary>
        /// If adding to a new transform, the child transform should be offset by this rotation.
        /// </summary>
        public Vector3 RotationOffset
        {
            get { return m_RotationOffset; }
            set { m_RotationOffset = value; }
        }

        /// <summary>
        /// True if the collider is a trigger.
        /// </summary>
        public bool IsTrigger
        {
            get { return m_IsTrigger; }
            set { m_IsTrigger = value; }
        }

        /// <summary>
        /// The collider's physics material. (Can be null.)
        /// </summary>
        public PhysicMaterial Material
        {
            get { return m_Material; }
            set { m_Material = value; }
        }

        /// <summary>
        /// Creates a new collider based on the settings.  
        /// </summary>
        /// <remarks>
        /// <para>
        /// The new collider will be be added directory to the target if not configured for a
        /// new transform.  Other it will be added to a new transform that is a child of the parent.
        /// </para>
        /// </remarks>
        /// <param name="target">The collider target.</param>
        /// <returns>The new collider.</returns>
        public abstract Collider Create(Transform target);

        public virtual string NewName
        {
            get { return "Collider"; }
        }

        /// <summary>
        /// Gets the GameObject the collider should be attached to. (Handles new GameObject 
        /// creation and parenting as appropriate.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// A new GameObject will be created if target is null or the settings specify a new
        /// transform.
        /// </para>
        /// <para>
        /// Parenting and local offsets of new transforms are applied as appropriate.
        /// </para>
        /// </remarks>
        /// <param name="target">The collider target. (Can be null.)</param>
        /// <returns>
        /// The GameObject the collider should be attached to.  (Will never be null.)
        /// </returns>
        protected GameObject GetAttachTarget(Transform target)
        {
            GameObject result;

            if (m_NewTransform || !target)
            {
                result = new GameObject();
                result.name = NewName;
                result.transform.parent = target;

                result.transform.localPosition = m_PositionOffset;
                result.transform.localEulerAngles = m_RotationOffset;
            }
            else
                result = target.gameObject;

            return result;
        }

        /// <summary>
        /// Applies the settings to an existing collider.  (New collider settings do not apply.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// The new collider settings, such as position and rotation offsets, do not apply to
        /// this method.  Only collider component settings such as trigger and material are applied.
        /// </para>
        /// </remarks>
        /// <param name="collider"></param>
        public virtual void ApplyTo(Collider collider)
        {
            collider.isTrigger = m_IsTrigger;
            collider.sharedMaterial = m_Material;

            return;
        }
    }

    /// <summary>
    /// Represents capsule collider settings.
    /// </summary>
    [System.Serializable]
    public class CapsuleColliderParams
        : ColliderParams
    {
        /// <summary>
        /// The direction of the capsule axis.
        /// </summary>
        public enum AxisDirection
        {
            /// <summary>
            /// Use the local x-axis.
            /// </summary>
            XAxis = 0,

            /// <summary>
            /// Use the local y-axis.
            /// </summary>
            YAxis = 1,
            
            /// <summary>
            /// Use the local z-axis.
            /// </summary>
            ZAxis = 2,
        }

        [SerializeField]
        [Tooltip("The capsule center offset.")]
        private Vector3 m_Center = Vector3.zero;

        [SerializeField]
        [Tooltip("The capsule radius. [Limit: >= 0]")]
        [ClampMinimum(0)]
        private float m_Radius = 0.5f;

        [SerializeField]
        [Tooltip("The capsule height. [Limit: >= 0]")]
        [ClampMinimum(0)]
        private float m_Height = 1;

        [SerializeField]
        [Tooltip("The direction of the capsule's axis.")]
        private AxisDirection m_Direction = AxisDirection.YAxis;

        /// <summary>
        /// The capsule center offset
        /// </summary>
        public Vector3 Center
        {
            get { return m_Center; }
            set { m_Center = value; }
        }

        /// <summary>
        /// The capsule radius. [Limit: >= 0]
        /// </summary>
        public float Radius
        {
            get { return m_Radius; }
            set { m_Radius = Mathf.Max(0, value); }
        }

        /// <summary>
        /// The capsule height. [Limit: >= 0]
        /// </summary>
        public float Height
        {
            get { return m_Height; }
            set { m_Height = Mathf.Max(0, value); }
        }

        /// <summary>
        /// The direction of the capsule's axis
        /// </summary>
        public AxisDirection Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }

        public override string NewName
        {
            get { return "CapsuleCollider"; }
        }

        public override Collider Create(Transform target)
        {
            var result = GetAttachTarget(target).AddComponent<CapsuleCollider>();

            ApplyTo(result);

            return result;
        }

        public override void ApplyTo(Collider collider)
        {
            var col = collider as CapsuleCollider;

            if (col)
            {
                col.center = m_Center;
                col.radius = m_Radius;
                col.height = m_Height;
                col.direction = (int)m_Direction;
            }

            base.ApplyTo(collider);
        }
    }

    /// <summary>
    /// Represents box collider settings.
    /// </summary>
    [System.Serializable]
    public class BoxColliderParams
        : ColliderParams
    {
        [SerializeField]
        [Tooltip("The box center offset.")]
        private Vector3 m_Center = Vector3.zero;

        /// <summary>
        /// The box center offset.
        /// </summary>
        public Vector3 Center
        {
            get { return m_Center; }
            set { m_Center = value; }
        }

        [SerializeField]
        [Tooltip("The box size. [Limits: All fields >= 0]")]
        private Vector3 m_Size = new Vector3(1, 1, 1);

        /// <summary>
        /// The box size. [Limits: All fields >= 0]
        /// </summary>
        public Vector3 Size
        {
            get { return m_Size; }
            set 
            {
                value.x = Mathf.Max(0, value.x);
                value.y = Mathf.Max(0, value.y);
                value.z = Mathf.Max(0, value.z);

                m_Size = value;
            }
        }

        public override string NewName
        {
            get { return "BoxCollider"; }
        }

        public override Collider Create(Transform target)
        {
            var result = GetAttachTarget(target).AddComponent<BoxCollider>();

            ApplyTo(result);

            return result;
        }

        public override void ApplyTo(Collider collider)
        {
            var col = collider as BoxCollider;

            if (col)
            {
                col.center = m_Center;
                col.size = m_Size;
            }

            base.ApplyTo(collider);
        }
    }

    /// <summary>
    /// Represents sphere collider settings.
    /// </summary>
    [System.Serializable]
    public class SphereColliderParams
        : ColliderParams
    {
        [SerializeField]
        [Tooltip("The sphere center offset.")]
        private Vector3 m_Center = Vector3.zero;

        [SerializeField]
        [Tooltip("The sphere radius. [Limit: >= 0]")]
        [ClampMinimum(0)]
        private float m_Radius = 0.5f;

        /// <summary>
        /// The sphere center offset.
        /// </summary>
        public Vector3 Center
        {
            get { return m_Center; }
            set { m_Center = value; }
        }

        /// <summary>
        /// The sphere radius. [Limit: >= 0]
        /// </summary>
        public float Radius
        {
            get { return m_Radius; }
            set { m_Radius = Mathf.Max(0, value); }
        }

        public override string NewName
        {
            get { return "SphereCollider"; }
        }

        public override Collider Create(Transform target)
        {
            var result = GetAttachTarget(target).AddComponent<SphereCollider>();

            ApplyTo(result);

            return result;
        }

        public override void ApplyTo(Collider collider)
        {
            var col = collider as SphereCollider;

            if (col)
            {
                col.center = m_Center;
                col.radius = m_Radius;
            }

            base.ApplyTo(collider);
        }
    }

    // Implement only when needed.  Not tested.
    //[System.Serializable]
    //public class RigidBodyParams
    //{
    //    [SerializeField]
    //    [ClampMinimum(0)]
    //    private float m_Mass = 1;

    //    [SerializeField]
    //    [ClampMinimum(0)]
    //    private float m_AngularDrag = 0.05f;

    //    [SerializeField]
    //    private bool m_UseGravity = true;

    //    [SerializeField]
    //    private bool m_IsKinematic = false;

    //    [SerializeField]
    //    private RigidbodyInterpolation m_Interpolation = RigidbodyInterpolation.None;

    //    [SerializeField]
    //    private CollisionDetectionMode m_CollisionDetection = CollisionDetectionMode.Discrete;

    //    // Needs to be a flag.
    //    [SerializeField]
    //    private RigidbodyConstraints m_Constraints = RigidbodyConstraints.None;

    //    public void Apply(Rigidbody rigidBody)
    //    {
    //        rigidBody.mass = m_Mass;
    //        rigidBody.angularDrag = m_AngularDrag;
    //        rigidBody.useGravity = m_UseGravity;
    //        rigidBody.isKinematic = m_IsKinematic;
    //        rigidBody.interpolation = m_Interpolation;
    //        rigidBody.collisionDetectionMode = m_CollisionDetection;
    //        rigidBody.constraints = m_Constraints;
    //    }
    //}
}

