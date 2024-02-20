using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gasimo.Streamer
{
    public class StreamerCore : MonoBehaviour
    {

        /// <summary>
        /// Hashset of all loaded areas
        /// </summary>
        public static HashSet<StreamerArea> areas;
        public static StreamerArea mainArea;

        public string PersistntSceneName;

        /// <summary>
        /// Loads the area, all its dependencies and owned items recursively with a given depth.
        /// </summary>
        /// <param name="area">Area to load</param>
        /// <param name="depth">Depth of dependencies which will be loaded.</param>
        /// <returns></returns>
        public static async Task LoadAreaWithDependencies(StreamerArea area, uint depth = 1)
        {

            // Load dependencies first
            HashSet<StreamerArea> dependencies = new HashSet<StreamerArea>();
            area.CollectDependenciesRecursively(depth, ref dependencies);
            dependencies.Add(area);

            AsyncOperation[] handles = new AsyncOperation[dependencies.Count];

            foreach (var dependency in dependencies)
            {
                if (dependency.status == StreamStatus.Unloaded)
                {
                    var handle = SceneManager.LoadSceneAsync(dependency.sceneName, LoadSceneMode.Additive);
                    handle.allowSceneActivation = false;
                    handles[dependencies.IndexOf(dependency)] = handle;
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

        }

        public static async Task<int> PurgeUnusedAreas()
        {

            int unloadedAreas = 0;

            List<StreamerArea> Dependencies = new List<StreamerArea>();
            mainArea.CollectDependenciesRecursively(1, ref Dependencies);

            foreach (var area in StreamerCore.areas)
            {
                if (!Dependencies.Contains(area))
                {
                    area.OnUnloadBegin?.Invoke();
                    await SceneManager.UnloadSceneAsync(area.sceneName);
                    unloadedAreas++;
                }
            }


            return unloadedAreas;
        }


        // C# Async task to unload this area
        public static async Task UnloadArea(StreamerArea area)
        {
            if (area.status == StreamStatus.Loaded)
            {
                area.status = StreamStatus.Loading;
                area.OnUnloadBegin?.Invoke();

                var scene = SceneManager.UnloadSceneAsync(area.sceneName);
                await scene;
                area.status = StreamStatus.Unloaded;

                foreach (var item in area.ownedItems)
                {
                    await item.Unload();
                }
            }
        }



    }
}