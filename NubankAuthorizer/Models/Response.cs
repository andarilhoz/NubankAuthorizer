using System;
using System.Collections.Generic;
using System.Linq;

namespace NubankAuthorizer.Models
{
    public class Response : IEquatable<Response>
    {
        public Account Account;
        public List<Violations> Violations;

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
            
            for (int i = 0; i < Violations.Count; i++)
            {
                if (Violations[i] != other.Violations[i])
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