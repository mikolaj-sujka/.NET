1. Which type of Azure Storage is used to store key/value pairs grouped in partitions?
    - Azure Table storage
    > - **Explanation:** Table storage is used to store key/value pairs in partitions. Azure Files is used to store and share files by using SMB and NFS. Data Lake Storage Gen2 is used to store structured and unstructured data for processing. Page blobs are used for VHDs.

<br>

2. Which two storage solutions can be mounted in Azure Synapse Analytics and used to process large volumes of data? Each correct answer presents a complete solution.
    - Azure Blob storage, Azure Data Lake Storage
    > - **Explanation:** Blob storage and Data Lake Storage can be used to store massive amounts of data and can be mounted in Azure Synapse Analytics. Azure Files and Table storage cannot be mounted in Azure Synapse Analytics.

<br>

3. What are two characteristics of Azure Table storage? Each correct answer presents a complete solution.
    - Each RowKey value is unique within a table partition, Items in the same partition are stored in a RowKey order.
    > - **Explanation:** RowKey is unique within a partition, not within a table. Items in the same partitions are stored in a row key order. Tables cannot have indexes to speed up queries.

<br>

4. Which type of blob should you use to store data blocks that are added to a file frequently but cannot be deleted?
    - append
    > - **Explanation:** An append blob allows you to frequently add new data to a file, but it does not allow for the modification or deletion of existing data.

<br>

5. Which Azure Cosmos DB API should you use for data in a column-family storage structure?
    - Apache Cassandra
    > - **Explanation:** The Cassandra API is used for tabular data in a column-family storage. The Gremlin API is used for graph databases. MongoDB API stores data in the Binary JSON (BSON) format. Table is used to retrieve key-value pairs.

<br>

6. Which Azure Cosmos DB API is queried by using a syntax based on SQL?
    - Apache Cassandra
    > - **Explanation:** The Cassandra API is queried by using SQL. The MongoDB API is queried by using MongoDB Query Language (MQL). The Gremlin API is queried by using Graph. The Table API is queried by using OData and LINQ queries.

<br>

7. Which storage solution allows you to aggregate data stored in JSON files for use in analytical reports without additional development effort?
    - Azure Cosmos DB
    > - **Explanation:** Azure Cosmos DB allows you to aggregate data in analytical reports without additional development. Azure SQL Database does not store data in JSON files. Blob storage and Data Lake Storage do not allow you to aggregate data for analytical reports without additional development effort.

<br>

8. Which service is managed and serverless, avoids the use of Windows Server licenses, and allows for each workload to have its own instance of the service being used?
    - Azure SQL Database
    > - **Explanation:** Azure SQL Database is a serverless platform as a service (PaaS) SQL instance. SQL Managed Instance is a PaaS service, but databases are maintained in the same SQL Managed Instance cluster. SQL Server on Azure Virtual Machines running Windows or Linux are not serverless options.

<br>

9. Which data service allows you to create a single database that can scale up and down without downtime?
    - SQL Azure SQL Database
    > - **Explanation:** Azure SQL Database allows you to provision a single database on a dedicated server and has on-demand scalability. SQL Managed Instance can support multiple databases, and SQL Server on Azure Virtual Machines is installed on a virtual machine, where each instance can support multiple databases.

<br>

10. Which data service allows you to migrate an entire Microsoft SQL Server to the cloud without requiring that you manage the infrastructure after the migration?
    - Azure SQL Managed Instance
    > - **Explanation:** SQL Managed Instance allows you to migrate an entire SQL server to the cloud without requiring that you manage the infrastructure after the migration. You must manage all aspects of SQL Server on Azure Virtual Machines. Azure SQL Database supports most, but not all, core database-level capabilities of SQL Server.

<br>

11. Which data service allows you to control the amount of RAM, change the I/O subsystem configuration, and add or remove CPUs?
    - SQL Server on Azure Virtual Machines
    > - **Explanation:** SQL Server on Azure Virtual Machines allows you the control to manage the system. Azure SQL Database and SQL Managed Instance are fully automated in terms of updates, backups, and recovery.

<br>

12. Which open-source database has built-in support for temporal data?
    - MariaDB
    > - **Explanation:** MariaDB has built-in support for temporal data. It enables applications to query data as the data appeared in previous points in time.

<br>

13. Which SQL engine is optimized for IoT scenarios?
    - Azure SQL Edge
    > - **Explanation:** SQL Edge is optimized for IoT scenarios that must work with streaming time series data. SQL Server on Azure Virtual Machinesis is best used when you want to retain control over the server and database configuration. SQL Managed Instance is ideal for cloud migrations where you need minimal change to existing apps. Azure SQL Database is best used for new cloud solutions.

