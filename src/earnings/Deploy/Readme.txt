-------------------------------------------------------------------------------------
DAS Data Lock Component
-------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------
1. Package contents
------------------------------------------------------------------------------------- 
 1.1 DLLs:
  - component\CS.Common.External.dll
  - component\Dapper.Contrib.dll
  - component\Dapper.dll
  - component\NLog.dll
  - component\SFA.DAS.CollectionEarnings.DataLock.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - sql\ddl\Ilr.Transient.DataLock.DDL.sql:
   - database tables that need to be present when the component is executed
 
 1.3 Test results:
  - test-results\TestResult.xml
  - test-results\TestResult-Integration.xml
 
 1.4 Validation messages CSV file
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