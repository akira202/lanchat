namespace Lanchat.Core.Api
{
    internal interface IInput
    {
        void OnDataReceived(object sender, string item);
    }
}