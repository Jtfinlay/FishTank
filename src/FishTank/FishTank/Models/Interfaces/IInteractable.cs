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
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FishTank.Models.Interfaces
{
    /// <summary>
    /// Interface used to represent all interactable objects in the game space.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Allows interactable to run logic that allows for movement, collision
        /// checking, and more.
        /// </summary>
        /// <param name="models"></param>
        void Update(List<IInteractable> models, GameTime gameTime);

        /// <summary>
        /// Called when the interactable should draw itself
        /// </summary>
        /// <param name="spriteBatch">Graphics resource for drawing</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Rectangle indicating the boundary box for the interactable
        /// </summary>
        Rectangle BoundaryBox { get; }

        /// <summary>
        /// State of the object indicating whether interactable with other objects
        /// </summary>
        InteractableState State { get; }
    }
}
