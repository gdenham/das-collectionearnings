# Collections Earnings (BETA)

This solution represents the code base relating to the Data Lock DC component and the Apprenticeship Earnings Calculator DC component.

# Data Lock Component ![Build Status](https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/c39e0c0b-7aff-4606-b160-3566f3bbce23/126/badge)

## Running Locally

## Running Database Integration Tests Locally

To successfully run the database integration tests you need:
* Create a database named 'DataLockTransient'.
* Run the 'src/datalock/SFA.DAS.CollectionEarnings.DataLock.IntegrationTests/Ilr.Transient.DDL.sql' script against the newly created database.
* Run the 'src/datalock/Deploy/sql/ddl/Ilr.Transient.DataLock.DDL.Tables.sql' script against the newly created database.
* Run the 'src/datalock/Deploy/sql/ddl/Ilr.Transient.DataLock.DDL.Views.sql' script against the newly created database.

### Prerequisites

To run the solution locally you will need:
* Visual Studio 2015

You should run Visual Studio as Administrator

# Apprenticeship Earnings Calculator Component

## Running Locally

## Running Database Integration Tests Locally

To successfully run the database integration tests you need:
* Create a database named 'ApprenticeshipEarningsTransient'.
* Run the 'src/earnings/SFA.DAS.CollectionEarnings.Calculator.IntegrationTests/Ilr.Transient.DDL.sql' script against the newly created database.
* Run the 'src/earnings/Deploy/sql/ddl/Ilr.Transient.Earnings.DDL.Tables.sql' script against the newly created database.
* Run the 'src/earnings/Deploy/sql/ddl/Ilr.Transient.Earnings.DDL.Views.sql' script against the newly created database.

### Prerequisites

To run the solution locally you will need:
* Visual Studio 2015

You should run Visual Studio as Administrator



