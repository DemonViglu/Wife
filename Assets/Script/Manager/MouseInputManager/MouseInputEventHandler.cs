using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace DemonViglu.MouseInput {
    public partial class MouseInputManager : MonoBehaviour {

        /// <summary>
        /// the paramter will be the mouseClick Count;
        /// </summary>
        public event Action<int> OnMouseMultiClickDown;
    }
}