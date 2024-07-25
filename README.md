
<h1 align="left">Unity C# Repository System</h1>

<div align="left">

[![Status](https://img.shields.io/badge/status-active-success.svg)]()
[![GitHub Issues](https://img.shields.io/github/issues/datlycan/Unity-RepositorySystem.svg)](https://github.com/DatLycan/Unity-RepositorySystem/issues)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](/LICENSE)

</div>

---

<p align="left"> Centralized data storage solution with full support for Unity's serialization and ScriptableObject's.
    <br> 
</p>

## 📝 Table of Contents

- [Getting Started](#getting_started)
- [Usage](#usage)

## 🏁 Getting Started <a name = "getting_started"></a>

### Installing

Simply import it in Unity with the Unity Package Manager using this URL:

``https://github.com/DatLycan/Unity-RepositorySystem.git``

## 🎈 Usage <a name="usage"></a>


   ```C#
    public class MyRepo : IRepository {
        public string MyString = "My String!";
    }
   ```
   ```C#
    public class MyClass : MonoBehaviour {
        private readonly Reference<MyRepo> reference = new();
    
        private void Start() => Debug.Log(reference.Repository.MyString);
    }
   ```

#### Note:
*When Unity serializes `Reference<T>`, it displays a dropdown of classes that inherit from `T`, or `T` itself if it is not abstract.*


