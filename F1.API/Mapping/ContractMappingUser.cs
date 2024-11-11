using F1.Application.Models;
using F1.Contracts.Requests;
using F1.Contracts.Responses;

namespace Task5.Mapping;

public static class ContractMappingUser
{
    public static User MapToUser(this RegisterUserRequest request)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            Password = request.Password,
            SubscriptionType = request.SubscriptionType
        };
    }

    public static User MapToUser(this UpdateUserRequest request, Guid id)
    {
        return new User
        {
            Id = id,
            FullName = request.FullName,
            Email = request.Email,
            Password = request.Password,
            SubscriptionType = request.SubscriptionType
        };
    }

    public static UserResponse MapToResponse(this User user)
    {
        return new UserResponse()
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            SubscriptionType = user.SubscriptionType
        };
    }
    
    public static UsersResponse MapToResponse(this IEnumerable<User> users)
    {
        return new UsersResponse
        {
            Items = users.Select(MapToResponse)    
        };
        
    }
}