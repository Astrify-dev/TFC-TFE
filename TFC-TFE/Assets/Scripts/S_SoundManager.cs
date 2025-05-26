using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SoundManager : MonoBehaviour
{
    [SerializeField] private SoundSystem MusicAmbiance;

    private void Start(){
        if (MusicAmbiance != null){
            MusicAmbiance.Play();
        }
    }
}
