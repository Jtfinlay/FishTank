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

using FishTank.Components;
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using FishTank.Utilities.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FishTank.Models
{

    /// <summary>
    /// Abstract class for Fish that feel hunger and drop coins
    /// </summary>
    public abstract class EconomicFish : Fish
    {
        public EconomicFish() : base() { }

        protected override void UpdateAlive(List<IInteractable> models, GameTime gameTime)
        {
            // Check whether to drop coin
            _timeSinceCoinDrop += gameTime.ElapsedGameTime;
            if (_timeSinceCoinDrop >= _dropCoinTime)
            {
                _timeSinceCoinDrop = TimeSpan.Zero;
                InvokeOnItemDrop(this, new ItemDropEventArgs(typeof(Coin), BoundaryBox.Center.ToVector2(), new Vector2(0,0)));
            }

            // First, try to find food if nearby
            if (SearchForFood(models))
            {
                _wanderingTarget = null;
                return;
            }

            _currentHunger -= _hungerDropPerFrame;
            if (_currentHunger <= 0)
            {
                State = InteractableState.Dead;
                return;
            }

            // Nothing to do, just wander
            WanderAround();
        }

        protected float CurrentHunger
        {
            get
            {
                return _currentHunger;
            }
            set
            {
                _currentHunger = (value > _maxHunger) ? _maxHunger : value;
            }
        }

        /// <summary>
        ///  The maximum hunger of the fish
        /// </summary>
        protected float _maxHunger;

        /// <summary>
        /// Current hunger of the fish. At zero the fish dies
        /// </summary>
        private float _currentHunger;

        /// <summary>
        /// Amount of nutrition the fish has consumed.
        /// </summary>
        protected float _totalConsumption;

        /// <summary>
        /// Value to decrement hunger each frame
        /// </summary>
        private const float _hungerDropPerFrame = 1 / (Constants.ExpectedFramesPerSecond * 25);

        /// <summary>
        /// Value where the fish hits first level of hunger and seeks food
        /// </summary>
        protected const float _hungerStartValue = .7f;

        /// <summary>
        /// Value where fish hits 'warning' level of hunger
        /// </summary>
        protected const float _hungerWarningValue = .4f;

        /// <summary>
        /// Value where fish hits 'danger' level of hunger
        /// </summary>
        protected const float _hungerDangerValue = .15f;

        /// <summary>
        /// Timespan indicating how often the fish should drop a coin. Required
        /// </summary>
        protected TimeSpan _dropCoinTime;

        /// <summary>
        /// Timespan tracking the time since the last coin drop
        /// </summary>
        private TimeSpan _timeSinceCoinDrop = TimeSpan.Zero;
    }
}
