using Lanchat.Core.Api;
using Lanchat.Tests.Mock.Models;

namespace Lanchat.Tests.Mock.ApiHandlers
{
    public class SingleUseHandler : ApiHandler<Model>
    {
        public int Counter { get; private set; }

        protected override void Handle(Model data)
        {
            Counter++;
            Disabled = true;
        }
    }
}