﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0, 100)]
        [SerializeField] private int            startingLevel  = 1;
        [SerializeField] private CharacterClass characterClass = CharacterClass.Player;
    }
}
