using Lanchat.Core.Api;
using Lanchat.Tests.Mock.Models;

namespace Lanchat.Tests.Mock.ApiHandlers
{
    public class PrivilegedHandler : ApiHandler<PrivilegedModel>
    {
        public bool Received;

        public PrivilegedHandler()
        {
            Privileged = true;
        }

        protected override void Handle(PrivilegedModel data)
        {
            Received = true;
        }
    }
}