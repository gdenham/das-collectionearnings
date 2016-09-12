using System;
using System.Collections.Generic;
using Amor.DCFT.BinaryTask;
using CS.Common.XmlToSql.Implementation;
using CS.Common.XmlToSql.Interfaces;
using CS.Common.XmlToSql.Model;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools.Ilr
{
    public class Shredder
    {
        private readonly string _connectionString;
        private readonly string _ilrFile;
        private readonly TableMap _ilrTableMap = new TableMap(AppDomain.CurrentDomain.BaseDirectory + @"\Tools\Ilr\Files\IlrTableMap.xml");
        private readonly ISeedGenerator _seedGenerator = new SeedGenerator();

        public Shredder()
        {
            _connectionString = ConnectionStringFactory.GetTransientConnectionString();
            _ilrFile = AppDomain.CurrentDomain.BaseDirectory + @"\Tools\Ilr\Files\ILR-10007459-1617-20160505-225627-99.xml";
        }

        public Shredder(string ilrFile)
        {
            _connectionString = ConnectionStringFactory.GetTransientConnectionString();
            _ilrFile = AppDomain.CurrentDomain.BaseDirectory + ilrFile;
        }

        public void Shred()
        {
            var settings = new XmlToSqlExecutionSettings()
            {
                DestinationConnectionString = _connectionString,
                PageSize = 10000,
                XmlFileName = _ilrFile,
                TableMap = _ilrTableMap,
                SeedGenerator = _seedGenerator
            };

            var shredder = new ShredXmlToSql();

            shredder.Execute(
                settings,
                new PreProcessingCallback(),
                new ProcessingContext(null, null, null, null, new Dictionary<string, string>(), null, null, null));
        }
    }
}
