using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace RTS.Core
{
    public static class GameSceneManager
    {
        public static Dictionary<string, SceneInstance> loadedScenes = new();
        public static async Task LoadScene(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            Debug.Log($"[{nameof(GameSceneManager)}] {name.Bold()} is loading...".Italic());

            var handle = Addressables.LoadSceneAsync(name, LoadSceneMode.Additive);

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
    }
}
