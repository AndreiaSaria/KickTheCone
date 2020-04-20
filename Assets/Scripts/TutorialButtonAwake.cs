using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButtonAwake : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.HasKey("TutorialDone"))
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
