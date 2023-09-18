using Moq.AutoMock;

namespace Microservice.Common.Test.Core;

public class BaseTestFixture<TSubject> where TSubject : class
{
    protected AutoMocker Container { get; } = new AutoMocker();

    private TSubject? _lazySut;
    protected TSubject Sut { get { return _lazySut ??= Container.CreateInstance<TSubject>(); } }
}
