using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Gasimo.Streamer
{
    /// <summary>
    /// Definition for a single streamable area
    /// </summary>
    [CreateAssetMenu(fileName = "StreamerArea", menuName = "Gasimo/Streamer/Area")]
    public class StreamerArea : ScriptableObject
    {
        /// <summary>
        /// Actions will be invoked when the area is loaded. An aditional parameter is the LOD for the area (distance from main area)
        /// </summary>
        public UnityAction<uint> OnAreaLoaded;
        public UnityAction OnUnloadBegin;

        public UnityAction OnPlayerEnter;
        public UnityAction OnPlayerExit;

        [NonSerialized]
        public StreamStatus status = StreamStatus.Unloaded;

        public List<StreamerArea> dependencies;
        

        /// <summary>
        /// Dynamic objects that are currently owned by this area.
        /// </summary>
        public HashSet<IRuntimeStreamable> ownedItems;


        // Reference to scene which represents this area
        // the scene in string
        [HideInInspector]
        public string sceneName;

#if UNITY_EDITOR

        // the scene in asset
        public UnityEditor.SceneAsset targetSceneAsset;

        // whenever you modify the scene in the project, this sets the new name in the
        // targetScene variable automatically.
        private void OnValidate()
        {
            sceneName = "";
            if (targetSceneAsset != null)
            {
                sceneName = targetSceneAsset.name;
            }
        }

#endif



        public void CollectDependenciesRecursively(uint depth, ref HashSet<StreamerArea> visitedAreas)
        {
            // If already visited, return
            if (visitedAreas.Contains(this))
                return;

            // Mark this area as visited
            visitedAreas.Add(this);

            if (depth > 0)
            {
                foreach (var dependency in dependencies)
                {
                    dependency.CollectDependenciesRecursively(depth - 1, ref visitedAreas);
                }
            }
        }

    }

    public enum StreamStatus
    {
        Unloaded,
        Loading,
        Loaded
    }

    public interface IRuntimeStreamable
    {

        /// <summary>
        /// Load the streamable object
        /// </summary>
        /// <returns></returns>
        Task Load();

        /// <summary>
        /// Unloads the sctreamable object
        /// </summary>
        /// <returns></returns>
        Task Unload();
    }
}