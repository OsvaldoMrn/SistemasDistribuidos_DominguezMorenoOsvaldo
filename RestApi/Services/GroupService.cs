using RestApi.Exceptions;
using RestApi.Models;
using RestApi.Repositories;

namespace RestApi.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;


    public GroupService(IGroupRepository groupRepository){
        _groupRepository = groupRepository;
    }
    private readonly IUserRepository _userRepository;
    public GroupService(IGroupRepository groupRepository, IUserRepository userRepository){
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }


    public async Task DeleteGroupByIdAsync(string id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
        if(group is null){
            throw new GroupNotFoundException();
        }

        await _groupRepository.DeleteByIdAsync(id, cancellationToken);
    }

    public async Task<GroupUserModel> GetGroupByIdAsync(string Id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(Id, cancellationToken);
        if(group is null){
            return null;
        }
        return new GroupUserModel{
            Id = group.Id,
            Name = group.Name,


            CreationDate = group.CreationDate
        };
    }
    public async Task<IEnumerable<GroupUserModel>> GetGroupsByNameAsync(string name, CancellationToken cancellationToken) // Nuevo método
    {
        var groups = await _groupRepository.GetByNameAsync(name, cancellationToken);
        return groups.Select(group => new GroupUserModel
        {
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate
        });
    }
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user !=null).ToList()

        };
    }

    public async Task<IEnumerable<GroupUserModel>> GetGroupsByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken)
    {
        var groups = await _groupRepository.GetByNameAsync(name, cancellationToken);

    public async Task<IEnumerable<GroupUserModel>> GetGroupsByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken)
    {
        var groups = await _groupRepository.GetByNameAsync(name, pageIndex, pageSize, orderBy, cancellationToken);

        var groupUserModels = await Task.WhenAll(groups.Select(async group => 
        {
            var users = await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)));
            return new GroupUserModel
            {
                Id = group.Id,
                Name = group.Name,
                CreationDate = group.CreationDate,
                Users = users.Where(user => user != null).ToList()
            };
        }));

        return groupUserModels;
    }


    public async Task<GroupUserModel> CreateGroupAsync(string name, Guid[] users, CancellationToken cancellationToken)
    {
        if(users.Length == 0){
            throw new InvalidGroupRequestFormatException();
        }

        var groups = await _groupRepository.GetByExactNameAsync(name, cancellationToken);
        if(groups is not null){
            throw new GroupAlreadyExistsException();
        }

        var usersDB = await Task.WhenAll(users.Select(async userId => 
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                throw new UserDoesNotExistsException(); 
            }
            return user;
        }));


        var group = await _groupRepository.CreateAsync(name, users, cancellationToken);
        return new GroupUserModel{
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user !=null).ToList()

        };
    }

    public async Task<GroupUserModel> GetGroupByExactNameAsync(string name, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByExactNameAsync(name, cancellationToken);
        if (group == null)
        {
            return null;
        }

        return new GroupUserModel
        {
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user != null).ToList()
        };
    }

    public async Task UpdateGroupAsync(string id, string name, Guid[] users, CancellationToken cancellationToken)
    {
        if (users.Length == 0)
        {
            throw new InvalidGroupRequestFormatException();
        }

        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var existingGroup = await _groupRepository.GetByExactNameAsync(name, cancellationToken);
        if (existingGroup is not null && existingGroup.Id != id)
        {
            throw new GroupAlreadyExistsException();
        }

        var usersDB = await Task.WhenAll(users.Select(async userId => 
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                throw new UserDoesNotExistsException(); 
            }
            return user;
        }));


        };
    }

    public async Task<GroupUserModel> GetGroupByExactNameAsync(string name, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByExactNameAsync(name, cancellationToken);
        if (group == null)
        {
            return null;
        }



        return orderedGroups
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return groupUserModels;
    }


    public async Task<GroupUserModel> CreateGroupAsync(string name, Guid[] users, CancellationToken cancellationToken)
    {
        if(users.Length == 0){
            throw new InvalidGroupRequestFormatException();
        }

        var groups = await _groupRepository.GetByExactNameAsync(name, cancellationToken);
        if(groups is not null){
            throw new GroupAlreadyExistsException();
        }

        var usersDB = await Task.WhenAll(users.Select(async userId => 
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                throw new UserDoesNotExistsException(); 
            }
            return user;
        }));

        var group = await _groupRepository.CreateAsync(name, users, cancellationToken);
        return new GroupUserModel{
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user !=null).ToList()

        };
    }

    public async Task<GroupUserModel> GetGroupByExactNameAsync(string name, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByExactNameAsync(name, cancellationToken);
        if (group == null)
        {
            return null;
        }

        return new GroupUserModel
        {
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user != null).ToList()
        };


    }

        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                throw new UserDoesNotExistsException(); 
            }
            return user;
        }));

        var group = await _groupRepository.CreateAsync(name, users, cancellationToken);
        return new GroupUserModel{
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user !=null).ToList()

        };
    }

    public async Task<GroupUserModel> GetGroupByExactNameAsync(string name, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByExactNameAsync(name, cancellationToken);
        if (group == null)
        {
            return null;
        }

        return new GroupUserModel
        {
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreationDate,
            Users = (await Task.WhenAll(group.Users.Select(userId => _userRepository.GetByIdAsync(userId, cancellationToken)))).Where(user => user != null).ToList()
        };
    }

    public async Task UpdateGroupAsync(string id, string name, Guid[] users, CancellationToken cancellationToken)
    {
        if (users.Length == 0)
        {
            throw new InvalidGroupRequestFormatException();
        }

        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var existingGroup = await _groupRepository.GetByExactNameAsync(name, cancellationToken);
        if (existingGroup is not null && existingGroup.Id != id)
        {
            throw new GroupAlreadyExistsException();
        }

        var usersDB = await Task.WhenAll(users.Select(async userId => 
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                throw new UserDoesNotExistsException(); 
            }
            return user;
        }));


        await _groupRepository.UpdateGroupAsync(id, name, users, cancellationToken);
    }

}