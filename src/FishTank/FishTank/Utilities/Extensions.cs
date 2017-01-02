//
//  Copyright 2017 James Finlay
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using FishTank.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FishTank.Utilities
{
    public static class Extensions
    {
        public static MouseState SetPosition(this MouseState mouseState, Vector2 position)
        {
            return new MouseState((int)position.X, (int)position.Y, mouseState.ScrollWheelValue,
                mouseState.LeftButton, mouseState.MiddleButton, mouseState.RightButton,
                mouseState.XButton1, mouseState.XButton2);
        }

        public static bool Within(this Point point, Rectangle area)
        {
            return (point.X > area.Left) && (point.X < area.Right)
                && (point.Y > area.Top) && (point.Y < area.Bottom);
        }

        public static Rectangle ApplyPadding(this Rectangle rectangle, int padding, Alignment sides)
        {
            var result = rectangle;
            var x = rectangle.Left;
            var y = rectangle.Top;
            var width = rectangle.Width;
            var height = rectangle.Height;

            if (sides.HasFlag(Alignment.Left))
            {
                result.X += padding;
                result.Width -= padding;
            }

            if (sides.HasFlag(Alignment.Top))
            {
                result.Y += padding;
                result.Height -= padding;
            }

            if (sides.HasFlag(Alignment.Right))
            {
                result.Width -= padding;
            }

            if (sides.HasFlag(Alignment.Bottom))
            {
                result.Height -= padding;
            }

            return result;
        }

        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Rectangle bounds, Alignment align, Color color)
        {
            Vector2 size = font.MeasureString(text);
            Vector2 pos = bounds.Center.ToVector2();
            Vector2 origin = size * 0.5f;

            if (align.HasFlag(Alignment.Left))
            {
                origin.X += bounds.Width / 2 - size.X / 2;
            }

            if (align.HasFlag(Alignment.Right))
            {
                origin.X -= bounds.Width / 2 - size.X / 2;
            }

            if (align.HasFlag(Alignment.Top))
            {
                origin.Y += bounds.Height / 2 - size.Y / 2;
            }

            if (align.HasFlag(Alignment.Bottom))
            {
                origin.Y -= bounds.Height / 2 - size.Y / 2;
            }

            spriteBatch.DrawString(font, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw given texture by the provided, and the texture's, center point
        /// </summary>
        /// <param name="spriteBatch">Helper object for drawing text strings and sprites in one or more optimized batches.</param>
        /// <param name="texture">Texture to draw</param>
        /// <param name="position">Position to draw the center of the texture</param>
        /// <param name="color">An optional color mask. Uses Microsoft.Xna.Framework.Color.White if null.</param>
        public static void DrawCenterAt(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color? color)
        {
            // Get top left position to draw at
            var drawPosition = position - new Vector2((texture.Width / 2), (texture.Height/2));
            spriteBatch.Draw(texture, drawPosition, color: color);
        }
    }
}
