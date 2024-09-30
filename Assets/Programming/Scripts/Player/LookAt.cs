using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform lookAtOBJ;
    public bool inCutscene;

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        inCutscene = false;
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
            GetComponent<Dialogue>().talkToSelf();
            GetComponent<BoxCollider>().enabled = false;
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
