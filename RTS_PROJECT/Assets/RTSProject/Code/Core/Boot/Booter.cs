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

        public static Task BootTask => bootTask ??= Boot();

        public const string BOOTSTRAP_SCENE_NAME = "Bootstrap";
        public const string CORE_SCENE_NAME = "Core";

        private static Task bootTask;
        public static bool Booted => bootTask != null && bootTask.IsCompleted;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadFirstScene()
        {
            LogMessage($"Preloading scenes", EBootStepState.Start, true);
            Addressables.LoadSceneAsync(CORE_SCENE_NAME, LoadSceneMode.Single).WaitForCompletion();
            LogMessage($"Preloading finished", EBootStepState.Completed, true);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static async void StartGame()
        {
            try
            {
                Application.targetFrameRate = 60;
                await BootTask;
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
            var globalBootstrapLoad = Addressables.LoadSceneAsync(BOOTSTRAP_SCENE_NAME, LoadSceneMode.Additive); ;

            LogMessage($"{BOOTSTRAP_SCENE_NAME} scene loading... [{Time.time:F2}]", EBootStepState.InProgress);

            while (!globalBootstrapLoad.IsDone)
            {
                await Task.Delay(20);
            }
            LogMessage($"{BOOTSTRAP_SCENE_NAME} scene loading... [{Time.time:F2}]", EBootStepState.Completed);
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