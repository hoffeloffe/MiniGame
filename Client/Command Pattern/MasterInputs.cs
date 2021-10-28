using NotAGame.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Command_Pattern
{
    class MasterInputs : ICommand
    {
        public void Excute(Player player)
        {
            player.ChangeInput();
        }
    }
}
