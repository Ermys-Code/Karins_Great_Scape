using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyControl : MonoBehaviour
{
    public Transform fairyPoint;
    public GameObject player;
    public Animator anim;
    public bool walking;
    public bool death;
    public GameObject IrisBody, IrisHair, IrisCrystal1, IrisCrystal2, IrisRing1, IrisRing2, IrisRing3, IrisRing4;
    //public ParticleSystem deathParticle;

    Color colorBody;
    Color colorWings;
    Color colorHair;
    Color colorCrystal1;
    Color colorCrystal2;
    Color colorRing1;
    Color colorRing2;
    Color colorRing3;
    Color colorRing4;
    public bool dying;

    void Start()
    {
    }

    private void FixedUpdate()
    {
        if (player.transform.parent == null)
        {
            transform.position = Vector3.Lerp(transform.position, fairyPoint.position, 5 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, fairyPoint.transform.rotation, 50 * Time.deltaTime);
        }
    }

    private void Update()
    {
        if (player.transform.parent != null)
        {
            transform.position = Vector3.Lerp(transform.position, fairyPoint.position, 5 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, fairyPoint.transform.rotation, 50 * Time.deltaTime);
        }
        if (dying) FadeOut();
    }

    void FadeOut()
    {
        colorBody = IrisBody.GetComponent<Renderer>().materials[0].color;
        // = IrisBody.GetComponent<Renderer>().materials[4].color;


        colorHair = IrisHair.GetComponent<Renderer>().material.color;
        colorCrystal1 = IrisCrystal1.GetComponent<Renderer>().material.color;
        colorCrystal2 = IrisCrystal2.GetComponent<Renderer>().material.color;
        colorRing1 = IrisRing1.GetComponent<Renderer>().material.color;
        colorRing2 = IrisRing2.GetComponent<Renderer>().material.color;
        colorRing3 = IrisRing3.GetComponent<Renderer>().material.color;
        colorRing4 = IrisRing4.GetComponent<Renderer>().material.color;
        colorBody.a -= 0.5f * Time.deltaTime;
        colorWings.a -= 0.5f * Time.deltaTime;
        colorHair.a -= 0.5f * Time.deltaTime;
        colorCrystal1.a -= 0.5f * Time.deltaTime;
        colorCrystal2.a -= 0.5f * Time.deltaTime;
        colorRing1.a -= 0.5f * Time.deltaTime;
        colorRing2.a -= 0.5f * Time.deltaTime;
        colorRing3.a -= 0.5f * Time.deltaTime;
        colorRing4.a -= 0.5f * Time.deltaTime;
        IrisBody.GetComponent<Renderer>().materials[0].color = colorBody;
        //IrisBody.GetComponent<Renderer>().materials[4].color = colorWings;


        IrisHair.GetComponent<Renderer>().material.color = colorHair;
        IrisCrystal1.GetComponent<Renderer>().material.color = colorCrystal1;
        IrisCrystal2.GetComponent<Renderer>().material.color = colorCrystal2;
        IrisRing1.GetComponent<Renderer>().material.color = colorRing1;
        IrisRing2.GetComponent<Renderer>().material.color = colorRing2;
        IrisRing3.GetComponent<Renderer>().material.color = colorRing3;
        IrisRing4.GetComponent<Renderer>().material.color = colorRing4;
        if (colorBody.a <= 0 && colorHair.a <= 0)
        {
            dying = false;
            print(colorHair.a);
            print(colorBody.a);
        }
            
    }

    public void Vanish()
    {
        gameObject.SetActive(false);
        transform.parent = fairyPoint;
        transform.localPosition = Vector3.zero;
    }

    public void Walk()
    {
        if (walking) anim.SetBool("Walk", true);
        if (!walking) anim.SetBool("Walk", false);
    }

    public void Death()
    {
        if (death)
        {
            //deathParticle.gameObject.SetActive(true);
            anim.SetBool("Death", true);

            Material[] mats = new Material[]
            {
                Resources.Load<Material>("IrisMat/Body/1"),
                Resources.Load<Material>("IrisMat/Body/1"),
                Resources.Load<Material>("IrisMat/Body/1"),
                Resources.Load<Material>("IrisMat/Body/1"),
                Resources.Load<Material>("IrisMat/Body/1")
            };

            IrisBody.GetComponent<Renderer>().materials = mats;
            IrisBody.GetComponent<Renderer>().materials = mats;
        

            IrisHair.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/1");
            IrisCrystal1.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/1");
            IrisCrystal2.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/1");
            IrisRing1.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/1");
            IrisRing2.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/1");
            IrisRing3.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/1");
            IrisRing4.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/1");
            dying = true;
        }
            
        if (!death)
        {
            transform.parent = null;
            colorBody = IrisBody.GetComponent<Renderer>().materials[0].color;
            //colorWings = IrisBody.GetComponent<SkinnedMeshRenderer>().materials[4].color;
            colorHair = IrisHair.GetComponent<Renderer>().material.color;
            colorCrystal1 = IrisCrystal1.GetComponent<Renderer>().material.color;
            colorCrystal2 = IrisCrystal2.GetComponent<Renderer>().material.color;
            colorRing1 = IrisRing1.GetComponent<Renderer>().material.color;
            colorRing2 = IrisRing2.GetComponent<Renderer>().material.color;
            colorRing3 = IrisRing3.GetComponent<Renderer>().material.color;
            colorRing4 = IrisRing4.GetComponent<Renderer>().material.color;
            colorBody.a = 1;
            colorWings.a = 1;
            colorHair.a = 1;
            colorCrystal1.a = 1;
            colorCrystal2.a = 1;
            colorRing1.a = 1;
            colorRing2.a = 1;
            colorRing3.a = 1;
            colorRing4.a = 1;
            IrisBody.GetComponent<Renderer>().materials[0].color = colorBody;
            IrisHair.GetComponent<Renderer>().material.color = colorHair;
            IrisCrystal1.GetComponent<Renderer>().material.color = colorCrystal1;
            IrisCrystal2.GetComponent<Renderer>().material.color = colorCrystal2;
            IrisRing1.GetComponent<Renderer>().material.color = colorRing1;
            IrisRing2.GetComponent<Renderer>().material.color = colorRing2;
            IrisRing3.GetComponent<Renderer>().material.color = colorRing3;
            IrisRing4.GetComponent<Renderer>().material.color = colorRing4;
            //IrisBody.GetComponent<Renderer>().materials[4].color = colorWings;

            Material[] mats = new Material[]
            { 
                Resources.Load<Material>("IrisMat/Body/0"),
                Resources.Load<Material>("IrisMat/Body/0"),
                Resources.Load<Material>("IrisMat/Body/0"),
                Resources.Load<Material>("IrisMat/Body/0"),
                Resources.Load<Material>("IrisMat/Body/0")
            };
            
            IrisBody.GetComponent<Renderer>().materials = mats;
            IrisBody.GetComponent<Renderer>().materials = mats;

            IrisHair.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/0");
            IrisCrystal1.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/0");
            IrisCrystal2.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/0");
            IrisRing1.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/0");
            IrisRing2.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/0");
            IrisRing3.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/0");
            IrisRing4.GetComponent<Renderer>().material = Resources.Load<Material>("IrisMat/Hair/0");
            gameObject.SetActive(true);
            anim.SetBool("Death", false);
        }
            
    }
}
