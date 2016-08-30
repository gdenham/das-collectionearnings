using System.Collections.Generic;
using System.Xml.Linq;
using Amor.DCFT.BinaryTask;
using CS.Common.XmlToSql.Interfaces;

namespace SFA.DAS.CollectionEarnings.DataLock.Common.Tests.Ilr
{
    internal class PreProcessingCallback : IPreProcessingCallback
    {
        public IEnumerable<XElement> Process(IEnumerable<XElement> elements, IProcessingContext context)
        {
            return elements;
        }
    }
}