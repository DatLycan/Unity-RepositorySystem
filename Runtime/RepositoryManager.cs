using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DatLycan.Packages.RepositorySystem {

    public static class RepositoryManager {
        private static readonly List<IRepository> Repositories = new();

        public static IRepository GetOrAdd(Type type)
            => Repositories.FirstOrDefault(r => r.GetType() == type) ?? GetOrCreateInstance(type);

        public static IRepository GetOrAdd<T>() where T : class, IRepository
            => Repositories.FirstOrDefault(r => r.GetType() == typeof(T)) ?? GetOrCreateInstance(typeof(T));

        private static IRepository GetOrCreateInstance(Type type) {
            IRepository repo = null;

            if (type.IsSubclassOf(typeof(ScriptableObject))) {
                if (!ValidateScriptableObjectType(type)) return null;
                repo = LoadScriptableObjectAsset(type);
            } else {
                repo = (IRepository)Activator.CreateInstance(type);
            }

            Register(repo);

            return repo;
        }

        private static bool ValidateScriptableObjectType(Type type) {
            if (typeof(ScriptableObject).IsAssignableFrom(type)) return true;
            
            Debug.LogError($"{type.Name} is not a ScriptableObject.");
            return false;

        }

        private static IRepository LoadScriptableObjectAsset(Type type) {
            string[] guids = AssetDatabase.FindAssets($"t:{type.Name}");

            switch (guids.Length) {
                case > 1:
                    Debug.LogWarning(
                        $"Multiple instances of {type.Name} found. Ensure only one asset exists in the project."
                    );
                    break;
                case 0:
                    Debug.LogError($"No instance of {type.Name} found. Ensure the asset exists in the project.");
                    return null;
            }

            string path = AssetDatabase.GUIDToAssetPath(guids.First());
            ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (scriptableObject != null && scriptableObject is IRepository repo) return repo;
            
            Debug.LogError($"{type.Name} found in assets is not an implementation of IRepository.");
            return null;

        }

        public static void Register(IRepository repo) => Repositories.Add(repo);
        public static void Unregister(IRepository repo) => Repositories.Remove(repo);
    }

    public interface IRepository {
    }

}
