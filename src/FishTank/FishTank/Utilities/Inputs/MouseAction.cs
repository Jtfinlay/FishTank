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

namespace FishTank.Utilities.Inputs
{
    /// <summary>
    /// Used by <see cref="InputEvent"/> to identiy mouse action taken on target element.
    /// </summary>
    public enum InputAction
    {
        /// <summary>
        /// Left button was pressed
        /// </summary>
        Click,

        /// <summary>
        /// Left button was released
        /// </summary>
        Release,

        /// <summary>
        /// Mouse is hovering over target element.
        /// </summary>
        Hover,

        /// <summary>
        /// Mouse has started hovering over target element
        /// </summary>
        HoverStart,

        /// <summary>
        /// Mouse was previously hovering over target element and was just moved away.
        /// </summary>
        HoverExit,

        /// <summary>
        /// Tap gesture for touch screen
        /// </summary>
        TouchTap,
    }
}
