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
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FishTank.Drawing
{
    /// <summary>
    /// Describes a drawable <see cref="Animation"/> object that manages resources to draw assets frame-by-frame.
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// <see cref="EventHandler"/> invoked when an iteration of the <see cref="Animation"/> has completed.
        /// </summary>
        public event EventHandler OnAnimationComplete;

        /// <summary>
        /// Denotes the number of <see cref="Frames"/> in one animation loop
        /// </summary>
        public int Frames => (int)(_sheetFrames?.Count);

        /// <summary>
        /// Creates a new instance of the <see cref="Animation"/> object using a given <see cref="SpriteSheet"/>
        /// and a list of <see cref="Point"/> coordinates that provide the locations on the <see cref="SpriteSheet"/>
        /// for each available animation frame.
        /// </summary>
        /// <param name="spriteSheet"><see cref="SpriteSheet"/> instance holding target asset</param>
        /// <param name="frames">List of locations on the <see cref="SpriteSheet"/> to use for this <see cref="Animation"/>.</param>
        /// <param name="loop">Whether to loop the animation frames</param>
        public Animation(SpriteSheet spriteSheet, List<Point> frames, bool loop = true, int animationDuration = 100)
        {
            _sheetFrames = frames;
            _spriteSheet = spriteSheet;
            _loop = loop;
            _animationDuration = animationDuration;
        }

        /// <summary>
        /// Submit the current animation frame for drawing in the current batch.
        /// </summary>
        /// <param name="spriteBatch">Help class for drawing asset in the current batch.</param>
        /// <param name="gameTime">Performance object tracking elapsed time.</param>
        /// <param name="position">Coordinates on the canvas to draw the current frame.</param>
        /// <param name="effects">Any special effects to apply to the drawn frame.</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 position, SpriteEffects effects)
        {
            IncrementAnimationFrame(gameTime);

            spriteBatch.Draw(
                texture: ContentBuilder.Instance.LoadTextureByName(_currentAnimationAsset),
                position: position,
                sourceRectangle: _currentSourceRectangle,
                effects: effects);
        }

        /// <summary>
        /// Helper method that updates the animation frame and returns the appropriate tile location as a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="gameTime">Performance object tracking elapsed time.</param>
        /// <returns>Source location of the current tile as a <see cref="Rectangle"/>.</returns>
        public Rectangle? CurrentSourceRectangle(GameTime gameTime)
        {
            IncrementAnimationFrame(gameTime);
            return _currentSourceRectangle;
        }

        /// <summary>
        /// Resets the <see cref="Animation"/>'s tile iterator and the target <see cref="TimeSpan"/>.
        /// </summary>
        public void Reset()
        {
            _changeAnimationFrameTarget = default(TimeSpan);
            _currentAnimationFrame = 0;
        }

        /// <summary>
        /// The asset to use when drawing. Either a specific asset from the provided list, or a full <see cref="SpriteSheet"/> asset.
        /// </summary>
        private string _currentAnimationAsset => _spriteSheet.AssetName;

        /// <summary>
        /// The area of the asset to draw. Either a specific area or the default value (when no spritesheet provided)
        /// </summary>
        private Rectangle? _currentSourceRectangle
        {
            get
            {
                Point origin = _sheetFrames[_currentAnimationFrame];
                return new Rectangle(origin, _spriteSheet.TileSize);
            }
        }

        /// <summary>
        /// Increment the current animation frame as neccessary depending on current <see cref="GameTime"/>.
        /// </summary>
        /// <param name="gameTime">Performance object tracking elapsed time.</param>
        private void IncrementAnimationFrame(GameTime gameTime)
        {
            if (_changeAnimationFrameTarget == default(TimeSpan))
            {
                // If the target animation time is not set, then animation is starting fresh.
                _changeAnimationFrameTarget = gameTime.TotalGameTime + TimeSpan.FromMilliseconds(_animationDuration);
            }

            if (gameTime.TotalGameTime > _changeAnimationFrameTarget)
            {
                _changeAnimationFrameTarget = gameTime.TotalGameTime + TimeSpan.FromMilliseconds(_animationDuration);
                _currentAnimationFrame++;
                if (_currentAnimationFrame >= Frames)
                {
                    _currentAnimationFrame = _loop ? 0 : Frames - 1;
                    OnAnimationComplete?.Invoke(this, null);
                }
            }
        }

        /// <summary>
        /// Time when to change animation frame.
        /// </summary>
        private TimeSpan _changeAnimationFrameTarget;

        /// <summary>
        /// Time, in milliseconds, until moving to next animation frame.
        /// </summary>
        private int _animationDuration;

        /// <summary>
        /// Index of the next animation frame to draw
        /// </summary>
        private int _currentAnimationFrame = 0;

        /// <summary>
        /// Boolean indicating whether the animation loops back to the first after completion
        /// </summary>
        private bool _loop;

        /// <summary>
        /// Handler for tiles multi-asset <see cref="SpriteSheet"/>.
        /// </summary>
        private SpriteSheet _spriteSheet;

        /// <summary>
        /// Ordered list of coordinates on the <see cref="SpriteSheet"/> that denote the individual
        /// frames to draw.
        /// </summary>
        private List<Point> _sheetFrames;
    }
}
