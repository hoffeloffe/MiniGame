using Microsoft.Xna.Framework;
using NotAGame.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame.Command_Pattern
{
    class MoveCommand : ICommand
    {
        private Vector2 velocity;

        public MoveCommand(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        public void Excute(Player player)
        {
            player.Move(velocity);
        }
    }
}
