using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleSwitch : MonoBehaviour
{
    [SerializeField] private AudioSource idleSound;
    [SerializeField] private AudioSource track96;
    [SerializeField] private Animator cameraPath;

    [SerializeField] private Animator worker1;
    [SerializeField] private Animator worker2;
    [SerializeField] private Animator worker3;
    [SerializeField] private Animator worker4;
    [SerializeField] private Animator worker5;
    [SerializeField] private Animator worker6;

    private bool switchOn;

    // Start is called before the first frame update
    void Start()
    {
        switchOn = false;
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
                StartCoroutine(WorkerCoroutine());
            }
        }
        
    }

    IEnumerator WorkerCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
        worker1.SetTrigger("start");
        worker2.SetTrigger("start");
        worker3.SetTrigger("start");
        worker4.SetTrigger("start");
        worker5.SetTrigger("start");
        worker6.SetTrigger("start");
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
