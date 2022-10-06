using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour
{
    public GameObject cam;
    public Slider sensivityXSlider;
    public Slider sensivityYSlider;
    public Button applyButton, cancelButton;
    public RaycastHit hitInfo;
    public Ray ray;

    void OnEnable()
    {
        sensivityXSlider.value = SettingsValues.sensitivityX;
        sensivityYSlider.value = SettingsValues.sensitivityY;
    }

    private void Update()
    {
        MouseDetect();
    }

    public void CancelButton()
    {
        gameObject.SetActive(false);
    }

    public void ApplyButton()
    {
        SettingsValues.sensitivityX = sensivityXSlider.value;
        SettingsValues.sensitivityY = sensivityYSlider.value;

        gameObject.SetActive(false);
    }

    void MouseDetect()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.tag == "Apply")
            {
                hitInfo.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/ApplyButtonSelected");
            }
            else if (hitInfo.transform.tag == "Cancel")
            {
                hitInfo.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/CancelButtonSelected");
            }
            else
            {
                applyButton.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/ApplyButton");
                cancelButton.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/CancelButton");
            }
        }
        else
        {
            applyButton.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/ApplyButton");
            cancelButton.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/CancelButton");
        }
    }
}
