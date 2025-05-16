using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBehaviour<T> : SingletonBehaviour where T : MonoBehaviour{
    private static T _instance;
    public static T instance{
        get{
            if (_instance != null) return _instance;
            var instances = FindObjectsOfType<T>();
            var count = instances.Length;
            if (count > 0){
                if (count == 1) return _instance = instances[0];
                Debug.LogWarning($"[{nameof(SingletonBehaviour)}<{typeof(T)}>] There should never be more than one {nameof(SingletonBehaviour)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                for (var i = 1; i < instances.Length; i++) Destroy(instances[i]);
                return _instance = instances[0];
            }

            Debug.Log($"[{nameof(SingletonBehaviour)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
            return _instance = new GameObject($"({nameof(SingletonBehaviour)}){typeof(T)}").AddComponent<T>();
        }
    }

    private bool _theSingleInstance = false;

    private void Awake(){
        if (_instance is not null){
            Destroy(this.gameObject);
            return;
        }

        _instance = this.GetComponent<T>();
        if (_instance is not null){
            _theSingleInstance = true;
            OnInstanceCreated();
        }
    }
    private void OnDestroy(){
        if (!_theSingleInstance) return;

        if (_instance is not null) OnInstanceDestroyed();
        _instance = null;
    }


    public static bool instanceExist { get { return _instance != null; } }

    virtual protected void OnInstanceCreated() { }

    virtual protected void OnInstanceDestroyed() { }
}
public abstract class SingletonBehaviour : MonoBehaviour { }

