//
// Copyright - James Finlay
// 

namespace FishTank.Models.Interfaces
{
    public enum InteractableState
    {
        /// <summary>
        /// Default state indicating the object is alive an interacting
        /// </summary>
        Alive = 0,

        Dead,

        /// <summary>
        /// Discard state indicates this object should be removed on next update cycle
        /// </summary>
        Discard,
    }
}
