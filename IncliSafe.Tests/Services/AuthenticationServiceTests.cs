using Xunit;
using Moq;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using IncliSafe.Client.Services;
using IncliSafe.Shared.Models;
using IncliSafe.Client.Auth;

namespace IncliSafe.Tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<HttpClient> _httpClientMock;
        private readonly Mock<AuthenticationStateProvider> _authStateProviderMock;
        private readonly Mock<ILocalStorageService> _localStorageMock;
        private readonly AuthenticationService _service;
        private const string BaseUrl = "api/auth";

        public AuthenticationServiceTests()
        {
            _httpClientMock = new Mock<HttpClient>();
            _authStateProviderMock = new Mock<AuthenticationStateProvider>();
            _localStorageMock = new Mock<ILocalStorageService>();
            _service = new AuthenticationService(
                _httpClientMock.Object,
                _authStateProviderMock.Object,
                _localStorageMock.Object);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsAuthResponse()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var expectedResponse = new AuthResponse
            {
                Token = "jwt_token",
                RefreshToken = "refresh_token",
                UserId = 1,
                Email = loginRequest.Email,
                Role = "Usuario"
            };

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = JsonContent.Create(expectedResponse);

            _httpClientMock.Setup(x => x.PostAsJsonAsync($"{BaseUrl}/login", loginRequest))
                .ReturnsAsync(response);

            // Act
            var result = await _service.Login(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Token, result.Token);
            Assert.Equal(expectedResponse.Email, result.Email);

            // Verificar que se guardó el token
            _localStorageMock.Verify(x => x.SetItemAsync("authToken", expectedResponse.Token), Times.Once);
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsAuthResponse()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "new@example.com",
                Password = "password123",
                Nombre = "Test User"
            };

            var expectedResponse = new AuthResponse
            {
                Token = "jwt_token",
                UserId = 1,
                Email = registerRequest.Email,
                Role = "Usuario"
            };

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Created);
            response.Content = JsonContent.Create(expectedResponse);

            _httpClientMock.Setup(x => x.PostAsJsonAsync($"{BaseUrl}/register", registerRequest))
                .ReturnsAsync(response);

            // Act
            var result = await _service.Register(registerRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Email, result.Email);
        }

        [Fact]
        public async Task Logout_ClearsAuthenticationState()
        {
            // Arrange
            _localStorageMock.Setup(x => x.RemoveItemAsync("authToken")).Returns(Task.CompletedTask);

            // Act
            await _service.Logout();

            // Assert
            _localStorageMock.Verify(x => x.RemoveItemAsync("authToken"), Times.Once);
            _localStorageMock.Verify(x => x.RemoveItemAsync("refreshToken"), Times.Once);
        }

        [Fact]
        public async Task RefreshToken_WithValidToken_ReturnsNewAuthResponse()
        {
            // Arrange
            var refreshRequest = new RefreshTokenRequest
            {
                Token = "old_token",
                RefreshToken = "refresh_token"
            };

            var expectedResponse = new AuthResponse
            {
                Token = "new_token",
                RefreshToken = "new_refresh_token",
                UserId = 1,
                Email = "test@example.com"
            };

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = JsonContent.Create(expectedResponse);

            _httpClientMock.Setup(x => x.PostAsJsonAsync($"{BaseUrl}/refresh", refreshRequest))
                .ReturnsAsync(response);

            // Act
            var result = await _service.RefreshToken(refreshRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Token, result.Token);
        }

        [Theory]
        [InlineData("", "password123")]
        [InlineData("invalid_email", "password123")]
        [InlineData("test@example.com", "")]
        public async Task Login_WithInvalidCredentials_ThrowsException(string email, string password)
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.Login(loginRequest));
        }

        [Fact]
        public async Task GetCurrentUser_ReturnsUserInfo()
        {
            // Arrange
            var expectedUser = new UserInfo
            {
                Id = 1,
                Email = "test@example.com",
                Role = "Usuario"
            };

            _httpClientMock.Setup(x => x.GetFromJsonAsync<UserInfo>($"{BaseUrl}/user"))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _service.GetCurrentUser();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Email, result.Email);
        }

        [Fact]
        public async Task ChangePassword_WithValidData_Succeeds()
        {
            // Arrange
            var request = new ChangePasswordRequest
            {
                OldPassword = "old_password",
                NewPassword = "new_password"
            };

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            _httpClientMock.Setup(x => x.PostAsJsonAsync($"{BaseUrl}/change-password", request))
                .ReturnsAsync(response);

            // Act & Assert
            await _service.ChangePassword(request); // No debería lanzar excepción
        }

        [Fact]
        public async Task IsAuthenticated_WithValidToken_ReturnsTrue()
        {
            // Arrange
            _localStorageMock.Setup(x => x.GetItemAsync<string>("authToken"))
                .ReturnsAsync("valid_token");

            // Act
            var result = await _service.IsAuthenticated();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsAuthenticated_WithNoToken_ReturnsFalse()
        {
            // Arrange
            _localStorageMock.Setup(x => x.GetItemAsync<string>("authToken"))
                .ReturnsAsync((string)null);

            // Act
            var result = await _service.IsAuthenticated();

            // Assert
            Assert.False(result);
        }
    }
} 