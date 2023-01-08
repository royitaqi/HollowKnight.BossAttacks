using System;
using System.Threading.Tasks;
using Modding;
using UnityEngine;

namespace BossAttacks
{
    internal class ModDisplay
    {
        internal static ModDisplay Instance;

        public bool Visible = true;
        public int Mode = 2; // 0 = off; 1 = auto-hide; 2 = on

        public string DisplayText = "Boss Attacks";
        public DateTime DisplayExpireTime = DateTime.Now;

        public string NotificationText = "Boss Attacks Notification";
        public DateTime NotificationExpireTime = DateTime.Now;

        internal TimeSpan FadeOutDuration = TimeSpan.FromSeconds(6);
        internal Vector2 TextSize = new(240, 100);
        internal Vector2 TextPosition = new(0.12f, 0.04f);

        private GameObject _canvas;


        public void Create(string text)
        {
            if (_canvas != null) return;
            // Create base canvas
            _canvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(1920, 1080));

            CanvasGroup canvasGroup = _canvas.GetComponent<CanvasGroup>();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            UnityEngine.Object.DontDestroyOnLoad(_canvas);

            CanvasUtil.CreateTextPanel(_canvas, text, 24, TextAnchor.MiddleCenter,
                new CanvasUtil.RectData(TextSize, Vector2.zero, TextPosition, TextPosition),
                CanvasUtil.GetFont("Perpetua"));

            Update();
        }

        public void Destroy()
        {
            if (_canvas != null) UnityEngine.Object.Destroy(_canvas);
            _canvas = null;
        }

        public void Update()
        {
            if (Visible && Mode != 0 && !(Mode == 1 && DateTime.Now > DisplayExpireTime && DateTime.Now > NotificationExpireTime))
            {
                Destroy();

                string text = DateTime.Now > NotificationExpireTime ? DisplayText : NotificationText;
                Create(text);
                
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
            DisplayText = text;
            DisplayExpireTime = DateTime.Now + FadeOutDuration;
            Update();
            Task.Delay(FadeOutDuration).ContinueWith(t => Update());
        }

        /**
         * Notify text (more temporary).
         */
        public void Notify(string text)
        {
            NotificationText = text;
            NotificationExpireTime = DateTime.Now + FadeOutDuration;
            Update();
            Task.Delay(FadeOutDuration).ContinueWith(t => Update());
        }
    }
}
