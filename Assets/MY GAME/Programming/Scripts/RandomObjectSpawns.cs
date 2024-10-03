using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawns : MonoBehaviour
{

    [SerializeField] private GameObject[] arrayOfObjs;

    private IEnumerator HalfObjects(int numberOfObjects)
    {
        yield return new WaitForSeconds(.5f);

        int totalObjects = arrayOfObjs.Length;
        List<int> indicesAppear = new List<int>();

        while (indicesAppear.Count < numberOfObjects)
        {
            int i = Random.Range(0, totalObjects);

            // Ensure the object at index i hasn't been deactivated already
            if (!indicesAppear.Contains(i))
            {
                Debug.Log("Show Random...");
                arrayOfObjs[i].SetActive(true);
                indicesAppear.Add(i);
            }
            yield return null;
        }

        // Set the rest of the items to inactive
        for (int i = 0; i < totalObjects; i++)
        {
            if (!indicesAppear.Contains(i))
            {
                arrayOfObjs[i].SetActive(false);
            }
        }
    }
}
