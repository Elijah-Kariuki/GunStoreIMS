// Domain/ValidationAttributes/RequireFflOrAddressAttribute.cs

using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GunStoreIMS.Shared.Validation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequireFflOrAddressAttribute : ValidationAttribute
    {
        private readonly string _fflProp;
        private readonly string _addrProp;
        private readonly string? _cityProp;
        private readonly string? _stateProp;
        private readonly string? _zipProp;

        // ─── 2-ARG OVERLOAD: FFL or a single “full address” string ──────────────────
        public RequireFflOrAddressAttribute(
            string fflProperty,
            string fullAddressProperty)
        {
            _fflProp = fflProperty;
            _addrProp = fullAddressProperty;
            _cityProp = null;
            _stateProp = null;
            _zipProp = null;
        }

        // ─── 5-ARG OVERLOAD: FFL or broken-out address fields ────────────────────────
        public RequireFflOrAddressAttribute(
            string fflProperty,
            string addressLine1Property,
            string cityProperty,
            string stateProperty,
            string zipProperty)
        {
            _fflProp = fflProperty;
            _addrProp = addressLine1Property;
            _cityProp = cityProperty;
            _stateProp = stateProperty;
            _zipProp = zipProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            var type = context.ObjectType;
            var instance = context.ObjectInstance;

            // grab the two mandatory props
            var fflVal = type.GetProperty(_fflProp)?.GetValue(instance) as string;
            var addrVal = type.GetProperty(_addrProp)?.GetValue(instance) as string;

            bool hasFfl = !string.IsNullOrWhiteSpace(fflVal);
            bool hasAddress = false;

            if (_cityProp == null)
            {
                // 2-arg mode: full-address string
                hasAddress = !string.IsNullOrWhiteSpace(addrVal);
            }
            else
            {
                // 5-arg mode: broken-out address
                var cityVal = type.GetProperty(_cityProp!)?.GetValue(instance) as string;
                var stateVal = type.GetProperty(_stateProp!)?.GetValue(instance);
                var zipVal = type.GetProperty(_zipProp!)?.GetValue(instance) as string;

                hasAddress =
                    !string.IsNullOrWhiteSpace(addrVal) &&
                    !string.IsNullOrWhiteSpace(cityVal) &&
                     stateVal != null &&
                    !string.IsNullOrWhiteSpace(zipVal);
            }

            if (hasFfl || hasAddress)
                return ValidationResult.Success;

            // neither provided → error on all relevant members
            string[] members = _cityProp == null
                ? new[] { _fflProp, _addrProp }
                : new[] { _fflProp, _addrProp, _cityProp!, _stateProp!, _zipProp! };

            return new ValidationResult(
                ErrorMessage ?? "Either an FFL number or a full address must be provided.",
                members
            );
        }
    }
}
