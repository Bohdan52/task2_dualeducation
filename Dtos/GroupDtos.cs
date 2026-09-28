using task2.Domain.Enums;

namespace task2.Dtos;

public record CreateGroupRequest(string Name, GroupType Type, string Currency);

public record AddMemberRequest(Guid UserId);

public record GroupResponse(
    Guid Id,
    string Name,
    GroupType Type,
    string Currency,
    Guid CreatedById,
    List<GroupMemberResponse> Members
);

public record GroupMemberResponse(Guid UserId, string FullName, MemberRole Role);