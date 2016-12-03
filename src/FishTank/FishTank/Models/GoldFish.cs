//
// Copyright - James Finlay
// 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FishTank.Models
{
    public enum FishState
    {
        None,
        MovingToPoint,
        RunFromAlien,
        SearchForFood,
    }
    internal class GoldFish : Fish
    {
        /// <summary>
        ///  The maximum hunger of the fish
        /// </summary>
        public override float MaxHunger => 100;

        private const float MaxSpeed = 3.0f;

        private FishState _state = FishState.None;

        private Random _random;

        /// <summary>
        /// Expect fish to move every 4 seconds or so.
        /// </summary>
        private readonly float _probabilityToMovePerFrame = 1f / (3.5f * Game1.ExpectedFramesPerSecond);

        /// <summary>
        /// Animation representing the fish
        /// </summary>
        public override Texture2D Texture { get; set; }

        public GoldFish(GraphicsDevice graphicsDevice) : base()
        {
            var rect = new Texture2D(graphicsDevice, 30, 30);

            Color[] data = new Color[30 * 30];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Chocolate;
            }
            rect.SetData(data);

            Texture = rect;
            Position = new Vector2(
                graphicsDevice.Viewport.TitleSafeArea.X, 
                graphicsDevice.Viewport.TitleSafeArea.Y + graphicsDevice.Viewport.TitleSafeArea.Height / 2);

            _random = new Random();
        }

        public override void Update()
        {
            switch (_state)
            {
                case FishState.None:
                    DetermineNextAction();
                    break;
                case FishState.MovingToPoint:
                    MoveToPoint();
                    break;
                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null);
        }

        private void MoveToPoint()
        {
            Position = Vector2.Add(Position, new Vector2(MaxSpeed, 0));
        }

        private void DetermineNextAction()
        {
            if (_random.NextDouble() > _probabilityToMovePerFrame)
            {
                // Doesn't meet probability check. Don't move to random location
                return;
            }
            _state = FishState.MovingToPoint;
        }
    }
}
