using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAudio : MonoBehaviour {

    private void OnEnable()
    {
        StartCoroutine(Disable(GetComponent<AudioSource>().clip.length));

    }


    private IEnumerator Disable(float delay)
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
