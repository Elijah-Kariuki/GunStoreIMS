namespace GunStoreIMS.Shared.Enums
{
    public enum RecoveryEventType
    {
        Lost,
        Stolen,
        Seized,        // e.g., evidence or ATF seizure
        Recovered,     // firearm found / returned
        Destroyed      // voluntarily destroyed or rendered unserviceable
    }
}
