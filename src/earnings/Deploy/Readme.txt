-------------------------------------------------------------------------------------
DAS Apprenticeship Earnings Funding Calculation Component
-------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------
1. Package contents
------------------------------------------------------------------------------------- 
 1.1 DLLs:
  - component\CS.Common.External.dll
  - component\Dapper.Contrib.dll
  - component\Dapper.dll
  - component\MediatR.dll
  - component\NLog.dll
  - component\SFA.DAS.CollectionEarnings.Calculator.dll
  - component\StructureMap.dll
 
 1.2 SQL scripts:
  - sql\ddl\Ilr.Deds.Earnings.DDL.Tables.sql:
   - database tables to store the component's output in the ILR Deds database
  - sql\ddl\Ilr.Transient.Earnings.DDL.Tables.sql:
   - transient database tables that need to be present when the component is executed
  - sql\ddl\Ilr.Transient.Earnings.DDL.Views.sql:
   - transient database views that need to be present when the component is executed
 
 1.3 Test results:
  - test-results\TestResult.xml
  - test-results\TestResult-Integration.xml
 
-------------------------------------------------------------------------------------
2. Expected context properties
-------------------------------------------------------------------------------------
 2.1 Transient database connection string:
  - key: TransientDatabaseConnectionString
  - value: ILR transient database connection string
 2.2 Log level:
  - key: LogLevel
  - value: one of the following is valid: Fatal, Error, Warn, Info, Debug, Trace, Off