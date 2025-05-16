using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject{
    public void OnPooled();

    public void OnBackToPool();

    public void OnClearPool();
}
