using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnAnim : MonoBehaviour {

    public void DisableParentOnAnim()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void DisableThisOnAnim()
    {
        gameObject.SetActive(false);
    }
}
