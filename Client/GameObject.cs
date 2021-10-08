using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SpaceRTS
{
    public abstract class GameObject
    {
        protected Texture2D sprite;
        protected Vector2 position;
        protected Color color;
        protected Vector2 origin;
        protected Vector2 scale;
        protected float rotation;
        protected int offsetX;
        protected int offsetY;

        public GameObject()
        {
        }

        public virtual Rectangle Collision
        {
            get
            {
                return new Rectangle(
                       (int)position.X + offsetX,
                       (int)position.Y,
                       (int)sprite.Width,
                       (int)sprite.Height + offsetY
                   );
            }
        }

        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 1);
        }

        public abstract void OnCollision(GameObject other);

        public void CheckCollision(GameObject other)
        {
            if (Collision.Intersects(other.Collision))
            {
                OnCollision(other);
            }
        }
    }
}