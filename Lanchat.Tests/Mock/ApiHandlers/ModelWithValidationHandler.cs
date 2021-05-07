using Lanchat.Core.Api;
using Lanchat.Tests.Mock.Models;

namespace Lanchat.Tests.Mock.ApiHandlers
{
    public class ModelWithValidationHandlerMock : ApiHandler<ModelWithValidation>
    {
        protected override void Handle(ModelWithValidation data)
        { }
    }
}