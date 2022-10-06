using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject optionsPanel, creditsPanel, galleryPanel;
    public RaycastHit hitInfo;
    public Ray ray;
    GameObject backButton;
    public Transform camPivot;
    public Animator animPlay, animOptions, animGallery, animCredits, animExit, animCreditsPanel, animGalleryPanel;
    public AudioSource sound;

    bool optionsPanelState = false;
    float rotationSpeed = 3.0f;

    private void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        MouseDetect();
    }

    private void FixedUpdate()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu") CameraRotation();
    }

    void CameraRotation()
    {
        camPivot.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OptionsButton()
    {
        optionsPanelState = !optionsPanelState;
        optionsPanel.SetActive(optionsPanelState);
        sound.clip = Resources.Load<AudioClip>("Sounds/UI/buttonSound");
        sound.Play();
    }

    public void GalleryButton()
    {
        galleryPanel.SetActive(true);
        sound.clip = Resources.Load<AudioClip>("Sounds/UI/buttonSound");
        sound.Play();
    }

    public void CreditsButton()
    {
        creditsPanel.SetActive(true);
        sound.clip = Resources.Load<AudioClip>("Sounds/UI/buttonSound");
        sound.Play();
    }

    public void ExitButton()
    {
        print("exit");
        Application.Quit();
    }

    public void ExitCreditsButton()
    {
        animCreditsPanel.SetTrigger("ClosePanel");
        sound.clip = Resources.Load<AudioClip>("Sounds/UI/buttonSound");
        sound.Play();
    }

    public void ExitGalleryButton()
    {
        animGalleryPanel.SetTrigger("ClosePanel");
        sound.clip = Resources.Load<AudioClip>("Sounds/UI/buttonSound");
        sound.Play();
    }

    public void MouseDetect()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.tag == "Back")
            {
                backButton = hitInfo.transform.gameObject;
                backButton.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/ExitButton2");
            }
            else
            {
                if (backButton != null) backButton.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/ExitButton");
            }
        }
        else
        {
            if (backButton != null) backButton.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/ExitButton");
        }
    }


}
