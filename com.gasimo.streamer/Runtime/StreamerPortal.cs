using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gasimo.Streamer
{
    /// <summary>
    /// Defines the in-game portal which triggers the loading of a new area
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class StreamerPortal : MonoBehaviour
    {

        public StreamerArea targetArea;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                targetArea.OnPlayerEnter?.Invoke();
                StreamerCore.Instance.PlayerEnteredArea(targetArea);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                targetArea.OnPlayerExit?.Invoke();
                StreamerCore.Instance.PlayerLeftArea(targetArea);
            }
        }

        /// <summary>
        /// Manually trigger the load of the area
        /// </summary>
        public void ForceAreaEntry()
        {
            targetArea.OnPlayerEnter?.Invoke();
            StreamerCore.Instance.PlayerEnteredArea(targetArea);
        }

        /// <summary>
        /// Manually trigger the deregistering of the area
        /// </summary>
        public void ForceAreaExit()
        {
            targetArea.OnPlayerExit?.Invoke();
            StreamerCore.Instance.PlayerLeftArea(targetArea);
        }


        /// <summary>
        /// Draw the portal in the editor. Select shape based on the assigned collider component.
        /// </summary>
        private void OnDrawGizmos()
        {
            
            if(Application.IsPlaying(this) && StreamerCore.Instance != null && StreamerCore.Instance.EnteredAreasEvidence.Count != 0 && StreamerCore.Instance.EnteredAreasEvidence[0] == targetArea)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.blue;
                
            }


            var collider = GetComponent<Collider>();
            if (collider is BoxCollider)
            {
                Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
            }
            else if (collider is SphereCollider)
            {
                Gizmos.DrawWireSphere(collider.bounds.center, collider.bounds.extents.magnitude);
            }
            else if (collider is CapsuleCollider)
            {
                Gizmos.DrawWireSphere(transform.position + Vector3.up * collider.bounds.extents.y, collider.bounds.extents.x);
                Gizmos.DrawWireSphere(transform.position - Vector3.up * collider.bounds.extents.y, collider.bounds.extents.x);
            } else if (collider is MeshCollider)
            {
                Gizmos.DrawWireMesh((collider as MeshCollider).sharedMesh, transform.position, transform.rotation, transform.localScale);
            }
        }



    }
}