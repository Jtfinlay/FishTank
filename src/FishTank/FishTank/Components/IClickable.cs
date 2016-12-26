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

using FishTank.Utilities.Inputs;
using Microsoft.Xna.Framework;

namespace FishTank.Components
{
    /// <summary>
    /// Implementing objects indicate that they expect to receive click events
    /// </summary>
    public interface IClickable
    {
        /// <summary>
        /// The target area of the object. Used by parent to filter whether object was clicked.
        /// </summary>
        Rectangle Area { get; }

        /// <summary>
        /// Invoked when a mouse action was performed on the element
        /// </summary>
        /// <param name="mouseEvent">Contains details of the action performed</param>
        /// <returns>Boolean indicating whether this object has taken action from event</returns>
        bool MouseEvent(InputEvent mouseEvent);
    }
}
