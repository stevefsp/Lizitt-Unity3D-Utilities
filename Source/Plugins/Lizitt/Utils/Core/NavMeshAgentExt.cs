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

// TODO: Unscheduled: This class needs more thorough testing.

namespace com.lizitt
{
    /// <summary>
    /// Provides NavMeshAgent extension methods.
    /// </summary>
    public static class NavMeshAgentExt
    {
        /// <summary>
        /// Gets a local line-of-sight position on the agent's navigation mesh.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The minimum/maximum distance is used to control the desired travel range.
        /// But the minimum distance is a soft value.  If an obstacle is hit before the desired
        /// minimum distance then hardMinimum is used to determine success or failure, so the
        /// true minimum is hardMinimum.  E.g. I want to move a distance of between 10 and 20
        /// in a random direction.  If an obstacle is hit in the chosen direction, then fail if 
        /// the distance is less than 1.
        /// </para>
        /// </remarks>
        /// <param name="navAgent"></param>
        /// <param name="minimumDistance">The desired minimimum distance.</param>
        /// <param name="maximumDistance">The maximum distance.</param>
        /// <param name="target"></param>
        /// <param name="hardMinimum">The absolute minimum distance. (Defaults to the NavMeshAgent radius.)</param>
        /// <returns></returns>
        public static bool GetRandomLocalTarget(this NavMeshAgent navAgent,
            float minimumDistance, float maximumDistance,
            out Vector3 target, float hardMinimum = -1)
        {
            var dist = Random.Range(minimumDistance, maximumDistance);

            var offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            target = navAgent.transform.position + offset.normalized * dist;

            NavMeshHit hit;
            navAgent.Raycast(target, out hit);

            if (hit.hit && hit.distance < dist)
                target = hit.position;

            hardMinimum = hardMinimum == -1 ? navAgent.radius : hardMinimum;

            if ((navAgent.transform.position - target).sqrMagnitude
                <= hardMinimum * hardMinimum)
            {
                target = Vector3.zero;
                return false;
            }

            return true;
        }
    }
}

