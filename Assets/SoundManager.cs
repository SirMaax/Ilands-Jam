using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{   


    [SerializeField] AudioSource[] SerialeffectSource;
    static AudioSource[] effectSource;

    private void Start()
    {
        effectSource = new AudioSource[SerialeffectSource.Length];
        effectSource = SerialeffectSource;
    }

    public static void Play(int index){
        effectSource[index].Play();
    }
    public static void Stop(int index){
        effectSource[index].Stop();
    }
    

}

