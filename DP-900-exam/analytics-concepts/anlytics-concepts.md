# Analytics concept

## Data ingestion pipelines

On Azure, large-scale data ingestion is managed by orchestrating ETL pipelines using tools like Azure Data Factory or Microsoft Fabric. Pipelines consist of activities that move and transform data between linked services (e.g., Blob Storage, SQL Database, Databricks, Azure Functions), enabling flexible workflows for loading, processing, and storing datasets in a unified environment.

## Data warehouse

A data warehouse is a relational database optimized for analytics, using schemas like star or snowflake (fact and dimension tables). It enables efficient aggregation and querying of transactional data (e.g., sales by product, store, or time) using SQL, making it ideal for structured reporting and business intelligence.

## Data lake

A data lake is a file store, usually on a distributed file system for high performance data access. Technologies like Spark or Hadoop are often used to process queries on the stored files and return data for reporting and analytics. These systems often apply a schema-on-read approach to define tabular schemas on semi-structured data files at the point where the data is read for analysis, without applying constraints when it's stored. Data lakes are great for supporting a mix of structured, semi-structured, and even unstructured data that you want to analyze without the need for schema enforcement when the data is written to the store.

### Hybrid Approach (data warehouse & data lakes)
You can use a hybrid approach that combines features of data lakes and data warehouses in a data lakehouse. 
- The raw data is stored as files in a data lake, and Microsoft Fabric SQL analytics endpoints expose them as tables, which can be queried using SQL. When you create a Lakehouse with Microsoft Fabric, a SQL analytics endpoint is automatically created. Data lakehouses are a relatively new approach in Spark-based systems, and are enabled through technologies like Delta Lake; which adds relational storage capabilities to Spark, so you can define tables that enforce schemas and transactional consistency, support batch-loaded and streaming data sources, and provide a SQL API for querying.

---

## Azure sercices for analytical stores

## Microsoft Fabric

Microsoft Fabric is a unified analytics platform that combines a high-performance SQL-based data warehouse, flexible data lake storage, native Spark integration, real-time analytics, and built-in data pipelines. It provides a single workspace for managing, ingesting, transforming, and analyzing data across multiple products (e.g., Data Factory, Synapse, Power BI), making it ideal for end-to-end analytics solutions.

## Azure Databricks

Azure Databricks is a managed analytics platform built on Apache Spark, offering interactive notebooks, native SQL, and workload-optimized clusters for data science and analytics. It is cloud-portable and integrates with Azure data lakes, making it a strong choice for organizations with Spark expertise or multicloud requirements.


> **Note:** Both Microsoft Fabric and Azure Databricks can act as analytical data stores, often processing data stored in a data lake. Solutions may combine these services—using Databricks notebooks for data transformation and Fabric Warehouse for structured analytics and reporting.

---

## Understand batch and stream processing
Data processing is simply the conversion of raw data to meaningful information through a process. There are two general ways to process data:
- Batch processing, in which multiple data records are collected and stored before being processed together in a single operation.
>Advantages of batch processing include:
Large volumes of data can be processed at a convenient time.
> - It can be scheduled to run at a time when computers or systems might otherwise be idle, such as overnight, or during off-peak hours.
Disadvantages of batch processing include:
The time delay between ingesting the data and getting the results.
> - All of a batch job's input data must be ready before a batch can be processed. This means data must be carefully checked. Problems with data, errors, and program crashes that occur during batch jobs bring the whole process to a halt. The input data must be carefully checked before the job can be run again. Even minor data errors can prevent a batch job from running.
- Stream processing, in which a source of data is constantly monitored and processed in real time as new data events occur.


## Understand differences between batch and stream processing
Apart from the way in which batch processing and streaming processing handle data, there are other differences:
- Data scope: Batch processing can process all the data in the dataset. Stream processing typically only has access to the most recent data received, or within a rolling time window (the last 30 seconds, for example).
- Data size: Batch processing is suitable for handling large datasets efficiently. Stream processing is intended for individual records or micro batches consisting of few records.
- Performance: Latency is the time taken for the data to be received and processed. The latency for batch processing is typically a few hours. Stream processing typically occurs immediately, with latency in the order of seconds or milliseconds.
- Analysis: You typically use batch processing to perform complex analytics. Stream processing is used for simple response functions, aggregates, or calculations such as rolling averages.

## Combining batch and stream processing

Large-scale analytics solutions often mix batch and stream processing for both historical and real-time analysis. Streaming captures real-time data for dashboards and quick insights, while batch processing handles periodic, large-scale analysis—both can persist results in a data store for unified reporting. Architectures like lambda and delta combine these approaches for end-to-end analytics.

---

## Real-time analytics services

Azure supports multiple technologies for real-time analytics of streaming data:
- **Azure Stream Analytics:** PaaS for defining streaming jobs, perpetual queries, and writing results to outputs.
- **Spark Structured Streaming:** Open-source library for complex streaming on Spark-based services (Fabric, Databricks).
- **Microsoft Fabric:** Unified platform with Real-Time Analytics, Data Engineering, Data Factory, Data Science, Data Warehouse, and Databases.

### Common sources for stream processing
- **Azure Event Hubs:** Manages event queues, ensures ordered and exactly-once processing.
- **Azure IoT Hub:** Optimized for IoT device event data.
- **Azure Data Lake Store Gen2:** Scalable storage, used for batch and streaming data.
- **Apache Kafka:** Open-source ingestion, often paired with Spark.

### Common sinks for stream processing
- **Azure Event Hubs:** Queues processed data for downstream steps.
- **Azure Data Lake Store Gen2, Microsoft OneLake, Azure Blob Storage:** Persists results as files.
- **Azure SQL Database, Azure Databricks, Microsoft Fabric:** Stores results in tables for querying/analysis.
- **Microsoft Power BI:** Real-time dashboards and visualizations.

---

# Questions
1. Which term is used to describe an analytics solution that combines data lake and relational data warehouse functionality?
    - Data lakehouse
    > - **Explanation:** A data lakehouse architecture combines the scalability and low-cost storage of a data lake with the structured querying and transactional capabilities of a relational data warehouse. It enables analytics directly on lake-stored data using table formats with ACID support.

<br>

2. Which SaaS analytics solution can you use to create a pipeline for data ingestion and processing?
    - Microsoft Fabric
    > - **Explanation:** Microsoft Fabric is an end-to-end SaaS analytics platform that includes Data Factory capabilities for building data pipelines to ingest, transform, and orchestrate data workflows within a unified environment.

<br>

3. Which open-source distributed processing engine does Microsoft Fabric include?
    - Apache Spark
    > - **Explanation:** Microsoft Fabric includes an integrated Apache Spark runtime that enables distributed data processing, large-scale transformations, and advanced analytics workloads such as machine learning and real-time analytics.

<br>

4. Which component of Microsoft Fabric provides a central place to manage streaming data?
    - Real-Time hub
    > - **Explanation:** The Real-Time hub in Microsoft Fabric provides a centralized location for discovering, managing, and consuming streaming data sources across the organization. It enables users to connect to event streams, monitor live data feeds, and integrate streaming data into analytics workloads in a governed and scalable manner.

<br>

5. What should you define in your data model to enable drill-up/down analysis?
    - A hierarchy
    > - **Explanation:** To enable drill-up and drill-down analysis, you must define a hierarchy in the data model (for example: Year → Quarter → Month → Day). Hierarchies establish ordered levels of aggregation, allowing users to navigate between summarized and detailed views of the data in reports and dashboards.