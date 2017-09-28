using Ao.OpsGenie.Api;
using Ao.OpsGenie.Api.Configuration;
using AO.Clients.Amazon.Api.Services;
using AO.Clients.Amazon.Host.Modules;
using AO.Clients.Amazon.Queries.Entities;
using AO.Clients.Amazon.Queries.Repositories;
using AO.Clients.Amazon.Services.CustomerDeliveryService;
using AO.Clients.Core.ClientPlatform;
using Autofac;
using MassTransit;
using Moq;
using NUnit.Framework;

namespace AO.Clients.Amazon.Tests.BusIntegrationTests
{
    public class FilterIntergarationTests : BaseFilterIntergarationTests
    {
        protected IContainer container;
        protected ContainerBuilder builder;
        protected IBusControl _bus;
        protected Mock<MarketplaceWebService.MarketplaceWebService> amazonMock;
        protected Mock<IClientGatewayProxy> cgMock;
        protected Mock<IAlertService> alertSerivceMock;
        protected Mock<IDeliverySchedule> deliveryDateService;
        protected Mock<IAlertService> alertService;
        protected Mock<IOpsGenieConfig> opsGenieConfig;
        protected Mock<IGenieRestApiClient> genieRestApiClient;
        protected Mock<IOpsGenieAlertRepository> opsGenieAlertRepository;
        protected Mock<IReportService> reportService;
        protected Mock<IGetDeliveryDate> getDeliveryDate;
        protected Mock<IDeliveryLocationFactory> deliveryLocationFactory;
        protected AmazonStoreConfiguration config;

        [OneTimeSetUp]
        public void Initialize()
        {
            builder = new ContainerBuilder();

            var assembly = typeof(MassTransitModule).Assembly;
            builder.RegisterAssemblyModules<MassTransitModule>(assembly);

            AddFakeRegistrations();
            OverrideRegistrations();
            PrepareTestData();

            container = builder.Build();

            _bus = container.Resolve<IBusControl>();

            _bus.Start();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (_bus != null)
            {
                _bus.Stop();
            }
        }

        protected virtual void PrepareTestData()
        {
        }

        protected virtual void OverrideRegistrations()
        {
        }

        private void AddFakeRegistrations()
        {
            builder.RegisterInstance(SessionFactory);

            alertService = new Mock<IAlertService>();
            builder.RegisterInstance(alertService.Object);

            config = new AmazonStoreConfiguration();
            builder.RegisterInstance(config);

            amazonMock = new Mock<MarketplaceWebService.MarketplaceWebService>();
            builder.RegisterInstance(amazonMock.Object);

            cgMock = new Mock<IClientGatewayProxy>();
            builder.RegisterInstance(cgMock.Object);

            deliveryDateService = new Mock<IDeliverySchedule>();
            builder.RegisterInstance(deliveryDateService.Object);

            alertSerivceMock = new Mock<IAlertService>();
            builder.RegisterInstance(alertSerivceMock.Object);

            opsGenieConfig = new Mock<IOpsGenieConfig>();
            builder.RegisterInstance(opsGenieConfig.Object);

            genieRestApiClient = new Mock<IGenieRestApiClient>();
            builder.RegisterInstance(genieRestApiClient);

            opsGenieAlertRepository = new Mock<IOpsGenieAlertRepository>();
            builder.RegisterInstance(opsGenieAlertRepository);

            reportService = new Mock<IReportService>();
            builder.RegisterInstance(reportService);

            getDeliveryDate = new Mock<IGetDeliveryDate>();
            builder.RegisterInstance(getDeliveryDate);

            deliveryLocationFactory = new Mock<IDeliveryLocationFactory>();
            builder.RegisterInstance(deliveryLocationFactory);
        }
    }
}
