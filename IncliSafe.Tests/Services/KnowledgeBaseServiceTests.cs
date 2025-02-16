using Xunit;
using Moq;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using IncliSafe.Client.Services;
using IncliSafe.Shared.Models;

namespace IncliSafe.Tests.Services
{
    public class KnowledgeBaseServiceTests
    {
        private readonly Mock<HttpClient> _httpClientMock;
        private readonly KnowledgeBaseService _service;

        public KnowledgeBaseServiceTests()
        {
            _httpClientMock = new Mock<HttpClient>();
            _service = new KnowledgeBaseService(_httpClientMock.Object);
        }

        [Fact]
        public async Task GetPatterns_ReturnsPatternList()
        {
            // Arrange
            var expectedPatterns = new List<KnowledgePattern>
            {
                new() { 
                    Id = 1, 
                    PatternName = "Patrón 1",
                    PatternType = "Stability",
                    Confidence = 0.85M
                },
                new() { 
                    Id = 2, 
                    PatternName = "Patrón 2",
                    PatternType = "Safety",
                    Confidence = 0.9M
                }
            };

            _httpClientMock.Setup(x => x.GetFromJsonAsync<List<KnowledgePattern>>($"{BaseUrl}/patterns"))
                .ReturnsAsync(expectedPatterns);

            // Act
            var result = await _service.GetPatterns();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Patrón 1", result[0].PatternName);
            Assert.Equal("Safety", result[1].PatternType);
        }

        [Fact]
        public async Task GetPattern_WithValidId_ReturnsPattern()
        {
            // Arrange
            var patternId = 1;
            var expectedPattern = new KnowledgePattern
            {
                Id = patternId,
                PatternName = "Test Pattern",
                PatternType = "Stability",
                Confidence = 0.85M
            };

            _httpClientMock.Setup(x => x.GetFromJsonAsync<KnowledgePattern>($"{BaseUrl}/patterns/{patternId}"))
                .ReturnsAsync(expectedPattern);

            // Act
            var result = await _service.GetPattern(patternId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patternId, result.Id);
            Assert.Equal("Test Pattern", result.PatternName);
        }

        [Fact]
        public async Task CreatePattern_WithValidData_ReturnsCreatedPattern()
        {
            // Arrange
            var pattern = new KnowledgePattern
            {
                PatternName = "New Pattern",
                PatternType = "Safety",
                Confidence = 0.9M
            };

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Created);
            response.Content = JsonContent.Create(pattern);

            _httpClientMock.Setup(x => x.PostAsJsonAsync($"{BaseUrl}/patterns", pattern))
                .ReturnsAsync(response);

            // Act
            var result = await _service.CreatePattern(pattern);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Pattern", result.PatternName);
        }

        [Fact]
        public async Task UpdatePattern_WithValidData_ReturnsUpdatedPattern()
        {
            // Arrange
            var pattern = new KnowledgePattern
            {
                Id = 1,
                PatternName = "Updated Pattern",
                PatternType = "Safety",
                Confidence = 0.95M
            };

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = JsonContent.Create(pattern);

            _httpClientMock.Setup(x => x.PutAsJsonAsync($"{BaseUrl}/patterns/{pattern.Id}", pattern))
                .ReturnsAsync(response);

            // Act
            var result = await _service.UpdatePattern(pattern);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Pattern", result.PatternName);
            Assert.Equal(0.95M, result.Confidence);
        }

        [Fact]
        public async Task DeletePattern_WithValidId_Succeeds()
        {
            // Arrange
            var patternId = 1;
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);

            _httpClientMock.Setup(x => x.DeleteAsync($"{BaseUrl}/patterns/{patternId}"))
                .ReturnsAsync(response);

            // Act & Assert
            await _service.DeletePattern(patternId); // No debería lanzar excepción
        }

        [Fact]
        public async Task GetStats_ReturnsKnowledgeStats()
        {
            // Arrange
            var expectedStats = new KnowledgeStats
            {
                ActivePatterns = 10,
                TotalDetections = 100,
                AverageConfidence = 0.85M
            };

            _httpClientMock.Setup(x => x.GetFromJsonAsync<KnowledgeStats>($"{BaseUrl}/stats"))
                .ReturnsAsync(expectedStats);

            // Act
            var result = await _service.GetStats();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.ActivePatterns);
            Assert.Equal(100, result.TotalDetections);
            Assert.Equal(0.85M, result.AverageConfidence);
        }

        [Fact]
        public async Task GetPatternDistribution_ReturnsDistributionData()
        {
            // Arrange
            var expectedDistribution = new Dictionary<string, double>
            {
                { "Stability", 0.4 },
                { "Safety", 0.3 },
                { "Performance", 0.3 }
            };

            _httpClientMock.Setup(x => x.GetFromJsonAsync<Dictionary<string, double>>($"{BaseUrl}/distribution"))
                .ReturnsAsync(expectedDistribution);

            // Act
            var result = await _service.GetPatternDistribution();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(0.4, result["Stability"]);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task CreatePattern_WithInvalidName_ThrowsException(string invalidName)
        {
            // Arrange
            var pattern = new KnowledgePattern { PatternName = invalidName };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.CreatePattern(pattern));
        }

        [Fact]
        public async Task GetPattern_WithInvalidId_ThrowsException()
        {
            // Arrange
            var invalidId = -1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.GetPattern(invalidId));
        }
    }
} 