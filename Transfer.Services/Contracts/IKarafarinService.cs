using System;
using System.Collections.Generic;
using System.Text;
using Transfer.Domain.DTOs.Request;

namespace Transfer.Services.Contracts
{
    public interface IKarafarinService
    {
        string GetLocalFundTransfer(KarafarinGetLocalFundTransferRequestVM getLocalFund);
        string GetAchOutgoingTransfer(KarafarinGetAchOutgoingTransferRequestVM getAchOutgoing);
        string GetTraceLocalFundTransfer(KarafarinGetTraceLocalFundTransferRequestVM getTraceLocal);
        string GetTraceAchOutgoingTransfer(KarafarinGetTraceAchOutgoingTransferRequestVM getTraceAchOutgoing);
        string GetTraceSatnaOutgoingTransfer(KarafarinGetTraceSatnaOutgoingTransferRequestVM getTraceSatna);
        string GetStatementByDate(KarafarinGetStatementByDateRequestVM getStatement);
        string GetAccountNumber(string iban);
        string GetClientInfoList(KarafarinGetClientInfoListRequestVM getClient);
        string GetOwners(string accountNumber);


    }
}
