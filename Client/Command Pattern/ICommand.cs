using NotAGame.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Command_Pattern
{
    interface ICommand
    {
        void Excute(Player player);
    }
}
