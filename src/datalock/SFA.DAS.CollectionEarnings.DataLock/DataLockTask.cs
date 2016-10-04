using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.DependencyResolution;
using SFA.DAS.Payments.DCFS;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.DCFS.Infrastructure.DependencyResolution;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    public class DataLockTask : DcfsTask
    {
        private IDependencyResolver _dependencyResolver;
        private const string DataLockSchema = "DataLock";

        public DataLockTask()
            : base(DataLockSchema)
        {
            _dependencyResolver = new TaskDependencyResolver();
        }

        public DataLockTask(IDependencyResolver dependencyResolver)
            : base(DataLockSchema)
        {
            _dependencyResolver = dependencyResolver;
        }

        protected override void Execute(ContextWrapper context)
        {
            _dependencyResolver.Init(typeof(DataLockProcessor), context);

            var processor = _dependencyResolver.GetInstance<DataLockProcessor>();

            processor.Process();
        }
    }
}
