using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndAnimationController : MonoBehaviour
{
    public RaycastHit hitInfo;
    public Ray ray;
    public GameObject winPanel, creditsPanel, menuButton, backButton, creditsButton, dialogPanel;
    public Animator animMenu, animCredits, animCreditsPanel;
    public Transform pointToLook, firstPoint, secondPoint, thirdPoint;
    public Transform firstKarinPoint, secondKarinPoint;
    public Camera cam;
    public GameObject karin;
    public Text dialogText;
    public Image dialogImage;
    public Text coinsValue, deathValue;
    public Image allCoinsImage, killTenImage, killThirtyImage, slimesImage, killBossImage;
    public Animator karinAnim, irisAnim, bossAnim;

    public bool animationDone = false, fadeInDone = false, dialogDone;
    bool menuAnimDone, creditsAnimDone;
    float fadeInValue;
    public int currentPoint;
    public int currentDialog;
    public float dialogTimer;
    bool allCoins, killTen, killThirty, slimes, killBoss;

    [System.Serializable]
    public class Dialogue
    {
        public Sprite dialogSprite;
        [TextArea(0, 300)]
        public string textDialog;
    }
    public List<Dialogue> dialogueList;

    void Start()
    {
        coinsValue.text = StaticPlayerValues.PlayerValues.coinsCount.ToString();
        deathValue.text = StaticPlayerValues.PlayerValues.totalDeaths.ToString();
        allCoins = StaticPlayerValues.PlayerValues.allCoins;
        killTen = StaticPlayerValues.PlayerValues.killTen;
        killThirty = StaticPlayerValues.PlayerValues.killThirty;
        slimes = StaticPlayerValues.PlayerValues.slimes;
        killBoss = StaticPlayerValues.PlayerValues.killBoss;
        SetAchievements();
        karinAnim.SetBool("Grounded", true);
        karinAnim.SetBool("Run", true);
        irisAnim.SetBool("Walk", true);
    }

    void Update()
    {
        if(animationDone) FadeIn();
        if (fadeInDone) MouseDetect();
        if(bossAnim.GetBool("Death") == false) bossAnim.SetBool("Death", true);
    }

    private void FixedUpdate()
    {
        if (!animationDone) Animation();
    }

    public void Animation()
    {
        cam.transform.LookAt(pointToLook);

        karin.transform.position = Vector3.MoveTowards(karin.transform.position, secondKarinPoint.position, 2 * Time.deltaTime);
        dialogPanel.gameObject.SetActive(true);
        dialogText.text = dialogueList[currentDialog].textDialog;
        dialogImage.sprite = dialogueList[currentDialog].dialogSprite;
        dialogTimer += Time.deltaTime;

        switch (currentDialog)
        {
            case 0:
                if (dialogTimer >= 3) NextDialogue();
                break;

            case 1:
                if (dialogTimer >= 3) EndDialogue();
                break;
        }

        switch (currentPoint)
        {
            case 0:
                pointToLook.position = Vector3.MoveTowards(pointToLook.position, secondPoint.position, 5 * Time.deltaTime);
                if (pointToLook.position == secondPoint.position)
                {
                    if (dialogDone) currentPoint++;
                }

                break;

            case 1:
                pointToLook.position = Vector3.MoveTowards(pointToLook.position, thirdPoint.position, 5 * Time.deltaTime);
                if (pointToLook.position == thirdPoint.position) animationDone = true;
                break;
        }
    }

    public void FadeIn()
    {
        if (winPanel.activeSelf == false) winPanel.SetActive(true);
        winPanel.GetComponent<CanvasGroup>().alpha = fadeInValue;
        if (winPanel.GetComponent<CanvasGroup>().alpha < 1) fadeInValue += 0.5f * Time.deltaTime;
        if(winPanel.GetComponent<CanvasGroup>().alpha == 1)
        {
            fadeInDone = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void MouseDetect()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.tag == "Menu")
            {
                hitInfo.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/WinMenu/BackMainMenu2");

                if (!menuAnimDone)
                {
                    menuAnimDone = true;
                    animMenu.SetBool("MenuButton", true);
                }
            }
            if (hitInfo.transform.tag == "Credits")
            {
                hitInfo.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/WinMenu/Credits2");

                if (!creditsAnimDone)
                {
                    creditsAnimDone = true;
                    animCredits.SetBool("CreditsButton", true);
                }
            }
            if (hitInfo.transform.tag == "Back")
            {
                hitInfo.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/ExitButton2");
            }
        }
        else
        {
            menuButton.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/WinMenu/BackMainMenu");
            creditsButton.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/WinMenu/Credits");
            backButton.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/ExitButton");

            animMenu.SetBool("MenuButton", false);
            menuAnimDone = false;
            animCredits.SetBool("CreditsButton", false);
            creditsAnimDone = false;
        }
    }

    void SetAchievements()
    {
        if (allCoins)
        {
            allCoinsImage.rectTransform.sizeDelta = new Vector2(520, 58);
            allCoinsImage.sprite = Resources.Load<Sprite>("Sprites/Achievements/Coins");
        }

        if (killTen)
        {
            killTenImage.rectTransform.sizeDelta = new Vector2(520, 58);
            killTenImage.sprite = Resources.Load<Sprite>("Sprites/Achievements/10Guards");
        }

        if (killThirty)
        {
            killThirtyImage.rectTransform.sizeDelta = new Vector2(520, 58);
            killThirtyImage.sprite = Resources.Load<Sprite>("Sprites/Achievements/30Guards");
        }

        if (slimes)
        {
            slimesImage.rectTransform.sizeDelta = new Vector2(520, 58);
            slimesImage.sprite = Resources.Load<Sprite>("Sprites/Achievements/Slimes");
        }

        if (killBoss)
        {
            killBossImage.rectTransform.sizeDelta = new Vector2(520, 58);
            killBossImage.sprite = Resources.Load<Sprite>("Sprites/Achievements/King");
        }

    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void CreditsButton()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCreditsButton()
    {
        animCreditsPanel.SetTrigger("ClosePanel");
    }

    void NextDialogue() 
    {
        dialogTimer = 0;
        currentDialog++;
    }

    void EndDialogue()
    {
        dialogPanel.SetActive(false);
        dialogDone = true;
    }
}
