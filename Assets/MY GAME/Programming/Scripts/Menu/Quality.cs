using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quality : MonoBehaviour
{
    public void ChangeQualiy(int Quality)
    {
        QualitySettings.SetQualityLevel(Quality);
    }
}
