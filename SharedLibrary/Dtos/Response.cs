using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
	public class Response<T> where T : class
	{
        public T Data { get; private set; }
		public int StatusCode { get;private set; }
        public ErrorDto Error { get; set; }
        public bool IsShow { get; set; }

		[JsonIgnore]
        public bool IsSuccesful { get; set; }


        public static Response<T> Success(T data,int statusCode)
		{
			return new Response<T> { Data = data, StatusCode = statusCode  ,IsSuccesful=true};
		}

		public static Response<T> Success(int statusCode)
		{
			return new Response<T> {StatusCode = statusCode ,Data=default , IsSuccesful = true };
		}

		public static Response<T> Fail(ErrorDto errorDto, int statusCode)
		{
			return new Response<T> { Error=errorDto, StatusCode = statusCode, IsSuccesful=false };
		}

		public static Response<T> Fail(int statusCode,bool isShow,string errorMessage)
		{
			var errorDto = new ErrorDto(errorMessage, isShow);

			return new Response<T> { StatusCode=statusCode, Error=errorDto , IsSuccesful = false };
		}
	}
}
