using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace PavelNazarov.Common.ServiceModel
{
    /// <summary>
    /// This class wraps a service client to prevent an exception on disposing in Faulted state
    /// </summary>
    /// <typeparam name="T">Any service client type that implements the ICommunicationObject interface</typeparam>
    public class ServiceClientSafeDisposingWrapper<T> : IDisposable where T : ICommunicationObject
    {
        private readonly T _serviceClient;

        /// <summary>
        /// Wrapped service client
        /// </summary>
        public T ServiceClient
        {
            get { return _serviceClient; }
        }

        public ServiceClientSafeDisposingWrapper(T serviceClient)
        {
            if (serviceClient == null) throw new ArgumentNullException("serviceClient");
            _serviceClient = serviceClient;
        }

        public void Dispose()
        {
            if (_serviceClient.State == CommunicationState.Faulted)
            {
                _serviceClient.Abort();
            }
            else if (_serviceClient.State != CommunicationState.Closed)
            {
                _serviceClient.Close();
            }
        }
    }
}
