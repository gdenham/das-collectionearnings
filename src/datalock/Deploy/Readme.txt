-------------------------------------------------------------------------------------
DAS Data Lock Component - ILR Submission
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
  - sql\ddl\Ilr.Transient.DataLock.DDL.Tables.sql:
   - transient database tables that need to be present when the component is executed
  - sql\ddl\Ilr.Transient.DataLock.DDL.Views.sql:
   - transient database views that need to be present when the component is executed
  - sql\ddl\Ilr.Transient.Reference.DDL.Tables.sql:
   - transient database tables that need to be present when the component is executed
  - sql\ddl\Ilr.Deds.DataLock.DDL.Tables.sql:
   - deds database tables that need to be present when the component is executed
  - sql\dml\Ilr.DataLock.Cleanup.Deds.DML.sql:
   - deds database cleanup script that needs to be executed before copying from the transient database to the deds database
  - sql\dml\Ilr.Transient.Reference.Populate.DML.sql:
   - populate reference data (from deds to transient) needed to perform data lock
 
 1.3 Copy to deds mapping xml:
  - copy mappings\DasDataLockCopyToDedsMapping.xml:
   - sql bulk copy binary task configuration file that copies data lock results (validation errors and found matches) from transient to deds
   - SourceConnectionString: transient connection string
   - DestinationConnectionString: deds das commitments connection string
 
 1.4 Test results:
  - test-results\TestResult.xml
  - test-results\TestResult-Integration.xml
 
 1.5 Validation messages CSV file
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
3. Expected data set keys
-------------------------------------------------------------------------------------
 3.1 ILR collection: ${ILR_Deds.FQ}
 3.2 DAS Commitments Reference Data Collection: ${DAS_Commitments.FQ}

-------------------------------------------------------------------------------------
4. Expected manifest das steps for the ilr submission process
-------------------------------------------------------------------------------------
 4.1 Build the transient database.
 4.2 Copy commitments reference data to transient using the 'Ilr.Transient.Reference.Populate.DML.sql' sql script.
 4.3 Execute the 'DAS Data Lock Component - ILR Submission' component
 4.4 Cleanup the deds data lock results using the 'Ilr.DataLock.Cleanup.Deds.DML.sql' sql script
 4.5 Bulk copy the data lock results from transient to deds

