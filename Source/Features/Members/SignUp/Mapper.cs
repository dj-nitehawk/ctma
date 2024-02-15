using Dom;

namespace Members.Signup;

sealed class Mapper : RequestMapper<Request, Member>
{
    public override Member ToEntity(Request r)
        => new();
}