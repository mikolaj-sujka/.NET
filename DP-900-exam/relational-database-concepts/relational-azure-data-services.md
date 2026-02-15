# Relational Azure Data Services

## 1. Azure SQL family (SQL Server-compatible)

### 🖥️ SQL Server on Azure Virtual Machines (VM) — *IaaS*
**When to choose:** lift-and-shift, full control, special features/agents, OS-level access.

- **Compatibility**: Highest compatibility with on-prem SQL Server.
- **You manage**: OS patches, SQL Server patches, backups (unless you configure managed backup), HA/DR configuration, monitoring.
- **Pros**: Maximum control, can use features requiring OS/admin level access.
- **Cons**: More operational work.

### 🧩 Azure SQL Managed Instance — *PaaS*
**When to choose:** migrate SQL Server apps with minimal changes, need near-full SQL Server feature set without managing VMs.

- **Compatibility**: Near 100% with SQL Server (more than Azure SQL Database).
- **Managed service**: Automated patching, backups, built-in high availability.
- **Best for**: Many existing SQL Server workloads that need PaaS.

### ☁️ Azure SQL Database — *PaaS*
**When to choose:** modern cloud apps, new development, per-database scaling.

- **Deployment models**: Single database, elastic pools.
- **Compute**: Provisioned or serverless.
- **Managed service**: Automated patching, backups, built-in high availability.
- **Best for**: New apps or refactored apps that don’t need full SQL Server instance-level features.

#### Quick comparison

| Service | Model | Primary goal | Typical choice |
|--------|-------|--------------|----------------|
| SQL Server on VM | IaaS | Full control / lift-and-shift | Minimal app changes, custom OS/SQL config |
| SQL Managed Instance | PaaS | Migrate SQL Server with fewer changes | “Almost full SQL Server” without VM management |
| Azure SQL Database | PaaS | Cloud-native DB per app | New apps, elastic scaling |

---

## 2. Open-source relational databases (PaaS)

### 🐘 Azure Database for PostgreSQL
- Managed PostgreSQL with built-in HA options (service tier dependent).
- Good for: apps already using Postgres, extensions (within Azure support).

### 🐬 Azure Database for MySQL
- Managed MySQL service.
- Good for: LAMP/LEMP style apps, legacy MySQL workloads.

### 🦭 Azure Database for MariaDB *(legacy / retired in some regions)*
- Historically available; for DP-900 remember it as an open-source option, but in practice many workloads move to MySQL/PostgreSQL.

> Note (DP-900): These are PaaS services — you **don’t** manage the OS or database engine patching.

---

## 3. IaaS vs PaaS vs SaaS (DP-900 perspective)

### 🏗️ IaaS (Infrastructure as a Service)
You get raw infrastructure (VMs, storage, networking). You install/configure/manage the software.

- **You manage**: OS, runtime, database engine, patching, backups, HA/DR.
- **Azure example**: **SQL Server on Azure Virtual Machines**.
- **When**: maximum control, custom configurations, legacy requirements.

### 🧰 PaaS (Platform as a Service)
Azure provides a managed platform; you focus on the database and data, not servers.

- **Azure manages**: OS and database engine patching, backups, built-in HA.
- **You manage**: schema, users, queries, performance tuning (logical).
- **Azure examples**: **Azure SQL Database**, **Azure SQL Managed Instance**, **Azure Database for PostgreSQL/MySQL**.
- **When**: reduce ops work, faster provisioning, easy scaling.

### 📦 SaaS (Software as a Service)
A complete application delivered as a service; you only use/configure the app.

- **Azure-related examples (data)**:
  - **Power BI (service)** for analytics/reporting,
  - **Dynamics 365** (business app with built-in data),
  - **Microsoft 365** (application-level service).
- **When**: you want a ready product, not a platform to build on.

#### Responsibility model (simplified)

| Layer | IaaS | PaaS | SaaS |
|------|------|------|------|
| Application | You | You | Provider |
| Data / configuration | You | You | You (mostly configuration) |
| Runtime / middleware | You | Provider | Provider |
| OS | You | Provider | Provider |
| Servers / storage / networking | Provider | Provider | Provider |