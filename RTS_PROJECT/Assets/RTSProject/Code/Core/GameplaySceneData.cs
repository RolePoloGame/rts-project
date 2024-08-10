using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

#if UNITY_EDITOR
using UnityEditor;
using NaughtyAttributes;
#endif

namespace RTS.Core
{
    [CreateAssetMenu(menuName = "RTS/Game/Gameplay Scene Data")]
    public class GameplaySceneData : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] protected List<AssetReference> gameplayScenes = new();
        [SerializeField] private AssetReference loadSceneReference;
        [SerializeField] private AssetReference controllersReference;

        private void OnValidate() => SerializeReferences();

        [Button("Reserialize", EButtonEnableMode.Editor)]
        private void SerializeReferences()
        {
            if (Application.isPlaying) return;
            gameplayScenesInOrder.Clear();
            foreach (var scene in gameplayScenes)
                if (scene != null)
                    gameplayScenesInOrder.Add(scene.AssetGUID);
            loadScene = loadSceneReference != null ? loadSceneReference.AssetGUID : string.Empty;
            controllers = controllersReference != null ? controllersReference.AssetGUID : string.Empty;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
#endif
        [SerializeField, ReadOnly] protected List<string> gameplayScenesInOrder = new();
        [SerializeField, ReadOnly] private string loadScene;
        [SerializeField, ReadOnly] private string controllers;

        public string LoadScene => loadScene;
        public string ControllersScene => controllers;
        public virtual List<string> GetGameplayScenes() => gameplayScenesInOrder;
    }
}
