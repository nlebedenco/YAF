﻿using UnityEngine;

public class Level: MonoBehaviour
{
    #region MonoBehaviour

    void Awake()
    {
        GameSystem.RegisterLevel(this);
    }

    void OnDestroy()
    {

    }

    #endregion
}
