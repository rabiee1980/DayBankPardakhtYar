using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Util;
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
    public class StatementController : ControllerBase
    {
        private readonly IServiceWrapper _service;
        private readonly IRemoteServiceWrapper _remoteService;
        private readonly AppSettings _appSettings;

        public StatementController(IServiceWrapper service, IRemoteServiceWrapper remoteService, IOptions<AppSettings> appSettings)
        {
            _service = service;
            _remoteService = remoteService;
            _appSettings = appSettings.Value;
        }

        [HttpPost("inquiry")]
        public async Task<IActionResult> Inquiry([FromHeader] string authorization, [FromBody] object _transferParams)
        {
            try
            {
                var transferRequest = ((JObject)JsonConvert.DeserializeObject(_transferParams.ToString()));
                List<string> properties = new List<string> { "iban", "fromDate", "toDate", "turnoverType", "fromAmount", "toAmount", "voucherDescription", "articleDescription", "referenceNumbers", "offset", "pageSize" };
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
                        if (transferRequest.Count > 11)
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
                StatementInquiryParams inquiryParams = JsonConvert.DeserializeObject<StatementInquiryParams>(_transferParams.ToString());
                string pass = _appSettings.UserName + ":" + _appSettings.Password;
                string encodedStr = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(pass));
                if (authorization == encodedStr)
                {
                    jsonDataOutput = "";
                    errorDescription = null;
                    extraData = null;
                    errorCode = null;
                    errorList = new List<string>();

                    isValid = true;
                    #region Validation
                    {
                        #region Bad Request Validation
                        {
                            if (inquiryParams.iban != null)
                            {
                                if (inquiryParams.iban.GetType() != typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "iban type Is Not valid";
                                    extraData = "iban";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                                if (inquiryParams.iban.ToString().Substring(0, 2) != "IR")
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "SourceIban type Is Not valid";
                                    extraData = "SourceIban";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (inquiryParams.referenceNumbers.Count > 0)
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
                            if (inquiryParams.turnoverType != null)
                            {
                                try
                                {
                                    int chTransferType = Convert.ToInt32(inquiryParams.turnoverType);
                                    if (inquiryParams.turnoverType.ToString() != "0" && inquiryParams.turnoverType.ToString() != "1")
                                    {
                                        isValid = false;
                                        errorCode = 6;
                                        errorDescription = "turnoverType Is Not Valid";
                                        extraData = "turnoverType-" + inquiryParams.turnoverType.ToString();

                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                }
                                catch
                                {
                                    isValid = false;
                                    errorCode = null;
                                    errorDescription = "turnoverType is not valid";
                                    extraData = "turnoverType";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" +
                                                  errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }

                            if (inquiryParams.fromDate != null)
                            {
                                if (inquiryParams.fromDate.GetType() == typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "fromDate type Is Not valid";
                                    extraData = "fromDate";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (inquiryParams.offset != null)
                            {
                                if (inquiryParams.offset.GetType() == typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "offset type Is Not valid";
                                    extraData = "offset";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (inquiryParams.pageSize != null)
                            {
                                if (inquiryParams.pageSize.GetType() == typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "pageSize type Is Not valid";
                                    extraData = "pageSize";
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

                            if (inquiryParams.toDate != null)
                            {
                                if (inquiryParams.toDate.GetType() == typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "toDate type Is Invalid";
                                    extraData = "toDate";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }


                            if (inquiryParams.voucherDescription != null)
                            {
                                if (inquiryParams.voucherDescription.GetType() != typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "voucherDescription type Is Not valid";
                                    extraData = "voucherDescription";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }
                            if (inquiryParams.articleDescription != null)
                            {
                                if (inquiryParams.articleDescription.GetType() != typeof(string))
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "articleDescription type Is Not valid";
                                    extraData = "articleDescription";
                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }


                            if (inquiryParams.fromDate != null)
                            {
                                try
                                {
                                    long? chfromDate = Convert.ToInt64(inquiryParams.fromDate);
                                    if (Convert.ToInt64(inquiryParams.fromDate) <= 0)
                                    {
                                        isValid = false;
                                        errorCode = null;
                                        errorDescription = "fromDate is not valid";
                                        extraData = "fromDate";

                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                }
                                catch
                                {
                                    isValid = false;
                                    errorCode = null;
                                    errorDescription = "fromDate is not valid";
                                    extraData = "fromDate";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" +
                                                  errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }

                            if (inquiryParams.toDate != null)
                            {
                                try
                                {
                                    long? chtoDate = Convert.ToInt64(inquiryParams.toDate);
                                    if (Convert.ToInt64(inquiryParams.toDate) <= 0)
                                    {
                                        isValid = false;
                                        errorCode = null;
                                        errorDescription = "toDate is not valid";
                                        extraData = "toDate";

                                        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                    }
                                }
                                catch
                                {
                                    isValid = false;
                                    errorCode = null;
                                    errorDescription = "toDate is not valid";
                                    extraData = "toDate";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" +
                                                  errorDescription + @""",""extraData"":""" + extraData + @"""}");
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
                                var response = JsonConvert.DeserializeObject(errorDetail);
                                return Ok(response);
                            }
                        }
                        #endregion
                        #region Check Null And Length Variable
                        {
                            if (inquiryParams.iban == null)
                            {
                                isValid = false;
                                errorCode = 1;
                                errorDescription = "Iban Is Null";
                                extraData = "Iban";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }
                            else
                            {
                                if (inquiryParams.iban.ToString().Length != 26)
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "Iban length is not allowed";
                                    extraData = "Iban";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                                //else
                                //{
                                //    if (inquiryParams.iban != null && (inquiryParams.iban.ToString() != "IR220660000000103408054007" && inquiryParams.iban.ToString() != "IR280660000000202995422005" && inquiryParams.iban.ToString() != "IR520660000000205114864007"))
                                //    {
                                //        isValid = false;
                                //        errorCode = 13;
                                //        errorDescription = "sourceIban is not valid";
                                //        extraData = "SourceIban-" + inquiryParams.iban;

                                //        errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                //    }
                                //}
                            }
                            if (inquiryParams.fromDate == null)
                            {
                                isValid = false;
                                errorCode = 1;
                                errorDescription = "fromDate Is Null";
                                extraData = "fromDate";
                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (inquiryParams.toDate == null)
                            {
                                isValid = false;
                                errorCode = 1;
                                errorDescription = "toDate Is Null";
                                extraData = "toDate";
                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (inquiryParams.offset == null)
                            {
                                isValid = false;
                                errorCode = 1;
                                errorDescription = "offset Is Null";
                                extraData = "offset";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }
                            else
                            {
                                if (Convert.ToDecimal(inquiryParams.offset) < 1 || inquiryParams.offset.ToString().Length > 6)
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "offset is not valid";
                                    extraData = "offset";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
                            }

                            if (inquiryParams.pageSize == null)
                            {
                                isValid = false;
                                errorCode = 1;
                                errorDescription = "pageSize Is Null";
                                extraData = "pageSize";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }
                            else
                            {
                                if (Convert.ToDecimal(inquiryParams.pageSize) > 1000 || inquiryParams.pageSize.ToString().Length > 4)
                                {
                                    isValid = false;
                                    errorCode = 5;
                                    errorDescription = "pageSize is not valid";
                                    extraData = "pageSize";

                                    errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                                }
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

                            if (inquiryParams.voucherDescription != null && (inquiryParams.voucherDescription.ToString().Length > 256))
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "voucherDescription Is Not Valid";
                                extraData = "voucherDescription";

                                errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");
                            }

                            if (inquiryParams.articleDescription != null && (inquiryParams.articleDescription.ToString().Length > 256))
                            {
                                isValid = false;
                                errorCode = 5;
                                errorDescription = "articleDescription Is Not Valid";
                                extraData = "articleDescription";

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
                                var response = JsonConvert.DeserializeObject(errorDetail);
                                return Ok(response);
                            }
                        }
                        #endregion


                    }
                    #endregion

                    double distanceDate = (DateHelpers.TimeStampToDateTime(Convert.ToInt64(inquiryParams.toDate)) - DateHelpers.TimeStampToDateTime(Convert.ToInt64(inquiryParams.fromDate))).TotalDays;
                    if (distanceDate <= 31)
                    {
                        string sourceDepositNumber = "", openerCustomerCif = "";
                        //----------------------------------------------- تبدیل شماره شبا مبدا به شماره حساب
                        ibanToDepositRequestBean ibanToDepositRequestBean = new ibanToDepositRequestBean();
                        ibanToDepositRequestBean.iban = inquiryParams.iban.ToString();
                        convertIbanToDepositNumberResponse convertIbanToDepositNumberResponse = await _remoteService.YaghutPaymentRemoteService.ConvertIbanToDepositNumber(ibanToDepositRequestBean);
                        sourceDepositNumber = convertIbanToDepositNumberResponse.@return;
                        //----------------------------------------------- دریافت شماره مشتری حساب مبدا با شماره حساب
                        depositCustomerRequestBean depositCustomerRequestBean = new depositCustomerRequestBean();
                        depositCustomerRequestBean.depositNumber = sourceDepositNumber;
                        getDepositCustomerResponse getDepositCustomerResponse = await _remoteService.YaghutPaymentRemoteService.GetDepositCustomer(depositCustomerRequestBean);
                        openerCustomerCif = getDepositCustomerResponse.@return.openerCustomerCif;
                        //-----------------------------------------------
                        var requestBean1 = new depositSearchRequestBean();
                        requestBean1.cif = openerCustomerCif;
                        string[] arraySourceDepositNumber = new[] { sourceDepositNumber };
                        requestBean1.depositNumbers = arraySourceDepositNumber;
                        var getDepositsResponse =
                            await _remoteService.YaghutPaymentRemoteService.GetDeposits(requestBean1);
                        if (!getDepositsResponse.@return.depositBeans[0].depositTitle.Contains("حساب کوتاه مدت پرداخت يار"))
                        {
                            isValid = false;
                            errorCode = 13;
                            errorDescription = "sourceIban is not valid";
                            extraData = "SourceIban-" + inquiryParams.iban;
                            errorList.Add(@"{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}");

                        }

                        //----------------------------------------------- دریافت صورتحساب از یاقوت                  
                        DateTime? LastTranDate = _service.AccountStatement.GetLastDate(inquiryParams.iban.ToString());
                        if (LastTranDate == null)
                        {
                            LastTranDate = DateTime.Now.AddDays(-30);
                        }
                        statementSearchRequestBean requestBean = new statementSearchRequestBean();
                        requestBean.cif = openerCustomerCif;
                        requestBean.depositNumber = sourceDepositNumber;
                        requestBean.fromDate = LastTranDate.Value.AddSeconds(1);
                        requestBean.fromDateSpecified = true;
                        requestBean.toDate = DateTime.Now;
                        requestBean.toDateSpecified = true;
                        getStatementResponse getStatementResponse = await _remoteService.YaghutPaymentRemoteService.GetStatement(requestBean);
                        statementBean[] statementBean = getStatementResponse.@return.statementBeans;
                        //----------------------------------------------- اضافه به لیست صورتحساب داخلی
                        if (statementBean != null)
                            foreach (statementBean item in statementBean)
                            {
                                AccountStatement accountStatement = new AccountStatement();
                                accountStatement.Iban = inquiryParams.iban.ToString();
                                accountStatement.StatementId = item.statementId;
                                accountStatement.Date = item.date;
                                accountStatement.DateSpecified = item.dateSpecified;
                                if (item.transferAmount < 0)
                                    accountStatement.TurnoverTypeId = 0;
                                else
                                    accountStatement.TurnoverTypeId = 1;
                                accountStatement.TransferAmount = Math.Abs(item.transferAmount);
                                accountStatement.TransferAmountSpecified = item.transferAmountSpecified;
                                accountStatement.Balance = item.balance;
                                accountStatement.BalanceSpecified = item.balanceSpecified;
                                accountStatement.Description = item.description;
                                accountStatement.PaymentId = item.paymentId;
                                accountStatement.ReferenceNumber = item.referenceNumber;
                                accountStatement.RegistrationNumber = item.registrationNumber;
                                accountStatement.RegistrationNumberSpecified = item.registrationNumberSpecified;
                                accountStatement.Serial = item.serial;
                                accountStatement.SerialSpecified = item.serialSpecified;
                                accountStatement.SerialNumber = item.serialNumber;
                                accountStatement.BranchCode = item.branchCode;
                                accountStatement.BranchName = item.branchName;
                                accountStatement.AgentBranchCode = item.agentBranchCode;
                                accountStatement.AgentBranchName = item.agentBranchName;

                                _service.AccountStatement.CreateAccountStatement(accountStatement);

                            }
                        //----------------------------------------------- دریافت صورتحساب داخلی
                        string iban = "", fdate = "", ldate = "", turnoverType = "", amount = "", voucherDescription = "", articleDescription = "", referenceNumbers = "", offset = "", pageSize = "";
                        #region Filters
                        {
                            if (inquiryParams.iban != null)
                            {
                                iban = inquiryParams.iban.ToString();
                            }

                            if (!string.IsNullOrEmpty(inquiryParams.fromDate.ToString()) && Convert.ToInt64(inquiryParams.fromDate) > 0 && !string.IsNullOrEmpty(inquiryParams.toDate.ToString()) && Convert.ToInt64(inquiryParams.toDate) > 0)
                            {
                                fdate = DateHelpers.TimeStampToDateTime(Convert.ToInt64(inquiryParams.fromDate)).ToString();
                                ldate = DateHelpers.TimeStampToDateTime(Convert.ToInt64(inquiryParams.toDate)).ToString();
                            }

                            if (inquiryParams.turnoverType != null)
                            {
                                turnoverType = " AND ( TurnoverTypeId = '" + inquiryParams.turnoverType + "' ) ";
                            }

                            if (inquiryParams.fromAmount != null && Convert.ToDecimal(inquiryParams.fromAmount) > 0 && inquiryParams.toAmount != null && Convert.ToDecimal(inquiryParams.toAmount) > 0)
                            {
                                amount = " AND ( TransferAmount >= '" + inquiryParams.fromAmount + "' AND TransferAmount <= '" + inquiryParams.toAmount + "' ) ";
                            }
                            if (inquiryParams.voucherDescription == null)
                            {
                                voucherDescription = "";
                            }
                            else
                            {
                                voucherDescription = inquiryParams.voucherDescription.ToString();
                            }
                            if (inquiryParams.articleDescription == null)
                            {
                                articleDescription = "";
                            }
                            else
                            {
                                articleDescription = inquiryParams.articleDescription.ToString();
                            }

                            if (inquiryParams.referenceNumbers.Count > 0)
                            {
                                if (inquiryParams.referenceNumbers.Count == 1)
                                {
                                    referenceNumbers = " AND ( ReferenceNumber IN('" + inquiryParams.referenceNumbers[0] + "') ) ";
                                }
                                else
                                {
                                    referenceNumbers = " AND ( ReferenceNumber IN( ";
                                    foreach (var item in inquiryParams.referenceNumbers)
                                    {
                                        referenceNumbers += " '" + item + "' ,";
                                    }
                                    referenceNumbers = referenceNumbers.Remove(referenceNumbers.Length - 1);
                                    referenceNumbers += "))";
                                }
                            }

                            if (inquiryParams.offset != null && Convert.ToDecimal(inquiryParams.offset) > 0)
                            {
                                offset = inquiryParams.offset.ToString();
                            }

                            if (inquiryParams.pageSize != null && Convert.ToDecimal(inquiryParams.pageSize) > 0)
                            {
                                pageSize = inquiryParams.pageSize.ToString();
                            }

                        }
                        #endregion

                        ConvertedStatementInquiryParams convertedStatementInquiryParams = new ConvertedStatementInquiryParams();
                        convertedStatementInquiryParams.iban = iban;
                        convertedStatementInquiryParams.fdate = fdate;
                        convertedStatementInquiryParams.ldate = ldate;
                        convertedStatementInquiryParams.turnoverType = turnoverType;
                        convertedStatementInquiryParams.amount = amount;
                        convertedStatementInquiryParams.voucherDescription = voucherDescription;
                        convertedStatementInquiryParams.articleDescription = articleDescription;
                        convertedStatementInquiryParams.referenceNumbers = referenceNumbers;
                        convertedStatementInquiryParams.offset = offset;
                        convertedStatementInquiryParams.pageSize = pageSize;

                        List<AccountStatement> accountStatementReport = _service.AccountStatement.StatementInquiry(convertedStatementInquiryParams);

                        jsonDataOutput = @"{""turnovers"":[";
                        foreach (AccountStatement item in accountStatementReport)
                        {
                            jsonDataOutput += @"{""voucherDate"": """ + item.Date + @""",""voucherDescription"":""" + item.Description + @""",""articleDescription"":""" + null + @""",""amount"":""" + item.TransferAmount + @""",""turnoverType"":""" + item.TurnoverTypeId + @""",""balance"":""" + item.Balance + @""",""voucherBranchCode"":""" + item.BranchCode + @""",""voucherBranchName"":""" + item.BranchName + @""",""referenceNumber"":""" + item.ReferenceNumber + @""",""voucherId"":""" + item.StatementId + @"""},";
                        }

                        if (accountStatementReport.Count > 0) jsonDataOutput = jsonDataOutput.Remove(jsonDataOutput.Length - 1);
                        jsonDataOutput += "],";

                        jsonDataOutput += @"""errors"":[]}";
                        var successResponse = JsonConvert.DeserializeObject(jsonDataOutput);

                        return Ok(successResponse);

                    }
                    else
                    {
                        jsonDataOutput = @"{""turnovers"":[],";
                        errorCode = 5;
                        errorDescription = "عدم همخوانی داده ارائه شده با قراردادهای سرویس،بازه تاریخ بیشتر از 31 روز";
                        extraData = "fromDate,toDate";
                        jsonDataOutput += @"""errors"":[{""errorCode"":""" + errorCode + @""",""errorDescription"":""" + errorDescription + @""",""extraData"":""" + extraData + @"""}]}";
                        var errorResponse = JsonConvert.DeserializeObject(jsonDataOutput);

                        return Ok(errorResponse);
                    }
                }
                else
                {
                    return Unauthorized(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.Unauthorized, Message = "دسترسی غیر مجاز", Value = new { }, Error = new { } });
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }
    }
}
