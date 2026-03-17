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

<br>

26. What is the difference between a clustered and unclustered index in SQL Server?
    - A clustered index sort and store the data rows based on their key values. Nonclustered indexes have a structure separate from the data rows.
    > - **Explanation:** In SQL Server, a clustered index defines the physical order of data in the table (the data rows are the index). A nonclustered index is a separate B-tree structure that contains key values and pointers to the actual data rows.

<br>

27. Which Azure data service is best used for moving data from one source to another destination, with the ability to transform the data (process it) along the way?
- Azure Data Factory
> - **Explanation:** Azure Data Factory is a cloud-based data integration service designed to orchestrate, move, and transform data between various sources and destinations. It supports ETL/ELT pipelines, enabling data processing during transit using mapping data flows or integration with compute services such as Azure Synapse and Spark.

<br>

28. What is the minimum number of Request Units per second (RU/s) that you can allocate to a Cosmos DB database or container?
    - 400 RU/s
    > - **Explanation:** The minimum throughput you can provision for an Azure Cosmos DB database or container in provisioned throughput mode is 400 Request Units per second (RU/s). This represents the baseline dedicated throughput allocation for predictable performance and guaranteed capacity.

<br>

29. When deploying an Azure Storage account, and you choose Zone Redundant Storage (ZRS), how many copies of your data does Azure keep?
    - 3
    > - **Explanation:** With Zone Redundant Storage (ZRS), Azure synchronously replicates your data across three availability zones within the same region. This ensures high availability and resilience against zone-level failures while maintaining data consistency.

<br>

30. What is the advantage of using Transaction Optimized storage for Azure File Storage?
    - Around 25%–33% of the cost of Hot storage per read-write operation, but twice as expensive per GB for storage. This can save you money for heavily accessed files without using the Premium access tier.
    > - **Explanation:** Transaction Optimized storage in Azure Files is designed for workloads with frequent transactions. It offers lower per-operation costs compared to Hot tier access pricing, while storage per GB is more expensive—making it cost-effective for file shares with many read/write operations but without the need for Premium performance.

<br>

31. How many databases can you create in a single Cosmos DB account?
    - 100
    > - **Explanation:** In Azure Cosmos DB, you can create up to 100 databases per account by default. This is a quota limit that can typically be increased by submitting a support request, depending on workload requirements and subscription limits.

<br>

32. What type of file system does Azure Data Lake Storage Gen2 use?
    - Hadoop Distributed File System (HDFS)
    > - **Explanation:** The Azure Data Lake uses a driver compatible with the Hadoop File System (HDFS). It is designed to be highly fault-tolerant, provide high throughput access, and is suitable for applications that have large data sets. It runs on top of the underlying disk file system. FAT32 has serious limitations for large data, and NTFS has certainly improved that with some tradeoffs.

<br>

33. You are developing a Power BI report for users to view on their desktop workstations. The report needs to show detailed data, and allow the user to customize the filters and modify the column to sort on. Additionally, users should be able to drill-through the report to other reports if they wish to see the data in another way. What type of Power BI report should you create?
    - Interactive Reports
    > - **Explanation:** Interactive Reports in Power BI allow users to interact with the data by customizing filters, sorting columns, and drilling through to other reports for further analysis. This type of report is ideal for users who want to explore and analyze data in a dynamic and interactive way.

<br>

34. You have a SQL Database Server named SQLDB, and two SQL Databases named DB1 and DB2. SQLDB has a server-level firewall that allows IP addresses in the range 123.123.123.0-123.123.123.255 and denies all other traffic. DB1 has a database level firewall rule that allows IPs in the range 123.123.123.0-123.123.124.255. When a client using the IP address 123.123.124.25 attempts to access DB1, are they successful?
    - Yes, the connection will succeed
    > - **Explanation:** You can have both server- and database-level firewall IP rules, and they exist independently. An IP can exist in a database rule and not a server rule, and the user would have access to the specific database and not all databases on the server. In this case, the client trying to access the database is allowed by a database-level IP rule. 

<br>

