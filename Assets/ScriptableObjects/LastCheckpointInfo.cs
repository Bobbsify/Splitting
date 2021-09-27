using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    [CreateAssetMenu(fileName = "LastCheckpointInfo", menuName = "ScriptableObjects/LastCheckpointInfo", order = 1)]
    public class LastCheckpointInfo : ScriptableObject
    {
        public int levelCheckpoint = 0;
    }
}