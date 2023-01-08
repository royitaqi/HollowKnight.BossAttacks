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

        public bool Visible = true;
        public int Mode = 2; // 0 = off; 1 = auto-hide; 2 = on

        public string DisplayText = "Boss Attacks";
        public DateTime DisplayExpireTime = DateTime.Now;

        public string NotificationText = "Boss Attacks Notification";
        public DateTime NotificationExpireTime = DateTime.Now;

        internal TimeSpan FadeOutDuration = TimeSpan.FromSeconds(6);
        internal Vector2 TextSize = new(440, 520);
        internal Vector2 TextPosition = new(0.13f, 0.23f);

        private GameObject _canvas;
        private UnityEngine.UI.Text _text;

        public void Create()
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

            if (Visible && Mode != 0 && !(Mode == 1 && DateTime.Now > DisplayExpireTime && DateTime.Now > NotificationExpireTime))
            {
                _text.text = DateTime.Now > NotificationExpireTime ? DisplayText : NotificationText;
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

        public void EnableDebugger()
        {
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
        }

        private void ModHooks_HeroUpdateHook()
        {
            // Require CTRL to be pressed to access the debugg ability
            if (!Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.RightControl))
            {
                return;
            }

            // Display: *horizontal* position and size
            if (Input.GetKeyDown(KeyCode.F))
            {
                ModDisplay.Instance.TextPosition = new Vector2(ModDisplay.Instance.TextPosition.x - 0.01f, ModDisplay.Instance.TextPosition.y);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                ModDisplay.Instance.TextPosition = new Vector2(ModDisplay.Instance.TextPosition.x + 0.01f, ModDisplay.Instance.TextPosition.y);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                ModDisplay.Instance.TextSize = new Vector2(ModDisplay.Instance.TextSize.x - 10, ModDisplay.Instance.TextSize.y);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                ModDisplay.Instance.TextSize = new Vector2(ModDisplay.Instance.TextSize.x + 10, ModDisplay.Instance.TextSize.y);
            }

            // Display: *vertical* position and size
            if (Input.GetKeyDown(KeyCode.J))
            {
                ModDisplay.Instance.TextPosition = new Vector2(ModDisplay.Instance.TextPosition.x, ModDisplay.Instance.TextPosition.y - 0.01f);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                ModDisplay.Instance.TextPosition = new Vector2(ModDisplay.Instance.TextPosition.x, ModDisplay.Instance.TextPosition.y + 0.01f);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                ModDisplay.Instance.TextSize = new Vector2(ModDisplay.Instance.TextSize.x, ModDisplay.Instance.TextSize.y - 10);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                ModDisplay.Instance.TextSize = new Vector2(ModDisplay.Instance.TextSize.x, ModDisplay.Instance.TextSize.y + 10);
            }

            ModDisplay.Instance.Update();
            this.LogModDebug($"{ModDisplay.Instance.TextPosition.x,0:F2} {ModDisplay.Instance.TextPosition.y,0:F2} - {ModDisplay.Instance.TextSize.x,0:F0} {ModDisplay.Instance.TextSize.y,0:F0}");
        }
    }
}
