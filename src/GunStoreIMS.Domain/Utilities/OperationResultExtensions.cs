using System;

namespace GunStoreIMS.Domain.Utilities
{
    /// <summary>
    /// Provides extension methods for chaining and handling operation results.
    /// </summary>
    public static class OperationResultExtensions
    {
        /// <summary>
        /// Chains an additional check onto an existing operation result.
        /// If the initial result fails, the next check is skipped.
        /// </summary>
        public static OperationResult Then(this OperationResult result, Func<OperationResult> next)
            => result.Succeeded ? next() : result;

        /// <summary>
        /// Chains a generic operation result.
        /// If the initial result fails, the next check is skipped.
        /// </summary>
        public static OperationResult<T> Then<T>(this OperationResult<T> result, Func<OperationResult<T>> next)
            => result.Succeeded ? next() : result;
    }
}
