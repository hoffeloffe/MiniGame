using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NotAGame.Component;
using NotAGame;

namespace SpaceRTS
{
    public class GameObject
    {
        #region fields

        protected Texture2D sprite;
        protected Vector2 position;
        protected Color color;
        protected Vector2 origin;
        protected Vector2 scale;
        protected float rotation;
        protected int offsetX;
        protected int offsetY;

        #endregion fields

        #region GameObject component version

        private Dictionary<string, Component> components = new Dictionary<string, Component>();
        public string Tag { get; set; }
        public Transform transform { get; set; }

        public GameObject()
        {
            transform = new Transform();
        }

        public void AddComponent(Component component)
        {
            components.Add(component.ToString(), component);

            component.GameObject = this;
        }

        public Component GetComponent(string component)
        {
            return components[component];
        }

        public void Awake()
        {
            foreach (Component component in components.Values)
            {
                component.Awake();
            }
        }

        public void Start()
        {
            foreach (Component component in components.Values)
            {
                component.Start();
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Component component in components.Values)
            {
                if (component.IsEnabled)
                {
                    component.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Component component in components.Values)
            {
                if (component.IsEnabled)
                {
                    component.Draw(spriteBatch);
                }
            }
        }

        #endregion GameObject component version

        //public GameObject()
        //{
        //}

        //public virtual Rectangle Collision
        //{
        //    get
        //    {
        //        return new Rectangle(
        //               (int)position.X + offsetX,
        //               (int)position.Y,
        //               (int)sprite.Width,
        //               (int)sprite.Height + offsetY
        //           );
        //    }
        //}

        //public abstract void LoadContent(ContentManager content);

        //public abstract void Update(GameTime gameTime);

        //public virtual void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(sprite, position, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 1);
        //}

        //public abstract void OnCollision(GameObject other);

        //public void CheckCollision(GameObject other)
        //{
        //    if (Collision.Intersects(other.Collision))
        //    {
        //        OnCollision(other);
        //    }
        //}
    }
}