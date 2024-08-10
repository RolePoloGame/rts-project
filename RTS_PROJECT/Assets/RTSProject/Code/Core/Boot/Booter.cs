using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace RTS.Core
{
    public static class Booter
    {
        private enum EBootStepState
        {
            Start,
            InProgress,
            Completed
        }

        public const string BOOTSTRAP_SCENE_NAME = "Bootstrap";
        public const string CORE_SCENE_NAME = "Core";

        public static Task BootTask => bootTask ??= Boot();
        private static Task bootTask;

        public delegate void OnBooterLoadedEvent();
        public static OnBooterLoadedEvent OnBooterLoaded;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void LoadFirstScene()
        {
            LogMessage($"Preloading scenes", EBootStepState.Start, true);
            GameSceneManager.LoadScene(CORE_SCENE_NAME, LoadSceneMode.Single);
            LogMessage($"Preloading finished", EBootStepState.Completed, true);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static async void StartGame()
        {
            try
            {
                Application.targetFrameRate = 60;
                await BootTask;
                OnBooterLoaded?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static async Task Boot()
        {
            LogMessage($"Starting Boot up!", EBootStepState.Start, true);
            await LoadBootstrap();
            LogMessage($"Boot finished!", EBootStepState.Completed, true);
        }

        private static async Task LoadBootstrap()
        {
            LogMessage($"{BOOTSTRAP_SCENE_NAME} initialization... [{Time.time:F4}]", EBootStepState.InProgress);
            await GameSceneManager.LoadScene(BOOTSTRAP_SCENE_NAME);
            LogMessage($"{BOOTSTRAP_SCENE_NAME} initialized [{Time.time:F4}]", EBootStepState.Completed);
        }

        private static void LogMessage(string message, EBootStepState state, bool greatStep = false)
        {
            string value = string.Empty;
            switch (state)
            {
                case EBootStepState.Start:
                    value = message.Color(Color.cyan);
                    break;
                case EBootStepState.InProgress:
                    value = message.Color(Color.yellow);
                    break;
                case EBootStepState.Completed:
                    value = message.Color(Color.green);
                    break;
            }
            value = $"[{nameof(Booter)}] {value}";
            if (greatStep)
                value = value.Bold().Size(14);
            Debug.Log(value);
        }
    }
}