using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class LikePostCommand: BaseCommand
    {
        public int MyProperty { get; set; }
    }
}