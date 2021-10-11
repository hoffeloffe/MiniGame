using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Command_Pattern
{
    class InputHandler : ICommand
    {
        #region Singleton
        private static InputHandler instance;
        public InputHandler Instance
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

        public Dictionary<Keys, ICommand> keybindings = new Dictionary<Keys, ICommand>();

        public InputHandler()
        {

        }

        public void Excute()
        {
            KeyboardState key = Keyboard.GetState();

            foreach (Keys k in keybindings.Keys)
            {

            }
        }
    }
}
