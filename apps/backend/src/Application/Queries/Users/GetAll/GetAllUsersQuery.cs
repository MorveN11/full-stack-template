using Application.Abstractions.Messaging;
using Application.Abstractions.Responses;

namespace Application.Queries.Users.GetAll;

public sealed record GetAllUsersQuery(int Page, int PageSize) : IQuery<PagedList<UserResponse>>;
