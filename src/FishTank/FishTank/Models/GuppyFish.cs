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
using FishTank.Models.Interfaces;
using FishTank.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FishTank.Models
{
    public class GuppyFish : Fish
    {
        /// <summary>
        /// Basic fish that chases food and dies
        /// </summary>
        /// <param name="graphicsDevice">Graphics resource for texture creation</param>
        public GuppyFish(GraphicsDevice graphicsDevice, ContentManager content) : base()
        {
            _dropCoinTime = TimeSpan.FromSeconds(15);
            _maxSpeed = 4.0f;
            _maxHunger = 1.0f;
            _currentHunger = _maxHunger;

            _swimArea = new Rectangle(0, 0, Constants.VirtualWidth, Constants.VirtualHeight);
            BoundaryBox = new Rectangle(_swimArea.X + Constants.VirtualWidth / 2, 100, 75, 60);

            // Preload assets
            ContentBuilder.Instance.LoadTextureByName(_healthyAssetName);
            ContentBuilder.Instance.LoadTextureByName(_hungryAssetName);
            ContentBuilder.Instance.LoadTextureByName(_starvingAssetName);
            ContentBuilder.Instance.LoadTextureByName(_deadAssetName);
        }

        /// <summary>
        /// Draw the model on the canvas
        /// </summary>
        /// <param name="spriteBatch">Graphics resource for drawing</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            string assetName = null;
            switch (State)
            {
                case InteractableState.Discard:
                    // fish is destroyed but awaiting cleanup. Don't draw
                    return;
                case InteractableState.Dead:
                    assetName = _deadAssetName;
                    break;
                case InteractableState.Alive:
                    if (_currentHunger <= _hungerDangerValue) assetName = _starvingAssetName;
                    else if (_currentHunger <= _hungerWarningValue) assetName = _hungryAssetName;
                    else assetName = _healthyAssetName;
                    break;
            }

            if (!string.IsNullOrWhiteSpace(assetName))
            {
                spriteBatch.Draw(ContentBuilder.Instance.LoadTextureByName(assetName), BoundaryBox.Location.ToVector2(), null);
            }
        }

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
            if (_currentHunger > _hungerStartsValue)
            {
                return false;
            }

            Pellet nearestPellet = models.Where((model) => model is Pellet)?
                .OrderBy(i => Vector2.Distance(i.BoundaryBox.Center.ToVector2(), BoundaryBox.Center.ToVector2())).FirstOrDefault() as Pellet;
            if (nearestPellet != null)
            {
                float distance = Vector2.Distance(nearestPellet.BoundaryBox.Center.ToVector2(), BoundaryBox.Center.ToVector2());
                if (distance < 30)
                {
                    _currentHunger = _maxHunger;
                    nearestPellet.Eat();
                    return true;
                }

                Vector2 direction = Vector2.Normalize(nearestPellet.BoundaryBox.Center.ToVector2() - BoundaryBox.Center.ToVector2());
                Translate(direction, _maxSpeed);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Drawable texture showing the fish
        /// </summary>
        private readonly string _healthyAssetName = "Guppy.png";

        private readonly string _hungryAssetName = "Guppy_Hungry.png";

        private readonly string _starvingAssetName = "Guppy_Starving.png";

        private readonly string _deadAssetName = "Guppy_Dead.png";
    }
}