<br>

14. Which type of data structure should you use to optimize create, read, update, and delete (CRUD) operations for data saved in a multi-column tabular format?
    - relational database
    > - **Explanation:** A relational database is the best option for CRUD operations and uses the least amount of storage space. A key/value store is used for simple lookups based on a single key to obtain a single value. A document database uses unstructured data such as JSON, and is optimized for retrieval, not CRUD operations. A graph database is used to store hierarchical data, such as organizational charts that have nodes and edges.

<br> 

15. Which type of data structure allows you to store data in a two-column format without requiring a complex database management system?
    - key/value store
    > - **Explanation:** A key/value store is used for simple lookups based on a single key to obtain a single value. A relational database is the best option for create, read, update, and delete (CRUD) operations and uses the least amount of storage space, but our solution does not require a database management system (DBMS) on the browser. A document database used unstructured data such as JSON, and optimized for retrieval, not CRUD operations. A graph database is used to store hierarchical data, such as organizational charts that have nodes and edges.

<br>

16. Which type of database should you use to store sequential data in the fastest way possible?
    - Time series database
    > - **Explanation:** Time series databases are used to store sequential data. Table storage is not suited for time series. Graph databases are used to store hierarchical data, such as organizational charts that have nodes and edges. Azure SQL Database is the best option for create, read, update, and delete (CRUD) operations, uses the least amount of storage space, and is not optimized for time series.

<br>

17. Which service allows you to store data as a graph database?
    - Azure Cosmos DB
    > - **Explanation:** Azure Cosmos DB allows you to store data as a graph database. Azure Synapse Analytics, SQL Managed Instance, and Azure SQL Database do not.

<br>

18. What should you use to process large amounts of data by using Apache Hadoop?
    - Azure HDInsight
    > - **Explanation:** HDInsight is used to process large amounts of data by using Hadoop. Databricks is used for processing large amounts of data, which is supported by multiple cloud providers. Data Factory is used to run ETL pipelines. Azure Synapse Analytics is an Azure native service built on Apache Spark.

<br>

19. Which type of data store uses star schemas, fact tables, and dimension tables?
    - data warehouse
    > - **Explanation:** Data warehouses use fact and dimension tables in a star/snowflake schema. Relational databases do not use fact and dimension tables. Cubes are generated from a data warehouse but are a table themselves. Data lakes store files.

<br>

20. Which two services allow you to create a pipeline to process data in response to an event? Each correct answer presents a complete solution.
    - Azure Data Factory, Azure Synapse Analytics
    > - **Explanation:** Azure Synapse Analytics and Data Factory both allow you to create a pipeline in response to an event. Databricks and HDInsight run data processing tasks.

<br>

21. Which two services allow you to pre-process a large volume of data by using Scala? Each correct answer presents a complete solution.
    - a serverless Apache Spark pool in Azure Synapse Analytics, Azure Databricks.
    > - **Explanation:** Databricks and the Spark pool in Azure Synapse Analytics run data processing for large amounts of data by using Scala.

<br>

22. You have an Azure Cosmos DB service running the SQL API. One of the operational databases has a lot of transactions.
Which service allows you to perform near real-time analytics on the operational data stored in Azure Cosmos DB?
    - Azure Synapse
    > - **Explanation:** Azure Synapse allows you to perform near real-time analytics on operational data. Databricks and HDInsight do not meet all the requirements. Data Lake Storage is a category of analytical data store.

<br>

23. In a stream processing architecture, what can you use to persist the processed results as files?
    - Azure Data Lake Storage Gen2
    > - **Explanation:** Data Lake Storage Gen2 can be used to store files. Azure Synapse Analytics and Databricks can be used to persist the data in a database for further querying and analysis. Event Hubs is a data ingestion service.

<br>

24. Which three services can be used to ingest data for stream processing? Each correct answer presents a complete solution.
    - Azure data lake storage, azure event hubs, azure iot hub.
    > - **Explanation:** Data Lake Storage, Event Hubs, and IoT Hub are sources commonly used to ingest data for stream processing. Azure SQL Database and Azure Function are outputs.

<br>

25. Which service allows you to aggregate data over a specific time window before the data is written to a data lake?
    - Azure Stream Analytics
    > - **Explanation:** Stream Analytics allows you to aggregate data from a specific period before it is written to a data lake. Event Hubs is used as a source or a sink for stream processing. Azure SQL Database is used to persist processed results in a database table for querying and analysis.
