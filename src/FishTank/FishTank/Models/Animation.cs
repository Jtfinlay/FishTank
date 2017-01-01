//
//  Copyright 2016 James Finlay
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace FishTank.Models
{
    public class Animation
    {
        public Animation(List<string> frames)
        {
            _animationFrames = frames;
        }

        public string CurrentAnimationFrame(GameTime gameTime)
        {
            if (gameTime.TotalGameTime > _changeAnimationFrameTarget)
            {
                _changeAnimationFrameTarget = gameTime.TotalGameTime + TimeSpan.FromMilliseconds(_animationDuration);
                _currentAnimationFrame++;
                if (_currentAnimationFrame >= _animationFrames.Count)
                {
                    _currentAnimationFrame = _loop ? 0 : _animationFrames.Count - 1;
                }
            }
            return _animationFrames[_currentAnimationFrame];
        }

        /// <summary>
        /// Time when to change animation frame.
        /// </summary>
        private TimeSpan _changeAnimationFrameTarget = default(TimeSpan);

        /// <summary>
        /// Time, in milliseconds, until moving to next animation frame.
        /// </summary>
        private int _animationDuration = 100;

        /// <summary>
        /// Index of the next animation frame to draw
        /// </summary>
        private int _currentAnimationFrame = 0;

        /// <summary>
        /// Boolean indicating whether the animation loops back to the first after completion
        /// </summary>
        private bool _loop = true;

        private List<string> _animationFrames;
    }
}
