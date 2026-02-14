# Core Concepts

## Three ways to represent data
- Structured Data
- Unstructured Data
- Semi-structured Data

## *Structured Data*
- has all the same shape
- organized in rows and columns
- rows represent one entity
- columns represent attribute of entity

Features of Structured Data
- the data has a strict format - rows and columns or objects and properties.
- usually associared with relational databases (for example SQL server, MYSQL or PostgreSQL).
- its easy to query

## *Unstructured Data*
- does not follow a strict format for examples text documents, word files, pdfs, video, audio files, emails etc.
- can live almost everywhere in file systems in cloud storage like Azure Blob Storage.
- data is not organized into any predefined structure.
- Windows File Folder, Blob Container.

## *Semi-Structured Data*
- data is labeled but the data doesn't have a set "shape"
- the data has some structure to it, but it's not script.
- for each object, not all properties must be present.
- The format of the data is often in a text format file like XML or JSON.
- The data can be searched, but it's not optimal.
- uses tags and labels

Common text-based markup/serializtion languages:
- html, yaml, json, xml. 

For instance one json object can include phone number and another not. Program can parse them but it has to be flexible.

---

## Options for data storage