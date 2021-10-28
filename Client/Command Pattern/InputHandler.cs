using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using NotAGame.Component;

namespace NotAGame.Command_Pattern
{
    class InputHandler
    {
        #region Singleton
        private static InputHandler instance;
        public static InputHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputHandler();
                }
                return instance;
            }
        }
        #endregion

        private bool noHoldDown;
        
        public Dictionary<Keys, ICommand> keybindings = new Dictionary<Keys, ICommand>();
        public Player player { get; set; }

        public InputHandler()
        {
            keybindings.Add(Keys.A, new MoveCommand(new Vector2(-1, 0)));
            keybindings.Add(Keys.D, new MoveCommand(new Vector2(1, 0)));
            keybindings.Add(Keys.W, new MoveCommand(new Vector2(0, -1)));
            keybindings.Add(Keys.S, new MoveCommand(new Vector2(0, 1)));

            keybindings.Add(Keys.N, new MasterInputs());
        }

        public void Excute(Player player)
        {
            KeyboardState key = Keyboard.GetState();

            foreach (Keys k in keybindings.Keys)
            {
                if (key.IsKeyDown(k) && noHoldDown == true)
                {
                    keybindings[k].Excute(player);
                    if (key.IsKeyDown(Keys.N))
                    {
                        noHoldDown = false;

                    }
                }
                else if (key.IsKeyUp(Keys.N))
                {
                    noHoldDown = true;
                }
                
            }
        }

    }
}
