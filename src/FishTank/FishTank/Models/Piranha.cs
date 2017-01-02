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

using FishTank.Components;
using FishTank.Content;
using FishTank.Drawing;
using FishTank.Instrumentation;
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FishTank.Models
{
    public class Piranha : EconomicFish
    {
        public Piranha() : base()
        {
            Log.LogVerbose("Creating piranha");

            _dropCoinTime = TimeSpan.FromSeconds(20);
            _maxHunger = 2.0f;
            _coinValue = Coin.DiamondCoinValue;
            CurrentHunger = _maxHunger;

            _swimArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);
            BoundaryBox = new Rectangle2(_swimArea.X + Constants.VirtualWidth / 2, 100, _width, _height);

            _spriteSheet = new SpriteSheet(_spriteSheetAssetName, BoundaryBox.Size.ToPoint());
            List<Point> animationFrames = new List<Point>()
            {
                new Point(0,0),
                new Point(0, _height),
                new Point(0,0),
                new Point(0, _height * 2),
            };
            _movementAnimation = new Animation(_spriteSheet, animationFrames);

            // Preload assets
            ContentBuilder.Instance.LoadTextureByName(_spriteSheetAssetName);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            SpriteEffects spriteEffects = (_facingLeft) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            bool fishIsMoving = Math.Abs(_currentVelocity.Length()) > _movementBuffer;
            if (fishIsMoving)
            {
                _movementAnimation.Draw(spriteBatch, gameTime, BoundaryBox.Location, spriteEffects);
            }
            else
            {
                spriteBatch.Draw(
                    texture: ContentBuilder.Instance.LoadTextureByName(_spriteSheet.AssetName),
                    sourceRectangle: _spriteSheet.DefaultTile,
                    position: BoundaryBox.Location,
                    effects: spriteEffects);
            }
        }

        protected override bool SearchForFood(List<IInteractable> models)
        {
            if (CurrentHunger > _hungerStartValue)
            {
                return false;
            }

            GuppyFish nearestGuppy = models.Where((model) => (model as GuppyFish)?.State == InteractableState.Alive)?
                .OrderBy(i => Vector2.Distance(i.BoundaryBox.Center, BoundaryBox.Center)).FirstOrDefault() as GuppyFish;
            if (nearestGuppy != null)
            {
                float distance = Vector2.Distance(nearestGuppy.BoundaryBox.Center, BoundaryBox.Center);
                if (distance < 30)
                {
                    CurrentHunger = _maxHunger;
                    nearestGuppy.Eat();
                    return true;
                }

                Vector2 direction = Vector2.Normalize(nearestGuppy.BoundaryBox.Center - BoundaryBox.Center);
                MoveTowards(direction);
                return true;
            }
            return false;
        }

        private const int _width = 120;

        private const int _height = 88;

        /// <summary>
        /// Threshold value which, when current velocity surpasses, triggers use of move animations
        /// </summary>
        private float _movementBuffer = 0.01f;

        private Animation _movementAnimation;

        private SpriteSheet _spriteSheet;

        private readonly string _spriteSheetAssetName = "Piranha\\piranha_sheet.png";
    }
}
