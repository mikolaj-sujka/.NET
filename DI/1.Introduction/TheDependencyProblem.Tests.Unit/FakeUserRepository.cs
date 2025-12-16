using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheDependencyProblem.Data;

namespace TheDependencyProblem.Tests.Unit;

public class FakeUserRepository : IUserRepository
{
    private Dictionary<Guid, User> _users { get; } = new();
    public Task<IEnumerable<User>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<User>>(_users.Values);
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        _users.TryGetValue(id, out var user);
        return Task.FromResult<User?>(user);
    }

    public Task<bool> CreateAsync(User user)
    {
        _users[user.Id] = user;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        var removed = _users.Remove(id);
        return Task.FromResult(removed);
    }
}