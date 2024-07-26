using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DatLycan.Packages.RepositorySystem {
    [Serializable]
    public class Reference<T> where T : class, IRepository {
        [SerializeField] private protected string repositoryAssemblyQualifiedName;
        private protected T cachedRepository;

        public T Repository {
            get {
                if (cachedRepository == null || repositoryAssemblyQualifiedName != cachedRepository.GetType().AssemblyQualifiedName) {
                    repositoryAssemblyQualifiedName ??= typeof(T).AssemblyQualifiedName;
                    var repositoryType = Type.GetType(repositoryAssemblyQualifiedName) 
                                         ?? throw new InvalidOperationException($"Type '{typeof(T).Name}' not found.");
                    cachedRepository = (T)RepositoryManager.GetOrAdd(repositoryType);
                }
                return cachedRepository;
            }
        }

        public static implicit operator T(Reference<T> reference) => reference.Repository;
    }
}
