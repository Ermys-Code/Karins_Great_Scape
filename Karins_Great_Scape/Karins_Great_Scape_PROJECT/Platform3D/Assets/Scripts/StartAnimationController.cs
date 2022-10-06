using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartAnimationController : MonoBehaviour
{
    public Camera cam;
    public Transform karin, iris, boss, jailDoor;
    public Transform startCamPoint, secondCamPoint;
    public Transform startIrisPoint, openJailPoint, secondIrisPosition, thirdIrisPosition;
    public Transform startKarinPoint, secondKarinPoint;
    public GameObject indicationPanel, dialogPanel, blackPanel;
    public Text dialogText;
    public Image dialogImage;
    public Animator karinAnim, irisAnim;


    public int currentDialog;
    public float dialogTimer, alphaValue;
    public bool firstScene = true, transition, secondScene, thirdScene, fadeIn, done;

    [System.Serializable]
    public class DialogListClass
    {
        public Sprite textImage;
        [TextArea(0, 300)]
        public string textDialog;
    } 
    public List<DialogListClass> dialogList;

    private void Start()
    {
        karinAnim.SetBool("Grounded", true);
    }

    void FixedUpdate()
    {

        if (firstScene)
        {
            FirstScene();
        }
        
        if (secondScene)
        {
            SecondScene();
        }

        if (thirdScene)
        {
            ThitdScene();
        }
    }

    private void Update()
    {
        if (transition)
        {
            Transition();
        }

        if (fadeIn)
        {
            alphaValue += 1f * Time.deltaTime;
            blackPanel.GetComponent<CanvasGroup>().alpha = alphaValue;

            if (blackPanel.GetComponent<CanvasGroup>().alpha == 1)
            {
                StartCoroutine(ChangeScene());
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene(2);
        }
    }

    void FirstScene()
    {
        cam.transform.LookAt(iris);

        iris.position = Vector3.MoveTowards(iris.position, openJailPoint.position, 3 * Time.deltaTime);
        dialogPanel.gameObject.SetActive(true);
        dialogText.text = dialogList[currentDialog].textDialog;
        dialogImage.sprite = dialogList[currentDialog].textImage;
        dialogTimer += Time.deltaTime;

        if (iris.position != openJailPoint.position) irisAnim.SetBool("Walk", true);
        else irisAnim.SetBool("Walk", false);

        switch (currentDialog)
        {
            case 0:
                if (dialogTimer >= 5) NextDialog();
                break;

            case 1:
                if (dialogTimer >= 4) EndFirstDialog();
                break;
        }
    }

    void Transition()
    {
        indicationPanel.SetActive(true);
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire3"))
        {
            indicationPanel.SetActive(false);
            transition = false;
            secondScene = true;
        }
    }

    void SecondScene()
    {
        jailDoor.rotation = Quaternion.Lerp(jailDoor.rotation, Quaternion.Euler(-90, 0, 0), 2 * Time.deltaTime);
        iris.position = Vector3.MoveTowards(iris.position, secondIrisPosition.position, 1 * Time.deltaTime);
        karin.position = Vector3.MoveTowards(karin.position, secondKarinPoint.position, 2 * Time.deltaTime);
        if(karin.position != secondKarinPoint.position)
        {
            Vector3 lookPosition = secondKarinPoint.position;
            lookPosition.y = karin.position.y;
            karin.transform.LookAt(lookPosition);
        }
        else
        {
            Vector3 lookIris = iris.position;
            lookIris.y = karin.position.y;
            var targetRotation = Quaternion.LookRotation(lookIris - karin.position);
            karin.transform.rotation = Quaternion.Lerp(karin.rotation, targetRotation, 2 *Time.deltaTime);
        }

        cam.transform.position = Vector3.MoveTowards(cam.transform.position, secondCamPoint.position, 1 * Time.deltaTime);

        if (karin.position != secondKarinPoint.position) karinAnim.SetBool("Run", true);
        else karinAnim.SetBool("Run", false);

        if (currentDialog < 4)
        {
            cam.transform.LookAt(karin);
            dialogPanel.SetActive(true);


            dialogText.text = dialogList[currentDialog].textDialog;
            dialogImage.sprite = dialogList[currentDialog].textImage;
            dialogTimer += Time.deltaTime;

            switch (currentDialog)
            {

                case 2:
                    if (dialogTimer >= 5.5f) NextDialog();
                    break;

                case 3:
                    if (dialogTimer >= 5) NextDialog();
                    break;
            }
        }
        if (iris.position == secondIrisPosition.position && karin.position == secondKarinPoint.position && currentDialog >= 4)
        {
            cam.transform.LookAt(boss);
            dialogPanel.SetActive(true);

            dialogText.text = dialogList[currentDialog].textDialog;
            dialogImage.sprite = dialogList[currentDialog].textImage;
            dialogTimer += Time.deltaTime;

            switch (currentDialog)
            {
                case 4:
                    if (dialogTimer >= 6.5f) NextDialog();
                    break;

                case 5:
                    if (dialogTimer >= 8.5f) NextDialog();
                    break;

                case 6:
                    if (dialogTimer >= 3.5f) EndSecondDialog();
                    break;

            }
        }
    }

    void ThitdScene()
    {
        cam.transform.LookAt(iris);

        iris.position = Vector3.MoveTowards(iris.position, thirdIrisPosition.position, 1 * Time.deltaTime);
        iris.transform.LookAt(karin);
        Vector3 lookPosition = iris.position;
        lookPosition.y = karin.position.y;
        karin.transform.LookAt(lookPosition);

        if (iris.position != thirdIrisPosition.position) irisAnim.SetBool("Walk", true);
        else irisAnim.SetBool("Walk", false);

        dialogText.text = dialogList[currentDialog].textDialog;
        dialogImage.sprite = dialogList[currentDialog].textImage;
        dialogTimer += Time.deltaTime;

        switch (currentDialog)
        {
            case 7:
                if (dialogTimer >= 6) NextDialog();
                break;

            case 8:
                if (dialogTimer >= 6) NextDialog();
                break;

            case 9:
                if (dialogTimer >= 6) NextDialog();
                break;

            case 10:
                if (dialogTimer >= 5) EndThirdDialog();
                break;
        }
    }

    void NextDialog()
    {
        dialogTimer = 0;
        currentDialog++;
    }

    void EndFirstDialog()
    {
        dialogPanel.SetActive(false);
        dialogTimer = 0;
        currentDialog++;
        firstScene = false;
        transition = true;
    }

    void EndSecondDialog()
    {
        dialogTimer = 0;
        currentDialog++;
        secondScene = false;
        thirdScene = true;
    }

    void EndThirdDialog()
    {
        secondScene = false;
        blackPanel.SetActive(true);
        fadeIn = true;
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }
}
