using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsAnim : MonoBehaviour
{
    public Image elementUI;
    public Sprite[] animState;

    public Animator anim;

    private void OnMouseEnter()
    {
        elementUI.sprite = animState[1];
        anim.SetBool("PlayButton", true);
    }

    private void OnMouseExit()
    {
        elementUI.sprite = animState[0];
        anim.SetBool("PlayButton", false);
    }
}
