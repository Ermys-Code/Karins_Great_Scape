using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasControl : MonoBehaviour
{
    public GameObject player;
    public Text dialogueText;
    public Sprite dialogueIrisSprite, dialogueKingSprite;
    public GameObject textPanel;
    public Image dialogueImage;
    public GameObject startBoss;

    public float textTimer;
    public bool textTimerOn, bossEncounter;
    public int currentDialog;

    void Start()
    {
        
    }

    void Update()
    {
        if (textTimerOn)
        {
            player.GetComponent<PlayerRigidBody>().attacksPanel.SetActive(false);
            TextTimer();
        }
            
    }

    void TextTimer()
    {
        textTimer += Time.deltaTime;
        if(textTimer > 5f)
        {
            textTimerOn = false;
            textPanel.SetActive(false);
            player.GetComponent<PlayerRigidBody>().attacksPanel.SetActive(true);
            textTimer = 0;
            if (bossEncounter)
            {
                print("llegooooooo");
                startBoss.GetComponent<BossStart>().StartBattle();
                bossEncounter = false;
                BattleDialog();
            }    
        }
    }

    public void NoKey()
    {
        print("llego");
        dialogueText.text = "No tienes la llave";
        dialogueImage.sprite = dialogueIrisSprite;
        textPanel.SetActive(true);
        textTimerOn = true;
    }

    public void FindCore()
    {
        dialogueText.text = "Look! That’s what I was talking about! Let's go crush it.";
        dialogueImage.sprite = dialogueIrisSprite;
        textPanel.SetActive(true);
        textTimerOn = true;
    }

    public void DestroyFirstCore()
    {
        dialogueText.text = "Wait we broke it and the barrier is still there? There should be more, let's go further.";
        dialogueImage.sprite = dialogueIrisSprite;
        textPanel.SetActive(true);
        textTimerOn = true;
    }

    public void DestroyLastCore()
    {
        dialogueText.text = "Look! It seems that the barrier has weakened, we should go defeat the King, it's our chance!";
        dialogueImage.sprite = dialogueIrisSprite;
        textPanel.SetActive(true);
        textTimerOn = true;
    }

    public void FirstKeyDialog()
    {
        dialogueText.text = "Oh that's a key! We should be able to open those big doors now, let's try!";
        dialogueImage.sprite = dialogueIrisSprite;
        textPanel.SetActive(true);
        textTimerOn = true;
    }

    public void BossEncounter()
    {
        dialogueText.text = "I can't believe this moment has arrived... I won't let you go princess, you will die here!";
        dialogueImage.sprite = dialogueKingSprite;
        textPanel.SetActive(true);
        textTimerOn = true;
        bossEncounter = true;
    }

    public void BattleDialog()
    {
        print("bossDialogue");
        dialogueText.text = "Look at his back! It may be his weak point we should aim there!";
        dialogueImage.sprite = dialogueIrisSprite;
        textPanel.SetActive(true);
        textTimerOn = true;
    }
}