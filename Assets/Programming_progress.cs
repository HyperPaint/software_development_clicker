using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame;

public class Programming_progress : MonoBehaviour
{
    public Image programming_progress;

    public float fill;

    void Start()
    {
        programming_progress = GetComponent<Image>();

        fill = 1f;
    }

    void Update()
    {
        programming_progress.fillAmount = fill;
    }
}
