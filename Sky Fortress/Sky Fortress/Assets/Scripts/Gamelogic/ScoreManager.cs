using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public GameObject scoreCanvas;
    public GameObject winCanvas;
    public GameObject deathCanvas;
    public StatsSystem[] players; //Le remplir à la main, c'est plus sûr
    public Transform[] playersInfos; //Le remplir à la main, c'est plus sûr
    public Transform[] spawnPoints; //Le remplir à la main, c'est plus sûr

    [Space(10)]

    [SerializeField] private AudioSource timesUpAppear;
    [SerializeField] private AudioSource timesUpDisppear;


    [Space(10)]

    public Image playerHealthBar;
    public Image playerWeaponBar;
    public Image playerWeaponLogo;
    public Color maxUpgradeBarColor;

    [Space(10)]

    public KeyCode[] touchesActivation;

    [Space(10)]

    public float countdown = 180f;
    private float countdownTimer;
    public TextMeshProUGUI countdownText;

    [HideInInspector] public bool isGameFinished = false;

    public static ScoreManager instance;



    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    // Use this for initialization
    void Start () {

        countdownTimer = countdown;

        scoreCanvas.SetActive(true);

        winCanvas.SetActive(false);
        winCanvas.transform.GetChild(0).gameObject.SetActive(false);
        winCanvas.transform.GetChild(1).gameObject.SetActive(false);


        deathCanvas.SetActive(false);

        UpdateHealthUI(1);



        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        }



        for (int i = 0; i < playersInfos.Length; i++)
        {
            playersInfos[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = "0/0";
        }

    }






    private void Update()
    {
        //print(PauseMenuButtons.instance.gameCanBePaused);

        for (int i = 0; i < touchesActivation.Length; i++)
        {
            if (Input.GetKeyDown(touchesActivation[i]))
            {
                if (!PauseMenuButtons.instance.isGamePaused && !isGameFinished)
                {
                    scoreCanvas.SetActive(!scoreCanvas.activeSelf);
                }
                else
                {
                    scoreCanvas.SetActive(false);
                }
            }
        }

        MatchCountdown();
    }


    
        private void MatchCountdown()
        {
            //Tant que le timer n'a pas atteint 0, le match continue
            if (countdownTimer > 0f)
            {
                countdownTimer -= Time.deltaTime;
                if (countdownTimer < 0f)
                {
                    countdownTimer = 0f;
                }

                ConvertTime(countdownTimer);

            }
            //Sinon, on arrête le match
            else
            {
                if (!isGameFinished)
                    EndGame();
            }
        }








    private void ConvertTime(float timer)
    {
        //Utilisé pour convertir le temps en minutes et secondes avant de l'afficher sur l'UI
        int min = (int)Mathf.Floor(timer / 60);
        int sec = (int)(timer % 60);

        if (sec == 60)
        {
            sec = 0;
            min++;
        }

        string minutes = min.ToString("0");
        string seconds = sec.ToString("00");

        countdownText.text = string.Format("{0}:{1}", minutes, seconds);
    }








    private void UpdateScoreUI()
    {
        for (int i = 0; i < players.Length; i++)
        {
            playersInfos[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = players[i].kills + "/" + players[i].deaths;
        }
    }










    public void UpdateWeaponUI()
    {
        if (!players[0].ms.currentWeapon.isAnUpgrade)
        {
            playerWeaponBar.fillAmount = players[0].ms.currentWeapon.currentExp / players[0].ms.currentWeapon.upgradeExp;

            //print(players[0].ms.currentWeapon.currentExp + " " +  players[0].ms.currentWeapon.upgradeExp);

            playerWeaponBar.color = players[0].ms.currentWeapon.logoColor;
            playerWeaponLogo.sprite = players[0].ms.currentWeapon.weaponLogo;
            playerWeaponLogo.color = players[0].ms.currentWeapon.logoColor;
        }
        else
        {
            playerWeaponBar.fillAmount = 1f;
            playerWeaponLogo.sprite = players[0].ms.currentWeapon.logoUpgrade;
            playerWeaponLogo.color = players[0].ms.currentWeapon.logoColor;
            playerWeaponBar.color = maxUpgradeBarColor;
        }
    }












    public void UpdateHealthUI(int joueurID)
    {
        if (!players[0].isDead)
        {
            playerHealthBar.fillAmount = players[0].currentHealth / players[0].maxHealth;
        }
        else
        {
            playerHealthBar.fillAmount = 0f;

            if(deathCanvas.activeSelf == false && joueurID == 1)
            deathCanvas.SetActive(true);
        }

        if (players[joueurID - 1].isDead)
        {
            StartCoroutine(RespawnPlayer(joueurID - 1));
        }
    }





    private IEnumerator RespawnPlayer(int index)
    {
        yield return new WaitForSeconds(3f);
        deathCanvas.gameObject.SetActive(false);
        players[index].Reset();

        UpdateWeaponUI();
        UpdateHealthUI(players[index].ms.joueurID);

        players[index].transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
    }















    public void UpdateScore(int killedPlayerID, int bulletID)
    {
        players[killedPlayerID - 1].deaths += 1f;

        if (bulletID != 0)
        {
            players[bulletID - 1].kills += 1f;



            players[bulletID - 1].ms.currentWeapon.GainExp();
        }


        UpdateScoreUI();

        if(bulletID == 1 || killedPlayerID == 1)
        UpdateWeaponUI();
    }









    public void EndGame()
    {
        isGameFinished = true;

        StartCoroutine(CheckWinner());
    }

    private IEnumerator CheckWinner()
    {
        winCanvas.SetActive(true);
        scoreCanvas.SetActive(false);
        PlayerManager.instance.StopAllPlayersMovement();


        //On active juste le texte du timer pour indiquer au joueur que la partie est finie
        timesUpAppear.Play();
        winCanvas.transform.GetChild(0).gameObject.SetActive(true);     
        yield return new WaitForSeconds(4f);
        timesUpDisppear.Play();
        winCanvas.transform.GetChild(0).gameObject.SetActive(false);





        //We check which player has won the match, and if there were ex-aequo winners
        if(GetWinner().Count > 1)
        {
            string str = GetWinner()[0].ms.joueurID.ToString();

            for (int i = 1; i < GetWinner().Count-1; i++)
            {
                str = string.Format("{0}, {1}", str, GetWinner()[i].ms.joueurID.ToString());

            }

            str = string.Format("{0} and {1}", str, GetWinner()[GetWinner().Count - 1].ms.joueurID.ToString());


            winCanvas.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Players " + str + " have won the match!";
        }
        else if(GetWinner().Count == 1)
        {
            winCanvas.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Player " + GetWinner()[0].ms.joueurID.ToString() + " has won the match!";
        }





        //Puis on active le tableau des scores
        winCanvas.transform.GetChild(1).gameObject.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }



    private List<StatsSystem> GetWinner()
    {
        StatsSystem winner = players[0];
        List<StatsSystem> exAequoWinners = new List<StatsSystem>();
        
        bool hasExAequo = false;

        for (int i = 0; i < players.Length; i++)
        {
            winCanvas.transform.GetChild(1).GetChild(0).GetChild(i + 1).GetChild(1).GetComponent<TextMeshProUGUI>().text = players[i].kills + "/" + players[i].deaths;
            winCanvas.transform.GetChild(1).GetChild(0).GetChild(i + 1).GetChild(2).GetComponent<TextMeshProUGUI>().text = players[i].GetKDR().ToString();
        }

        for (int i = 0; i < players.Length; i++)
        {


            if(players[i] != winner && players[i].GetKDR() > winner.GetKDR())
            {
                winner = players[i];
                exAequoWinners.Clear();
            }
            else if (players[i] != winner && players[i].GetKDR() == winner.GetKDR())
            {
                hasExAequo = true;

                if (!exAequoWinners.Contains(winner))
                {
                    exAequoWinners.Add(winner);
                }

                exAequoWinners.Add(players[i]);
            }

        }

        if (!hasExAequo)
        {

            exAequoWinners.Clear();
            exAequoWinners.Add(winner);
        }

        return exAequoWinners;
    }
}
