using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RTS.Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameplaySceneData gameplaySceneData;

        public override void Initialize()
        {
            DontDestroyOnLoad(gameObject);
            //TODO: MainMenu
            Booter.OnBooterLoaded += StartGame;
            base.Initialize();
        }

        #region Start Game
        public void StartGame()
        {
            Booter.OnBooterLoaded -= StartGame;
            InitializeGameplay(gameplaySceneData);
        }

        public static async void InitializeGameplay(GameplaySceneData data)
        {
            await GameSceneManager.LoadScene(data.LoadScene);
            SpawnServiceManager();
            await GameSceneManager.LoadScene(data.ControllersScene);
            await Task.Delay(2000);
            foreach (var scene in data.GetGameplayScenes())
                await GameSceneManager.LoadScene(scene);
            await Task.Delay(2000);
            await GameSceneManager.UnloadScene(data.LoadScene);
        }

        private static void SpawnServiceManager()
        {
            var prefab = Addressables.LoadAssetAsync<ServiceManager>(nameof(ServiceManager)).WaitForCompletion();
            Addressables.InstantiateAsync(prefab).WaitForCompletion();
        }
        #endregion
    }
}
