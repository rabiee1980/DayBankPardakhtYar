using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Core.Model.Base
{
    public class InquiryParams
    {
        public List<object> sourceIbans { get; set; }

        public int? transferType { get; set; }

        public object fromTransferDate { get; set; }

        public object toTransferDate { get; set; }

        public object fromRegistrationDate { get; set; }

        public object toRegistrationDate { get; set; }

        public List<object> trackingNumbers { get; set; }

        public List<object> referenceNumbers { get; set; }

        public List<object> transactionIds { get; set; }

        public int? status { get; set; }

        public object fromAmount { get; set; }

        public object toAmount { get; set; }

        public List<object> destinationIbans { get; set; }
    }

    public class StatementInquiryParams
    {
        public object iban { get; set; }

        public object fromDate { get; set; }

        public object toDate { get; set; }

        public object turnoverType { get; set; }

        public object fromAmount { get; set; }

        public object toAmount { get; set; }

        public object voucherDescription { get; set; }

        public object articleDescription { get; set; }

        public List<object> referenceNumbers { get; set; }

        public object offset { get; set; }

        public object pageSize { get; set; }
    }

    public class ConvertedInquiryParams
    {
        public string sourceIbans { get; set; }
        public string transferType { get; set; }
        public string ftransferDate { get; set; }
        public string ltransferDate { get; set; }
        public string fregistrationDate { get; set; }
        public string lregistrationDate { get; set; }
        public string trackingNumbers { get; set; }
        public string referenceNumbers { get; set; }
        public string transactionIds { get; set; }
        public string status { get; set; }
        public string famount { get; set; }
        public string lamount { get; set; }
        public string destinationIbans { get; set; }
    }
    public class ConvertedStatementInquiryParams
    {
        public string iban { get; set; }
        public string fdate { get; set; }        
        public string ldate { get; set; }        
        public string turnoverType { get; set; }
        public string amount { get; set; }        
        public string voucherDescription { get; set; }
        public string articleDescription { get; set; }
        public string referenceNumbers { get; set; }
        public string offset { get; set; }
        public string pageSize { get; set; }
    }
}
