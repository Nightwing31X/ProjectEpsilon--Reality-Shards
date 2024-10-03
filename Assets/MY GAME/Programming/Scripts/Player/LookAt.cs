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

    [SerializeField] private bool _haveCustomCamera;
    [SerializeField] private Camera _CustomCamera;

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(SecsOfReading);
        inCutscene = false;
    }

    public void RotateToObject()
    {
        if (!_haveCustomCamera)
        {
            Vector3 direction = lookAtOBJ.position - Camera.main.transform.position;
            Quaternion rotationToOBJ = Quaternion.LookRotation(direction);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, rotationToOBJ, 2 * Time.deltaTime);
        }
        else
        {
            Vector3 direction = lookAtOBJ.position - _CustomCamera.transform.position;
            Quaternion rotationToOBJ = Quaternion.LookRotation(direction);
            _CustomCamera.transform.rotation = Quaternion.Lerp(_CustomCamera.transform.rotation, rotationToOBJ, 2 * Time.deltaTime);
        }
        StartCoroutine(Delay());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inCutscene = true;
            //GetComponent<Dialogue>().talkToSelf();
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
