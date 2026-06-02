using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ManagerUtility
{
    public static LoopManager loopManager
    {
        get
        {
            return Object.FindObjectOfType<LoopManager>();
        }
    }

    public static SoundManager soundManager
    {
        get
        {
            return Object.FindObjectOfType<SoundManager>();
        }
    }

    public static ConnectionManager connectionManager
    {
        get
        {
            return Object.FindObjectOfType<ConnectionManager>();
        }
    }
}
