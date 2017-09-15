using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using ItronXchangeServices;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System.Globalization;
using Microsoft.Owin.Hosting;

namespace Stateless1
{
    internal class OwinCommunicationListener : ICommunicationListener
    {
        private readonly Startup _startup;
        private readonly ServiceContext _serviceContext;
        private readonly string _endpointName;
        private readonly string _appRoot;

        private IDisposable _webApp;
        private string _publishAddress;
        private string _listeningAddress;
       

        public OwinCommunicationListener(Startup startup, ServiceContext serviceContext,  string endpointName)
            : this(startup, serviceContext, endpointName, null)
        {
        }

        public OwinCommunicationListener(Startup startup, ServiceContext serviceContext,  string endpointName, string appRoot)
        {
            if (startup == null)
            {
                throw new ArgumentNullException(nameof(startup));
            }

            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            if (endpointName == null)
            {
                throw new ArgumentNullException(nameof(endpointName));
            }

          

            _startup = startup;
            _serviceContext = serviceContext;
            _endpointName = endpointName;
            
            _appRoot = appRoot;
        }

        public bool ListenOnSecondary { get; set; }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var serviceEndpoint = _serviceContext.CodePackageActivationContext.GetEndpoint(_endpointName);
            int port = serviceEndpoint.Port;

            if (_serviceContext is StatefulServiceContext)
            {
                StatefulServiceContext statefulServiceContext = _serviceContext as StatefulServiceContext;

                _listeningAddress = string.Format(
                    CultureInfo.InvariantCulture,
                    "http://+:{0}/{1}{2}/{3}/{4}",
                    port,
                    string.IsNullOrWhiteSpace(_appRoot)
                        ? string.Empty
                        : _appRoot.TrimEnd('/') + '/',
                    statefulServiceContext.PartitionId,
                    statefulServiceContext.ReplicaId,
                    Guid.NewGuid());
            }
            else if (_serviceContext is StatelessServiceContext)
            {
                _listeningAddress = string.Format(
                    CultureInfo.InvariantCulture,
                    "http://+:{0}/{1}",
                    port,
                    string.IsNullOrWhiteSpace(_appRoot)
                        ? string.Empty
                        : _appRoot.TrimEnd('/') + '/');
            }
            else
            {
                throw new InvalidOperationException();
            }

            _publishAddress = _listeningAddress.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);

            try
            {
                

                _webApp = WebApp.Start(_listeningAddress, appBuilder => _startup.Configuration(appBuilder));

                

                return Task.FromResult(_publishAddress);
            }
            catch (Exception ex)
            {
                

                StopWebServer();

                throw;
            }
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            

            StopWebServer();

            return Task.FromResult(true);
        }

        public void Abort()
        {
            

            StopWebServer();
        }

        private void StopWebServer()
        {
            if (_webApp != null)
            {
                try
                {
                    _webApp.Dispose();
                }
                catch (ObjectDisposedException)
                {
                    // no-op
                }
            }
        }

       

        private long GetReplicaOrInstanceId(ServiceContext context)
        {
            StatelessServiceContext stateless = context as StatelessServiceContext;
            if (stateless != null)
            {
                return stateless.InstanceId;
            }

            StatefulServiceContext stateful = context as StatefulServiceContext;
            if (stateful != null)
            {
                return stateful.ReplicaId;
            }

            throw new NotSupportedException("Context type not supported.");
        }
    }
}