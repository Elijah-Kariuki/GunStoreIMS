namespace GunStoreIMS.Shared.Enums
{
    // ATF Note: Ensure these types align with ATF Form 4473 and eForms options.
    public enum FirearmType
    {
        Pistol,
        Revolver,
        Rifle,
        Shotgun,
        Receiver,            // Frame or Receiver (ATF Final Rule 2021R-05F)
        Frame,               // Explicitly "Frame" if distinguished from "Receiver"
        Silencer,            // NFA Item
        ShortBarreledRifle,  // NFA Item (SBR)
        ShortBarreledShotgun,// NFA Item (SBS)
        MachineGun,          // NFA Item
        DestructiveDevice,   // NFA Item (DD)
        AnyOtherWeapon,      // NFA Item (AOW)
        Other                // If no other category fits, requires description
    }

}
