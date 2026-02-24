using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TowerDefense;
using UnityEngine;

public class ResetMap : SingletonBase<ResetMap>
{

    public Action ResetMapCompletion()
    {
        return () =>
        {
            MapCompletion.Instance.ResetMapCompletion();
        };
    }
}
