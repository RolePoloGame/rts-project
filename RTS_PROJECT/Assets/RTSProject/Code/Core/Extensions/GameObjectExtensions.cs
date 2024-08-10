using UnityEngine;

namespace RTS.Core
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent(out T component))
                component = gameObject.AddComponent<T>();
            return component;
        }

        public static T GetStoredComponent<T>(this GameObject gameObject, T element) where T : Component
        {
            if (element == null)
                return gameObject.GetOrAddComponent<T>();
            return element;
        }

        public static Bounds GetMeshBounds(this GameObject go)
        {
            MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();

            if (meshFilters.Length == 0)
                return new Bounds();

            Bounds bound = meshFilters[0].mesh.bounds;
            for (int i = 1; i < meshFilters.Length; i++)
                bound.Encapsulate(meshFilters[i].mesh.bounds);
            return bound;

        }
    }
}