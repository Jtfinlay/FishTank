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
    public class Piranha : Fish
    {
        public Piranha() : base()
        {
            Log.LogVerbose("Creating piranha");

            _dropCoinTime = TimeSpan.FromSeconds(20);
            _maxHunger = 2.0f;
            _currentHunger = _maxHunger;

            _swimArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);
            BoundaryBox = new Rectangle(_swimArea.X + Constants.VirtualWidth / 2, 100, 75, 60);

            // Preload assets
            ContentBuilder.Instance.CreateRectangleTexture(_assetName, BoundaryBox.Width, BoundaryBox.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
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
                    if (_currentHunger <= _hungerDangerValue) color = Color.Purple;
                    else if (_currentHunger <= _hungerWarningValue) color = Color.Pink;
                    else color = Color.Red;
                    break;
            }

            if (color != null)
            {
                spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(_assetName), BoundaryBox.Location.ToVector2(), color ?? Color.White);
            }
        }

        protected override bool SearchForFood(List<IInteractable> models)
        {
            if (_currentHunger > _hungerStartsValue)
            {
                return false;
            }

            GuppyFish nearestGuppy = models.Where((model) => (model as GuppyFish)?.State == InteractableState.Alive)?
                .OrderBy(i => Vector2.Distance(i.BoundaryBox.Center.ToVector2(), BoundaryBox.Center.ToVector2())).FirstOrDefault() as GuppyFish;
            if (nearestGuppy != null)
            {
                float distance = Vector2.Distance(nearestGuppy.BoundaryBox.Center.ToVector2(), BoundaryBox.Center.ToVector2());
                if (distance < 30)
                {
                    _currentHunger = _maxHunger;
                    nearestGuppy.Eat();
                    return true;
                }

                Vector2 direction = Vector2.Normalize(nearestGuppy.BoundaryBox.Center.ToVector2() - BoundaryBox.Center.ToVector2());
                MoveTowards(direction);
                return true;
            }
            return false;
        }

        private readonly string _assetName = TextureNames.PiranhaAsset;
    }
}
