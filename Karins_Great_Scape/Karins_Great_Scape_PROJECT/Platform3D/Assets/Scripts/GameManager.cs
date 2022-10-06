using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel, settingsPanel;
    public GameObject continueButton, restartButton, optionsButton, menuButton;
    public GameObject player, boss;
    public Image allCoins, tenGuards, thirtyGuards, threeSlimes, killBoss;
    public Text allCoinsText, tenGuardsText, thirtyGuardsText, threeSlimesText;
    public RaycastHit hitInfo;
    public Ray ray;
    public AudioSource sound;

    bool paused;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
        cam.transform.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/Music/ost1");
        sound.Play();
    }

    void Update()
    {
        Pause();

        MouseDetect();

        AchievementsControl();
    }

    void Pause()
    {
        if (paused) Time.timeScale = 0;
        else Time.timeScale = 1;

        if (Input.GetButtonDown("Pause"))
        {
            print("Pause"); 
            if (player.GetComponent<PlayerRigidBody>().playerPaused == false) player.GetComponent<PlayerRigidBody>().playerPaused = true;
            else player.GetComponent<PlayerRigidBody>().playerPaused = false;

            if (cam.GetComponent<ThirdPersonCamera>().camPaused == false) cam.GetComponent<ThirdPersonCamera>().camPaused = true;
            else cam.GetComponent<ThirdPersonCamera>().camPaused = false;

            paused = !paused;
            settingsPanel.SetActive(false);
            pausePanel.SetActive(paused);
            player.GetComponent<PlayerRigidBody>().bossLifePanel.SetActive(!paused);
            player.GetComponent<PlayerRigidBody>().attacksPanel.SetActive(!paused);
            player.GetComponent<PlayerRigidBody>().coinsPanel.SetActive(!paused);
            player.GetComponent<PlayerRigidBody>().lifePanel.SetActive(!paused);
        }
    }

    void MouseDetect()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.tag == "Continue")
            {
                hitInfo.transform.GetComponent<Image>().enabled = true;
                restartButton.transform.GetComponent<Image>().enabled = false;
                optionsButton.transform.GetComponent<Image>().enabled = false;
                menuButton.transform.GetComponent<Image>().enabled = false;
            }
            else if (hitInfo.transform.tag == "Restart")
            {
                hitInfo.transform.GetComponent<Image>().enabled = true;
                continueButton.transform.GetComponent<Image>().enabled = false;
                optionsButton.transform.GetComponent<Image>().enabled = false;
                menuButton.transform.GetComponent<Image>().enabled = false;
            }
            else if (hitInfo.transform.tag == "Options")
            {
                hitInfo.transform.GetComponent<Image>().enabled = true;
                continueButton.transform.GetComponent<Image>().enabled = false;
                restartButton.transform.GetComponent<Image>().enabled = false;
                menuButton.transform.GetComponent<Image>().enabled = false;
            }  
            else if (hitInfo.transform.tag == "Menu")
            {
                hitInfo.transform.GetComponent<Image>().enabled = true;
                continueButton.transform.GetComponent<Image>().enabled = false;
                restartButton.transform.GetComponent<Image>().enabled = false;
                optionsButton.transform.GetComponent<Image>().enabled = false;
            }    
            else
            {
                continueButton.transform.GetComponent<Image>().enabled = false;
                restartButton.transform.GetComponent<Image>().enabled = false;
                optionsButton.transform.GetComponent<Image>().enabled = false;
                menuButton.transform.GetComponent<Image>().enabled = false;
            }
        }
    }

    void AchievementsControl()
    {
        allCoinsText.text = player.GetComponent<PlayerRigidBody>().coinCount.ToString() + " / 319";
        tenGuardsText.text = player.GetComponent<PlayerRigidBody>().enemyCount.ToString() + " / 10";
        thirtyGuardsText.text = player.GetComponent<PlayerRigidBody>().enemyCount.ToString() + " / 30";
        threeSlimesText.text = player.GetComponent<PlayerRigidBody>().slimeCount.ToString() + " / 3";

        if (player.GetComponent<PlayerRigidBody>().coinCount == 319)
        {
            allCoins.rectTransform.sizeDelta = new Vector2(520, 58);
            allCoins.sprite = Resources.Load<Sprite>("Sprites/Achievements/Coins");
            StaticPlayerValues.PlayerValues.allCoins = true;
        }

        if (player.GetComponent<PlayerRigidBody>().enemyCount == 10)
        {
            tenGuards.rectTransform.sizeDelta = new Vector2(520, 58);
            tenGuards.sprite = Resources.Load<Sprite>("Sprites/Achievements/10Guards");
            StaticPlayerValues.PlayerValues.killTen = true;
        }

        if (player.GetComponent<PlayerRigidBody>().enemyCount == 30)
        {
            thirtyGuards.rectTransform.sizeDelta = new Vector2(520, 58);
            thirtyGuards.sprite = Resources.Load<Sprite>("Sprites/Achievements/30Guards");
            StaticPlayerValues.PlayerValues.killThirty = true;
        }

        if (player.GetComponent<PlayerRigidBody>().slimeCount == 3)
        {
            threeSlimes.rectTransform.sizeDelta = new Vector2(520, 58);
            threeSlimes.sprite = Resources.Load<Sprite>("Sprites/Achievements/Slimes");
            StaticPlayerValues.PlayerValues.slimes = true;
        }

        if (boss.gameObject == null && player.GetComponent<PlayerRigidBody>().finalBattleTimer <= 60)
        {
            killBoss.rectTransform.sizeDelta = new Vector2(520, 58);
            killBoss.sprite = Resources.Load<Sprite>("Sprites/Achievements/King");
            StaticPlayerValues.PlayerValues.killBoss = true;

        }

    }

    public void ContinueButton()
    {
        player.GetComponent<PlayerRigidBody>().playerPaused = false;
        
        cam.GetComponent<ThirdPersonCamera>().camPaused = false;

        paused = !paused;
        pausePanel.SetActive(paused);
        player.GetComponent<PlayerRigidBody>().bossLifePanel.SetActive(!paused);
        player.GetComponent<PlayerRigidBody>().attacksPanel.SetActive(!paused);
        player.GetComponent<PlayerRigidBody>().coinsPanel.SetActive(!paused);
        player.GetComponent<PlayerRigidBody>().lifePanel.SetActive(!paused);

        Cursor.lockState = CursorLockMode.Locked;
        sound.clip = Resources.Load<AudioClip>("Sounds/UI/buttonSound");
        sound.Play();
    }

    public void RestartButton()
    {
        paused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void MenuButton()
    {
        paused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void SettingsButton()
    {
        settingsPanel.SetActive(true);
        sound.clip = Resources.Load<AudioClip>("Sounds/UI/buttonSound");
        sound.Play();
    }

    public void SettingsButtonClose()
    {
        sound.clip = Resources.Load<AudioClip>("Sounds/UI/buttonSound");
        sound.Play();
    }
}
