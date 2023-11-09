using AutofacContrib.NSubstitute;

namespace Microservice.Common.Test.Core;

public class BaseTestFixture<TSubject> where TSubject : class
{
    protected AutoSubstituteBuilder Builder { get; } = AutoSubstitute.Configure();

    private AutoSubstitute? _lazyContainer;
    protected AutoSubstitute Container { get { return _lazyContainer ??= Builder.Build(); } } 

    private TSubject? _lazySut;
    protected TSubject Sut { get { return _lazySut ??= Container.Resolve<TSubject>(); } }
}
