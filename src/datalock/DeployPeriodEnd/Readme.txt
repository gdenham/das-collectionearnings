-------------------------------------------------------------------------------------
DAS Data Lock Component - DAS Period End
-------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------
1. Package contents
------------------------------------------------------------------------------------- 
 1.1 DLLs:
  - component\CS.Common.External.dll
  - component\Dapper.dll
  - component\FastMember.dll
  - component\MediatR.dll
  - component\NLog.dll
  - component\SFA.DAS.CollectionEarnings.DataLock.dll
  - component\SFA.DAS.Payments.DCFS.dll
  - component\SFA.DAS.Payments.DCFS.StructureMap.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - sql\ddl\PeriodEnd.Transient.DataLock.DDL.Tables.sql:
   - transient database tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.DataLock.DDL.Views.sql:
   - transient database views that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Transient.Reference.DDL.Tables.sql:
   - transient database tables that need to be present when the component is executed
  - sql\ddl\PeriodEnd.Deds.DataLock.DDL.Tables.sql:
   - deds database tables that need to be present when the component is executed
  - sql\dml\PeriodEnd.DataLock.Cleanup.Deds.DML.sql:
   - deds database cleanup script that needs to be executed before copying from the transient database to the deds database
 
 1.4 Copy to transient mapping xml:
  - copy mappings\DasPeriodEndDataLockCopyToTransientMapping.xml:
   - sql bulk copy binary task configuration file that copies das commitments from deds to transient
   - SourceConnectionString: deds das commitments connection string
   - DestinationConnectionString: transient connection string
 
 1.5 Copy to deds mapping xml:
  - copy mappings\DasPeriodEndDataLockCopyToDedsMapping.xml:
   - sql bulk copy binary task configuration file that copies data lock results (validation errors and found matches) from transient to deds
   - SourceConnectionString: transient connection string
   - DestinationConnectionString: deds das period end connection string
 
 1.6 Test results:
  - test-results\TestResult.xml
  - test-results\TestResult-Integration.xml
 
 1.7 Validation messages CSV file
  - DataLock.ValidationMessages.csv
  
-------------------------------------------------------------------------------------
2. Expected context properties
-------------------------------------------------------------------------------------
 2.1 Transient database connection string:
  - key: TransientDatabaseConnectionString
  - value: ILR transient database connection string
 2.2 Log level:
  - key: LogLevel
  - value: one of the following is valid: Fatal, Error, Warn, Info, Debug, Trace, Off

-------------------------------------------------------------------------------------
3. Expected data set keys in the manifest that runs the component
-------------------------------------------------------------------------------------
 3.1 Current ILR Collection: ${ILR_Current.FQ}
 3.1 Current ILR Summarisation Collection: ${ILR_Summarisation.FQ}
 3.3 DAS Period End Collection: ${DAS_PeriodEnd.FQ}

-------------------------------------------------------------------------------------
4. Expected manifest steps for the das period end process - data lock period end
-------------------------------------------------------------------------------------
 4.1 Build the transient database.
 4.2 Bulk copy commitments from deds to transient
 4.3 Execute the 'DAS Data Lock Component - DAS Period End' component
 4.4 Cleanup the deds data lock results using the 'PeriodEnd.DataLock.Cleanup.Deds.DML.sql' sql script
 4.5 Bulk copy the data lock results from transient to deds

