using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nancy.Json.Simple;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Transfer.Core.Helpers;
using Transfer.Core.Model.Base;
using Transfer.Domain.Models;
using Transfer.RemoteServices.Base;
using Transfer.Services.Base;
using YaghutBankingService;

namespace Transfer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly IServiceWrapper _service;
        private readonly IRemoteServiceWrapper _remoteService;
        private readonly AppSettings _appSettings;

        public TransferController(IServiceWrapper service, IRemoteServiceWrapper remoteService, IOptions<AppSettings> appSettings)
        {
            _service = service;
            _remoteService = remoteService;
            _appSettings = appSettings.Value;
        }


        public class TransferParams
        {

            public object SourceIban { get; set; }

            public object TransferType { get; set; }

            public object TransactionDate { get; set; }

            public object TrackingNumber { get; set; }

            public object ReferenceNumber { get; set; }

            public object Amount { get; set; }

            public object DestinationIban { get; set; }

            public object OwnerName { get; set; }

            public object OwnerNationalId { get; set; }

            public object Description { get; set; }
        }

        [HttpPost("Request")]
        public async Task<IActionResult> TransferRequest([FromHeader] string authorization, [FromBody] object _transferParams)
        {
            var transferRequest = ((JArray)JsonConvert.DeserializeObject(_transferParams.ToString())).Values<JObject>();
            List<string> properties = new List<string> { "sourceIban", "transferType", "transactionDate", "trackingNumber", "referenceNumber", "amount", "destinationIban", "ownerName", "ownerNationalId", "description" };
            string key = null;
            string value = null;
            var isValid = true;
            var jsonDataOutput = "[";
            foreach (var item in transferRequest)
            {
                string errorDescription = null;
                string extraData = null;
                int? errorCode = null;
                var errorList = new List<string>();

                #region Validation
                {
                    #region Check Parameter Count , Data Type And Spelling Mistakes 
                    {
                        if (item.Count > 10)
                        {
                            isValid = false;
                            errorCode = 0;
                            errorDescription = "";
                            extraData = "The number of parameters is not valid";

                            errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                        }

                        var child = (JObject)item.First.Parent;
                        foreach (var ch in child)
                        {
                            key = ch.Key;
                            if (string.IsNullOrEmpty(properties.Find(a => a == key)))
                            {
                                isValid = false;
                                errorCode = 0;
                                errorDescription = "";
                                extraData = $"{key} is not valid parameter";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }
                        }

                        var propertyDetails = new Dictionary<string, object>();
                        propertyDetails.Add(key, value);
                        foreach (var kDetail in propertyDetails)
                        {

                        }
                    }
                    #endregion
                }
                #endregion

                var errorDetail = "";
                foreach (var er in errorList)
                {
                    errorDetail += er + ",";
                }
                if (!string.IsNullOrEmpty(errorDetail)) errorDetail = errorDetail.Remove(errorDetail.Length - 1);

                jsonDataOutput += @"{""trackingNumber"": """ + item["trackingNumber"] + @""",""transactionId"":""" + 0 + @""",""status"":""" + 0;
                jsonDataOutput += @""",""errors"":[" + errorDetail + @"],""registrationDate"":""" + DateHelpers.DateTimeToTimestamp(DateTime.Now) + @"""},";
            }
            if (transferRequest != null) jsonDataOutput = jsonDataOutput.Remove(jsonDataOutput.Length - 1);
            jsonDataOutput += "]";
            var response = ((JArray)JsonConvert.DeserializeObject(jsonDataOutput)).Values<JObject>();
            if (!isValid) return Ok(response);














            List<TransferParams> transferParams = JsonConvert.DeserializeObject<List<TransferParams>>(_transferParams.ToString());
            var pass = _appSettings.UserName + ":" + _appSettings.Password;
            var encodedStr = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(pass));
            if (authorization == encodedStr)
            {
                isValid = true;
                //----------------------------------------------- Control Bad Request Field
                jsonDataOutput = "[";
                foreach (var item in transferParams)
                {
                    string errorDescription = null;
                    string extraData = null;
                    int? errorCode = null;
                    var errorList = new List<string>();

                    #region Validation
                    {
                        #region Check TransferType 
                        {
                            if (item.TransferType != null)
                            {
                                if (item.TransferType.GetType() != typeof(int))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "TransferTypr type Is Not valid";
                                    extraData = "TransferTypr";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                                else
                                {
                                    try
                                    {
                                        int chTransferType = Convert.ToInt32(item.TransferType);
                                        if (!_service.TransferType.IsExists(Convert.ToInt32(item.TransferType)))
                                        {
                                            isValid = false;
                                            errorCode = 6;
                                            errorDescription = "transferType is not define";
                                            extraData = "transferType-" + item.TransferType;

                                            errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" +
                                                          errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                        }
                                    }
                                    catch
                                    {
                                        isValid = false;
                                        errorCode = 5;
                                        errorDescription = "transferType is not valid";
                                        extraData = "transferType";

                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" +
                                                      errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                }
                            }
                            if (item.SourceIban != null)
                            {
                                if (item.SourceIban.GetType() != typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "SourceIban type Is Not valid";
                                    extraData = "SourceIban";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                                if (item.SourceIban.ToString().Substring(0, 2) != "IR")
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "SourceIban type Is Not valid";
                                    extraData = "SourceIban";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (item.DestinationIban != null)
                            {
                                if (item.DestinationIban.GetType() != typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "DestinationIban type Is Not valid";
                                    extraData = "DestinationIban";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (item.TrackingNumber != null)
                            {
                                if (item.TrackingNumber.GetType() != typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "TrackingNumber type Is Not valid";
                                    extraData = "TrackingNumber";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (item.Description != null)
                            {
                                if (item.Description.GetType() != typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "Description type Is Not valid";
                                    extraData = "Description";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (item.ReferenceNumber != null)
                            {
                                if (item.ReferenceNumber.GetType() != typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "ReferenceNumber type Is Not valid";
                                    extraData = "ReferenceNumber";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (item.OwnerName != null)
                            {
                                if (item.OwnerName.GetType() != typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "OwnerName type Is Not valid";
                                    extraData = "OwnerName";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (item.Amount != null)
                            {
                                if (item.Amount.GetType() == typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "Amount type Is Not valid";
                                    extraData = "Amount";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                                try
                                {
                                    decimal? chAmount = Convert.ToDecimal(item.Amount);
                                }
                                catch
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "Amount is not valid";
                                    extraData = "Amount";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" +
                                                  errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }

                            }

                            if (item.TransactionDate != null)
                            {
                                try
                                {
                                    long? chTransactionDate = Convert.ToInt64(item.TransactionDate);
                                }
                                catch
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "TransactionDate is not valid";
                                    extraData = "TransactionDate";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" +
                                                  errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }

                            if (item.OwnerNationalId != null)
                            {
                                if (item.OwnerNationalId.GetType() != typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "OwnerNationalId type Is Not valid";
                                    extraData = "OwnerNationalId";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                                try
                                {
                                    long? chOwnerNationalId = Convert.ToInt64(item.OwnerNationalId);
                                }
                                catch
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "OwnerNationalId is not valid";
                                    extraData = "OwnerNationalId";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" +
                                                  errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (item.TransactionDate != null)
                            {
                                if (item.TransactionDate.GetType() == typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "TransactionDate type Is Not valid";
                                    extraData = "TransactionDate";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    var errorDetail = "";
                    foreach (var er in errorList)
                    {
                        errorDetail += er + ",";
                    }
                    if (!string.IsNullOrEmpty(errorDetail)) errorDetail = errorDetail.Remove(errorDetail.Length - 1);

                    jsonDataOutput += @"{""trackingNumber"": """ + item.TrackingNumber + @""",""transactionId"":""" + 0 + @""",""status"":""" + 0;
                    jsonDataOutput += @""",""errors"":[" + errorDetail + @"],""registrationDate"":""" + DateHelpers.DateTimeToTimestamp(DateTime.Now) + @"""},";
                }
                if (transferParams.Count > 0) jsonDataOutput = jsonDataOutput.Remove(jsonDataOutput.Length - 1);
                jsonDataOutput += "]";
                response = ((JArray)JsonConvert.DeserializeObject(jsonDataOutput)).Values<JObject>();
                if (!isValid) return Ok(response);
                //----------------------------------------------- Control Mandatory Field
                jsonDataOutput = "[";
                foreach (var item in transferParams)
                {
                    string errorDescription = null;
                    string extraData = null;
                    int? errorCode = null;
                    var errorList = new List<string>();

                    #region Validation
                    {
                        #region Check Null Variable
                        {
                            if (item.SourceIban == null)
                            {
                                isValid = false;
                                errorCode = 1;
                                errorDescription = "sourceIban Is Null";
                                extraData = "sourceIban";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }



                            if (item.TransferType == null)
                            {
                                isValid = false;
                                errorCode = 1;
                                errorDescription = "transferType Is Null";
                                extraData = "transferType";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (item.TrackingNumber == null)
                            {
                                isValid = false;
                                errorCode = 1;
                                errorDescription = "trackingNumber Is Null";
                                extraData = "trackingNumber";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (item.Amount == null)
                            {
                                isValid = false;
                                errorCode = 1;
                                errorDescription = "amount Is Null";
                                extraData = "amount";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (item.DestinationIban == null)
                            {
                                isValid = false;
                                errorCode = 1;
                                errorDescription = "destinationIban Is Null";
                                extraData = "destinationIban";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (item.OwnerName == null)
                            {
                                isValid = false;
                                errorCode = 1;
                                errorDescription = "ownerName Is Null";
                                extraData = "ownerName";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (item.OwnerNationalId == null)
                            {
                                isValid = false;
                                errorCode = 1;
                                errorDescription = "ownerNationalId Is Null";
                                extraData = "ownerNationalId";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }
                        }
                        #endregion

                        #region Validation
                        {
                            if (item.SourceIban != null && item.SourceIban.ToString().Length != 26)
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "sourceIban length is not allowed";
                                extraData = "sourceIban";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (item.TrackingNumber != null && item.TrackingNumber.ToString().Length > 64)
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "trackingNumber length is not allowed";
                                extraData = "trackingNumber";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }
                            if (item.ReferenceNumber != null)
                            {
                                if (item.ReferenceNumber.ToString().Length > 35)
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "referenceNumber length is not allowed";
                                    extraData = "referenceNumber";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }


                            if (Convert.ToDecimal(item.Amount) <= 0 || item.Amount.ToString().Length > 15)
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "amount value is not valid";
                                extraData = "amount";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (item.DestinationIban != null && item.DestinationIban.ToString().Length != 26)
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "destinationIban length is not allowed";
                                extraData = "destinationIban";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (item.OwnerName != null && item.OwnerName.ToString().Length > 64)
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "ownerName length is not allowed";
                                extraData = "ownerName";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (item.OwnerNationalId != null && item.OwnerNationalId.ToString().Length > 16)
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "ownerNationalId length is not allowed";
                                extraData = "ownerNationalId";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (item.Description != null)
                            {
                                if (item.Description.ToString().Length > 250)
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "description length is not allowed";
                                    extraData = "description";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }


                            //if (item.SourceIban != null && (item.SourceIban.ToString() != "IR220660000000103408054007" && item.SourceIban.ToString() != "IR280660000000202995422005" && item.SourceIban.ToString() != "IR520660000000205114864007"))
                            //{
                            //    isValid = false;
                            //    errorCode = 13;
                            //    errorDescription = "sourceIban is not valid";
                            //    extraData = "SourceIban-" + item.SourceIban;

                            //    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            //}

                            #region CheckIbans
                            {
                                //----------------------------------- بررسی شبای مبدا
                                //----------------------------------------------- تبدیل شماره شبا مبدا به شماره حساب
                                var ibanToDepositRequestBean = new ibanToDepositRequestBean();
                                string sourceDepositNumber = null;
                                if (item.SourceIban != null)
                                {
                                    ibanToDepositRequestBean.iban = item.SourceIban.ToString();
                                }

                                if (item.SourceIban != null && isValid)
                                {
                                    try
                                    {
                                        var convertIbanToDepositNumberResponse = await _remoteService.YaghutPaymentRemoteService.ConvertIbanToDepositNumber(ibanToDepositRequestBean);
                                        sourceDepositNumber = convertIbanToDepositNumberResponse.@return;
                                    }
                                    catch (Exception e)
                                    {
                                        isValid = false;
                                        errorCode = 13;
                                        errorDescription = "sourceIban is not valid";
                                        extraData = "SourceIban-" + item.SourceIban;

                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                }

                                if (isValid)
                                {
                                    //----------------------------------------------- دریافت شماره مشتری حساب مبدا با شماره حساب
                                    var depositCustomerRequestBean = new depositCustomerRequestBean();
                                    depositCustomerRequestBean.depositNumber = sourceDepositNumber;
                                    var getDepositCustomerResponse =
                                        await _remoteService.YaghutPaymentRemoteService.GetDepositCustomer(depositCustomerRequestBean);
                                    var openerCustomerCif = getDepositCustomerResponse.@return.openerCustomerCif;
                                    //-----------------------------------------------
                                    var requestBean = new depositSearchRequestBean();
                                    requestBean.cif = openerCustomerCif;
                                    string[] arraySourceDepositNumber = new[] { sourceDepositNumber };
                                    requestBean.depositNumbers = arraySourceDepositNumber;
                                    var getDepositsResponse =
                                        await _remoteService.YaghutPaymentRemoteService.GetDeposits(requestBean);
                                    if (getDepositsResponse.@return.totalRecord == 0)
                                    {
                                        isValid = false;
                                        errorCode = 5;
                                        errorDescription = "sourceIban is not found";
                                        extraData = "SourceIban";

                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" +
                                                      errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                    else
                                    {
                                        //if (item.Amount > getDepositsResponse.@return.depositBeans[0].balance)
                                        //{
                                        //    isValid = false;
                                        //    errorCode = 12;
                                        //    errorDescription = "Balance is not enough";
                                        //    extraData = "SourceIban Balance";

                                        //    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" +
                                        //                  errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                        //}

                                        if (getDepositsResponse.@return.depositBeans[0].depositStatus != depositStatusWS.OPEN)
                                        {
                                            isValid = false;
                                            errorCode = 12;
                                            errorDescription = "Deposit Status is not Open";
                                            extraData = item.SourceIban.ToString();

                                            errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" +
                                                          errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                        }
                                        if (!getDepositsResponse.@return.depositBeans[0].depositTitle.Contains("حساب کوتاه مدت پرداخت يار"))
                                        {
                                            isValid = false;
                                            errorCode = 13;
                                            errorDescription = "sourceIban is not valid";
                                            extraData = "SourceIban-" + item.SourceIban;

                                            errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");

                                        }
                                    }
                                }

                                //----------------------------------- بررسی شبای مقصد
                                if (item.DestinationIban != null)
                                {
                                    try
                                    {
                                        ibanInformationRequestBean ibanInfoRequestBean =
                                            new ibanInformationRequestBean();
                                        ibanInfoRequestBean.iban = item.DestinationIban.ToString();
                                        getIbanInformationResponse getIbanInformationResponse =
                                            await _remoteService.YaghutPaymentRemoteService.GetIbanInfo(
                                                ibanInfoRequestBean);
                                        string bankName = getIbanInformationResponse.@return.bankName;
                                        if (string.IsNullOrEmpty(bankName))
                                        {
                                            isValid = false;
                                            errorCode = 6;
                                            errorDescription = "destinationIban is not valid";
                                            extraData = "destinationIban-" + item.DestinationIban;
                                            errorList.Add(@"{""errorCode"":""" + errorCode +
                                                          @""",""errorDescription"":""" + errorDescription +
                                                          @""",""extraData"":""" + extraData + @"""}");
                                        }
                                    }
                                    catch (Exception )
                                    {
                                        isValid = false;
                                        errorCode = 5;
                                        errorDescription = "destinationIban is not valid";
                                        extraData = "destinationIban";
                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" +
                                                      errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion

                    var errorDetail = "";
                    foreach (var er in errorList)
                    {
                        errorDetail += er + ",";
                    }
                    if (!string.IsNullOrEmpty(errorDetail)) errorDetail = errorDetail.Remove(errorDetail.Length - 1);

                    jsonDataOutput += @"{""trackingNumber"": """ + item.TrackingNumber + @""",""transactionId"":""" + 0 + @""",""status"":""" + 0;
                    jsonDataOutput += @""",""errors"":[" + errorDetail + @"],""registrationDate"":""" + DateHelpers.DateTimeToTimestamp(DateTime.Now) + @"""},";
                }
                if (transferParams.Count > 0) jsonDataOutput = jsonDataOutput.Remove(jsonDataOutput.Length - 1);
                jsonDataOutput += "]";
                response = ((JArray)JsonConvert.DeserializeObject(jsonDataOutput)).Values<JObject>();
                if (!isValid) return Ok(response);
                //----------------------------------------------- With Out Error
                jsonDataOutput = "[";
                foreach (var item in transferParams)
                {
                    var transactions = new Transactions();
                    transactions.RegistrationDate = DateHelpers.DateTimeToTimestamp(DateTime.Now);
                    transactions.SourceIban = item.SourceIban.ToString();
                    if (_service.TransferType.IsExists(Convert.ToInt32(item.TransferType)))
                        transactions.TransferTypeId = _service.TransferType.Find(Convert.ToInt32(item.TransferType)).Id;

                    if (item.TransactionDate != null)
                        transactions.TransactionDate = Convert.ToInt64(item.TransactionDate);
                    else
                        transactions.TransactionDate = 0;
                    transactions.TrackingNumber = item.TrackingNumber.ToString();
                    transactions.ReferenceNumber = item.ReferenceNumber != null ? item.ReferenceNumber.ToString() : "";
                    transactions.Amount = Convert.ToDecimal(item.Amount);
                    transactions.DestinationIban = item.DestinationIban.ToString();
                    transactions.OwnerName = item.OwnerName.ToString();
                    transactions.OwnerNationalId = item.OwnerNationalId.ToString();
                    transactions.Description = item.Description != null ? item.Description.ToString() : null;
                    transactions.TransferStatusId = 0;
                    transactions.LastUpdate = DateHelpers.DateTimeToTimestamp(DateTime.Now);

                    _service.Transactions.CreateTransaction(transactions);

                    jsonDataOutput += @"{""trackingNumber"": """ + item.TrackingNumber + @""",""transactionId"":""" + transactions.Id + @""",""status"":""" + _service.TransferStatus.Find(transactions.TransferStatusId).Value;
                    jsonDataOutput += @""",""errors"":[],""registrationDate"":""" + transactions.RegistrationDate + @"""},";
                }
                if (transferParams.Count > 0) jsonDataOutput = jsonDataOutput.Remove(jsonDataOutput.Length - 1);
                jsonDataOutput += "]";
                response = ((JArray)JsonConvert.DeserializeObject(jsonDataOutput)).Values<JObject>();
                await RequestProcess();
                return Ok(response);
            }
            else
            {
                return Unauthorized(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.Unauthorized, Message = "دسترسی غیر مجاز", Value = new { }, Error = new { } });
            }
        }

        [HttpPost("RequestProcess")]
        public async Task<IActionResult> RequestProcess()
        {
            List<Transactions> transactions = _service.Transactions.GetTransactionsForTransfer();
            foreach (Transactions item in transactions)
            {
                string sourceDepositNumber = "", openerCustomerCif = "";
                try
                {
                    if (item.TransactionDate <= DateHelpers.DateTimeToTimestamp(DateTime.Now))
                    {
                        //------------------------------------------- تبدیل شماره شبا مبدا به شماره حساب
                        ibanToDepositRequestBean ibanToDepositRequestBean = new ibanToDepositRequestBean();
                        ibanToDepositRequestBean.iban = item.SourceIban;
                        convertIbanToDepositNumberResponse convertIbanToDepositNumberResponse = await _remoteService.YaghutPaymentRemoteService.ConvertIbanToDepositNumber(ibanToDepositRequestBean);
                        sourceDepositNumber = convertIbanToDepositNumberResponse.@return;
                        //------------------------------------------- دریافت شماره مشتری حساب مبدا با شماره حساب
                        depositCustomerRequestBean depositCustomerRequestBean = new depositCustomerRequestBean();
                        depositCustomerRequestBean.depositNumber = sourceDepositNumber;
                        getDepositCustomerResponse getDepositCustomerResponse = await _remoteService.YaghutPaymentRemoteService.GetDepositCustomer(depositCustomerRequestBean);
                        openerCustomerCif = getDepositCustomerResponse.@return.openerCustomerCif;

                        achNormalTransferRequestBean requestBean = new achNormalTransferRequestBean();
                        requestBean.additionalDocumentDesc = "توضیحات اضافی";
                        requestBean.amount = item.Amount;
                        requestBean.cif = openerCustomerCif;
                        requestBean.description = item.Description;
                        requestBean.factorNumber = item.Id.ToString();
                        requestBean.ibanNumber = item.DestinationIban;
                        requestBean.ownerName = item.OwnerName;
                        requestBean.payId = item.ReferenceNumber;
                        requestBean.reasonCode = "DRPA";
                        requestBean.reasonTitle = "تایده دیون";
                        requestBean.sourceDepositNumber = sourceDepositNumber;
                        requestBean.transferDescription = "یاداشت";



                        //if (item.Amount > 0 && item.Amount <= 500000000)
                        //{
                        //-------------------------------------------------------------------------پایا
                        achNormalTransferResponse achNormalTransferResponse = await _remoteService.YaghutPaymentRemoteService.AchNormalTransfer(requestBean, item.ReferenceNumber);

                        string currency = achNormalTransferResponse.@return.currency;
                        string referenceId = achNormalTransferResponse.@return.referenceId;
                        string transactionStatus = achNormalTransferResponse.@return.transactionStatus.ToString();
                        string transferDescription = achNormalTransferResponse.@return.transferDescription;
                        string transferStatus = achNormalTransferResponse.@return.transferStatus.ToString();

                        int transferStatusId = 0;
                        if (transactionStatus == "ACCEPTED")
                        {
                            transferStatusId = 1;
                        }
                        else
                        {
                            transferStatusId = 0;
                        }

                        _service.Transactions.UpdateTransferValue(item.Id, currency, referenceId, transactionStatus, transferDescription, transferStatus, transferStatusId);
                        //}
                        //else
                        //{
                        //    //-------------------------------------------------------------------------  ساتنا
                        //    rtgsNormalTransferRequestBean rtgsRequestBean = new rtgsNormalTransferRequestBean();
                        //    //rtgsNormalTransferResponse rtgsNormalTransferResponse = await _remoteService.YaghutPaymentRemoteService.RtgsNormalTransfer(rtgsRequestBean);
                        //    return null;
                        //}
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return Ok();
        }

        [HttpPost("Inquiry")]
        public async Task<IActionResult> Inquiry([FromHeader] string Authorization, [FromBody] object _transferParams)
        {
            try
            {

                var transferRequest = ((JObject)JsonConvert.DeserializeObject(_transferParams.ToString()));
                List<string> properties = new List<string> { "sourceIbans", "transferType", "fromTransferDate", "toTransferDate", "fromRegistrationDate", "toRegistrationDate", "trackingNumbers", "referenceNumbers", "transactionIds", "status", "fromAmount", "toAmount", "destinationIbans" };
                string key = null;
                string value = null;
                var isValid = true;
                var jsonDataOutput = "[";

                string errorDescription = null;
                string extraData = null;
                int? errorCode = null;
                var errorList = new List<string>();

                #region Validation
                {
                    #region Check Parameter Count , Data Type And Spelling Mistakes 
                    {
                        if (transferRequest.Count > 13)
                        {
                            isValid = false;
                            errorCode = 0;
                            errorDescription = "";
                            extraData = "The number of parameters is not valid";

                            errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                        }

                        var child = (JObject)transferRequest.First.Parent;
                        foreach (var ch in child)
                        {
                            key = ch.Key;
                            if (string.IsNullOrEmpty(properties.Find(a => a == key)))
                            {
                                isValid = false;
                                errorCode = 0;
                                errorDescription = "";
                                extraData = $"{key} is not valid parameter";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }
                        }

                        var propertyDetails = new Dictionary<string, object>();
                        propertyDetails.Add(key, value);
                        foreach (var kDetail in propertyDetails)
                        {

                        }
                    }
                    #endregion
                }
                #endregion

                var errorDetail = "";
                foreach (var er in errorList)
                {
                    errorDetail += er + ",";
                }
                if (!string.IsNullOrEmpty(errorDetail)) errorDetail = errorDetail.Remove(errorDetail.Length - 1);

                jsonDataOutput += @"{""trackingNumber"": """ + transferRequest["trackingNumber"] + @""",""transactionId"":""" + 0 + @""",""status"":""" + 0;
                jsonDataOutput += @""",""errors"":[" + errorDetail + @"],""registrationDate"":""" + DateHelpers.DateTimeToTimestamp(DateTime.Now) + @"""},";
                if (transferRequest != null) jsonDataOutput = jsonDataOutput.Remove(jsonDataOutput.Length - 1);
                jsonDataOutput += "]";
                var responsee = ((JArray)JsonConvert.DeserializeObject(jsonDataOutput)).Values<JObject>();
                if (!isValid) return Ok(responsee);














                InquiryParams inquiryParams = JsonConvert.DeserializeObject<InquiryParams>(_transferParams.ToString());



                var pass = _appSettings.UserName + ":" + _appSettings.Password;
                var encodedStr = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(pass));
                if (Authorization == encodedStr)
                {
                    if (inquiryParams.sourceIbans != null)
                    {
                        if (inquiryParams.sourceIbans.Count <= 0 && inquiryParams.trackingNumbers.Count <= 0 && inquiryParams.referenceNumbers.Count <= 0 && inquiryParams.transactionIds.Count <= 0)
                        {
                            errorDetail = @"{""turnovers"":[],""errors"":[]}";

                            var ErrResponse = JsonConvert.DeserializeObject(errorDetail);
                            return Ok(ErrResponse);
                        }
                    }


                    errorDescription = null;
                    extraData = null;
                    errorCode = null;
                    errorList = new List<string>();
                    isValid = true;
                    #region Validation
                    {
                        #region Check Length 
                        {
                            if (inquiryParams.sourceIbans != null && inquiryParams.sourceIbans.Count > 0 && inquiryParams.sourceIbans.Count > 250)
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "Iban Count Is Out Of Range";
                                extraData = "Iban";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (inquiryParams.trackingNumbers != null && inquiryParams.trackingNumbers.Count > 0 && inquiryParams.trackingNumbers.Count > 250)
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "trackingNumbers Count Is Out Of Range";
                                extraData = "TrackingNumbers";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (inquiryParams.referenceNumbers != null && inquiryParams.referenceNumbers.Count > 0 && inquiryParams.referenceNumbers.Count > 250)
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "referenceNumbers Count Is Out Of Range";
                                extraData = "ReferenceNumbers";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (inquiryParams.transactionIds != null && inquiryParams.transactionIds.Count > 0 && inquiryParams.transactionIds.Count > 250)
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "transactionIds Count Is Out Of Range";
                                extraData = "transactionIds";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (inquiryParams.fromAmount != null && (Convert.ToDecimal(inquiryParams.fromAmount) <= 0 || inquiryParams.fromAmount.ToString().Length > 15))
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "fromAmount Is Not Valid";
                                extraData = "fromAmount";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (inquiryParams.toAmount != null && (Convert.ToDecimal(inquiryParams.toAmount) <= 0 || inquiryParams.toAmount.ToString().Length > 15))
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "toAmount Is Not Valid";
                                extraData = "toAmount";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (inquiryParams.destinationIbans != null && inquiryParams.destinationIbans.Count > 0 && inquiryParams.destinationIbans.Count > 250)
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "destinationIbans Count Is Out Of Range";
                                extraData = "destinationIbans";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (!isValid)
                            {
                                errorDetail = "";
                                errorDetail = @"{""turnovers"":[],""errors"":[";
                                if (errorList.Count > 0)
                                {
                                    foreach (string er in errorList)
                                    {
                                        errorDetail += er + ",";
                                    }

                                    if (!string.IsNullOrEmpty(errorDetail))
                                        errorDetail = errorDetail.Remove(errorDetail.Length - 1);
                                }

                                errorDetail += "]}";
                                var errorResponse = JsonConvert.DeserializeObject(errorDetail);
                                return Ok(errorResponse);
                            }
                        }
                        #endregion

                        #region Bad Request Validation
                        {
                            if (inquiryParams.sourceIbans != null && inquiryParams.sourceIbans.Count > 0)
                            {
                                foreach (var item in inquiryParams.sourceIbans)
                                {
                                    if (item.GetType() != typeof(string))
                                    {
                                        isValid = false;
                                        errorCode = 5;
                                        errorDescription = "sourceIbans type Is Not valid";
                                        extraData = "sourceIbans";
                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                    if (item.ToString().Substring(0, 2) != "IR")
                                    {
                                        isValid = false;
                                        errorCode = 5;
                                        errorDescription = "SourceIban type Is Not valid";
                                        extraData = "SourceIban";
                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                }
                            }
                            if (inquiryParams.destinationIbans != null && inquiryParams.destinationIbans.Count > 0)
                            {
                                foreach (var item in inquiryParams.destinationIbans)
                                {
                                    if (item.GetType() != typeof(string))
                                    {
                                        isValid = false;
                                        errorCode = 5;
                                        errorDescription = "destinationIbans type Is Not valid";
                                        extraData = "destinationIbans";
                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                }
                            }
                            if (inquiryParams.trackingNumbers != null && inquiryParams.trackingNumbers.Count > 0)
                            {
                                foreach (var item in inquiryParams.trackingNumbers)
                                {
                                    if (item.GetType() != typeof(string))
                                    {
                                        isValid = false;
                                        errorCode = 5;
                                        errorDescription = "trackingNumbers type Is Not valid";
                                        extraData = "trackingNumbers";
                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                }
                            }
                            if (inquiryParams.referenceNumbers != null && inquiryParams.referenceNumbers.Count > 0)
                            {
                                foreach (var item in inquiryParams.referenceNumbers)
                                {
                                    if (item.GetType() != typeof(string))
                                    {
                                        isValid = false;
                                        errorCode = 5;
                                        errorDescription = "referenceNumbers type Is Not valid";
                                        extraData = "referenceNumbers";
                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                }
                            }
                            if (inquiryParams.transactionIds != null && inquiryParams.transactionIds.Count > 0)
                            {
                                foreach (var item in inquiryParams.transactionIds)
                                {
                                    if (item.GetType() != typeof(string))
                                    {
                                        isValid = false;
                                        errorCode = 5;
                                        errorDescription = "transactionIds type Is Not valid";
                                        extraData = "transactionIds";
                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                }
                            }
                            if (inquiryParams.fromRegistrationDate != null)
                            {
                                if (inquiryParams.fromRegistrationDate.GetType() == typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "fromRegistrationDate type Is Not valid";
                                    extraData = "fromRegistrationDate";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (inquiryParams.fromTransferDate != null)
                            {
                                if (inquiryParams.fromTransferDate.GetType() == typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "fromTransferDate type Is Not valid";
                                    extraData = "fromTransferDate";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (inquiryParams.toRegistrationDate != null)
                            {
                                if (inquiryParams.toRegistrationDate.GetType() == typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "toRegistrationDate type Is Not valid";
                                    extraData = "toRegistrationDate";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (inquiryParams.toTransferDate != null)
                            {
                                if (inquiryParams.toTransferDate.GetType() == typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "toTransferDate type Is Not valid";
                                    extraData = "toTransferDate";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (inquiryParams.fromAmount != null)
                            {
                                if (inquiryParams.fromAmount.GetType() == typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "fromAmount type Is Not valid";
                                    extraData = "fromAmount";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (inquiryParams.toAmount != null)
                            {
                                if (inquiryParams.toAmount.GetType() == typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "toAmount type Is Not valid";
                                    extraData = "toAmount";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (!string.IsNullOrEmpty(inquiryParams.transferType.ToString()))
                            {
                                if (!_service.TransferType.IsExists(inquiryParams.transferType))
                                {
                                    isValid = false;
                                    errorCode = null;
                                    errorDescription = "transferType Is Not Valid";
                                    extraData = "TransferType";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }

                            if (!string.IsNullOrEmpty(inquiryParams.status.ToString()))
                            {
                                if (!_service.TransferStatus.IsExists(inquiryParams.status))
                                {
                                    isValid = false;
                                    errorCode = null;
                                    errorDescription = "status Is Not Valid";
                                    extraData = "Status";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }

                            if (!isValid)
                            {
                                errorDetail = "";
                                errorDetail = @"{""turnovers"":[],""errors"":[";
                                if (errorList.Count > 0)
                                {
                                    foreach (string er in errorList)
                                    {
                                        errorDetail += er + ",";
                                    }

                                    if (!string.IsNullOrEmpty(errorDetail))
                                        errorDetail = errorDetail.Remove(errorDetail.Length - 1);
                                }

                                errorDetail += "]}";
                                var errorResponse = JsonConvert.DeserializeObject(errorDetail);
                                return Ok(errorResponse);
                            }
                        }
                        #endregion
                    }
                    #endregion

                    await AchTransferReport();

                    string sourceIban = "", transferType = "", ftransactionDate = "", ltransactionDate = "", fregistrationDate = "", lregistrationDate = "", trackingNumber = "", referenceNumber = "", id = "", transferStatusId = "", famount = "", lamount = "", destinationIban = "";
                    #region Filters
                    {
                        if (inquiryParams.sourceIbans != null && inquiryParams.sourceIbans.Count > 0)
                        {
                            if (inquiryParams.sourceIbans.Count == 1)
                            {
                                sourceIban = inquiryParams.sourceIbans[0].ToString();
                            }
                            else
                            {
                                //sourceIban = " AND ( SourceIban IN('IR620660000000200009690001','IR890660000000200422593004' ) ) ";
                                foreach (var item in inquiryParams.sourceIbans)
                                {
                                    sourceIban += item + ",";
                                }
                                sourceIban = sourceIban.Remove(sourceIban.Length - 1);
                            }
                        }

                        if (!string.IsNullOrEmpty(inquiryParams.transferType.ToString()))
                        {
                            if (_service.TransferType.IsExists(inquiryParams.transferType))
                            {
                                transferType = _service.TransferType.Find(inquiryParams.transferType).Id.ToString();
                            }
                            else
                            {
                                transferType = inquiryParams.transferType.ToString();
                            }
                        }

                        if (inquiryParams.fromTransferDate != null && Convert.ToInt64(inquiryParams.fromTransferDate) > 0)
                        {
                            ftransactionDate = inquiryParams.fromTransferDate.ToString();
                        }
                        if (inquiryParams.toTransferDate != null && Convert.ToInt64(inquiryParams.toTransferDate) > 0)
                        {
                            ltransactionDate = inquiryParams.toTransferDate.ToString();
                        }

                        if (inquiryParams.fromRegistrationDate != null && Convert.ToInt64(inquiryParams.fromRegistrationDate) > 0)
                        {
                            fregistrationDate = inquiryParams.fromRegistrationDate.ToString();
                        }
                        if (inquiryParams.toRegistrationDate != null && Convert.ToInt64(inquiryParams.toRegistrationDate) > 0)
                        {
                            lregistrationDate = inquiryParams.toRegistrationDate.ToString();
                        }

                        if (inquiryParams.trackingNumbers != null && inquiryParams.trackingNumbers.Count > 0)
                        {
                            if (inquiryParams.trackingNumbers.Count == 1)
                            {
                                trackingNumber = inquiryParams.trackingNumbers[0].ToString();
                            }
                            else
                            {
                                //" AND ( TrackingNumber IN('13970511PF010000001','13970511PF010000002','13970511PF010000003','13970511PF010000004' ) ) ";    
                                foreach (var item in inquiryParams.trackingNumbers)
                                {
                                    trackingNumber += item + ",";
                                }
                                trackingNumber = trackingNumber.Remove(trackingNumber.Length - 1);
                            }
                        }

                        if (inquiryParams.referenceNumbers != null && inquiryParams.referenceNumbers.Count > 0)
                        {
                            if (inquiryParams.referenceNumbers.Count == 1)
                            {
                                referenceNumber = inquiryParams.referenceNumbers[0].ToString();
                            }
                            else
                            {
                                //" AND ( ReferenceNumber IN('13970511PF010000001','13970511PF010000002','13970511PF010000003','13970511PF010000004' ) ) ";    
                                foreach (var item in inquiryParams.referenceNumbers)
                                {
                                    referenceNumber += item + ",";
                                }
                                referenceNumber = referenceNumber.Remove(referenceNumber.Length - 1);
                            }
                        }

                        if (inquiryParams.transactionIds != null && inquiryParams.transactionIds.Count > 0)
                        {
                            if (inquiryParams.transactionIds.Count == 1)
                            {
                                id = inquiryParams.transactionIds[0].ToString();
                            }
                            else
                            {
                                //" AND ( Id IN('32','33','34','35')) ";
                                foreach (var item in inquiryParams.transactionIds)
                                {
                                    id += item + ",";
                                }
                                id = id.Remove(id.Length - 1);
                            }
                        }

                        if (!string.IsNullOrEmpty(inquiryParams.status.ToString()))
                        {
                            if (_service.TransferStatus.IsExists(inquiryParams.status))
                            {
                                transferStatusId = _service.TransferStatus.Find(inquiryParams.status).Id.ToString();
                            }
                            else
                            {
                                transferStatusId = inquiryParams.status.ToString();
                            }
                        }

                        if (inquiryParams.fromAmount != null)
                        {
                            famount = inquiryParams.fromAmount.ToString();
                        }
                        if (inquiryParams.toAmount != null)
                        {
                            lamount = inquiryParams.toAmount.ToString();
                        }

                        if (inquiryParams.destinationIbans != null && inquiryParams.destinationIbans.Count > 0)
                        {
                            if (inquiryParams.destinationIbans.Count == 1)
                            {
                                destinationIban = inquiryParams.destinationIbans[0].ToString();
                            }
                            else
                            {
                                //" AND ( DestinationIban IN('IR680600681570005667452001','IR680600681570005667452002')) ";
                                foreach (var item in inquiryParams.destinationIbans)
                                {
                                    destinationIban += item + ",";
                                }
                                destinationIban = destinationIban.Remove(destinationIban.Length - 1);
                            }
                        }

                    }
                    #endregion

                    var convertedInquiryParams = new ConvertedInquiryParams();
                    convertedInquiryParams.sourceIbans = sourceIban;
                    convertedInquiryParams.transferType = transferType;
                    convertedInquiryParams.ftransferDate = ftransactionDate;
                    convertedInquiryParams.ltransferDate = ltransactionDate;
                    convertedInquiryParams.fregistrationDate = fregistrationDate;
                    convertedInquiryParams.lregistrationDate = lregistrationDate;
                    convertedInquiryParams.trackingNumbers = trackingNumber;
                    convertedInquiryParams.referenceNumbers = referenceNumber;
                    convertedInquiryParams.transactionIds = id;
                    convertedInquiryParams.status = transferStatusId;
                    convertedInquiryParams.famount = famount;
                    convertedInquiryParams.lamount = lamount;
                    convertedInquiryParams.destinationIbans = destinationIban;

                    var transactions = _service.Transactions.TransferInquiry(convertedInquiryParams);
                    jsonDataOutput = "[";
                    foreach (var item in transactions)
                    {
                        jsonDataOutput += @"{""sourceIban"": """ + item.SourceIban + @""",""transferType"":""" + _service.TransferStatus.Find(item.TransferTypeId).Value + @""",""transactionDate"":""" + item.TransactionDate + @""",""trackingNumber"":""" + item.TrackingNumber + @""",""referenceNumber"":""" + item.ReferenceNumber + @""",""amount"":""" + item.Amount + @""",""destinationIban"":""" + item.DestinationIban + @""",""ownerName"":""" + item.OwnerName + @""",""ownerNationalId"":""" + item.OwnerNationalId + @""",""description"":""" + item.Description + @""",""transactionId"":""" + item.Id + @""",""externalTransactionId"":""" + null + @""",""status"":""" + _service.TransferStatus.Find(item.TransferStatusId).Value;
                        jsonDataOutput += @""",""errors"":[],""registrationDate"":""" + item.RegistrationDate + @""",""lastUpdate"":""" + item.LastUpdate + @"""},";
                    }
                    if (transactions.Count > 0) jsonDataOutput = jsonDataOutput.Remove(jsonDataOutput.Length - 1);
                    jsonDataOutput += "]";
                    var response = ((JArray)JsonConvert.DeserializeObject(jsonDataOutput)).Values<JObject>();
                    return Ok(response);
                }
                else
                {
                    return Unauthorized(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.Unauthorized, Message = "دسترسی غیر مجاز", Value = new { }, Error = new { } });
                }

            }
            catch (Exception ex)
            {
                var x = ex;
                return Ok(ex.ToString());
            }
        }

        [HttpPost("AchTransferReport")]
        public async Task<IActionResult> AchTransferReport()
        {
            List<Transactions> transactions = _service.Transactions.GetTranForUpdateTransferStatus();
            foreach (Transactions item in transactions)
            {
                string sourceDepositNumber = "", openerCustomerCif = "";
                try
                {
                    //----------------------------------------------- تبدیل شماره شبا مبدا به شماره حساب
                    ibanToDepositRequestBean ibanToDepositRequestBean = new ibanToDepositRequestBean();
                    ibanToDepositRequestBean.iban = item.SourceIban;
                    convertIbanToDepositNumberResponse convertIbanToDepositNumberResponse = await _remoteService.YaghutPaymentRemoteService.ConvertIbanToDepositNumber(ibanToDepositRequestBean);
                    sourceDepositNumber = convertIbanToDepositNumberResponse.@return;
                    //----------------------------------------------- دریافت شماره مشتری حساب مبدا با شماره حساب
                    depositCustomerRequestBean depositCustomerRequestBean = new depositCustomerRequestBean();
                    depositCustomerRequestBean.depositNumber = sourceDepositNumber;
                    getDepositCustomerResponse getDepositCustomerResponse = await _remoteService.YaghutPaymentRemoteService.GetDepositCustomer(depositCustomerRequestBean);
                    openerCustomerCif = getDepositCustomerResponse.@return.openerCustomerCif;
                    //-------------------------------------------
                    achTransferSearchRequestBean requestBean = new achTransferSearchRequestBean();
                    requestBean.cif = openerCustomerCif;
                    requestBean.sourceDepositIban = item.SourceIban;
                    requestBean.destinationIbanNumber = item.DestinationIban;
                    requestBean.referenceId = item.ReferenceId;
                    //requestBean.factorNumber = item.Id.ToString();

                    achTransferReportResponse achTransferReportResponse = await _remoteService.YaghutPaymentRemoteService.AchTransferReport(requestBean);

                    achTransferBean[] achTransferBeans = achTransferReportResponse.@return.achTransferBeans;
                    int transferStatusId = 1;

                    if (_service.TransferStatusWs.IsExists(achTransferBeans[0].status.ToString()))
                        transferStatusId = _service.TransferStatusWs.Find(achTransferBeans[0].status.ToString()).TransferStatusId;

                    _service.Transactions.UpdateTransferStatus(item.Id, item.ReferenceId, transferStatusId, achTransferBeans[0].status.ToString(), achTransferBeans[0].acceptable, achTransferBeans[0].cancelable, achTransferBeans[0].resumeable, achTransferBeans[0].suspendable);

                }
                catch (Exception )
                {

                }
            }

            return Ok();
        }

        [HttpPost("Login")]
        public IActionResult Login()
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "عدم اعتبار صفحه", Value = new { }, Error = new { ErrorMsg = ModelState } });
            }

            _remoteService.YaghutPaymentRemoteService.Login();
            return null;
        }

        [HttpPost("Authorize")]
        public IActionResult Authorize([FromHeader] string userName, [FromHeader] string password)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "صفحه نامعتبر است", Value = new { }, Error = new { ErrorMsg = ModelState } });
            }

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                if (_appSettings.UserName == userName && _appSettings.Password == password)
                {
                    string token = _service.Transactions.GenToken(userName, password).AccessToken;
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "ساخت توکن با موفقیت انجام شد", Value = new { Response = token }, Error = new { } });
                }
                else
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.Forbidden, Message = "نام کاربری و رمز عبور معتبر نمی باشد", Value = new { }, Error = new { } });
                }
            }
            else
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.Forbidden, Message = "نام کاربری و رمز عبور وارد نشده است", Value = new { }, Error = new { } });
            }
        }
    }
}
