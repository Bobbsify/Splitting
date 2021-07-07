using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Splitting { 
    public class InputSettings
    {

        [SerializeField]
        private KeyCode jumpButton = KeyCode.Space;

        [SerializeField]
        private KeyCode pickupButton = KeyCode.LeftControl;

        [SerializeField]
        private KeyCode releaseItemButton = KeyCode.LeftControl;

        [SerializeField]
        private KeyCode undoThrowButton = KeyCode.LeftControl;

        [SerializeField]
        private KeyCode throwButton = KeyCode.LeftShift;

        [SerializeField]
        private KeyCode switchCharacterButton = KeyCode.Q;

        [SerializeField]
        private KeyCode interactButton = KeyCode.E;

        [SerializeField]
        private KeyCode hackingButton = KeyCode.F;

        [SerializeField]
        private KeyCode flashlightButton = KeyCode.T;

        [SerializeField]
        private KeyCode torchAngleUpButton = KeyCode.Alpha1;

        [SerializeField]
        private KeyCode torchAngleDownButton = KeyCode.Alpha3;

        [SerializeField]
        private KeyCode pauseButton = KeyCode.Escape;

        public KeyCode JumpButton { get => jumpButton; set => jumpButton = value; }
        public KeyCode PickupButton { get => pickupButton; set => pickupButton = value; }
        public KeyCode ReleaseItemButton { get => releaseItemButton; set => releaseItemButton = value; }
        public KeyCode UndoThrowButton { get => undoThrowButton; set => undoThrowButton = value; }
        public KeyCode ThrowButton { get => throwButton; set => throwButton = value; }
        public KeyCode SwitchCharacterButton { get => switchCharacterButton; set => switchCharacterButton = value; }
        public KeyCode InteractButton { get => interactButton; set => interactButton = value; }
        public KeyCode HackingButton { get => hackingButton; set => hackingButton = value; }
        public KeyCode FlashlightButton { get => flashlightButton; set => flashlightButton = value; }
        public KeyCode TorchAngleUpButton { get => torchAngleUpButton; set => torchAngleUpButton = value; }
        public KeyCode TorchAngleDownButton { get => torchAngleDownButton; set => torchAngleDownButton = value; }
        public KeyCode PauseButton { get => pauseButton; set => pauseButton = value; }
    }
}
