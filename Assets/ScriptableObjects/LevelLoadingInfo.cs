using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting
{
    [CreateAssetMenu(fileName = "LevelLoadingInfo", menuName = "ScriptableObjects/LevelLoadingInfo", order = 1)]
    public class LevelLoadingInfo : ScriptableObject
    {
        public LevelInfo levelInfo;
    }

    [Serializable]
    public class LevelInfo
    {
        public int levelToLoad;
    }
}