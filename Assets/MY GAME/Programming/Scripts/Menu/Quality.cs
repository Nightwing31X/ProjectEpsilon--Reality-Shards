using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    [AddComponentMenu("GameDev/Menu/Quality")]
    public class Quality : MonoBehaviour
    {
        public void ChangeQualiy(int Quality)
        {
            QualitySettings.SetQualityLevel(Quality);
        }
    }
}