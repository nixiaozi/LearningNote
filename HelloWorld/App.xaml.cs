using Autofac;
using Autofac.Extensions.DependencyInjection;
using HelloWorld.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace HelloWorld
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // 实验证明 同预想，构造函数会先于 Application_Startup 执行
            Debug.Print("do method ==>App Init");
        }

        private ServiceProvider _serviceProvider;

        private IServiceProvider _iServiceProvider;


        private IContainer TheContainer; 
        public void  DoApp() 
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            //serviceCollection.AddAutofac(container => {
            //    AutofacContainBuilder(container);
            //}); // 添加AutoFac 引用,这种添加方法似乎不行的
            serviceCollection.AddSingleton<ITextService>(provider => new TextService("Hi WPF .NET Core 3.0!"));

            InitAutofacRegister(serviceCollection);
        }

        private void InitAutofacRegister(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services); // 可以使用Populate 把MicrosoftDependencyInject注册的服务加入到 autofac 中。
            containerBuilder.RegisterType<WelcomeService>().As<IWelcomeService>();

            TheContainer = containerBuilder.Build();

        }

        private void AutofacContainBuilder(ContainerBuilder builder)
        {

            builder.RegisterType<WelcomeService>().As<IWelcomeService>();
        }

        private void Application_Startup(object sender, StartupEventArgs e) // 这个方法会在构造函数APP()之前执行
        {
            Debug.Print("do method ==> Application_Startup");
            DoApp();
            #region 错误示范，这是在main函数中定义的
            //var builder = new HostBuilder()
            //    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            //    .ConfigureContainer<ContainerBuilder>(af =>
            //    {
            //        AutofacContainBuilder(af);
            //    });
            //var buildered= builder.Build();
            //_iServiceProvider = buildered.Services;
            //buildered.Run();
            #endregion

            var autofacwelcomeService = TheContainer.Resolve<IWelcomeService>();
            MessageBox.Show(autofacwelcomeService.DoWelocme());

            var autofactextService = TheContainer.Resolve<ITextService>();

            #region MessageBox 会出错 AddAutofac 没有用
            //// var builder = new HostBuilder()
            //var _welcomeService = _serviceProvider.GetService<IWelcomeService>();

            //// MessageBox.Show("Hello World APP StartUp.");
            //MessageBox.Show(_welcomeService.DoWelocme());
            #endregion

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();



        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            

            Debug.Print("现在时间为：" + DateTime.Now);
        }

    }
}
