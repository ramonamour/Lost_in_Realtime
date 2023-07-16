using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleSwitch : MonoBehaviour
{

    [SerializeField] private AudioSource idleSound;
    [SerializeField] private AudioSource track96;
    [SerializeField] private Animator cameraPath;
    [SerializeField] private Animator sinkingHands;
    [SerializeField] private Animator doors;
    [SerializeField] private Animator curtain;
    
    [SerializeField] private Animator teleporter;

    [SerializeField] private GameObject workers;
    [SerializeField] private GameObject workersOutside;
    [SerializeField] private GameObject workersUpstairs;

    [SerializeField] private GameObject outsideGoals;

    [SerializeField] private GameObject goal1;
    [SerializeField] private GameObject goal2;
    [SerializeField] private GameObject goal3;
    [SerializeField] private GameObject goal4;
    [SerializeField] private GameObject goal5;

    [SerializeField] private GameObject screen;
    [SerializeField] private GameObject destructionFloor;

    [SerializeField] private GameObject ivy;
    [SerializeField] private GameObject screenEffects;

    private Material screenMaterial;
    private Material floorMaterial;

    private bool switchOn;

    // Start is called before the first frame update
    void Start()
    {
        switchOn = false;

        screenMaterial = screen.GetComponent<Renderer>().material;
        floorMaterial = destructionFloor.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        // press space key to leave idle mode and start the experience
         if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            switchOn = true;
        }

        if (switchOn == true) 
        {

            // stop looping the idle audio and transition to main audio
            if (idleSound.timeSamples >= 733000) {
                idleSound.Stop ();
                track96.Play ();
                switchOn = false;

                // trigger to start camera animation
                cameraPath.SetTrigger("audioSwitched");

                // trigger to start animation, that teleports the main characterinto different positions throughout the experience
                teleporter.SetTrigger("startTeleport");
                StartCoroutine(WorkerCoroutine());
                doors.SetTrigger("audioSwitched");
            }
        }
        
    }

    IEnumerator WorkerCoroutine()
    {
        // activate workers in foyer
        yield return new WaitForSeconds(5);
        workers.SetActive(true);

        //activate workers outside building
        yield return new WaitForSeconds(20);
        workersOutside.SetActive(true);

        // activate workers upstairs
        yield return new WaitForSeconds(10);
        workersUpstairs.SetActive(true);
        
        // turn of screen flickering and turn of screenEffects
        yield return new WaitForSeconds(83);
        screenMaterial.SetColor("_Color", Color.black);
        screenMaterial.DisableKeyword("_EMISSION");
        Debug.Log("Emission off");
        screenEffects.SetActive(false);
        
        // sink hands in water
        yield return new WaitForSeconds(2);
        sinkingHands.SetTrigger("sink");

        // deactivate all crowd simulation goals inside building and activate goals outside, so workers leave the building
        goal1.SetActive(false);
        goal2.SetActive(false);
        goal3.SetActive(false);
        goal4.SetActive(false);
        goal5.SetActive(false);
        outsideGoals.SetActive(true);
        workersUpstairs.SetActive(false);

        // start building destruction effect and grow ivy
        yield return new WaitForSeconds(30);
        StartCoroutine(DestructionCoroutine());
        ivy.SetActive(true);

        // open digital curtain twin (signal for users)
        yield return new WaitForSeconds(30);
        curtain.SetTrigger("open");

    }

    IEnumerator DestructionCoroutine()
    {
        // gradually strengthen destructed look of building
        while (floorMaterial.GetFloat("_DamageAmount") < .8f) {
            floorMaterial.SetFloat("_DamageAmount", floorMaterial.GetFloat("_DamageAmount")+.01f);
            yield return new WaitForSeconds(.1f);
        }


        yield return null;
    }
}


