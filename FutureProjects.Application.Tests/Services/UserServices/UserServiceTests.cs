using AutoMapper;
using FutureProjects.API.Controllers;
using FutureProjects.Application.Abstractions.IServices;
using FutureProjects.Application.Mappers;
using FutureProjects.Domain.Entities.DTOs;
using FutureProjects.Domain.Entities.Models;
using Moq;

namespace FutureProjects.Application.Tests.Services.UserServices
{
    public class UserServiceTests
    {
        private readonly Mock<IUserService> _userservice = new Mock<IUserService>();

        public static IEnumerable<object[]> GetUserFromDataGenerator()
        {
            yield return new object[]
            {
                new UserDTO()
                {
                    Name = "Test Product 1",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                },
                new User()
                {
                    Name = "Test Product 34",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                }
            };

            yield return new object[]
            {
                new UserDTO()
                {
                    Name = "Test Product 2",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                },
                new User()
                {
                    Name = "Test Product 2",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                }
            };

            yield return new object[]
            {
                new UserDTO()
                {
                    Name = "Test Product 3",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                },
                new User()
                {
                    Name = "Test Product 311",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                }
            };

            yield return new object[]
            {
                new UserDTO()
                {
                    Name = "Test Product 4",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                },
                new User()
                {
                    Name = "Test Product 4",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                }
            };

            yield return new object[]
            {
                new UserDTO()
                {
                    Name = "Test Product 5",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                },
                new User()
                {
                    Name = "Test Product 5",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                }
            };

            yield return new object[]
            {
                new UserDTO()
                {
                    Name = "Test Product 6",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                },
                new User()
                {
                    Name = "Test Product 6",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                }
            };

            yield return new object[]
            {
                new UserDTO()
                {
                    Name = "Test Product 7",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                },
                new User()
                {
                    Name = "Test Product 7",
                    Email = "komilov@gmail.com",
                    Password = "123",
                    Login = "tes123",
                    Role = "Admin"
                }
            };
        }


        [Theory]
        [MemberData(nameof(GetUserFromDataGenerator), MemberType = typeof(UserServiceTests))]
        public async void Create_User_Test(UserDTO inputUser, User expextedUser)
        {

            var mockMapepr  = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new AutoMapperProfile());
            });

            var myMapper = mockMapepr.CreateMapper(); 

            var result = myMapper.Map<User>(inputUser);
            // logic
            _userservice.Setup(x => x.Create(It.IsAny<UserDTO>()))
            .ReturnsAsync(result);

            var controller = new UsersController(_userservice.Object);

            
            // Act
            var createdUser = await controller.CreateUser(inputUser);

            // Assert
            Assert.NotNull(createdUser); // Verify a user object is returned

            Assert.True(CompareModels(expextedUser, createdUser));
        }

        public static bool CompareModels(User inputUser, User user)
        {
            if (inputUser.Name == user.Name && inputUser.Email == user.Email && inputUser.Password == user.Password
                && inputUser.Login == user.Login && inputUser.Role == user.Role)
            {
                return true;
            }

            return false;
        }


    }

}
