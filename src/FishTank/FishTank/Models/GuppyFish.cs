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

using FishTank.Content;
using FishTank.Drawing;
using FishTank.Instrumentation;
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using FishTank.Utilities.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FishTank.Models
{
    /// <summary>
    /// Class instance for the <see cref="GuppyFish"/>. This is the simplest <see cref="Fish"/> available in the game. It
    /// chases food, drops coins, and dies. Upgrades to a <see cref="ClownFish"/>.
    /// </summary>
    public class GuppyFish : EconomicFish
    {
        /// <summary>
        /// Basic fish that chases food and dies
        /// </summary>
        /// <param name="graphicsDevice">Graphics resource for texture creation</param>
        public GuppyFish() : base()
        {
            Log.LogVerbose("Creating guppy fish");

            _dropCoinTime = TimeSpan.MaxValue;
            _maxHunger = 1.0f;
            _coinValue = 0;
            CurrentHunger = _maxHunger;

            BoundaryBox = new Rectangle2(_swimArea.X + Constants.VirtualWidth / 2, 100, _width, _height);

            LoadAssets();
        }

        /// <summary>
        /// Draw the <see cref="GuppyFish"/> to the canvas
        /// </summary>
        /// <param name="spriteBatch">Graphics resource for drawing</param>
        /// <param name="gameTime">Time measurements for the game world</param>
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
                    else if (_animationState == AnimationState.Eat)
                    {
                        sourceRectangle = _eatAnimation.CurrentSourceRectangle(gameTime);
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

        /// <summary>
        /// Invoked by another entity that has targeted this fish as food
        /// </summary>
        public void Eat()
        {
            State = InteractableState.Discard;
        }

        /// <summary>
        /// If fish is hungry, find nearby food and move to consume it
        /// </summary>
        /// <param name="models">List of  all interactable objects on the field</param>
        /// <returns>Bool indicating whether targeting a source of food</returns>
        protected override bool SearchForFood(List<IInteractable> models)
        {
            if (CurrentHunger > _hungerStartValue)
            {
                return false;
            }

            Pellet nearestPellet = models.Where((model) => model is Pellet)?
                .OrderBy(i => Vector2.Distance(i.BoundaryBox.Center, BoundaryBox.Center)).FirstOrDefault() as Pellet;
            if (nearestPellet != null)
            {
                float distance = Vector2.Distance(nearestPellet.BoundaryBox.Center, BoundaryBox.Center);
                if (distance < 30)
                {
                    ConsumeFood(nearestPellet.Eat());
                    return true;
                }

                Vector2 direction = Vector2.Normalize(nearestPellet.BoundaryBox.Center - BoundaryBox.Center);
                MoveTowards(direction);
                return true;
            }
            return false;
        }

        private void ConsumeFood(float nutrition)
        {
            CurrentHunger += nutrition;
            _totalConsumption += nutrition;

            _animationState = AnimationState.Eat;
            _eatAnimation.Reset();

            if (_totalConsumption >= _upgradeHungerThreshold)
            {
                InvokeOnItemDrop(this, new ItemDropEventArgs(new ClownFish(BoundaryBox.Left, BoundaryBox.Top)));
                State = InteractableState.Discard;
            }
        }

        private void LoadAssets()
        {
            _spriteSheet = new SpriteSheet(_spriteSheetAssetName, BoundaryBox.Size.ToPoint());

            // Healthy movement
            var animationFrames = new List<Point>()
            {
                new Point(0,0),
                new Point(0, _height),
                new Point(0,0),
                new Point(0, _height * 2),
            };
            _healthyMovementAnimation = new Animation(_spriteSheet, animationFrames);

            // Hungry movement
            animationFrames = animationFrames.Select((point) => { point += new Point(_width, 0); return point; }).ToList();
            _hungryMovementAnimation = new Animation(_spriteSheet, animationFrames);

            // Starving movement
            animationFrames = animationFrames.Select((point) => { point += new Point(_width, 0); return point; }).ToList();
            _starvingMovementAnimation = new Animation(_spriteSheet, animationFrames);

            // Eat animation
            animationFrames = new List<Point>() { new Point(_width * 3, _height) };
            _eatAnimation = new Animation(_spriteSheet, animationFrames, false, 200);
            _eatAnimation.OnAnimationComplete += OnEatAnimationComplete;

            // Still frames
            _healthyTile = new Rectangle(new Point(0, 0), _spriteSheet.TileSize);
            _hungryTile = new Rectangle(new Point(_width, 0), _spriteSheet.TileSize);
            _starvingTile = new Rectangle(new Point(_width * 2, 0), _spriteSheet.TileSize);
            _deadTile = new Rectangle(new Point(_width * 3, 0), _spriteSheet.TileSize);

            // Preload assets
            ContentBuilder.Instance.LoadTextureByName(_spriteSheetAssetName);
        }

        private void OnEatAnimationComplete(object sender, EventArgs e)
        {
            _animationState = AnimationState.None;
        }

        private const float _upgradeHungerThreshold = 10;

        /// <summary>
        /// Constant width used for all instances of the <see cref="GuppyFish"/>.
        /// </summary>
        private const int _width = 47;

        /// <summary>
        /// Constant height used for all instances of the <see cref="GuppyFish"/>.
        /// </summary>
        private const int _height = 40;

        /// <summary>
        /// Threshold value which, when current velocity surpasses, triggers use of move animations
        /// </summary>
        private float _movementBuffer = 0.01f;

        private AnimationState _animationState;

        /// <summary>
        /// Set of <see cref="Animation"/> objects used for the different hunger states 
        /// of the <see cref="GuppyFish"/> during movement.
        /// </summary>
        private Animation _healthyMovementAnimation, _hungryMovementAnimation, _starvingMovementAnimation;

        /// <summary>
        /// <see cref="Animation"/> object used to show guppy eating.
        /// </summary>
        private Animation _eatAnimation;

        private SpriteSheet _spriteSheet;

        private readonly string _spriteSheetAssetName = "sheets\\guppy_sheet.png";

        /// <summary>
        /// Set of <see cref="Rectangle"/> instances used to show non-animated states from the <see cref="SpriteSheet"/>.
        /// </summary>
        private Rectangle _healthyTile, _hungryTile, _starvingTile, _deadTile;
    }
}
