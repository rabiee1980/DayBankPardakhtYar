using Transfer.RemoteServices.Contracts;

namespace Transfer.RemoteServices.Base
{
    public interface IRemoteServiceWrapper
    {
        IYaghutPaymentRemoteService YaghutPaymentRemoteService { get; }
        IKarafarinService karafarinService { get; }

    }
}
