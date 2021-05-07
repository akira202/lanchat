using Lanchat.Core.Api;

namespace Lanchat.Tests.Mock.Api
{
    public class OutputMock : IOutput
    {
        public string LastOutput { get; private set; }

        public void SendData(object content)
        {
            LastOutput = content.ToString();
        }

        public void SendPrivilegedData(object content)
        {
            LastOutput = content.ToString();
        }
    }
}