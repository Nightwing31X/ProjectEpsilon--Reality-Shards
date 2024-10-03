using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class RandomObjectSpawns : MonoBehaviour
{

    [SerializeField] private GameObject[] ObjsToAppear;
    [Tooltip("How many objects do you want to appear")]
    [SerializeField] private int numberOfObjectsToAppear;
    [Tooltip("Will not run on start - Must run yourself using a trigger or anything else call FunctionRandomOBJS")]
    [SerializeField] private bool _wait = false;

    private void Start()
    {
        Debug.Log("Last key is randomly spawned...");
        if (!_wait)
        {
            StartCoroutine(HalfObjects(numberOfObjectsToAppear));
        }
    }

    public void FunctionRandomOBJS()
    {
        StartCoroutine(HalfObjects(numberOfObjectsToAppear));
    }

    private IEnumerator HalfObjects(int numberOfObjectsToAppear)
    {
        yield return new WaitForSeconds(.01f);

        int totalObjects = ObjsToAppear.Length;
        List<int> indicesAppear = new List<int>();

        while (indicesAppear.Count < numberOfObjectsToAppear)
        {
            int i = Random.Range(0, totalObjects);

            // Ensure the object at index i hasn't been deactivated already
            if (!indicesAppear.Contains(i))
            {
                Debug.Log("Show Random...");
                ObjsToAppear[i].SetActive(true);
                indicesAppear.Add(i);
            }
            yield return null;
        }

        // Set the rest of the items to inactive
        for (int i = 0; i < totalObjects; i++)
        {
            if (!indicesAppear.Contains(i))
            {
                ObjsToAppear[i].SetActive(false);
            }
        }
    }
}
