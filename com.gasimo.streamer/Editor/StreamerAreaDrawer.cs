using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Gasimo.Streamer.Editor
{
    [CustomPropertyDrawer(typeof(StreamerArea))]
    public class StreamerAreaDrawer : PropertyDrawer
    {

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            Button b = new Button(() =>
            {
                // Load the areas dependencies in editor
                var prop = (StreamerArea)property.boxedValue;

                foreach (var dependency in prop.dependencies)
                {
                    bool sceneLoaded = false;
                    for (int i = 0; i < SceneManager.loadedSceneCount; i++)
                    {
                        if(dependency.sceneName == SceneManager.GetSceneAt(i).name)
                        {
                            sceneLoaded = true;
                        }
                    }

                    if (!sceneLoaded)
                    {
                        // Load the scene in the editor by looking at scenes in build settings and comparing the name
                        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                        {
                            if (EditorBuildSettings.scenes[i].path.Contains(dependency.sceneName))
                            {
                                EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path, OpenSceneMode.Additive);
                            }
                        }
                    }
                }

            }
                )
            {
                text = "Load"
            };


            PropertyField field = new PropertyField(property);

            if (property.objectReferenceValue != null)
            {
                b.SetEnabled(true);
                field.label = ((StreamerArea)property.boxedValue).sceneName;
            }

            field.RegisterValueChangeCallback((evt) =>
            {
                if (property.objectReferenceValue != null)
                {
                    b.SetEnabled(true);
                    field.label = ((StreamerArea)property.boxedValue).sceneName;
                }
                else
                {
                    b.SetEnabled(false);
                }
            });


            container.Add(field);
            container.Add(b);

            // When the window is modified, redraw the window




            return container;
        }
    }
}