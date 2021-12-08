using Microsoft.Extensions.Options;
using Transfer.Core.Model.Base;
using Transfer.RemoteServices.Contracts;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using Transfer.Resources;
using YaghutBankingService;

namespace Transfer.RemoteServices
{
    public class YaghutPaymentRemoteService : IYaghutPaymentRemoteService
    {

        private class TokenBean : loginResponseBean
        {
            public DateTime CreatedTime { get; set; }
        }

        private SoapServicesClient ServiceClient;
        private const string CERT_PASSWORD = "1053744";
        private static X509Certificate2 YaghoutCert;
        private int tokenAlive;
        private string username;
        private string password;
        private bool mockEnabled;
        private static TokenBean Token;

        static YaghutPaymentRemoteService()
        {
            SetCertificate();
        }

        public YaghutPaymentRemoteService(IOptions<AppSettings> appSettings)
        {
            tokenAlive = appSettings.Value.YaghutTokenAliveInMinutes;
            username = appSettings.Value.PaymentUsername;
            password = appSettings.Value.PaymentPassword;
            mockEnabled = appSettings.Value.MockRemoteEnable;
            ServiceClient = new SoapServicesClient(SoapServicesClient.EndpointConfiguration.soapPort, appSettings.Value.YaghutUserServiceUrl);
            var clientAddress = ServiceClient.Endpoint.Address.Uri.Scheme;
            if (string.Compare(clientAddress, "https", true) == 0)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolErr) { return true; };
                ((BasicHttpBinding)ServiceClient.Endpoint.Binding).Security.Mode = BasicHttpSecurityMode.Transport;
                ((BasicHttpBinding)ServiceClient.Endpoint.Binding).AllowCookies = false;
                ((BasicHttpBinding)ServiceClient.Endpoint.Binding).Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
                ServiceClient.ClientCredentials.ClientCertificate.Certificate = YaghoutCert;

