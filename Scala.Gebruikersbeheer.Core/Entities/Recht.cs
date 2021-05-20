using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;

namespace Scala.Gebruikersbeheer.Core.Entities
{
    [Table("Rechten")]
    public class Recht
    {
        [ExplicitKey]
        public string GebruikerId { get; private set; }
        public string OnderdeelId { get; private set; }
        public Recht(string gebruikerId, string onderdeelId)
        {
            GebruikerId = gebruikerId;
            OnderdeelId = onderdeelId;
        }

    }
}
