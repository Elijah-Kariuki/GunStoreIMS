using System;
using System.Linq;
using GunStoreIMS.Shared.Enums;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Domain.Utilities;

namespace GunStoreIMS.Domain.BusinessRules
{
    public class ATFComplianceRules
    {
        // helper to emit Success/ail in one line
        private OperationResult Check(bool predicate, string errorMessage)
            => predicate
               ? OperationResult.Success()
               : OperationResult.Fail(errorMessage);

        public OperationResult ValidateManufactureDate(DateTime manufactureDate)
            => Check(
                manufactureDate <= DateTime.UtcNow,
                "Manufacture date cannot be in the future.");

        public OperationResult ValidateAcquisitionDate(DateTime acquisitionDate)
            => Check(
                acquisitionDate <= DateTime.UtcNow,
                "Acquisition date cannot be in the future.");

        public OperationResult ValidateDispositionDate(DateTime? dispositionDate, DateTime acquisitionDate)
            => !dispositionDate.HasValue
               ? OperationResult.Success()
               : Check(
                   dispositionDate.Value <= DateTime.UtcNow,
                   "Disposition date cannot be in the future.")
                 .Then(() => Check(
                   dispositionDate.Value >= acquisitionDate,
                   "Disposition date cannot be before the acquisition date."));

        public OperationResult ValidateImporterDetails(
            bool isImported,
            string? importerName,
            string? importerCity,
            USState? importerState,
            string countryOfOrigin)
            => !isImported
               ? OperationResult.Success()
               : Check(
                   !string.IsNullOrWhiteSpace(importerName)
                   && !string.IsNullOrWhiteSpace(importerCity)
                   && importerState.HasValue,
                   "Full importer details (Name, City, State) are required for imported firearms.")
                 .Then(() => Check(
                   !countryOfOrigin.Equals("USA", StringComparison.OrdinalIgnoreCase)
                   && !countryOfOrigin.Equals("United States", StringComparison.OrdinalIgnoreCase),
                   "Imported firearms cannot have 'USA' or 'United States' as Country of Origin."));

        public OperationResult ValidateNonImportedCountry(
            bool isImported,
            string countryOfOrigin)
            => isImported
               ? OperationResult.Success()
               : Check(
                   countryOfOrigin.Equals("USA", StringComparison.OrdinalIgnoreCase)
                   || countryOfOrigin.Equals("United States", StringComparison.OrdinalIgnoreCase),
                   "Non‐imported firearms should have 'USA' or 'United States' as Country of Origin.");

        public OperationResult ValidateSerialNumber(string serialNumber)
            => Check(
                !string.IsNullOrWhiteSpace(serialNumber),
                "Serial number is required and must be at least 1 character long.");

        public OperationResult ValidateOtherType(
            FirearmEnumType firearmType,
            string? otherTypeDescription)
            => (firearmType == FirearmEnumType.Other
                || firearmType == FirearmEnumType.Frame
                || firearmType == FirearmEnumType.Receiver)
               ? Check(
                   !string.IsNullOrWhiteSpace(otherTypeDescription),
                   "A description is required when firearm type is 'Other', 'Frame', or 'Receiver'.")
               : OperationResult.Success();

        public OperationResult ValidateNfaMeasurements(
            bool isNFAItem,
            FirearmEnumType firearmType,
            decimal? barrelLength,
            decimal? overallLength)
            => !isNFAItem
               ? OperationResult.Success()
               : Check(
                   barrelLength.HasValue && barrelLength > 0,
                   "Barrel length is required for NFA firearms.")
                 .Then(() => Check(
                   overallLength.HasValue && overallLength > 0,
                   "Overall length is required for NFA firearms."))
                 .Then(() => firearmType == FirearmEnumType.ShortBarreledRifle
                             && barrelLength >= 16m
                             ? OperationResult.Fail(
                                 "A Short-Barreled Rifle must have a barrel length less than 16 inches.")
                             : OperationResult.Success());

        public OperationResult ValidatePrivatelyMade(
            bool isPrivatelyMadeFirearm,
            FirearmStatus currentStatus,
            string? yourFflMarking)
            => isPrivatelyMadeFirearm && currentStatus == FirearmStatus.InInventory
               ? Check(
                   !string.IsNullOrWhiteSpace(yourFflMarking),
                   "FFL marking is required for Privately Made Firearms held in inventory.")
               : OperationResult.Success();

        public OperationResult ValidateAll(Firearm f)
        {
            var checks = new Func<OperationResult>[]
            {
                () => ValidateManufactureDate(f.ManufactureDate),
                () => ValidateAcquisitionDate(f.InitialAcquisitionDate),
                () => ValidateDispositionDate(f.DateOfDisposition, f.InitialAcquisitionDate),
                () => ValidateImporterDetails(f.IsImported, f.ImporterName, f.ImporterCity, f.ImporterState, f.CountryOfOrigin),
                () => ValidateNonImportedCountry(f.IsImported, f.CountryOfOrigin),
                () => ValidateSerialNumber(f.SerialNumber),
                () => ValidateOtherType(f.FirearmType, f.OtherTypeDescription),
                () => ValidateNfaMeasurements(f.IsNFAItem, f.FirearmType, f.BarrelLength, f.OverallLength),
                () => ValidatePrivatelyMade(f.IsPrivatelyMadeFirearm, f.CurrentStatus, f.YourFFLMarking)
            };

            // fail fast on first error
            return checks
                   .Select(c => c())
                   .FirstOrDefault(r => !r.Succeeded)
                   ?? OperationResult.Success();
        }
    }
}
