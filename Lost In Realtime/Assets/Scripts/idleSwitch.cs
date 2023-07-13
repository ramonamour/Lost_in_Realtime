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
         if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            switchOn = true;
        }

        if (switchOn == true) 
        {

            if (idleSound.timeSamples >= 733000) {
                idleSound.Stop ();
                track96.Play ();
                switchOn = false;

                cameraPath.SetTrigger("audioSwitched");
                teleporter.SetTrigger("startTeleport");
                StartCoroutine(WorkerCoroutine());
                doors.SetTrigger("audioSwitched");
            }
        }
        
    }

    IEnumerator WorkerCoroutine()
    {


        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
        workers.SetActive(true);

        yield return new WaitForSeconds(20);
        workersOutside.SetActive(true);

        yield return new WaitForSeconds(10);
        workersUpstairs.SetActive(true);
        
        yield return new WaitForSeconds(83);
        screenMaterial.SetColor("_Color", Color.black);
        screenMaterial.DisableKeyword("_EMISSION");
        Debug.Log("Emission off");
        
        yield return new WaitForSeconds(2);
        sinkingHands.SetTrigger("sink");

        goal1.SetActive(false);
        goal2.SetActive(false);
        goal3.SetActive(false);
        goal4.SetActive(false);
        goal5.SetActive(false);
        outsideGoals.SetActive(true);
        workersUpstairs.SetActive(false);

        yield return new WaitForSeconds(30);
        StartCoroutine(DestructionCoroutine());
        curtain.SetTrigger("open");

    }

    IEnumerator DestructionCoroutine()
    {
        while (floorMaterial.GetFloat("_DamageAmount") < 1f) {
            floorMaterial.SetFloat("_DamageAmount", floorMaterial.GetFloat("_DamageAmount")+.01f);
            yield return new WaitForSeconds(.1f);
        }


        yield return null;
    }
}