35. Which of the following techniques is an example of Least Privileged Access?
    - Just-In-Time (JIT) access
    > - **Explanation:** Just-In-Time (JIT) access is an example of Least Privileged Access because it provides users with temporary access only when needed for a specific task or time period. This minimizes the risk of unauthorized access and reduces the attack surface by limiting access to the bare minimum required for the task at hand.

<br>

36. Which of the follwing is a consequnce of having a foreign key relationship between two relational data tables?
    - An INSERT statement will fail if the value of the foreign key column, other than NULL, doesn't exist in the other table.
    > - **Explanation:** This is correct because a foreign key constraint ensures referential integrity between two tables. If an INSERT statement tries to add a value to the foreign key column that does not exist in the referenced table, the operation will fail to maintain data consistency.

<br>

37. What method of provisioning a non-relational database involves writing a script, which is a set of commands that you can run from any operating system prompt such as Linux, macOS or Windows?
    - Command-line interface (CLI) provisioning
    > - **Explanation:** You can use command-line scripts using Powershell or CLI to make changes to Azure. For instance, you can list all of the Virtual Machines in a region, add a new VM, delete a VM, or make other changes to your resources - all from the command line. You can provision Cosmos DB from the command line as well.

<br>

38. What do you get when you use Cool access tier for Azure Blob Storage?
    - Less expensive per GB to store the file, and more expensive to access the file

<br>

39. Which of the following activities would be considered part of Azure Data Factory's Control Flow?
    - If Condition

<br>

40. You need a non-relational data store that supports key-value storage. This needs to be the most cost-effective way for storing data in Azure as you are expecting a large volume of data that needs to be sorted, processed and cleaned up before being migrated into another long-term data format. Which non-relational data store do you recommend?
    - Azure Table Storage
    > - **Explanation:** Azure Table Storage is a highly cost-effective, scalable NoSQL key-value store designed for large volumes of structured, non-relational data. It is well-suited for transient or staging datasets that require sorting, processing, and cleanup before migration to a long-term storage solution, offering low storage and transaction costs compared to fully managed database services like Cosmos DB.

41. What type of non-relational data revolves around storing and retriving large binary files or blobs, such as images, videos, text files and audio files?
    - Object (Blob) storage/data
    > - **Explanation:** This type of non-relational data storage is designed for handling large unstructured binary objects (BLOBs) such as images, videos, audio files, and text documents. In Azure, this is implemented using Azure Blob Storage, which is optimized for scalable, durable storage of massive amounts of unstructured data.

<br>

42. A customer just purchased a service from your company, and a new data row was added to table SALES to indicate this event as soon as it happened. What type of database is this?
    - OLTP
    > - **Explanation:** OLTP (Online Transaction Processing) databases are designed for real-time transaction processing. They are optimized for inserting, updating, and deleting small amounts of data quickly and efficiently, making them suitable for scenarios where data needs to be added or modified as soon as events occur, such as in the case of a new customer purchase.

<br>

43. Which of the following metrics affect how much an Azure Redis Cache instance costs?
    - Region, pricing tier, hours

<br>

44. Which of the following methods is commonly used to secure data in transit?
    - Site-to-site VPN
    > - **Explanation:** Site-to-site VPN is commonly used to secure data in transit by creating a secure encrypted connection between two networks. This ensures that data transferred between the networks is protected from unauthorized access or interception

<br>

45. What should you create first for an integration process that copies data from Microsoft Excel files to Parquet files by using Azure Data Factory?
    - a linked service
    > - **Explanation:** A linked service must be created first. Pipelines use existing linked services to load and process data. Datasets are the input and output, and activities can be defined as the data flow.

<br>

46. Which service allows you to perform on-demand analysis of large volumes of data from text logs, websites and IoT devices by using a common querying language for all the data sources?
    - Azure Data Explorer
    > - **Explanation:** Data Explorer is used for the analysis of large amounts of text log data, websites, and IoT devices and uses a common querying language. Data Lake Storage Gen2 is a data source, Azure Stream Analytics is used to define streaming jobs, apply a perpetual query, and write the results to an output. Azure Cosmos DB stores data.

