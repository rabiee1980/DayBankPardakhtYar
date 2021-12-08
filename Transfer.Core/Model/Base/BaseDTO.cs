using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Core.Model.Base
{
    public class Meta
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public Meta(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    public class BaseDTO
    {
        public Meta Meta { get; set; }
        public Object Object { get; set; }

        public BaseDTO(int code, string message, object @object)
        {
            Meta = new Meta(code, message);
            Object = @object;
        }

        public BaseDTO(object @object)
        {
            Meta = new Meta(0, "عملیات با موفقیت انجام شد");
            Object = @object;
        }


    }

    public class BaseDTO<T>
    {
        public Meta Meta { get; set; }
        public T Object { get; set; }
        public BaseDTO() { }
    }
}
