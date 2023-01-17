using System;
using System.Threading.Tasks;
using BossAttacks.Utils;
using Modding;
using UnityEngine;

namespace BossAttacks
{
    internal class ModDisplay
    {
        internal static ModDisplay Instance;

        private string DisplayText = "Boss Attacks";
        private DateTime DisplayExpireTime = DateTime.Now;
        private TimeSpan DisplayDuration = TimeSpan.FromSeconds(6);

        private string NotificationText = "Boss Attacks Notification";
        private DateTime NotificationExpireTime = DateTime.Now;
        private TimeSpan NotificationDuration = TimeSpan.FromSeconds(2);

        private Vector2 TextSize = new(800, 500);
        private Vector2 TextPosition = new(0.22f, 0.25f);

        private GameObject _canvas;
        private UnityEngine.UI.Text _text;

        private void Create()
        {
            if (_canvas != null) return;

            // Create base canvas
            _canvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(1920, 1080));

            CanvasGroup canvasGroup = _canvas.GetComponent<CanvasGroup>();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            UnityEngine.Object.DontDestroyOnLoad(_canvas);

            _text = CanvasUtil.CreateTextPanel(
                _canvas, "Boss Attacks", 24, TextAnchor.LowerLeft,
                new CanvasUtil.RectData(TextSize, Vector2.zero, TextPosition, TextPosition),
                CanvasUtil.GetFont("Perpetua")
            ).GetComponent<UnityEngine.UI.Text>();
        }

        public void Destroy()
        {
            if (_canvas != null) UnityEngine.Object.Destroy(_canvas);
            _canvas = null;
            _text = null;
        }

        public void Update()
        {
            Create();

            if (!BossAttacks.Instance.GlobalData.FadeDisplay || DateTime.Now < DisplayExpireTime || DateTime.Now < NotificationExpireTime)
            {
                _text.text = DateTime.Now >= NotificationExpireTime ? DisplayText : NotificationText;
                _canvas.SetActive(true);
            }
            else
            {
                _canvas?.SetActive(false);
            }
        }

        /**
         * Display text (less temporary).
         */
        public void Display(string text)
        {
            DisplayText = text.Trim();
            DisplayExpireTime = DateTime.Now + DisplayDuration;
            Update();
            Task.Delay(DisplayDuration + TimeSpan.FromMilliseconds(100)).ContinueWith(t => Update());
        }

        /**
         * Notify text (more temporary).
         */
        public void Notify(string text)
        {
            NotificationText = text.Trim();
            NotificationExpireTime = DateTime.Now + NotificationDuration;
            Update();
            Task.Delay(NotificationDuration + TimeSpan.FromMilliseconds(100)).ContinueWith(t => Update());
        }

        public void EnableDebugger()
        {
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
        }

        private void ModHooks_HeroUpdateHook()
        {
            Vector2? posDelta = null;
            Vector2? sizeDelta = null;

            // Display: *horizontal* position and size
            if (Input.GetKeyDown(KeyCode.F))
            {
                posDelta = new Vector2(-0.01f, 0);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                posDelta = new Vector2(0.01f, 0);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                sizeDelta = new Vector2(-10, 0);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                sizeDelta = new Vector2(10, 0);
            }

            // Display: *vertical* position and size
            if (Input.GetKeyDown(KeyCode.J))
            {
                posDelta = new Vector2(0, -0.01f);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                posDelta = new Vector2(0, 0.01f);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                sizeDelta = new Vector2(0, -10);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                sizeDelta = new Vector2(0, 10);
            }

            if (posDelta != null || sizeDelta != null)
            {
                if (posDelta != null)
                {
                    TextPosition += posDelta.Value;
                }
                if (sizeDelta != null)
                {
                    TextSize += sizeDelta.Value;
                }
                Destroy();
                Update();
                this.LogModDebug($"{TextPosition.x,0:F2} {TextPosition.y,0:F2} - {TextSize.x,0:F0} {TextSize.y,0:F0}");
            }
        }
    }
}
