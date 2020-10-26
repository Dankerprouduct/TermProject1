using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loki2D.Core.Scene;
using Loki2D.GUI;
using Loki2D.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TermProject1.AI.Actions;
using TermProject1.Entity;

namespace TermProject1.World
{
    public class PlayerUI
    {
        public enum Display
        {
            Storage,
            Actions,
            None
        }

        public Display CurrentDisplay = Display.None;
        private UIElement _actionsDisplay = new UIElement();
        private UIGraphicElement _endTurnButton, _actionsButton, _storageButton;
        private UITextElement _healthText;
        private UITextElement _roundNumber;
        public PlayerUI()
        {
            GUIManager.Instance.RegisterFont("MenuFont", "Fonts/MenuFont");
            GUIManager.Instance.RegisterFont("HealthFont", "Fonts/HealthFont");
            
            var bottomBar = new UIElement();
            bottomBar.SetPosition(new Point(GUIManager.ScreenWidth - 300, GUIManager.ScreenHeight - 80));
            bottomBar.Width = 300;
            bottomBar.Height = 80; 
            bottomBar.SetColor(Color.DarkGray);

            _endTurnButton = new UIGraphicElement();
            _endTurnButton.SetPath("UI_EndTurn");
            _endTurnButton.Position = new Point(0, 0);
            _endTurnButton.SetParent(bottomBar);


            _actionsButton = new UIGraphicElement();
            _actionsButton.SetPath("UI_Actions");
            _actionsButton.Position = new Point(_endTurnButton.Width + 5, 0);
            _actionsButton.SetParent(bottomBar);
            
            _storageButton = new UIGraphicElement();
            _storageButton.SetPath("UI_Storage");
            _storageButton.Position = new Point(_actionsButton.Position.X + _actionsButton.Width + 5, 0);
            _storageButton.SetParent(bottomBar);
            

            GUIManager.Instance.AddElement(bottomBar);

            _actionsDisplay = new UIElement();
            _actionsDisplay.Width = 320;
            _actionsDisplay.Height = GUIManager.ScreenHeight - 10 - bottomBar.Height;
            _actionsDisplay.SetPosition(new Point(GUIManager.ScreenWidth - _actionsDisplay.Width, 5));
            _actionsDisplay.SetColor(new Color(97, 57, 19));

            GUIManager.Instance.AddElement(_actionsDisplay);

            UpdateUI();

            _healthText = new UITextElement();
            _healthText.Text = Player.Instance.Health.ToString();
            _healthText.TextScale = 1; 
            _healthText.SetColor(Color.White);
            _healthText.SetPosition(new Point(10, GUIManager.ScreenHeight - 100));
            _healthText.SetFont(GUIManager.Instance.GetFont("HealthFont"));

            _roundNumber = new UITextElement();
            _roundNumber.Text = $"Round: {TurnManager.Round}";
            _roundNumber.TextScale = 1;
            _roundNumber.SetColor(Color.White);
            _roundNumber.SetPosition(new Point(10, 10));
            _roundNumber.SetFont(GUIManager.Instance.GetFont("HealthFont"));

            GUIManager.Instance.AddElement(_healthText);
            GUIManager.Instance.AddElement(_roundNumber);
        }

        public void Update(GameTime gameTime)
        {
            _healthText.Text = Player.Instance.Health.ToString();
            _roundNumber.Text = $"Round: {TurnManager.Round}";


            // end turn
            if (InputManager.KeyPressed(Keys.D1))
            {
                UpdateUI();
            }

            // actions
            if (InputManager.KeyPressed(Keys.K))
            {
                CurrentDisplay = Display.Actions;
                UpdateUI();
            }

            // storage
            if (InputManager.KeyPressed(Keys.L))
            {
                CurrentDisplay = Display.Storage;
                UpdateUI();
            }
            if (InputManager.KeyPressed(Keys.Q))
            {
                CurrentDisplay = Display.None;
                GameManager.Instance.GetBattleMap().ClearOverlay();
                Player.Instance.ShowRange();
                UpdateUI();
            }

            
        }

        public void UpdateUI()
        {
            if (CurrentDisplay == Display.Actions)
            {
                _actionsDisplay.CanDraw = true;
                _actionsDisplay.CanUpdate = true;

                int i = 0;
                foreach (var action in Player.Instance.Actions)
                {
                    var background = new UIElement();
                    background.Width = _actionsDisplay.Width - 20;
                    background.Height = 40;

                    background.SetColor(new Color(192, 103, 66));
                    background.SetPosition(new Point(10, (i * 45) + 5));
                    background.SetParent(_actionsDisplay);


                    var actionText = new UITextElement();
                    actionText.Text = $"{action.Name} x{action.UsesLeft}";
                    actionText.SetColor(Color.White);
                    actionText.SetPosition(new Point(5,15));
                    actionText.SetFont(GUIManager.Instance.GetFont("MenuFont"));
                    actionText.SetParent(background);


                    var actionNumber = new UITextElement();
                    actionNumber.Text = (i + 1).ToString();
                    actionNumber.SetColor(Color.White);
                    actionNumber.SetPosition(new Point(background.Width - 20, 15));
                    actionNumber.SetFont(GUIManager.Instance.GetFont("MenuFont"));
                    actionNumber.SetParent(background);

                    var rangeText = new UITextElement();
                    rangeText.Text = $"DP: {((DamageAction)action).Damage} R: {action.Range}";
                    rangeText.SetColor(Color.White);
                    rangeText.SetPosition(new Point(background.Width - 125, 15));
                    rangeText.SetFont(GUIManager.Instance.GetFont("MenuFont"));
                    rangeText.SetParent(background);

                    i++;
                }

            }
            else
            {
                _actionsDisplay.CanDraw = false;
                _actionsDisplay.CanUpdate = false;

                _actionsDisplay.Children.Clear();
            }
        }

    }

}
