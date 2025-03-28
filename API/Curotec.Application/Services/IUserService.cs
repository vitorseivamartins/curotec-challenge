using Curotec.Application.DTOs;
using Curotec.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Curotec.Application.Services
{
    public interface IUserService
    {
        public Task<ActionResult<User>> CreateUser(UserDto userToBeCreated, CancellationToken cancellationToken = default);

        public Task<ActionResult<LoginResponse>> LoginUser(UserDto user, CancellationToken cancellationToken = default);
    }
}
