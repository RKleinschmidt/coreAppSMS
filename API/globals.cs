using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace coreAppSMS.Api
{
    public class Request
        {
            [JsonProperty("content")]
            public string Content { get; set; }

            [JsonProperty("to")]
            public List<string> To { get; set; } = new List<string>();

            public Request(string to, string msg)
            {
                Content = msg;
                To.Add(to);
            }

            public string ToJson()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public class Message
        {
            [JsonProperty("apiMessageId")]
            public string ApiMessageId { get; set; }

            [JsonProperty("accepted")]
            public bool Accepted { get; set; }

            [JsonProperty("to")]
            public string To { get; set; }

            [JsonProperty("errorCode")]
            public string ErrorCode { get; set; }

            [JsonProperty("errorDescription")]
            public string ErrorDescription { get; set; }

            [JsonProperty("error")]
            public object Error { get; set; }
        }

        public class SeedMessage
        {
            [JsonProperty("msg")]
            public string msg { get; set; }
        }

        public class Response 
        {
            [JsonProperty("messages")]
            public List<Message> Messages { get; set; }

            [JsonProperty("error")]
            public object Error { get; set; }

            public string Status
            {
                get
                {
                    if (Error != null)
                        return Error.ToString();
                    else if (Messages[0].Error != null)
                        return Messages[0].Error.ToString();
                    else
                        return (Messages[0].Accepted ? "Delivered" : "Failed");
                }
                set { }
            }
        }
}