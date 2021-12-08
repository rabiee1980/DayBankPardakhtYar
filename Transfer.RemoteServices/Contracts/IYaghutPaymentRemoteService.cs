using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
//using YaghoutBankingService;
using YaghutBankingService;
using static Transfer.RemoteServices.YaghutPaymentRemoteService;

namespace Transfer.RemoteServices.Contracts
{
    public interface IYaghutPaymentRemoteService
    {
        void Login();

        void CheckToken();

        Task<getIbanInformationResponse> GetIbanInfo(ibanInformationRequestBean requestBean);

        Task<getCustomerInfoResponse> GetCustomerInfo(userRequestBean requestBean);

        Task<getDepositsResponse> GetDeposits(depositSearchRequestBean requestBean);

        Task<getDepositCustomerResponse> GetDepositCustomer(depositCustomerRequestBean requestBean);

        Task<getStatementResponse> GetStatement(statementSearchRequestBean requestBean);

        Task<normalTransferResponse> NormalTransfer(normalTransferRequestBean requestBean);

        Task<achNormalTransferResponse> AchNormalTransfer(achNormalTransferRequestBean requestBean,string referenceNumber);

        Task<rtgsNormalTransferResponse> RtgsNormalTransfer(rtgsNormalTransferRequestBean requestBean);

        Task<achTransferReportResponse> AchTransferReport(achTransferSearchRequestBean requestBean);

        Task<achTransactionReportResponse> AchTransactionReport(achTransactionSearchRequestBean requestBean);

        Task<rtgsTransferReportResponse> RtgsTransferReport(rtgTransferSearchRequestBean requestBean);

        Task<rtgsTransferDetailReportResponse> RtgsTransferDetailReport(rtgTransferDetailSearchRequestBean requestBean);

        Task<convertDepositNumberToIbanResponse> ConvertDepositNumberToIban(depositNumberToIbanRequestBean requestBean);

        Task<convertIbanToDepositNumberResponse> ConvertIbanToDepositNumber(ibanToDepositRequestBean requestBean);
    }
}