<br>

47. Which two visuals in Microsoft Power BI allow you to visually compare numeric values for discrete categories? Each correct answer presents a complete solution.
    - a bar chart, a column chart
    > - **Explanation:** Bar charts and column charts allow you to compare numeric values for discrete values. A card is used to track a single number or value. A matrix makes it easier to view data across multiple dimensions.

<br>

48. Which SQL clause can be used to copy all the rows from one table to a new table?
    - Select Into
    > - **Explanation:** SELECT - INTO does an insert into a table. SELECT – OVER determines the partitioning and ordering of the rowset before a windowing function is applied. INSERT – VALUES inserts values into a single row. SELECT – HAVING filters data.

<br>

49. You have a complex query that selects data from multiple tables.
Which three database objects should allow you to reuse the query definition? Each correct answer presents a complete solution.
Select all answers that apply.
    - A view, stored procedure, a function.
    > - **Explanation:** A view, a function, and a stored procedure allow you to reuse the query definition for a complex query that selects data from multiple tables.

<br>

50. You need to recommend a solution that meets the following requirements:
Encapsulates a business logic that can rename the products in a database
Adds entries to tables
What should you include in the recommendation?
    - a stored procedure
    > - **Explanation:** A stored procedure can encapsulate any type of business logic that can be reused in the application. A stored procedure can modify existing data as well as add new entries to tables. A stored procedure can be run from an application as well as from the server.
An inline function cannot be used to complete the task because it cannot modify nor create objects. It can be used to query a database. A view cannot be used to complete the task because it cannot modify nor create objects. It can be used to query a database. A table-valued function cannot be used to complete the task because it cannot modify or create objects. It can be used to query a database.

<br>

51. You need to process many JSON files every minute, while keeping the data from the files accessible by using native queries.
Which Azure Cosmos DB API should you use?
    - NoSQL
    > - **Explanation:** SQL is the native API in Cosmos DB. It manages data in the JSON format. The Cassandra API uses a column-family storage structure. The Table API is used to work with data in key/value tables. The Gremlin API is used with data in a graph structure.

52. Which service can you use to perpetually retrieve data from a Kafka queue, process the data, and write the data to Azure Data Lake?
    - Azure Stream Analytics
    > - **Explanation:** Stream Analytics can handle stream processing from Kafka to Data Lake. Azure Synapse Analytics does not process streaming data. Azure Cosmos DB does not handle data streaming. Data Factory does not handle streams.

53. Which SQL clause can be used to copy all the rows from one table to a new table?
    - SELECT - INTO
    > - **Explanation:** SELECT - INTO does an insert into a table. SELECT – OVER determines the partitioning and ordering of the rowset before a windowing function is applied. INSERT – VALUES inserts values into a single row. SELECT – HAVING filters data.

54. Which data service allows you to use every feature of Microsoft SQL Server in the cloud?
    - SQL Server on an Azure Virtual Machines running Windows
    > - **Explanation:** SQL Server on an Azure Virtual Machines running Windows is the only option that supports all the SQL Server features in the cloud. Azure SQL Database, SQL Managed Instance, and SQL Server on an Azure Virtual Machines running Linux do not support all the SQL Server features.

55. Which open-source database has built-in support for temporal data?
    - MariaDB
    > - **Explanation:** MariaDB has built-in support for temporal data. It enables applications to query data as the data appeared in previous points in time.

56. Which Azure SQL Database feature ensures that users can see only their own rows when multiple customers share the same tables?
    - row-level security (RLS)
    > - **Explanation:** Azure SQL Database supports RLS, which restricts access to rows in a table based on a user’s identity or execution context. This enables multiple customers to share the same tables, while ensuring that each customer can see only their own data. Dynamic data masking hides sensitive values but does not prevent access to rows, Always Encrypted protects data at rest and in use but does not filter query results, and TDE protects data at rest only. Therefore, RLS is the correct feature.

