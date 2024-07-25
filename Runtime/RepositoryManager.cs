using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DatLycan.Packages.RepositorySystem {
    public static class RepositoryManager {
        private static readonly List<IRepository> repositories = new();

        public static IRepository GetOrAdd(Type type)
            => repositories.FirstOrDefault(r => r.GetType() == type) ?? CreateInstance(type);
        
        public static IRepository GetOrAdd<T>() where T : class, IRepository 
            => repositories.FirstOrDefault(r => r.GetType() == typeof(T)) ?? CreateInstance(typeof(T));
        
        private static IRepository CreateInstance(Type type) {
            IRepository repo;

            if (type.IsSubclassOf(typeof(ScriptableObject))) {
                Object[] objects = Resources.FindObjectsOfTypeAll(type);

                if (objects.Length > 1) 
                    Debug.LogWarning($"Multiple instances of {type.Name} found. Ensure only one asset exists in the project.");

                if (objects.IsNullOrEmpty()) 
                    Debug.LogError($"No instance of {type.Name} found. Ensure the asset exists in the project.");

                repo = objects.FirstOrDefault() as IRepository;
            } else {
                repo = (IRepository)Activator.CreateInstance(type);
            }

            Register(repo);
            return repo;
        }

        public static void Register(IRepository repo) => repositories.Add(repo);
        public static void Unregister(IRepository repo) => repositories.Remove(repo);
    }
    
    public interface IRepository { }
}
