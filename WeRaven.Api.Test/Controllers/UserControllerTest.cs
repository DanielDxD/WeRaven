using FakeItEasy;
using FluentAssertions;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using WeRaven.Api.Controllers;
using WeRaven.Api.Models;
using WeRaven.Api.Models.Requests;
using WeRaven.Api.Repositories.Interfaces;
using WeRaven.Api.Services.Interfaces;

namespace WeRaven.Api.Test.Controllers
{
    public class UserControllerTest
    {
        private readonly User _mockUser = new()
        {
            Id = Guid.Parse("f49ef6d9-c430-48a0-85b6-5875927fc135"),
            Birthdate = DateTime.Parse("2002-02-02", CultureInfo.InvariantCulture).ToUniversalTime(),
            Email = "test@test.com",
            Name = "name",
            Password = Argon2.Hash("123"),
            Username = "username"
        };
        private readonly Auth _mockAuth = new()
        {
            UserId = Guid.Parse("f49ef6d9-c430-48a0-85b6-5875927fc135"),
            Code = 454545
        };

        [Fact]
        public async Task Create_ShouldCreateAnUser()
        {
            // Arrange
            var body = new CreateUserRequest
            {
                Birthdate = "2002-02-02",
                Email = "test@test.com",
                Name = "name",
                Password = "password",
                Username = "username",
            };

            var emailService = A.Fake<IEmailService>();
            var userRepository = A.Fake<IUserRepository>();

            var controller = new UserController(userRepository);

            // Act
            var result = await controller.Create(body, emailService);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(CreatedResult));
        }
        [Fact]
        public async Task Create_ShouldNotCreateAnUser()
        {
            // Arrange
            var body = new CreateUserRequest
            {
                Birthdate = "2002-02-02",
                Email = "test@test.com",
                Name = "name",
                Password = "password",
                Username = "username",
            };

            var emailService = A.Fake<IEmailService>();
            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.ExistEmail(body.Email)).Returns(true);

            var controller = new UserController(userRepository);

            // Act
            var result = await controller.Create(body, emailService);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }
        [Fact]
        public async Task Resend_ShouldResendEmail()
        {
            // Arrange
            var emailService = A.Fake<IEmailService>();
            var userRepository = A.Fake<IUserRepository>();

            var controller = new UserController(userRepository);

            // Act
            var result = await controller.Resend("test@test.com", emailService);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkResult));
        }
        [Fact]
        public async Task Verify_ShouldVerifyAnUser()
        {
            // Arrange
            var body = new VerifyRequest
            {
                Email = "test@test.com",
                Code = 454545
            };

            var tokenService = A.Fake<ITokenService>();
            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.GetUserAsync(_mockUser.Email, false)).Returns(_mockUser);
            A.CallTo(() => userRepository.GetAuthAsync(_mockUser.Id)).Returns(_mockAuth);


            var controller = new UserController(userRepository);

            // Act
            var result = await controller.Verify(body, tokenService);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }
        [Fact]
        public async Task Verify_ShouldNotVerifyAnUser()
        {
            // Arrange
            var body = new VerifyRequest
            {
                Email = "test@test.com",
                Code = 325236
            };

            var tokenService = A.Fake<ITokenService>();
            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.GetAuthAsync(_mockUser.Id)).Returns(_mockAuth);


            var controller = new UserController(userRepository);

            // Act
            var result = await controller.Verify(body, tokenService);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(UnauthorizedObjectResult));
        }

        [Fact]
        public async Task Login_ShouldSignInUser()
        {
            // Arrange
            var body = new LoginRequest
            {
                EmailOrUsername = "test@test.com",
                Password = "123"
            };

            var tokenService = A.Fake<ITokenService>();
            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.GetUserAsync(_mockUser.Email, true)).Returns(_mockUser);

            var controller = new UserController(userRepository);

            // Act
            var result = await controller.Login(body, tokenService);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async Task Login_ShouldNotSignInUser()
        {
            // Arrange
            var body = new LoginRequest
            {
                EmailOrUsername = "test@test.com",
                Password = "1asd23"
            };

            var tokenService = A.Fake<ITokenService>();
            var userRepository = A.Fake<IUserRepository>();
            A.CallTo(() => userRepository.GetUserAsync(_mockUser.Email, true)).Returns(_mockUser);

            var controller = new UserController(userRepository);

            // Act
            var result = await controller.Login(body, tokenService);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async Task Get_ShouldGetAnUser()
        {
            // Arrange
            var userRepository = A.Fake<IUserRepository>();

            var controller = new UserController(userRepository);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "email@email.com")
            }, "mock"));
            controller.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                User = user
            };


            // Act
            var result = await controller.Get();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}