57. Which type of Azure Storage is used to store large amounts of files to be shared with virtual machines by using SMB?
    - Azure Files
    > - **Explanation:** Azure Files is used to share files by using NFS and SMB. Data Lake Storage Gen2 is used for storing huge amounts of data to be processed, not to be shared among virtual machines. Page blobs are used for VHDs. Table storage is used for two dimensional tables.

58. Which three actions can you perform directly from an Azure Databricks notebook? Each correct answer presents a complete solution.
    - Create a database.
    > - **Explanation:** Databricks notebooks are interactive development environments that enable users to execute code and SQL statements directly against the data in a lakehouse. From within a notebook, you can run queries to explore and analyze data, create tables, such as Delta tables, to persist structured data, and create databases in the metastore by running SQL commands. These actions represent core, in-notebook capabilities that support large-scale analytics workloads. In contrast, creating an additional notebook is a workspace management task performed through the Databricks UI, not from within a running notebook. In addition, creating a schema is intentionally excluded at the DP-900 level to avoid unnecessary conceptual overlap with databases, which are the primary organizational construct emphasized for foundational learners.

59. You need to ingest, transform, and visualize data from a continuously generated data source. The solution must minimize latency and administrative effort.
Which type of data processing should you use?
    - stream
    > - **Explanation:** Stream processing supports continuously generated data by processing events as they occur, which enables low-latency ingestion, transformation, and visualization while requiring less operational management than interval-based approaches. Batch processing introduces delays because data is collected and processed at scheduled times, and microbatching still processes data in small intervals rather than immediately. Although the term “real-time” is commonly used to describe low-latency scenarios, it is not a formal data processing model; stream processing is the appropriate classification for this type of workload.

60. What should you create in a data model to allow users to drill up and drill down in a report?
    - a hierarchy
    > - **Explanation:** A hierarchy enables drill up and drill down in a dimension. A dimension enables navigation, but hierarchy is used to drill up and down in a dimension. Fact tables have values. Cubes are not created in Microsoft PowerBI.

61. Which type of Microsoft Fabric data store supports large-scale analytical workloads?
    - a data warehouse
    > - **Explanation:** Fabric provides multiple types of data stores that are optimized for different types of workloads. A data warehouse is specifically designed to support large-scale analytical workloads, offering optimized storage and query performance for complex, high-concurrency analytical queries over structured data. In contrast, a SQL database is intended for transactional or operational scenarios, a lakehouse combines data lake storage with analytical processing but is positioned as a flexible analytics option rather than the primary enterprise analytical store at the fundamentals level, and a eventhouse is optimized for real-time streaming and event data. Therefore, the data warehouse is the most appropriate analytical data store to support large-scale analytics.

62. You need to share a report that you created in Microsoft Power BI Desktop with other users.
What should you do first?
    - Publish the report to the Power BI service.
    > - **Explanation:** To share data models and reports created in Power BI Desktop, they are first published to the Power BI service.

63. Which SQL operation is used to combine the content of two tables based on a shared column?
    - JOIN
    > - **Explanation:** JOIN is used to combine data from two tables based on a shared key. HAVING is used to filter content from a GROUP BY command. UNION displays the content of two sets of columns from two tables but is not based on a shared key. INTERSECT shows only values that exist in both tables.

64. Which Azure Blob Storage feature prevents blobs from being modified or deleted for a specified retention period?
    - an immutable blob
    > - **Explanation:** Blob Storage supports immutable blobs, which use time-based retention policies to prevent data from being modified or deleted for a specified period. This is commonly used in compliance and data protection scenarios, where data integrity must be preserved. Legal holds prevent deletion but do not enforce time-based immutability, while soft delete and point-in-time restore enable recovery after deletion or modification rather than preventing those actions. Therefore, immutable blobs provide the strongest protection against modification or deletion.

65. Which type of database can be used for semi-structured data that will be processed by an Apache Spark pool in Azure Synapse Analytics?
    - column-family
    > - **Explanation:** Column-family databases are used to store unstructured, tabular data comprising rows and columns. Azure Synapse Analytics Spark pools do not directly support graph or relational databases.