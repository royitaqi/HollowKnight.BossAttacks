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
            var leftKey = KeyCode.Alpha1;
            var attackKey = KeyCode.Alpha2;

            if (Input.GetKeyDown(leftKey))
            {
                Initialize();
                _leftDown = true;
                this.LogModTEMP($"fake left: {_leftDown}");
            }
            if (Input.GetKeyUp(leftKey))
            {
                Initialize();
                _leftDown = false;
                this.LogModTEMP($"fake left: {_leftDown}");
            }
            if (Input.GetKeyDown(attackKey))
            {
                Initialize();
                _attackDown = true;
                this.LogModTEMP($"fake attack: {_attackDown}");
            }
            if (Input.GetKeyUp(attackKey))
            {
                Initialize();
                _attackDown = false;
                this.LogModTEMP($"fake attack: {_attackDown}");
            }
        }

        private void Initialize()
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
        }

        private bool MyIsPressed(Func<OneAxisInputControl, bool> orig, OneAxisInputControl self)
        {
            if (self == _inputHandler?.inputActions?.left)
            {
                this.LogModTEMP($"attack (is): {_leftDown}");
                return _leftDown;
            }
            if (self == _inputHandler?.inputActions?.attack)
            {
                this.LogModTEMP($"attack (is): {_attackDown}");
                return _attackDown;
            }
            return orig(self);
        }

        private bool MyWasPressed(Func<OneAxisInputControl, bool> orig, OneAxisInputControl self)
        {
            if (self == _inputHandler?.inputActions?.left)
            {
                this.LogModTEMP($"attack (is): {_leftDown}");
                return _leftDown;
            }
            if (self == _inputHandler?.inputActions?.attack)
            {
                this.LogModTEMP($"attack (was): {_attackDown}");
                return _attackDown;
            }
            return orig(self);
        }

        private InputHandler _inputHandler;
        private Hook _isPressed;
        private Hook _wasPressed;
        private bool _leftDown;
        private bool _attackDown;
    }
}
