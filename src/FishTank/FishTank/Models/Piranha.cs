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
using FishTank.Content;
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
            BoundaryBox = new Rectangle2(_swimArea.X + Constants.VirtualWidth / 2, 100, 75, 60);

            // Preload assets
            ContentBuilder.Instance.CreateRectangleTexture(_assetName, BoundaryBox.ToRectangle().Width, BoundaryBox.ToRectangle().Height);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Color? color = null;
            switch (State)
            {
                case InteractableState.Discard:
                    // fish is destroyed but awaiting cleanup. Don't draw
                    return;
                case InteractableState.Dead:
                    color = Color.Black;
                    break;
                case InteractableState.Alive:
                    if (CurrentHunger <= _hungerDangerValue) color = Color.Purple;
                    else if (CurrentHunger <= _hungerWarningValue) color = Color.Pink;
                    else color = Color.Red;
                    break;
            }

            if (color != null)
            {
                spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(_assetName), BoundaryBox.Location, color ?? Color.White);
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

        private readonly string _assetName = TextureNames.PiranhaAsset;
    }
}
