using IncliSafe.Shared.Models.Enums;
using System;
using System.Collections.Generic;

namespace IncliSafe.Shared.Models.DTOs
{
    public class VehicleDTO : BaseDTO
    {
        public override int Id { get; set; }
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string VIN { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public VehicleStatus Status { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime NextMaintenanceDate { get; set; }
        public int Mileage { get; set; }
        public FuelType FuelType { get; set; }
        public decimal FuelEfficiency { get; set; }
        public string InsuranceProvider { get; set; }
        public string InsuranceNumber { get; set; }
        public DateTime InsuranceExpiryDate { get; set; }
        public List<LicenseDTO> Licenses { get; set; }
        public List<SensorReadingDTO> SensorReadings { get; set; }
        public List<MaintenanceRecordDTO> MaintenanceRecords { get; set; }
        public List<InspectionDTO> Inspections { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime? UpdatedAt { get; set; }
    }

    public class LicenseDTO : BaseDTO
    {
        public override int Id { get; set; }
        public int VehicleId { get; set; }
        public LicenseType Type { get; set; }
        public string Number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public LicenseStatus Status { get; set; }
        public string IssuingAuthority { get; set; }
        public string Restrictions { get; set; }
        public override DateTime CreatedAt { get; set; }
        public override DateTime? UpdatedAt { get; set; }
    }

    public class SensorReadingDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public SensorType SensorType { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; }
        public DateTime Timestamp { get; set; }
        public SensorStatus Status { get; set; }
        public string Location { get; set; }
        public decimal BatteryLevel { get; set; }
        public SignalStrength SignalStrength { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class MaintenanceRecordDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public MaintenanceType ServiceType { get; set; }
        public DateTime ServiceDate { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public string Technician { get; set; }
        public string ServiceProvider { get; set; }
        public MaintenanceStatus Status { get; set; }
        public List<string> PartsReplaced { get; set; }
        public List<string> ServicesPerformed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class InspectionDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime InspectionDate { get; set; }
        public string Inspector { get; set; }
        public InspectionStatus Status { get; set; }
        public string Notes { get; set; }
        public List<string> Defects { get; set; }
        public List<string> Recommendations { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
} 