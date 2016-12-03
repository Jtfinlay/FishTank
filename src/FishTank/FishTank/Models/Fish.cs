//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FishTank.Models
{
    /// <summary>
    /// G
    /// </summary>
    internal abstract class Fish
    {
        /// <summary>
        /// Current hunger of the fish. At zero the fish dies
        /// </summary>
        public float Hunger { get; set; }

        /// <summary>
        /// The maximum hunger of the fish
        /// </summary>
        public abstract float MaxHunger { get; }

        /// <summary>
        /// Position of the fish relative to the upper left of the screen
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Animation representing the fish
        /// </summary>
        public abstract Texture2D Texture { get; set; }

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
