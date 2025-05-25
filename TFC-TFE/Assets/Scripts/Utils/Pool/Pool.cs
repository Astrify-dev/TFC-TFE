using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pool<E> : Pool where E : Object{
    private E _prefab;
    private Queue<E> _elements = new();

    private List<E> _outElements = new();

    private string _path;
    private Transform _poolTransform;
    public Pool(string path){
        _path = path;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    ~Pool(){
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1){
        ClearPool();
    }

    public void ClearPool(){

        for (int i = 0; i < _outElements.Count; i++){
            if (_outElements[i] is IPoolObject obj)
                obj.OnClearPool();

        }
        while (_elements.Count > 0){
            E elem = _elements.Dequeue();
            if (elem is IPoolObject obj)
                obj.OnClearPool();

        }
        _outElements.Clear();
        _elements.Clear();
    }

    public E GetFromPool(){
        if (_prefab is null)
            _prefab = Resources.Load<E>(_path);
        if (_poolTransform is null)
            _poolTransform = new GameObject("[Pool:" + _prefab.name + "]").transform;
        E element = GetNext();
        _outElements.Add(element);
        if (element is IPoolObject)
            ((IPoolObject)element).OnPooled();

        return element;
    }

    public void SetPrefab(E prefab){
        _prefab = prefab;
    }

    public void BackToPool(E element)
    {

        if (_elements.Contains(element)) return;
        if (element is IPoolObject)
            ((IPoolObject)element).OnBackToPool();
        _elements.Enqueue(element);
        _outElements.Remove(element);
    }
    private E GetNext() => _elements.Count <= 0 ? MonoBehaviour.Instantiate<E>(_prefab, _poolTransform) : _elements.Dequeue();

    public List<E> GetActiveElements()
    {
        return _outElements;
    }
}
public abstract class Pool { }
