﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel1 : MonoBehaviour
{
    public void RestartScene()
    {
        print("Restart");
        SceneManager.LoadScene("Level1");
    }
}