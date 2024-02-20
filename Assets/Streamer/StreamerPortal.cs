using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gasimo.Streamer
{

    [RequireComponent(typeof(Collider))]
    public class StreamerPortal : MonoBehaviour
    {

        public StreamerArea targetArea;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StreamerCore.Instance.PlayerEnteredArea(targetArea);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StreamerCore.Instance.PlayerLeftArea(targetArea);
            }
        }



    }
}