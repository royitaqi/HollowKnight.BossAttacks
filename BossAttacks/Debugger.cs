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
        public static Debugger Instance;

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
            _vector?.Dispose();
            _vector = null;
            _value?.Dispose();
            _value = null;
            _x?.Dispose();
            _x = null;
            _2HasChanged?.Dispose();
            _2HasChanged = null;
            _2IsPressed?.Dispose();
            _2IsPressed = null;
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
                this.LogModTEMP("Hooked _isPressed");
            }
            if (_wasPressed == null)
            {
                var wasPressed = typeof(OneAxisInputControl).GetMethod("get_WasPressed");
                _wasPressed = new Hook(wasPressed, MyWasPressed);
                this.LogModTEMP("Hooked _wasPressed");
            }
            if (_vector == null)
            {
                var vector = typeof(TwoAxisInputControl).GetMethod("get_Vector");
                _vector = new Hook(vector, MyVector);
                this.LogModTEMP("Hooked _vector");
            }
            if (_value == null)
            {
                var value = typeof(TwoAxisInputControl).GetMethod("get_Value");
                _value = new Hook(value, MyValue);
                this.LogModTEMP("Hooked _value");
            }
            if (_x == null)
            {
                var x = typeof(TwoAxisInputControl).GetMethod("get_X");
                _x = new Hook(x, MyX);
                this.LogModTEMP("Hooked _x");
            }
            if (_2HasChanged == null)
            {
                var method = typeof(TwoAxisInputControl).GetMethod("get_HasChanged");
                _2HasChanged = new Hook(method, My2HasChanged);
                this.LogModTEMP("Hooked _2HasChanged");
            }
            if (_2IsPressed == null)
            {
                var method = typeof(TwoAxisInputControl).GetMethod("get_IsPressed");
                _2IsPressed = new Hook(method, My2IsPressed);
                this.LogModTEMP("Hooked _2IsPressed");
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
                this.LogModTEMP($"left (is): {_leftDown}");
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
                this.LogModTEMP($"left (was): {_leftDown}");
                return _leftDown;
            }
            if (self == _inputHandler?.inputActions?.attack)
            {
                this.LogModTEMP($"attack (was): {_attackDown}");
                return _attackDown;
            }
            return orig(self);
        }

        private Vector2 MyVector(Func<TwoAxisInputControl, Vector2> orig, TwoAxisInputControl self)
        {
            this.LogModTEMP("MyVector()");
            if (self == _inputHandler?.inputActions?.moveVector)
            {
                var vec = _leftDown ? Vector2.left : Vector2.zero;
                this.LogModTEMP($"vector: {vec}");
                return vec;
            }
            return orig(self);
        }

        private Vector2 MyValue(Func<TwoAxisInputControl, Vector2> orig, TwoAxisInputControl self)
        {
            this.LogModTEMP("MyValue()");
            if (self == _inputHandler?.inputActions?.moveVector)
            {
                var vec = _leftDown ? Vector2.left : Vector2.zero;
                this.LogModTEMP($"value: {vec}");
                return vec;
            }
            return orig(self);
        }

        private float MyX(Func<TwoAxisInputControl, float> orig, TwoAxisInputControl self)
        {
            this.LogModTEMP("MyX()");
            if (self == _inputHandler?.inputActions?.moveVector)
            {
                var vec = _leftDown ? Vector2.left : Vector2.zero;
                this.LogModTEMP($"x: {vec.x}");
                return vec.x;
            }
            return orig(self);
        }

        private bool My2HasChanged(Func<TwoAxisInputControl, bool> orig, TwoAxisInputControl self)
        {
            this.LogModTEMP("My2HasChanged()");
            if (self == _inputHandler?.inputActions?.moveVector)
            {
                this.LogModTEMP($"2 has changed: true");
                return true;
            }
            return orig(self);
        }

        private bool My2IsPressed(Func<TwoAxisInputControl, bool> orig, TwoAxisInputControl self)
        {
            this.LogModTEMP("My2IsPressed()");
            if (self == _inputHandler?.inputActions?.moveVector)
            {
                this.LogModTEMP($"2 is pressed: true");
                return true;
            }
            return orig(self);
        }

        private InputHandler _inputHandler;
        // one axis
        private Hook _isPressed;
        private Hook _wasPressed;
        // two axis
        private Hook _vector;
        private Hook _value;
        private Hook _x;
        private Hook _2HasChanged;
        private Hook _2IsPressed;
        // fake input
        private bool _leftDown;
        private bool _attackDown;
    }
}
