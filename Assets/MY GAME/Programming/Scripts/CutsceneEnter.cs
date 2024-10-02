using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEnter : MonoBehaviour
{
    public GameObject thePlayer;
    public GameObject cutsceneCam;
    public bool oneTime = true;
    public bool inCutscene = false;
    public float cutsceneDuration;


    void Awake()
    {
        cutsceneCam.SetActive(inCutscene);
    }
    void OnTriggerEnter(Collider other)
    {
        inCutscene = true;
        if (oneTime)
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        cutsceneCam.SetActive(inCutscene);
        thePlayer.SetActive(!inCutscene);
        StartCoroutine(FinishCutscene());
    }

    IEnumerator FinishCutscene()
    {
        Debug.Log("Waiting...");
        yield return new WaitForSeconds(cutsceneDuration);
        Debug.Log("Finished...");
        inCutscene = false;
        thePlayer.SetActive(!inCutscene);
        cutsceneCam.SetActive(inCutscene);
    }
}
