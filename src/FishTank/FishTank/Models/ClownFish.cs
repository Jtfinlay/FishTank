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
    public class ClownFish : EconomicFish
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ClownFish"/> at given coordinates.
        /// </summary>
        /// <param name="x">X coordinate of the top-left corner of the created <see cref="ClownFish"/>.</param>
        /// <param name="y">Y coordinate of the top-left corner of the created <see cref="ClownFish"/>.</param>
        public ClownFish(float x, float y) : base()
        {
            Log.LogVerbose("Creating clown fish");

            // Set basic values
            _dropCoinTime = TimeSpan.FromSeconds(15);
            _maxHunger = 1.0f;
            _coinValue = Coin.SilverCoinValue;
            CurrentHunger = _maxHunger;

            BoundaryBox = new Rectangle2(x, y, _width, _height);

            LoadAssets();
        }

        /// <summary>
        /// Draw the <see cref="ClownFish"/> to the canvas
        /// </summary>
        /// <param name="spriteBatch">Graphics resource used for drawing</param>
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

        /// <summary>
        /// Manage hunger and internal states from provided nutrition value.
        /// </summary>
        /// <param name="nutrition">Float value indicating the effectiveness of the consumed food.</param>
        private void ConsumeFood(float nutrition)
        {
            CurrentHunger += nutrition;
            _totalConsumption += nutrition;

            _animationState = AnimationState.Eat;
            _eatAnimation.Reset();

            if (_totalConsumption >= _upgradeHungerThreshold)
            {
                // TODO
            }
        }

        /// <summary>
        /// <see cref="EventHandler"/> for when the 'eat' <see cref="Animation"/> has completed.
        /// </summary>
        /// <param name="sender">Instance of the invoking <see cref="Animation"/> element.</param>
        /// <param name="e">Argument object for this event. Null.</param>
        private void OnEatAnimationComplete(object sender, EventArgs e)
        {
            _animationState = AnimationState.None;
        }

        /// <summary>
        /// Load assets, <see cref="SpriteSheet"/>s, and <see cref="Animation"/>s for drawing.
        /// </summary>
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

        /// <summary>
        /// Threshold value at which the <see cref="ClownFish"/> evolves to a bigger <see cref="Fish"/>.
        /// </summary>
        private const float _upgradeHungerThreshold = 50;

        /// <summary>
        /// Constant width used for all instances of the <see cref="ClownFish"/>.
        /// </summary>
        private const int _width = 70;

        /// <summary>
        /// Constant height used for all instances of the <see cref="ClownFish"/>.
        /// </summary>
        private const int _height = 60;

        /// <summary>
        /// Threshold value which, when current velocity surpasses, triggers use of move animations
        /// </summary>
        private float _movementBuffer = 0.01f;

        /// <summary>
        /// Current <see cref="AnimationState"/> of the instance. Used for non-repeating <see cref="Animation"/>s.
        /// </summary>
        private AnimationState _animationState;

        /// <summary>
        /// Non-looping <see cref="Animation"/> invoked when consuming food.
        /// </summary>
        private Animation _eatAnimation;

        /// <summary>
        /// Handler for tiled multi-asset <see cref="SpriteSheet"/>. Holds all assets for the <see cref="Fish"/>.
        /// </summary>
        private SpriteSheet _spriteSheet;

        /// <summary>
        /// Path to the <see cref="SpriteSheet"/> asset for this <see cref="Fish"/>.
        /// </summary>
        private readonly string _spriteSheetAssetName = "sheets\\clownfish_sheet.png";

        /// <summary>
        /// Set of <see cref="Animation"/> objects used for the different hunger states 
        /// of the <see cref="ClownFish"/> during movement.
        /// </summary>
        private Animation _healthyMovementAnimation, _hungryMovementAnimation, _starvingMovementAnimation;

        /// <summary>
        /// Set of <see cref="Rectangle"/> instances used to show non-animated states from the <see cref="SpriteSheet"/>.
        /// </summary>
        private Rectangle _healthyTile, _hungryTile, _starvingTile, _deadTile;
    }
}
