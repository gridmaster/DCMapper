using Logger;
using Core.Interface;
using Ninject.Modules;
using Services.Interface;
using Services.Service;

namespace DCMapper.DIModule
{
    public class ProductionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<ConsoleLogger>().InSingletonScope()
                .WithConstructorArgument("loglevel", LogLevelEnum.Fatal);

            Bind<IMyFakeService>().To<MyFakeService>().InSingletonScope();
            Bind<IMyOtherService>().To<MyOtherService>().InSingletonScope();
        }
    }
}
