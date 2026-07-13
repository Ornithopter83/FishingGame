#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace FishingGame.Editor
{
    public static class SceneSetup
    {
        private const string BootstrapPath = "Assets/Scenes/Bootstrap/Bootstrap.unity";
        private const string LoadingPath = "Assets/Scenes/Bootstrap/LoadingScene.unity";

        [MenuItem("Fishing Game/Create Bootstrap and Loading Scenes")]
        public static void CreateScenes()
        {
            EnsureDirectory(Path.GetDirectoryName(BootstrapPath));
            CreateBootstrapScene();
            CreateLoadingScene();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UpdateBuildSettings();
            AssetDatabase.SaveAssets();
            ValidateScenes();
            Debug.Log("[FishingGame] Bootstrap and Loading scenes created.");
        }

        public static void ValidateScenes()
        {
            EditorSceneManager.OpenScene(BootstrapPath, OpenSceneMode.Single);
            if (UnityEngine.Object.FindObjectOfType<BootstrapController>() == null)
            {
                throw new InvalidOperationException("BootstrapController component was not found.");
            }

            EditorSceneManager.OpenScene(LoadingPath, OpenSceneMode.Single);
            if (UnityEngine.Object.FindObjectOfType<LoadingController>() == null)
            {
                throw new InvalidOperationException("LoadingController component was not found.");
            }

            string[] enabledPaths = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();
            if (!enabledPaths.Contains(BootstrapPath) || !enabledPaths.Contains(LoadingPath))
            {
                throw new InvalidOperationException("Bootstrap or LoadingScene is missing from Build Settings.");
            }

            if (EditorBuildSettings.scenes.Any(scene => scene.enabled && scene.guid.Empty()))
            {
                throw new InvalidOperationException("An enabled Scene has an empty GUID in Build Settings.");
            }

            Debug.Log("[FishingGame] Scene validation passed.");
        }

        private static void CreateBootstrapScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateCamera(new Color(0.035f, 0.08f, 0.12f));
            new GameObject("Bootstrap").AddComponent<BootstrapController>();
            EditorSceneManager.SaveScene(scene, BootstrapPath);
        }

        private static void CreateLoadingScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateCamera(new Color(0.02f, 0.035f, 0.065f));
            new GameObject("LoadingController").AddComponent<LoadingController>();
            EditorSceneManager.SaveScene(scene, LoadingPath);
        }

        private static void CreateCamera(Color background)
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = background;
            cameraObject.AddComponent<AudioListener>();
        }

        private static void UpdateBuildSettings()
        {
            var scenePaths = new[] { BootstrapPath, LoadingPath };
            var existing = EditorBuildSettings.scenes
                .Where(scene => !scenePaths.Contains(scene.path))
                .ToList();

            var scenes = new List<EditorBuildSettingsScene>
            {
                CreateBuildSettingsScene(BootstrapPath),
                CreateBuildSettingsScene(LoadingPath)
            };
            scenes.AddRange(existing);
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private static EditorBuildSettingsScene CreateBuildSettingsScene(string scenePath)
        {
            string guidText = AssetDatabase.AssetPathToGUID(scenePath);
            if (string.IsNullOrEmpty(guidText))
            {
                throw new InvalidOperationException("Scene GUID was not found: " + scenePath);
            }

            return new EditorBuildSettingsScene(new GUID(guidText), true);
        }

        private static void EnsureDirectory(string assetPath)
        {
            if (!string.IsNullOrEmpty(assetPath) && !Directory.Exists(assetPath))
            {
                Directory.CreateDirectory(assetPath);
            }
        }
    }
}
#endif
