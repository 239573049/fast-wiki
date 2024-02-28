using FastWiki.Service.Application.Users.Queries;
using FastWiki.Service.Contracts.Users.Dto;
using FastWiki.Service.Domain.Users.Repositories;

namespace FastWiki.Service.Application.Users;

public class UserQueryHandler(IUserRepository userRepository, IMapper mapper)
{
    [EventHandler]
    public async Task UserInfoAsync(UserInfoQuery query)
    {
        var dto = await userRepository.FindAsync(x => x.Account == query.Account);

        if (dto == null)
        {
            throw new UserFriendlyException("�˺Ų�����");
        }

        if (!dto.CheckCipher(query.Pass))
        {
            throw new UserFriendlyException("�������");
        }

        query.Result = mapper.Map<UserDto>(dto);
    }
}