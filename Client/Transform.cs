
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame
{
    public class Transform
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }

        public void Translate(Vector2 translation)
        {
            if (!float.IsNaN(translation.X) && !float.IsNaN(translation.Y))
            {
                Position += translation;
            }
        }
        public void Roll(float rotation)
        {
            if (!float.IsNaN(rotation))
            {
                Rotation += rotation;
            }
        }
    }
}
