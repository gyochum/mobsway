using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Mobsway.Web
{
    public class DatabaseConfig
    {

        public static DocumentStore ConfigureRavenDatabase()
        {
            DocumentStore result = null;

            result = new DocumentStore()
            {
                ConnectionStringName = "RavenDB"
            };

            result.Initialize();

            IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), result);

            return result;
        }

    }
}