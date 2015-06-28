using Logger;
using Core.Interface;
using Ninject.Modules;
using Services.Interface;
using Services.Service;

namespace DCMapper.DIModule
{
    class DebugModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Log4NetLogger>().InSingletonScope()
                .WithConstructorArgument("loglevel", LogLevelEnum.Debug);

            Bind<IDCMapService>().To<DCMapService>().InSingletonScope();
        }
    }
}
