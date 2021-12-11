using Microsoft.Extensions.Options;
using Transfer.Core.Model.Base;
using Transfer.Data.Base;
using Transfer.RemoteServices;
using Transfer.RemoteServices.Base;
using Transfer.RemoteServices.Contracts;

namespace Transfer.RemoteServices.Base
{
    public class RemoteServiceWrapper : IRemoteServiceWrapper
    {
        private IRepositoryWrapper _repository;
        private readonly IOptions<AppSettings> _appSettings;                
        public IYaghutPaymentRemoteService YaghutPaymentRemoteService => new YaghutPaymentRemoteService(_appSettings);
        public IKarafarinService karafarinService => new KarafarinService(_appSettings);
        public RemoteServiceWrapper(IRepositoryWrapper repository,IOptions<AppSettings> appSettings)
        {
            _repository = repository;
            _appSettings = appSettings;            
        }
    }
}