                ServiceClient.ClientCredentials.ServiceCertificate.SslCertificateAuthentication =
                new X509ServiceCertificateAuthentication()
                {
                    CertificateValidationMode = X509CertificateValidationMode.None,
                    RevocationMode = X509RevocationMode.NoCheck
                };
            }
        }

        private static void SetCertificate()
        {
            YaghoutCert = new X509Certificate2(MainResources.EnYaghutTest, CERT_PASSWORD, X509KeyStorageFlags.DefaultKeySet);
        }

        private contextEntry[] UserSession()
        {
            CheckToken();
            return new contextEntry[] { new contextEntry { key = "SESSIONID", value = Token.sessionId.ToString() } };
        }
        private contextEntry[] UserSessionAch(string referenceNumber)
        {
            CheckToken();
            return new contextEntry[] { new contextEntry { key = "SESSIONID", value = Token.sessionId.ToString() }, new contextEntry { key = "InstrId", value = referenceNumber } };
        }
        public void CheckToken()
        {
            if (YaghoutCert == null || YaghoutCert.RawData == null || YaghoutCert.RawData.Length <= 0)
            {
                SetCertificate();
            }

            if (Token == null || string.IsNullOrEmpty(Token.sessionId))
            {
                Login();
            }
            else if ((DateTime.Now - Token.CreatedTime) >= TimeSpan.FromMinutes(tokenAlive))
            {
                Login();
            }
        }

        public void Login()
        {
            loginResponseBean loginResponseBean = ServiceClient.loginStatic(null, new userInfoRequestBean { username = username, password = password });
            Token = new TokenBean()
            {
                sessionId = loginResponseBean.sessionId,
                CreatedTime = DateTime.Now
            };
        }

        public async Task<getIbanInformationResponse> GetIbanInfo(ibanInformationRequestBean requestBean)
        {
            getIbanInformationResponse getIbanInformationResponse = await ServiceClient.getIbanInformationAsync(UserSession(), requestBean);
            return getIbanInformationResponse;
        }

        public async Task<getCustomerInfoResponse> GetCustomerInfo(userRequestBean requestBean)
        {
            getCustomerInfoResponse getCustomerInfoResponse = await ServiceClient.getCustomerInfoAsync(UserSession(), requestBean);
            return getCustomerInfoResponse;
        }

        public async Task<getDepositsResponse> GetDeposits(depositSearchRequestBean requestBean)
        {
            getDepositsResponse getDepositsResponse = await ServiceClient.getDepositsAsync(UserSession(), requestBean);
            return getDepositsResponse;
        }

        public async Task<getDepositCustomerResponse> GetDepositCustomer(depositCustomerRequestBean requestBean)
        {
            getDepositCustomerResponse getDepositCustomerResponse = await ServiceClient.getDepositCustomerAsync(UserSession(), requestBean);
            return getDepositCustomerResponse;
        }

        public async Task<getStatementResponse> GetStatement(statementSearchRequestBean requestBean)
        {
            getStatementResponse getStatementResponse = await ServiceClient.getStatementAsync(UserSession(), requestBean);
            return getStatementResponse;
        }

        public async Task<normalTransferResponse> NormalTransfer(normalTransferRequestBean requestBean)
        {
            normalTransferResponse normalTransferResponse = await ServiceClient.normalTransferAsync(UserSession(), requestBean);
            return normalTransferResponse;
        }

        public async Task<achNormalTransferResponse> AchNormalTransfer(achNormalTransferRequestBean requestBean, string referenceNumber)
        {
            try
            {
                achNormalTransferResponse achNormalTransferResponse = await ServiceClient.achNormalTransferAsync(UserSessionAch(referenceNumber), requestBean);
                return achNormalTransferResponse;
            }
            catch (Exception x) { return null; }
        }

        public async Task<rtgsNormalTransferResponse> RtgsNormalTransfer(rtgsNormalTransferRequestBean requestBean)
        {
            rtgsNormalTransferResponse rtgsNormalTransferResponse = await ServiceClient.rtgsNormalTransferAsync(UserSession(), requestBean);
            return rtgsNormalTransferResponse;
        }

        public async Task<achTransferReportResponse> AchTransferReport(achTransferSearchRequestBean requestBean)
        {
            achTransferReportResponse achTransferReportResponse = await ServiceClient.achTransferReportAsync(UserSession(), requestBean);
            return achTransferReportResponse;
        }

        public async Task<achTransactionReportResponse> AchTransactionReport(achTransactionSearchRequestBean requestBean)
        {
            achTransactionReportResponse achTransactionReportResponse = await ServiceClient.achTransactionReportAsync(UserSession(), requestBean);
            return achTransactionReportResponse;
        }

        public async Task<rtgsTransferReportResponse> RtgsTransferReport(rtgTransferSearchRequestBean requestBean)
        {
            rtgsTransferReportResponse rtgsTransferReportResponse = await ServiceClient.rtgsTransferReportAsync(UserSession(), requestBean);
            return rtgsTransferReportResponse;
        }

        public async Task<rtgsTransferDetailReportResponse> RtgsTransferDetailReport(rtgTransferDetailSearchRequestBean requestBean)
        {
            rtgsTransferDetailReportResponse rtgsTransferDetailReportResponse = await ServiceClient.rtgsTransferDetailReportAsync(UserSession(), requestBean);
            return rtgsTransferDetailReportResponse;
        }

        public async Task<convertDepositNumberToIbanResponse> ConvertDepositNumberToIban(depositNumberToIbanRequestBean requestBean)
        {
            convertDepositNumberToIbanResponse convertDepositNumberToIbanResponse = await ServiceClient.convertDepositNumberToIbanAsync(UserSession(), requestBean);
            return convertDepositNumberToIbanResponse;
        }

        public async Task<convertIbanToDepositNumberResponse> ConvertIbanToDepositNumber(ibanToDepositRequestBean requestBean)
        {
            convertIbanToDepositNumberResponse convertIbanToDepositNumberResponse = await ServiceClient.convertIbanToDepositNumberAsync(UserSession(), requestBean);
            return convertIbanToDepositNumberResponse;
        }
    }
}
