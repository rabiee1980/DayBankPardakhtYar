using Microsoft.Extensions.Options;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Transfer.Core.Model.Base;
using Transfer.Domain.DTOs.Request;
using Transfer.Services.Contracts;


namespace Transfer.Services
{
    public class KarafarinService : IKarafarinService
    {
        private readonly AppSettings _appSettings;
        public KarafarinService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public string GetLocalFundTransfer(KarafarinGetLocalFundTransferRequestVM getLocalFund)
        {
            var json = new JavaScriptSerializer().Serialize(getLocalFund);
            var client = new RestClient(_appSettings.KaraFarainURL + "account/transfer/local-fund/v1");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJ2aWVyYSIsImlwIjoiNS4yMDEuMTc5LjIzMiIsInByZWZlcnJlZF91c2VybmFtZSI6InZpZXJhIiwiZXhwIjoxNjM4OTY4NTE4LCJpYXQiOjE2Mzg5NjY3MTh9.22Qm3qCQPn_Q7TYP3sfExoM1hTvZXpziM20r9MBJaSkwbTpKFHPsi8pxHxTWz16g2K4q4ns3_UR-MqBTiX5Pow");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response;

        }
        public string GetAchOutgoingTransfer(KarafarinGetAchOutgoingTransferRequestVM getAchOutgoing)
        {
            var json = new JavaScriptSerializer().Serialize(getAchOutgoing);
            var client = new RestClient(_appSettings.KaraFarainURL + "/account/transfer/paya/transfer/v1");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJ2aWVyYSIsImlwIjoiNS4yMDEuMTc5LjIzMiIsInByZWZlcnJlZF91c2VybmFtZSI6InZpZXJhIiwiZXhwIjoxNjM4OTY5NjgyLCJpYXQiOjE2Mzg5Njc4ODJ9.2U8VPTcGrJbQUg8WrL9fy0E3MPsXYXzvND2-SfqITkdaXI7n50twzbtPU2Mdl-wWoSeFfmHivve3OIr_4o5gPg");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response;
        }
        public string GetTraceLocalFundTransfer(KarafarinGetTraceLocalFundTransferRequestVM getTraceLocal)
        {
            var json = new JavaScriptSerializer().Serialize(getTraceLocal);
            var client = new RestClient(_appSettings.KaraFarainURL + "/account/trace/local-fund/v1");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response;
        }
        public string GetTraceAchOutgoingTransfer(KarafarinGetTraceAchOutgoingTransferRequestVM getTraceAchOutgoing)
        {
            var json = new JavaScriptSerializer().Serialize(getTraceAchOutgoing);
            var client = new RestClient(_appSettings.KaraFarainURL + "/account/transfer/paya/trace/v1");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response;
        }

        public string GetTraceSatnaOutgoingTransfer(KarafarinGetTraceSatnaOutgoingTransferRequestVM getTraceSatna)
        {
            var json = new JavaScriptSerializer().Serialize(getTraceSatna);
            var client = new RestClient(_appSettings.KaraFarainURL + "/account/transfer/satna/trace/v1");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response;
        }

        public string GetSatnaOutgoingTransferValues(KarafarinGetSatnaOutgoingTransferValuesRequestVM getSatanOutgoing)
        {
            var json = new JavaScriptSerializer().Serialize(getSatanOutgoing);
            var client = new RestClient(_appSettings.KaraFarainURL + "/account/transfer/satna/transfer/v1");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJ2aWVyYSIsImlwIjoiNS4yMDEuMTc5LjIzMiIsInByZWZlcnJlZF91c2VybmFtZSI6InZpZXJhIiwiZXhwIjoxNjM4OTY5ODA0LCJpYXQiOjE2Mzg5NjgwMDR9.40fqL_reMTP5ViuVwFs-tPxDJeTqkNqvXYhMyu_8V9cdFCJQsUs2-560AMYGCa1mwzWgZJKopZf8uUtFmGXbcg");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response;
        }
        public string GetStatementByDate(KarafarinGetStatementByDateRequestVM getStatement)
        {
            var client = new RestClient(_appSettings.KaraFarainURL+ "/account/"+getStatement.accountNumber+"/statement/date/v1?fromDate="+getStatement.fromDate + "&toDate="+getStatement.toDate + "");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJ2aWVyYSIsImlwIjoiNS4yMDEuMTc5LjIzMiIsInByZWZlcnJlZF91c2VybmFtZSI6InZpZXJhIiwiZXhwIjoxNjM4OTcwNzc5LCJpYXQiOjE2Mzg5Njg5Nzl9.Nn9oc_3kXiPwHdrEhnGm5nbiw65lBKxx-C4WOJFJ5yhM9byOBrdl48GZ6QYaiaD2hOa09JZx1qV3OgzQ5SF23A");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response;
        }
        public string GetStatementByCount(KarafarinGetStatementByCountRequestVM getStatment)
        {
            var client = new RestClient(_appSettings.KaraFarainURL+ "/account/"+getStatment.accountNumber+"/statement/cycle/v1?count="+getStatment.count+"");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response;
        }
        public string GetAccountNumber(string iban)
        {
            var client = new RestClient(_appSettings.KaraFarainURL+ "/account/iban/"+iban+"/v1");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
        public string GetClientInfoList(KarafarinGetClientInfoListRequestVM getClient)
        {
            var client = new RestClient(_appSettings.KaraFarainURL+ "/client/id/"+getClient.key+"/"+getClient.value+"/v1");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

    }
}
