# Azure Cosmos DB

Azure Cosmos DB is a highly scalable, globally distributed NoSQL database service in Azure.

![Diagram relacyjny](images/cosmos-db.png)

## Key Features
- **Multiple APIs:** Supports SQL, MongoDB, Cassandra, Gremlin, Table APIs—developers use familiar query languages.
- **Automatic partitioning:** Cosmos DB allocates space for partitions in containers; each partition can grow up to 10 GB.
- **Automatic indexing:** Indexes are created and maintained automatically for fast queries.
- **Global distribution:** Multi-region writes and reads, low latency for users worldwide.
- **Virtually no admin overhead:** Scaling, indexing, and partitioning are managed by Azure.

## When to use Cosmos DB
Cosmos DB is a foundational Azure service, used by Microsoft for mission-critical apps (Skype, Xbox, Microsoft 365, Azure).

### Typical scenarios

| Scenario | Why Cosmos DB? |
|----------|---------------|
| **IoT & telematics** | Handles large, bursty data ingestion; integrates with analytics and real-time processing (Azure ML, Power BI, Azure Functions). |
| **Retail & marketing** | Used for e-commerce platforms, catalog data, event sourcing in order pipelines. |
| **Gaming** | Delivers fast, personalized content (stats, leaderboards, social features); scales for spikes during launches. |
| **Web & mobile apps** | Models social interactions, integrates with third-party services, builds rich personalized experiences; SDKs for .NET MAUI, iOS, Android. |

---

**Summary:** Cosmos DB is ideal for applications needing global scale, high performance, flexible data models, and minimal database management.

# Questions
1. Which Azure Cosmos DB API is best suited for applications requiring single-millisecond latencies for reads and writes?
    - Azure Cosmos DB for NoSQL

    > - **Explanation:** Azure Cosmos DB for NoSQL is optimized for low-latency, high-throughput workloads and is the core native API of Cosmos DB, delivering single-digit millisecond reads and writes at global scale. It provides automatic indexing, elastic scalability, and turnkey multi-region replication, making it ideal for performance-critical applications.

<br>

2. Which use case is least aligned with the features of Azure Cosmos DB?
    - Complex transactional processing requiring strict ACID compliance.
    > - **Explanation:** Azure Cosmos DB is optimized for globally distributed, horizontally scalable, low-latency workloads such as retail catalogs and gaming applications. While it provides transactional guarantees within a logical partition, it is not designed for complex, cross-entity, strictly ACID-compliant transactional processing typical of traditional relational database systems.

<br>

3. Your organization needs to store data in a graph structure for complex relationship modeling. Which Azure Cosmos DB API is most appropriate?
    - Azure Cosmos DB for Apache Gremlin
    > - **Explanation:** Azure Cosmos DB for Apache Gremlin is specifically designed for graph data models, enabling storage and traversal of highly connected data using vertices and edges. It supports complex relationship queries and graph traversal patterns, making it ideal for scenarios such as social networks, fraud detection, and recommendation engines.