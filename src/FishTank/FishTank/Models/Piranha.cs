﻿//
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

            BoundaryBox = new Rectangle2(new Vector2(_swimArea.X + Constants.VirtualWidth / 2, 100), _size.ToVector2());

            LoadAssets();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            SpriteEffects spriteEffects = (_facingLeft) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Rectangle? sourceRectangle = default(Rectangle?);
            switch (State)
            {
                case InteractableState.Discard:
                    // fish is destroyed but await cleanup. Don't draw
                    break;
                case InteractableState.Dead:
                    sourceRectangle = _deadTile;
                    break;
                case InteractableState.Alive:
                    bool fishIsMoving = Math.Abs(_currentVelocity.Length()) > _movementBuffer;
                    if (CurrentHunger <= _hungerDangerValue)
                    {
                        sourceRectangle = (fishIsMoving) ? _starvingMovementAnimation.CurrentSourceRectangle(gameTime) : _starvingTile;
                    }
                    else if (CurrentHunger <= _hungerWarningValue)
                    {
                        sourceRectangle = (fishIsMoving) ? _hungryMovementAnimation.CurrentSourceRectangle(gameTime) : _hungryTile;
                    }
                    else
                    {
                        sourceRectangle = (fishIsMoving) ? _healthyMovementAnimation.CurrentSourceRectangle(gameTime) : _healthyTile;
                    }
                    break;
            }

            if (sourceRectangle.HasValue && !((Rectangle)sourceRectangle).IsEmpty)
            {
                spriteBatch.Draw(
                    texture: ContentBuilder.Instance.LoadTextureByName(_spriteSheet.AssetName),
                    sourceRectangle: sourceRectangle,
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

            // Find the nearest alive Clownfish at stage 0 (a guppy)
            ClownFish nearestGuppy = models.Where((model) => ((model as ClownFish)?.State == InteractableState.Alive) && (model as ClownFish)?.Stage == 0)?
                .OrderBy(i => Vector2.Distance(i.BoundaryBox.Center, BoundaryBox.Center)).FirstOrDefault() as ClownFish;
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

        private void LoadAssets()
        {
            // Healthy movement
            var animationFrames = new List<Point>()
            {
                new Point(0,0),
                new Point(0, _size.Y),
                new Point(0,0),
                new Point(0, _size.Y * 2),
            };
            _healthyMovementAnimation = new Animation(_spriteSheet, animationFrames);

            // Hungry movement
            animationFrames = animationFrames.Select((point) => { point += new Point(_size.X, 0); return point; }).ToList();
            _hungryMovementAnimation = new Animation(_spriteSheet, animationFrames);

            // Starving movement
            animationFrames = animationFrames.Select((point) => { point += new Point(_size.X, 0); return point; }).ToList();
            _starvingMovementAnimation = new Animation(_spriteSheet, animationFrames);

            // Eat animation
            animationFrames = new List<Point>() { new Point(_size.X * 3, _size.Y) };
            _eatAnimation = new Animation(_spriteSheet, animationFrames, false);

            // Still frames
            _healthyTile = new Rectangle(new Point(0, 0), _spriteSheet.TileSize);
            _hungryTile = new Rectangle(new Point(_size.X, 0), _spriteSheet.TileSize);
            _starvingTile = new Rectangle(new Point(_size.X * 2, 0), _spriteSheet.TileSize);
            _deadTile = new Rectangle(new Point(_size.X * 3, 0), _spriteSheet.TileSize);
        }

        private Point _size => _spriteSheet.TileSize;

        /// <summary>
        /// Threshold value which, when current velocity surpasses, triggers use of move animations
        /// </summary>
        private float _movementBuffer = 0.01f;

        private Animation _healthyMovementAnimation;

        private Animation _hungryMovementAnimation;

        private Animation _starvingMovementAnimation;

        private Animation _eatAnimation;

        private Rectangle _healthyTile, _hungryTile, _starvingTile, _deadTile;

        private SpriteSheet _spriteSheet => SpriteSheets.PiranhaSheet;
    }
}
