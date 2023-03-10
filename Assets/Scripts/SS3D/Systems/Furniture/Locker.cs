using FishNet.Object.Synchronizing;
using SS3D.Core.Behaviours;
using SS3D.Interactions;
using SS3D.Interactions.Interfaces;
using SS3D.Systems.Inventory.Interactions;
using UnityEngine;

namespace SS3D.Systems.Furniture
{
    /// <summary>
    /// A temporary locker class for easily testing permission checking
    /// </summary>
    public class Locker : NetworkActor, IInteractionTarget
    {
        [SyncVar] public bool Locked;
        [SerializeField, SyncVar] private IDPermission permissionToUnlock;

        public IInteraction[] CreateTargetInteractions(InteractionEvent interactionEvent)
        {
            LockLockerInteraction lockLockerInteraction = 
                new LockLockerInteraction(this, permissionToUnlock);

            UnlockLockerInteraction unlockLockerInteraction = 
                new UnlockLockerInteraction(this, permissionToUnlock);

            return new IInteraction[] { lockLockerInteraction, unlockLockerInteraction };
        }
    }
}