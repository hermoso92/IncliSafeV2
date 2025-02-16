using Xunit;
using Moq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace IncliSafe.Tests.Services
{
    public class DobackAnalysisServiceTests
    {
        private readonly Mock<HttpClient> _httpClientMock;
        private readonly Mock<ILogger<DobackAnalysisService>> _loggerMock;
        private readonly DobackAnalysisService _service;

        public DobackAnalysisServiceTests()
        {
            _httpClientMock = new Mock<HttpClient>();
            _loggerMock = new Mock<ILogger<DobackAnalysisService>>();
            _service = new DobackAnalysisService(_httpClientMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AnalyzeFile_WithValidData_ReturnsAnalysis()
        {
            // Arrange
            var vehicleId = 1;
            var fileName = "test.txt";
            var fileContent = "Header\n0.1;0.2;0.3;0.4;0.5;0.6;0.7;0.8;0.9;1.0;1.1;1.2;1.3;1.4;1.5;0.85;1.7;1.8;1.9;0;50;30";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));

            // Act
            var result = await _service.AnalyzeFile(vehicleId, stream, fileName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(vehicleId, result.VehicleId);
            Assert.Equal(fileName, result.FileName);
            Assert.NotNull(result.Data);
            Assert.Equal(0.85M, result.Data.StabilityIndex);
        }

        [Fact]
        public async Task GetVehicleAnalyses_ReturnsAnalyses()
        {
            // Arrange
            var vehicleId = 1;
            var analyses = new List<DobackAnalysis>
            {
                new() { VehicleId = vehicleId, StabilityIndex = 0.8M },
                new() { VehicleId = vehicleId, StabilityIndex = 0.85M }
            };

            _httpClientMock.Setup(x => x.GetFromJsonAsync<List<DobackAnalysis>>($"api/doback/vehicle/{vehicleId}/analyses"))
                .ReturnsAsync(analyses);

            // Act
            var result = await _service.GetVehicleAnalyses(vehicleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, a => Assert.Equal(vehicleId, a.VehicleId));
        }

        [Fact]
        public async Task GetLatestAnalysis_ReturnsLatestAnalysis()
        {
            // Arrange
            var vehicleId = 1;
            var analysis = new AnalysisResult
            {
                StabilityScore = 0.85M,
                SafetyScore = 0.9M,
                MaintenanceScore = 0.8M
            };

            _httpClientMock.Setup(x => x.GetFromJsonAsync<AnalysisResult>($"api/doback/vehicle/{vehicleId}/latest"))
                .ReturnsAsync(analysis);

            // Act
            var result = await _service.GetLatestAnalysis(vehicleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0.85M, result.StabilityScore);
        }

        [Fact]
        public async Task GetTrends_ReturnsTrends()
        {
            // Arrange
            var vehicleId = 1;
            var trends = new Dictionary<string, double>
            {
                { "stability", 0.85 },
                { "safety", 0.9 }
            };

            _httpClientMock.Setup(x => x.GetFromJsonAsync<Dictionary<string, double>>($"api/doback/vehicle/{vehicleId}/trends"))
                .ReturnsAsync(trends);

            // Act
            var result = await _service.GetTrends(vehicleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(0.85, result["stability"]);
        }

        [Fact]
        public async Task AnalyzeFile_WithInvalidData_ThrowsException()
        {
            // Arrange
            var vehicleId = 1;
            var fileName = "test.txt";
            var fileContent = "Invalid Data";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));

            // Act & Assert
            await Assert.ThrowsAsync<FormatException>(() => 
                _service.AnalyzeFile(vehicleId, stream, fileName));
        }

        [Fact]
        public async Task GetVehicleAnalyses_WithNoData_ReturnsEmptyList()
        {
            // Arrange
            var vehicleId = 1;
            _httpClientMock.Setup(x => x.GetFromJsonAsync<List<DobackAnalysis>>($"api/doback/vehicle/{vehicleId}/analyses"))
                .ReturnsAsync(new List<DobackAnalysis>());

            // Act
            var result = await _service.GetVehicleAnalyses(vehicleId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(0.85, 50, 30, true)]  // Valores normales
        [InlineData(0.3, 80, 45, false)]  // Valores críticos
        [InlineData(0.95, 30, 15, true)]  // Valores óptimos
        public void ValidateDobackData_ReturnsExpectedResult(decimal stability, decimal speed, decimal steer, bool expectedResult)
        {
            // Arrange
            var data = new DobackData
            {
                StabilityIndex = stability,
                Speed = speed,
                Steer = steer
            };

            // Act
            var result = _service.ValidateDobackData(data);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ParseDobackFile_WithValidFormat_ReturnsData()
        {
            // Arrange
            var content = "Header\n0.1;0.2;0.3;0.4;0.5;0.6;0.7;0.8;0.9;1.0;1.1;1.2;1.3;1.4;1.5;0.85;1.7;1.8;1.9;0;50;30";

            // Act
            var result = _service.ParseDobackFile(content);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0.85M, result.StabilityIndex);
            Assert.Equal(50M, result.Speed);
            Assert.Equal(30M, result.Steer);
        }

        [Fact]
        public void ParseDobackFile_WithInvalidFormat_ThrowsException()
        {
            // Arrange
            var content = "Invalid Format";

            // Act & Assert
            Assert.Throws<FormatException>(() => _service.ParseDobackFile(content));
        }
    }
} 