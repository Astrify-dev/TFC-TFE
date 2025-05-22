using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_wallBreak : MonoBehaviour, IbreakWall
{
    [SerializeField] Renderer _render;
    [SerializeField] GameObject _wallCollider;
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _curveDestruct;

    private MaterialPropertyBlock _mpb;
    private IEnumerator _coroutine;

    private void Start()
    {
        _wallCollider.SetActive(true);
        _mpb = new MaterialPropertyBlock();
    }

    public void BreakWall()
    {
        _wallCollider.SetActive(false);
    }

    private IEnumerator _StartDestruct(bool RightBreak)
    {
        float Timer = 0;
        float Value = 0.5f;
        _mpb.SetFloat("_SliderDirBreak", Value);
        _render.SetPropertyBlock(_mpb);

        while (Timer < 1)
        {
            Timer += Time.deltaTime * _speed;

            Value = _curveDestruct.Evaluate(Value) * 0.5f;


            yield return null;
        }
    }




}
