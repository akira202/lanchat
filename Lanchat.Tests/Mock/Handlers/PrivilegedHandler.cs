using Lanchat.Core.API;
using Lanchat.Tests.Mock.Models;

namespace Lanchat.Tests.Mock.Handlers
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