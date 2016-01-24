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
    /// Provides utility features related to the Mecanim Animator.
    /// </summary>
    public static class MecanimUtil
    {
        /*
         * Design Notes:
         * 
         * The animator extentions tend to be strict.  By default they either make the changes 
         * safely and completely, or they don't make any at all.  This is because a bad change can
         * result in hard to predict behavior.  For example, sync'ing a state while a source layer
         * is in transition might result in the target animator getting stuck in a state because a 
         * trigger it needs to get out has been consumed.
         * 
         */

        /// <summary>
        /// A delegate used for syncronizing to Animators.
        /// </summary>
        /// <param name="target">
        /// The Animator whose state will be synchronized with the <paramref name="source"/>.
        /// </param>
        /// <param name="source">
        /// The Animator whose state will be synchronzied to the <paramref name="target"/>.
        /// </param>
        /// <returns>
        /// True if the synchronaization succeeds. (The method is expected to post error messages.)
        /// </returns>
        public delegate bool Sync(Animator target, Animator source);

        /// <summary>
        /// Synchronize layer weight, layer state, and parameter values from the source animator
        /// to the target. (Does not sync IK.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// Performs a 'safe' synchronization.  I.e. Performs the 
        /// <see cref="CanSafelySyncFrom(Animator, Animator, bool)"/> checks.
        /// </para>
        /// </remarks>
        /// <param name="target">
        /// The Animator whose state will be synchronized with the <paramref name="source"/>.
        /// </param>
        /// <param name="source">
        /// The Animator whose state will be synchronzied to the <paramref name="target"/>.
        /// </param>
        /// <returns>
        /// True if the synchronaization succeeds.
        /// </returns>
        public static bool SynchronizeFrom(this Animator target, Animator source)
        {
            // Keep this method's signature compatible with the Sync delegate.
            return SynchronizeFrom(target, source, false);
        }

        /// <summary>
        /// Synchronzie layer weight, layer state, and parameter values from the source animator
        /// to the target. (Does not sync IK.)
        /// </summary>
        /// <param name="target">
        /// The Animator whose state will be synchronized with the <paramref name="source"/>.
        /// </param>
        /// <param name="source">
        /// The Animator whose state will be synchronzied to the <paramref name="target"/>.
        /// </param>
        /// <param name="force">
        /// If true only the mandatory validity checks will be performed.  Otherwise 
        /// <see cref="CanSafelySyncFrom(Animator, Animator, bool)"/> will be checked first.
        /// </param>
        /// <returns>
        /// True if the synchronaization succeeds.
        /// </returns>
        public static bool SynchronizeFrom(this Animator target, Animator source, bool force)
        {
            if (target.runtimeAnimatorController != source.runtimeAnimatorController)
            {
                // Can't force this.
                Debug.LogError("Different runtime animator controllers.", target);
                return false;
            }

            if (!(force || target.CanSafelySyncFrom(source, true)))
                return false;

            for (int i = 0; i < source.layerCount; i++)
            {
                if (i != 0)
                    target.SetLayerWeight(i, source.GetLayerWeight(i));

                var sstate = source.GetCurrentAnimatorStateInfo(i);
                target.Play(sstate.fullPathHash, i, sstate.normalizedTime);
            }

            foreach (var svar in source.parameters)
            {
                int hash = svar.nameHash;

                if (target.IsParameterControlledByCurve(hash))
                    continue;

                switch (svar.type)
                {
                    case AnimatorControllerParameterType.Bool:

                        target.SetBool(hash, source.GetBool(hash));
                        break;

                    case AnimatorControllerParameterType.Int:

                        target.SetInteger(hash, source.GetInteger(hash));
                        break;

                    case AnimatorControllerParameterType.Float:

                        target.SetFloat(hash, source.GetFloat(hash));
                        break;

                    case AnimatorControllerParameterType.Trigger:

                        if (source.GetBool(hash))
                            target.SetTrigger(hash);
                        break;
                }
            }


            return true;
        }

        /// <summary>
        /// Can the animator be safely synchronized from the source.  (Does not check IK.)
        /// </summary>
        /// <remarks>
        /// <para>
        /// The animators are considered safe to synchronze if the following conditions are met:
        /// </para>
        /// <ul>
        /// <li>The animators have the same controller.</li>
        /// <li>Neither animator is performing target matching.</li>
        /// <li><see cref="source"/> is not transitioning on any layer.</li>
        /// </ul>
        /// </remarks>
        /// <param name="target">
        /// The Animator whose state needs to be synchronized with the <paramref name="source"/>.
        /// </param>
        /// <param name="source">
        /// The Animator whose state needs to be be synchronzied to the <paramref name="target"/>.
        /// </param>
        /// <param name="postError">
        /// If true, any detected problem will be posted as an error to the debug console.
        /// Otherwise the method will be silent.
        /// </param>
        /// <returns>
        /// True if the animators are safe to synchronize.
        /// </returns>
        public static bool CanSafelySyncFrom(
            this Animator target, Animator source, bool postError = false)
        {
            if (target.runtimeAnimatorController != source.runtimeAnimatorController)
            {
                if (postError)
                {
                    // Can't force this.
                    Debug.LogError("Source and target have different runtime animator controllers.",
                        target);
                }
                return false;
            }

            if (target.isMatchingTarget)
            {
                if (postError)
                    Debug.LogError("Target animator is performing target matching.", target);
                return false;
            }

            if (source.isMatchingTarget)
            {
                if (postError)
                    Debug.LogError("Source animator is matching target.", target);
                return false;
            }

            for (int i = 0; i < source.layerCount; i++)
            {
                if (source.IsInTransition(i))
                {
                    if (postError)
                        Debug.LogError("Source animator layer is in transition.", target);
                    return false;
                }
            }

            return true;
        }
    }
}
