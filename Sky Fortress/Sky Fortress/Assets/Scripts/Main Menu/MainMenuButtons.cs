using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour {

    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject rulesPanel;

    [SerializeField] private string youtubeLink;
    private SceneFader sf;

    private bool showRules = false;
    private bool showControls = false;


    


    // Use this for initialization
    void Start () {

        controlsPanel.SetActive(false);
        rulesPanel.SetActive(false);
        sf = FindObjectOfType<SceneFader>();
    }




    public void Play()
    {
        GetComponent<AudioSource>().Play();
        sf.FadeToScene(1);
    }










    public void Rules(bool b)
    {
        if (showControls)
            return;

        showRules = b;

        if (b)
            rulesPanel.SetActive(b);
        else
            rulesPanel.GetComponent<Animator>().SetTrigger("hideUI");
    }





    public void Controls(bool b)
    {

        if (showRules)
            return;

        showControls = b;

        if (b)
            controlsPanel.SetActive(b);
        else
            controlsPanel.GetComponent<Animator>().SetTrigger("hideUI");

    }








    public void Youtube()
    {
        Application.OpenURL(youtubeLink);
    }

    public void Quit()
    {
        sf.FadeToQuitScene();
    }
}
