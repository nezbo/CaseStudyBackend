using Microservice.Common.EntityFrameworkCore;
using Microservice.Common.Models;
using Moq.AutoMock;
using MockQueryable;

namespace Microservice.Common.Test.Core;

public class BaseTestFixture<TSubject> where TSubject : class
{
    protected AutoMocker Container { get; } = new AutoMocker();

    private TSubject _lazySut;
    protected TSubject Sut { get { return _lazySut ??= Container.CreateInstance<TSubject>(); } }
}
