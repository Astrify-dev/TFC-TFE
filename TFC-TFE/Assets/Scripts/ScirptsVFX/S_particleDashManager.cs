using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class S_particleDashManager : MonoBehaviour
{
    [SerializeField] VisualEffect _dashPulse;

    private IEnumerator _dashPulseCoroutineReset;
    private void Start()
    {
        _dashPulse.Stop();
        _dashPulse.gameObject.SetActive(false);
    }

    public void StartDashPulse(Vector3 Position, Vector3 Dir)
    {
        if(_dashPulseCoroutineReset is not null)
            StopCoroutine( _dashPulseCoroutineReset );

        Vector3 NewPos = (Position + Vector3.up * 0.7f) + Dir * 5;
        _dashPulse.transform.position = NewPos;
        _dashPulse.transform.LookAt(Dir + NewPos);
        _dashPulse.gameObject.SetActive(true);
        _dashPulse.Play();

        _dashPulseCoroutineReset = DashPulseReset();
        StopCoroutine( _dashPulseCoroutineReset );

    }

    IEnumerator DashPulseReset()
    {

        yield return new WaitForSeconds(0.5f);
        _dashPulse.gameObject.SetActive(false);
    }
}
