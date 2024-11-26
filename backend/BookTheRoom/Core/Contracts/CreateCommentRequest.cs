using Core.Enums;

namespace Core.Contracts
{
    public record CreateCommentRequest
    (
        string Description,
        PropertyCategory PropertyCategory
    );
}
