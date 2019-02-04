using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableAudio : MonoBehaviour {


    private void OnEnable()
    {
        GetComponent<AudioSource>().Play();
    }
}
