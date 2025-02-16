using Xunit;
using Moq;
using IncliSafe.Client.Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IncliSafe.Tests.Services
{
    public class PredictiveAnalysisServiceTests
    {
        private readonly Mock<IDobackAnalysisService> _dobackServiceMock;
        private readonly Mock<ILogger<PredictiveAnalysisService>> _loggerMock;
        private readonly PredictiveAnalysisService _service;

        public PredictiveAnalysisServiceTests()
        {
            _dobackServiceMock = new Mock<IDobackAnalysisService>();
            _loggerMock = new Mock<ILogger<PredictiveAnalysisService>>();
            _service = new PredictiveAnalysisService(_dobackServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task PredictStability_WithValidData_ReturnsPrediction()
        {
            // Arrange
            var vehicleId = 1;
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var historicalData = new List<DobackAnalysis>
            {
                new() { StabilityIndex = 0.8M },
                new() { StabilityIndex = 0.85M },
                new() { StabilityIndex = 0.82M }
            };

            _dobackServiceMock.Setup(x => x.GetHistoricalData(vehicleId, startDate, endDate))
                .ReturnsAsync(historicalData);

            // Act
            var result = await _service.PredictStability(vehicleId, startDate, endDate);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Predictions);
            Assert.NotEmpty(result.UpperBound);
            Assert.NotEmpty(result.LowerBound);
            Assert.True(result.Confidence > 0);
        }

        [Fact]
        public async Task DetectAnomalies_WithValidData_ReturnsAnomalies()
        {
            // Arrange
            var vehicleId = 1;
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var data = new List<DobackAnalysis>
            {
                new() { StabilityIndex = 0.8M },
                new() { StabilityIndex = 0.3M }, // Anomalía
                new() { StabilityIndex = 0.85M }
            };

            _dobackServiceMock.Setup(x => x.GetHistoricalData(vehicleId, startDate, endDate))
                .ReturnsAsync(data);

            // Act
            var anomalies = await _service.DetectAnomalies(vehicleId, startDate, endDate);

            // Assert
            Assert.NotNull(anomalies);
            Assert.Single(anomalies);
            Assert.Equal(AnomalyType.Low, anomalies[0].Type);
        }

        [Fact]
        public async Task AnalyzeTrends_WithValidData_ReturnsTrendAnalysis()
        {
            // Arrange
            var vehicleId = 1;
            var startDate = DateTime.Now.AddDays(-30);
            var endDate = DateTime.Now;
            var data = new List<DobackAnalysis>
            {
                new() { StabilityIndex = 0.7M },
                new() { StabilityIndex = 0.75M },
                new() { StabilityIndex = 0.8M }
            };

            _dobackServiceMock.Setup(x => x.GetHistoricalData(vehicleId, startDate, endDate))
                .ReturnsAsync(data);

            // Act
            var trends = await _service.AnalyzeTrends(vehicleId, startDate, endDate);

            // Assert
            Assert.NotNull(trends);
            Assert.NotNull(trends.ShortTerm);
            Assert.NotNull(trends.MediumTerm);
            Assert.NotNull(trends.LongTerm);
            Assert.True(trends.ShortTerm.Slope > 0);
        }

        [Fact]
        public async Task DetectPatterns_WithValidData_ReturnsPatterns()
        {
            // Arrange
            var vehicleId = 1;
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;
            var data = new List<DobackAnalysis>
            {
                new() { StabilityIndex = 0.8M },
                new() { StabilityIndex = 0.7M },
                new() { StabilityIndex = 0.8M },
                new() { StabilityIndex = 0.7M },
                new() { StabilityIndex = 0.8M }
            };

            _dobackServiceMock.Setup(x => x.GetHistoricalData(vehicleId, startDate, endDate))
                .ReturnsAsync(data);

            // Act
            var patterns = await _service.DetectPatterns(vehicleId, startDate, endDate);

            // Assert
            Assert.NotNull(patterns);
            Assert.NotEmpty(patterns);
            Assert.Equal(PatternType.Cyclic, patterns[0].Type);
        }

        [Fact]
        public async Task PredictStability_WithNoData_ThrowsException()
        {
            // Arrange
            var vehicleId = 1;
            var startDate = DateTime.Now.AddDays(-7);
            var endDate = DateTime.Now;

            _dobackServiceMock.Setup(x => x.GetHistoricalData(vehicleId, startDate, endDate))
                .ReturnsAsync(new List<DobackAnalysis>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _service.PredictStability(vehicleId, startDate, endDate));
        }

        [Theory]
        [InlineData(0.3, "Crítico")]
        [InlineData(0.5, "Regular")]
        [InlineData(0.7, "Bueno")]
        [InlineData(0.9, "Excelente")]
        public void AnalyzeStability_ReturnsCorrectStatus(decimal stabilityIndex, string expectedStatus)
        {
            // Arrange
            var metrics = new DashboardMetrics { StabilityIndex = stabilityIndex };

            // Act
            var result = _service.AnalyzeStability(metrics);

            // Assert
            Assert.Equal(expectedStatus, result.Status);
        }
    }
} 