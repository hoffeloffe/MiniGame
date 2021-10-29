using Microsoft.Xna.Framework;
using NotAGame.Command_Pattern;
using SpaceRTS;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace NotAGame.Component
{
    class Opponent : Component
    {
        private Vector2 Position = new Vector2(200, 200);
        SpriteRenderer sr;
        public SoundEffectInstance _effect;
        public bool Loop { get; set; } = false;
        public bool ID { get; set; }
        public string SoundPath { get; set; } = "Sounds/Bark1";

        public Opponent()
        {
            _effect = GameWorld.Instance.Content.Load<SoundEffect>("Sounds/Bark1").CreateInstance();
        }
        public Opponent(Color color)
        {
            sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            sr.Color = color;
        }
        public Opponent(int x, int y)
        {
            //Position = new Vector2(x, y);
        }
        public override void Awake()
        {
            //GameObject.transform.Position = new Vector2(975, 500);
        }
        public override void Start()
        {
            sr = (SpriteRenderer)GameObject.GetComponent("SpriteRenderer");
            sr.SetSpriteName("Sprites/dog_stand");
            //sr.Color = new Color(0, 0, 0);
        }
        public override void Update(GameTime gametime)
        {
            //sr.GameObject.transform.Position = Position;
            base.Update(gametime);
        }

        public override string ToString()
        {
            return "Opponent";
        }

        private void UpdatePanning(float x)
        {
            //Panorering af lyd:
            float percentage = x * 100.0f / GameWorld.Instance.GraphicsDevice.Viewport.Width - 1f;
            float calculation = 0 + (2 - 0) * percentage / 100 - 1f;
            if (calculation > 1)
            {
                _effect.Pan = 1f;
            }
            else if (calculation < -1)
            {
                _effect.Pan = -1f;
            }
            else
            {
                _effect.Pan = calculation;
            }
        }

        public void PlaySound(bool loop, float x)
         {
            _effect = GameWorld.Instance.Content.Load<SoundEffect>(SoundPath).CreateInstance();
            Loop = loop;
            _effect.IsLooped = loop;

            //Pitch randomization:
            Random random = new Random();
            double val = (random.NextDouble() * (1 - (-1)) + (-1));
            _effect.Pitch = (float)val;

            UpdatePanning(x);
            _effect.Stop();
            _effect.Play();
        }
    }
}
