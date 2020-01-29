using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Models;
using UnityEngine;

namespace Models
{
    /// <summary>
    /// Содержит значения физичеких параметров модели.
    /// </summary>
    [CreateAssetMenu(fileName = "NewPhysics", menuName = "RPG_Van_Helsing/CharacterPhysicsParams", order = 0)]
    public class PhysicsParams : ScriptableObject
    {
        public float FallForce; // = 50f; // Скорость падения
        public float Height; // = 2.5f; // Высота перса
        public float JumpForce; // = 250f; // Сила прыжка
        public float Mass; // = 100f; // Масса игрока
        public float Drag; // = 0f; //Сопротивление игрока
        public float Radius; // = 0.5f; // Радиус
        public float RollSpeed; // = 25000f; // Сила кувырка
        public float Speed; // = 3f; // Скорость перса
    }
}
