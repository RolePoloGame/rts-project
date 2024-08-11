using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace RTS.Core
{
    public static class GameSceneManager
    {
        public static Dictionary<string, SceneInstance> loadedScenes = new();
        public static async Task LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            if (string.IsNullOrEmpty(name)) return;
            Debug.Log($"[{nameof(GameSceneManager)}] {name.Bold()} is loading...".Italic());

            var handle = Addressables.LoadSceneAsync(name, mode);

            while (!handle.IsDone)
            {
                await Task.Delay(20);
            }
            loadedScenes.Add(name, handle.Result);
            Debug.Log($"[{nameof(GameSceneManager)}] {name.Bold()} loaded.");
        }
        public static async Task UnloadScene(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            if (!loadedScenes.ContainsKey(name)) return;
            Debug.Log($"[{nameof(GameSceneManager)}] {name.Bold()} is unloading...".Italic());

            var handle = Addressables.UnloadSceneAsync(loadedScenes[name]);
            while (!handle.IsDone)
                await Task.Delay(20);
            loadedScenes.Remove(name);

            Debug.Log($"[{nameof(GameSceneManager)}] {name.Bold()} unloaded.");
        }

        public static void SetActive(string scene)
        {
            if (string.IsNullOrEmpty(scene)) return;
            if (!loadedScenes.ContainsKey(scene)) return;
            SceneManager.SetActiveScene(loadedScenes[scene].Scene);
        }
    }
}
