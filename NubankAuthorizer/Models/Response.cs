using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NubankAuthorizer.Models
{
    public class Response : IEquatable<Response>
    {
        [JsonProperty("account")]
        public Account Account { get; set; }
        
        [JsonProperty("violations")]
        public List<Violations> Violations { get; set; }

        public static Response Generate(Account account, List<Violations> violationsList)
        {
            return new Response()
            {
                Account = account,
                Violations = violationsList
            };
        }
        
        public static Response Generate(Account account, Violations violation)
        {
            return new Response()
            {
                Account = account,
                Violations = new List<Violations>(){ violation }
            };
        }

        public bool Equals(Response other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other.Account != null && other.Account.ActiveCard != Account.ActiveCard)
                return false;
            if (other.Account != null && other.Account.AvailableLimit != Account.AvailableLimit)
                return false;

            if (other.Violations.Count != Violations.Count)
                return false;

            if (Violations.Except(other.Violations).Any())
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            Response other = obj as Response;
            if( other == null ) 
                return false;
            return Equals (other);    
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Account, Violations);
        }
    }
}