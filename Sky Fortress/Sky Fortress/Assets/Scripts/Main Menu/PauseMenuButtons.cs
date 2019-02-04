using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButtons : MonoBehaviour {

    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject rulesPanel;

    [SerializeField] private string[] touchesActivation;
    private SceneFader sf;


    public bool isGamePaused = false;
    public bool gameCanBePaused = true;
    private bool showRules = false;
    private bool showControls = false;


    public static PauseMenuButtons instance;


    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }



    // Use this for initialization
    void Start()
    {

        pauseCanvas.SetActive(false);
        controlsPanel.SetActive(false);
        rulesPanel.SetActive(false);
        sf = FindObjectOfType<SceneFader>();
    }


    private void Update()
    {
        for (int i = 0; i < touchesActivation.Length; i++)
        {
            if (Input.GetKeyDown(touchesActivation[i]) && gameCanBePaused && !ScoreManager.instance.isGameFinished)
            {
                PauseGame();
            }
        }
    }




    private void PauseGame()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = (isGamePaused) ? 0f : 1f;

        Cursor.visible = isGamePaused;
        Cursor.lockState = (isGamePaused) ? CursorLockMode.None : CursorLockMode.Locked;

        pauseCanvas.SetActive(isGamePaused);

        if (isGamePaused)
        {
            ScoreManager.instance.scoreCanvas.SetActive(false);
        }
    }




    public void Resume()
    {
        PauseGame();
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

    

    public void ReturnToMainMenu()
    {
        sf.FadeToScene(0);
    }
}
