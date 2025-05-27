using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class S_Sign : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] AnimationCurve curve; //evaluate
    [SerializeField] float animationSpeed = 1f;
    [SerializeField] float minDistance = 10f;
    private bool playerIsEnter = false;

    private Vector3 originalScale;

    IEnumerator animSpawn;
    IEnumerator animExit;

    void Start()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        playerIsEnter = false;
    }

    private void Update()
    {
        float distance = Vector3.Distance(playerPos.position, transform.position);

        if (!playerIsEnter && distance < minDistance)
        {
            playerIsEnter = true;

            animSpawn = spawnSign(true);
            StartCoroutine(animSpawn);
        }

        if(playerIsEnter && distance > minDistance)
        {
            playerIsEnter = false;

            animExit = spawnSign(false);
            StartCoroutine(animExit);
        }    
    }

    IEnumerator spawnSign( bool enable)
    {
        float Timer = 0;

        while (Timer < 1)
        {
            Timer += Time.deltaTime * animationSpeed;
            float Value = curve.Evaluate(enable? Timer : 1 - Timer); // for doing +1 or -1 in the anim

            transform.localScale = originalScale * Value;

            yield return null;
            
        }
        transform.localScale = originalScale * (enable ? 1 : 0); // to reset values correctly
    }
}
