using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace GunStoreIMS.Domain.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequireFflOrAddressAttribute : ValidationAttribute
    {
        private readonly string _fflProp;
        private readonly string[] _addressProps;

        public RequireFflOrAddressAttribute(string fflPropertyName, params string[] addressPropertyNames)
        {
            _fflProp = fflPropertyName;
            _addressProps = addressPropertyNames;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            var type = value!.GetType();
            var fflVal = (string?)type.GetProperty(_fflProp)?.GetValue(value);
            bool hasFfl = !string.IsNullOrWhiteSpace(fflVal);

            bool hasAddress = _addressProps
                .Select(p => type.GetProperty(p)?.GetValue(value))
                .All(v =>
                {
                    if (v is string s) return !string.IsNullOrWhiteSpace(s);
                    if (v is Enum) return v != null;
                    return false;
                });

            if (!hasFfl && !hasAddress)
            {
                return new ValidationResult(
                    ErrorMessage ?? $"Either {_fflProp} or full address must be provided.",
                    new[] { _fflProp }
                );
            }

            return ValidationResult.Success;
        }
    }
}
