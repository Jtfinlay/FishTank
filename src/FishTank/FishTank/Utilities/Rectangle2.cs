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

using Microsoft.Xna.Framework;

namespace FishTank.Utilities
{
    /// <summary>
    /// Describes a 2D-rectangle using floating point
    /// </summary>
    public class Rectangle2
    {
        /// <summary>
        /// Height of this <see cref="Rectangle2"/>.
        /// </summary>
        public float Width;

        /// <summary>
        /// Width of this <see cref="Rectangle2"/>.
        /// </summary>
        public float Height;

        /// <summary>
        /// The x coordinate of the top-left corner of this <see cref="Rectangle2"/>.
        /// </summary>
        public float X;

        /// <summary>
        /// The y coordinate of the top-left corner of this <see cref="Rectangle2"/>.
        /// </summary>
        public float Y;

        /// <summary>
        /// Creates a new instance of the <see cref="Rectangle2"/> object, with the
        /// specified location and size
        /// </summary>
        /// <param name="x">X coordinate of the top-left corner of the created <see cref="Rectangle2"/>.</param>
        /// <param name="y">Y coordinate of the top-left corner of the created <see cref="Rectangle2"/>.</param>
        /// <param name="width">Width of the created <see cref="Rectangle2"/>.</param>
        /// <param name="height">Height of the created <see cref="Rectangle2"/>.</param>
        public Rectangle2(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rectangle2"/> object, with the
        /// specified location and size
        /// </summary>
        /// <param name="location">X and Y coordinate of the top-left corner of the created <see cref="Rectangle2"/>.</param>
        /// <param name="size">Width and Height of the created <see cref="Rectangle2"/>.</param>
        public Rectangle2(Vector2 location, Vector2 size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.X;
            Height = size.Y;
        }

        /// <summary>
        /// Returns a <see cref="Rectangle2"/> with X=0, Y=0, Width=0, Height=0.
        /// </summary>
        public static Rectangle2 Empty => new Rectangle2(0, 0, 0, 0);

        /// <summary>
        /// Returns the y coordinate of the bottom oedge of this <see cref="Rectangle2"/>.
        /// </summary>
        public float Bottom => Y + Height;

        /// <summary>
        /// A <see cref="Vector2"/> located in the center of this <see cref="Rectangle2"/>
        /// </summary>
        public Vector2 Center => Location + Size / 2;

        /// <summary>
        /// Returns the x coordinate of the left edge of this <see cref="Rectangle2"/>.
        /// </summary>
        public float Left => X;

        /// <summary>
        /// The top-left coordinates of this <see cref="Rectangle2"/>.
        /// </summary>
        public Vector2 Location => new Vector2(X, Y);

        /// <summary>
        /// Returns the x coordinate of the right edge of this <see cref="Rectangle2"/>.
        /// </summary>
        public float Right => X + Width;

        /// <summary>
        /// The width-height coordinates of this <see cref="Rectangle2"/>.
        /// </summary>
        public Vector2 Size => new Vector2(Width, Height);

        /// <summary>
        /// Returns the y coordinate of the top edge of this <see cref="Rectangle2"/>.
        /// </summary>
        public float Top => Y;

        /// <summary>
        /// Determine whether or not the provided coordinates lie within the bounds of this <see cref="Rectangle2"/>.
        /// </summary>
        /// <param name="x">The x coordinate of the point to check for containment</param>
        /// <param name="y">They y coordinate of the point to check for containment</param>
        /// <returns>true if the provided coordinates lie inside this <see cref="Rectangle2"/>; false otherwise.</returns>
        public bool Contains(float x, float y)
        {
            if (x > Right) return false;
            if (x < Left) return false;
            if (y < Top) return false;
            if (y > Bottom) return false;
            return true;
        }

        /// <summary>
        /// Determine whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="Rectangle2"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vector2"/> to check for containment</param>
        /// <returns>true if the provided <see cref="Vector2"/> lies inside this <see cref="Rectangle2"/>; false otherwise.</returns>
        public bool Contains(Vector2 value) => Contains(value.X, value.Y);

        /// <summary>
        /// Determine whether or not the provided <see cref="Point"/> lies within the bounds of this <see cref="Rectangle2"/>.
        /// </summary>
        /// <param name="value">The <see cref="Point"/> to check for containment</param>
        /// <returns>true if the provided <see cref="Point"/> lies inside this <see cref="Rectangle2"/>; false otherwise.</returns>
        public bool Contains(Point value) => Contains(value.X, value.Y);

        /// <summary>
        /// Creates a new instance of the <see cref="Rectangle"/>
        /// </summary>
        /// <returns>Instance of <see cref="Rectangle"/></returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int) X, (int) Y, (int) Width, (int)Height);
        }
    }
}
