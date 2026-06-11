using server.DTOs;
using server.Models;
using System.Runtime.CompilerServices;

namespace server.Mappers
{
    public static class UserMapper
    {
        public static UserDto toUserDto(this User u)
        {
            return new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                RoleName=u.UserRole.Name
            };
        }
    }
}
