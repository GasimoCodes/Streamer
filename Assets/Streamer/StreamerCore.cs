using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gasimo.Streamer
{
    public class StreamerCore : MonoBehaviour
    {

        // Singleton
        public static StreamerCore Instance { 
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<StreamerCore>();
                }
                return instance;
            }
        }

        private static StreamerCore instance;


        


        /// <summary>
        /// Hashset of all loaded areas
        /// </summary>
        public HashSet<StreamerArea> areas = new();

        public List<StreamerArea> EnteredAreasEvidence;

        public string PersistntSceneName;

        /// <summary>
        /// Loads the area, all its dependencies and owned items recursively with a given depth.
        /// </summary>
        /// <param name="area">Area to load</param>
        /// <param name="depth">Depth of dependencies which will be loaded.</param>
        /// <returns></returns>
        public async Task LoadAreaWithDependencies(StreamerArea area, uint depth = 1)
        {

            // Load dependencies first
            HashSet<StreamerArea> dependenciesHashSet = new HashSet<StreamerArea>();
            area.CollectDependenciesRecursively(depth, ref dependenciesHashSet);
            List<StreamerArea> dependencies = new();
            List<AsyncOperation> handles = new();
            

            foreach (var dependency in dependenciesHashSet.ToList())
            {
                if (dependency.status == StreamStatus.Unloaded)
                {
                    var handle = SceneManager.LoadSceneAsync(dependency.sceneName, LoadSceneMode.Additive);
                    //handle.allowSceneActivation = false;
                    dependencies.Add(dependency);
                    handles.Add(handle);
                    dependency.status = StreamStatus.Loading;
                }
            }

            // Wait for loading to catch up
            foreach (var handle in handles)
            {
                await handle;
            }


            // Activate scenes
            for (int i = 0; i < dependencies.Count; i++)
            {
                handles[i].allowSceneActivation = true;
                dependencies[i].status = StreamStatus.Loaded;
                areas.Add(dependencies[i]);
            }

            // Purge
            await PurgeUnusedAreas();
        }

        // Function to handle when player leaves an area
        public void PlayerLeftArea(StreamerArea area)
        {
            EnteredAreasEvidence.Remove(area);

            if(EnteredAreasEvidence.Count != 0)
            {
                Debug.Log("Player is in " + EnteredAreasEvidence[0]);
                LoadAreaWithDependencies(EnteredAreasEvidence[0], 1);
            }
            
        }

        // Function to handle when player enters an area
        public void PlayerEnteredArea(StreamerArea area)
        {
            // If the area is already in the list, remove it
            if (EnteredAreasEvidence.Contains(area))
            {
                EnteredAreasEvidence.Remove(area);
            }

            // Add the area at the beginning of the list to ensure it's the current area
            EnteredAreasEvidence.Insert(0, area);
        }

        public async Task<int> PurgeUnusedAreas()
        {
            int unloadedAreas = 0;
            HashSet<StreamerArea> activeDependencies = new HashSet<StreamerArea>();
            EnteredAreasEvidence[0].CollectDependenciesRecursively(1, ref activeDependencies);

            List<StreamerArea> scheduledRemove = new();

            for (int i = 0; i < areas.Count; i++)
            {
                StreamerArea area = areas.ElementAt(i);

                if (!activeDependencies.Contains(area))
                {    
                    area.OnUnloadBegin?.Invoke();
                    await SceneManager.UnloadSceneAsync(area.sceneName);
                    area.status = StreamStatus.Unloaded;
                    scheduledRemove.Add(area);
                    unloadedAreas++;
                }
            }

            foreach (var item in scheduledRemove)
            {
                areas.Remove(item);
            }


            return unloadedAreas;
        }

    }
}