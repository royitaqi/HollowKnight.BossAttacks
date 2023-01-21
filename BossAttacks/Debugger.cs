using System;
using BossAttacks.Utils;
using InControl;
using Modding;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace BossAttacks
{
    internal class Debugger
    {
        public void Load()
        {
            ModHooks.HeroUpdateHook += OnHeroUpdate;
        }

        public void Unload()
        {
            ModHooks.HeroUpdateHook -= OnHeroUpdate;
            _isPressed?.Dispose();
            _isPressed = null;
            _wasPressed?.Dispose();
            _wasPressed = null;
        }

        private void OnHeroUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (_isPressed == null)
                {
                    var isPressed = typeof(OneAxisInputControl).GetMethod("get_IsPressed");
                    _isPressed = new Hook(isPressed, MyIsPressed);
                }
                if (_wasPressed == null)
                {
                    var wasPressed = typeof(OneAxisInputControl).GetMethod("get_WasPressed");
                    _wasPressed = new Hook(wasPressed, MyWasPressed);
                }
                if (_inputHandler == null)
                {
                    _inputHandler = typeof(HeroController).GetField("inputHandler", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(HeroController.instance) as InputHandler;
                }

                _down = true;
                this.LogModTEMP($"1: {_down}");
            }
            else if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                _down = false;
                this.LogModTEMP($"1: {_down}");
            }
        }

        private bool MyIsPressed(Func<OneAxisInputControl, bool> orig, OneAxisInputControl self)
        {
            if (self == _inputHandler?.inputActions?.left)
            {
                this.LogModTEMP($"left (is): {_down}");
                return _down;
            }
            return orig(self);
        }

        private bool MyWasPressed(Func<OneAxisInputControl, bool> orig, OneAxisInputControl self)
        {
            if (self == _inputHandler?.inputActions?.left)
            {
                this.LogModTEMP($"left (was): {_down}");
                return _down;
            }
            return orig(self);
        }

        private InputHandler _inputHandler;
        private Hook _isPressed;
        private Hook _wasPressed;
        private bool _down;
    }
}
