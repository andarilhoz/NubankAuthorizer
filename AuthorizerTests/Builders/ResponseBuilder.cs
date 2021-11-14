using System.Collections.Generic;
using NubankAuthorizer.Models;

namespace AuthorizerTests.Builders
{
    public class ResponseBuilder
    {
        private Account account;
        private List<Violations> violations = new List<Violations>();

        public ResponseBuilder withAccount(Account account)
        {
            this.account = account;
            return this;
        }

        public ResponseBuilder withViolation(Violations violation)
        {
            violations.Add(violation);
            return this;
        }

        public Response Build()
        {
            return new Response()
            {
                Account = account,
                Violations = violations
            };
        }
    }
}