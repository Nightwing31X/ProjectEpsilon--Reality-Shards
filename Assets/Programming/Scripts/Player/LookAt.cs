using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform lookAtOBJ;
    public bool inCutscene;
    public bool manual;
    [Tooltip("If used with the Prompts.cs it will use that 'SecondsOfReading'. If not using that script then please provide a number.")]
    public float SecsOfReading;

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(SecsOfReading);
        inCutscene = false;
        GameManager.instance.OnPlay();
    }

    public void RotateToObject()
    {
        Vector3 direction = lookAtOBJ.position - Camera.main.transform.position;
        Quaternion rotationToOBJ = Quaternion.LookRotation(direction);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, rotationToOBJ, 2 * Time.deltaTime);
        GameManager.instance.OnMenu();
        StartCoroutine(Delay());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inCutscene = true;
            //GetComponent<Dialogue>().talkToSelf();
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void Awake()
    {
        if (!manual)
        {
            SecsOfReading = GetComponent<Prompts>().SecondsOfReading;
        }
    }

    void Update()
    {
        if (inCutscene)
        {
            RotateToObject();
        }
    }
}
