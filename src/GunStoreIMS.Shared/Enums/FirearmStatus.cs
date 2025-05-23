using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GunStoreIMS.Shared.Enums
{
    // ATF Note: These statuses help manage inventory and A&D record lifecycle.
    public enum FirearmStatus
    {
        InInventory,         // Actively in stock
        Disposed,            // Sold, transferred, etc.
        PendingDisposition,  // Sale agreed, awaiting completion (e.g., NICS delay)
        Lost,                // Firearm is lost
        Stolen,              // Firearm is stolen
        Destroyed,           // Firearm has been destroyed
        InRepair,      // Out for repair/modification
        ReturnedFromGunsmith,// Returned from repair/modification
        Reserved,            // Held for a specific customer/purpose
        TransferredToMuseum, // Specific disposition type
        Archived             // Record archived post-retention period (for soft delete/archive strategy)
    }
}
