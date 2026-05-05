from __future__ import annotations

from dataclasses import dataclass
from datetime import date
from pathlib import Path
from typing import Iterable

import pandas as pd
from reportlab.lib import colors
from reportlab.lib.enums import TA_CENTER
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import ParagraphStyle, getSampleStyleSheet
from reportlab.lib.units import cm
from reportlab.platypus import (
    PageBreak,
    Paragraph,
    Preformatted,
    SimpleDocTemplate,
    Spacer,
    Table,
    TableStyle,
)
from xml.sax.saxutils import escape


ROOT = Path(__file__).resolve().parent
SYLLABUS = ROOT / "Syllabus.xlsx"
OUTPUT_PDF = ROOT / "Capgemini_Sprint_Programme_Study_Book.pdf"
OUTPUT_MD = ROOT / "Capgemini_Sprint_Programme_Study_Book.md"


@dataclass
class SyllabusRow:
    topic: str
    detail: str


def clean(value: object) -> str:
    if value is None or pd.isna(value):
        return ""
    return str(value).strip()


def load_syllabus() -> list[SyllabusRow]:
    df = pd.read_excel(SYLLABUS, sheet_name="Datewise", header=0)
    rows: list[SyllabusRow] = []
    skip_words = [
        "SUNDAY",
        "HOLIDAY",
        "Assessment",
        "Revision",
        "Re-Assessment",
        "Mock",
        "Evaluation",
        "OCEAN L1 Test",
    ]
    for _, row in df.iterrows():
        topic = clean(row.get("Topic"))
        detail = clean(row.get("Detailed concepts"))
        if not topic:
            continue
        if any(word.lower() in topic.lower() for word in skip_words):
            continue
        if topic.lower() == "sprint implementation":
            continue
        if not detail and "Power Skill" not in topic and "Git" not in topic:
            continue
        rows.append(SyllabusRow(topic=topic.replace("\n", " ").strip(), detail=detail))
    return rows


def split_concepts(detail: str) -> list[str]:
    if not detail:
        return []
    parts = []
    for line in detail.replace("\r", "\n").split("\n"):
        text = line.strip(" #\t-")
        if text:
            parts.append(text)
    return parts


def category(topic: str, detail: str) -> str:
    text = f"{topic} {detail}".lower()
    if ".net 8" in text or "c# 12" in text or "oop" in text or "threading" in text or "collections" in text or "delegates" in text:
        return "dotnet_csharp"
    if "programming foundation" in text or "data structures" in text or "algorithm" in text or "recursion" in text:
        return "problem_solving"
    if "unit testing" in text or "xunit" in text or "nunit" in text:
        return "testing"
    if "rdbms" in text or "sql server" in text or "transact-sql" in text:
        return "sql"
    if "linq" in text:
        return "linq"
    if "entity framework" in text or "ef core" in text:
        return "efcore"
    if "web basics" in text or "html" in text or "css" in text or "javascript" in text:
        return "web"
    if "mvc" in text or "razor pages" in text:
        return "mvc"
    if "web api" in text or "rest api" in text or "cors" in text:
        return "webapi"
    if "devops" in text:
        return "devops"
    if "cloud computing" in text or "azure fundamentals" in text:
        return "cloud"
    if "network" in text or "storage" in text or "api management" in text or "cdn" in text:
        return "azure_network_storage"
    if "paas" in text or "function" in text or "logic app" in text or "cosmos" in text or "app service" in text or "azure sql" in text:
        return "azure_paas_data"
    if topic.strip().lower() == "git":
        return "git"
    if "power skill" in text or "communication" in text or "presentation" in text:
        return "power_skills"
    return "general"


def base_notes(cat: str) -> list[str]:
    notes = {
        "dotnet_csharp": [
            ".NET is a managed platform built around a runtime, base class libraries, SDK tooling, and language compilers. In Sprint work, think in terms of project file, Program.cs, dependencies, build output, and runtime behavior.",
            "C# code should be read by separating syntax from intent: what data type is being modeled, what lifetime it has, how errors are handled, and whether the code is synchronous, asynchronous, mutable, or immutable.",
            "A strong answer connects language features to maintainability. For example, interfaces support loose coupling, generics avoid repeated casting, async improves scalability for I/O, and exceptions create a standard error flow.",
        ],
        "problem_solving": [
            "Problem solving is not only memorizing algorithms. It is the habit of identifying input, output, constraints, edge cases, state changes, and complexity before writing code.",
            "Data structures matter because each one optimizes a different operation. Arrays offer indexed access, stacks model last-in-first-out, queues model first-in-first-out, trees model hierarchy, and graphs model relationships.",
            "In assessments, always discuss time complexity and space complexity. A correct O(n log n) sort may be preferable to a simple O(n^2) sort when input size grows.",
        ],
        "testing": [
            "Unit tests validate small units of behavior in isolation. A useful test has clear arrange, act, and assert phases, and it should fail for one understandable reason.",
            "xUnit and NUnit both support facts/tests, setup/teardown, parameterized data, assertions, fixtures, and parallel execution. Know the vocabulary even if you use one framework more often.",
            "Mocking is used when a class depends on external systems such as databases, APIs, queues, or file systems. The test should verify business behavior, not re-test the framework.",
        ],
        "sql": [
            "RDBMS thinking starts with tables, rows, columns, keys, constraints, and relationships. Good schema design prevents invalid data before application code even runs.",
            "SQL Server work often combines DDL for schema, DML for data changes, DCL for permissions, joins for related data, indexes for performance, and stored procedures for reusable database logic.",
            "Normalize to remove duplication, then denormalize only when there is a proven reporting or performance reason. Always understand primary keys, foreign keys, unique constraints, and nullability.",
        ],
        "linq": [
            "LINQ gives a consistent query style over objects, EF Core queries, XML, and other providers. The same-looking LINQ expression can execute in memory or be translated to SQL.",
            "Query syntax resembles SQL, while method syntax chains extension methods. You should be comfortable reading both because projects often mix them.",
            "Deferred execution means a query is not executed until enumerated. Immediate execution happens with operators such as ToList, Count, First, Single, Sum, and Average.",
        ],
        "efcore": [
            "EF Core is an ORM that maps C# classes to database tables and tracks changes through DbContext. It reduces boilerplate but does not remove the need to understand SQL.",
            "Code First begins with entities and migrations. Database First begins with an existing schema and scaffolds classes. Both still require relationship and configuration awareness.",
            "Loading strategy matters. Eager loading uses Include, explicit loading loads related data when requested, and lazy loading loads on navigation access but can cause hidden N+1 queries.",
        ],
        "web": [
            "The web stack has layers: HTML for structure, CSS for presentation, JavaScript for behavior, HTTP for communication, and browser APIs for storage and events.",
            "Forms are central to MVC and API work. Understand input names, GET versus POST, labels, placeholders, validation attributes, and how submitted values become server-side model properties.",
            "Client-side validation helps users, but server-side validation protects the application. Never trust browser checks alone.",
        ],
        "mvc": [
            "ASP.NET Core MVC separates the application into Models, Views, and Controllers. Controllers receive requests, coordinate with services/data, and choose responses.",
            "Razor views are server-rendered templates. Strongly typed views and view models are preferred for real applications because they are clearer and safer than loose ViewBag data.",
            "Middleware forms the request pipeline. Routing selects endpoints, static files serve assets, authentication identifies users, authorization checks permissions, and endpoints execute actions.",
        ],
        "webapi": [
            "A Web API exposes application behavior over HTTP. REST-style APIs use resources, standard verbs, status codes, request/response bodies, and predictable routes.",
            "DTOs are API contracts. They prevent overposting, hide database-only fields, and allow the API to evolve separately from EF entities.",
            "Cross-cutting concerns such as validation, error handling, logging, security, CORS, caching, and versioning should be designed deliberately instead of sprinkled randomly.",
        ],
        "devops": [
            "Azure DevOps is a platform for planning work, storing code, building and releasing applications, sharing packages, and managing test activity.",
            "A CI/CD pipeline should restore, build, test, package, and deploy with clear environment variables and secrets. Manual deployment should become the exception, not the default.",
            "Boards, Repos, Pipelines, Artifacts, and Test Plans are connected pieces of a delivery workflow, not separate tools to memorize in isolation.",
        ],
        "cloud": [
            "Cloud computing trades local ownership for managed capacity. You pay for usage, gain elasticity, and take responsibility for correct architecture and cost control.",
            "Public, private, and hybrid cloud models differ by ownership and access. Azure provides compute, storage, networking, identity, databases, monitoring, and governance services.",
            "Resource groups, subscriptions, roles, policies, and regions are governance concepts. They decide who can create resources, where they live, and how cost/security is managed.",
        ],
        "azure_network_storage": [
            "Azure networking controls how resources talk to each other and to the internet. VNets, subnets, NSGs, private endpoints, peering, gateways, and load balancers are the core building blocks.",
            "Azure Storage covers blobs, files, queues, tables, and disks. Choose storage based on data shape, access pattern, durability, cost, and integration needs.",
            "Security and cost should be part of every design: restrict public access, prefer private connectivity, monitor logs, choose the right redundancy tier, and stop unused compute.",
        ],
        "azure_paas_data": [
            "PaaS services reduce infrastructure responsibility. App Service, Function Apps, Logic Apps, Azure SQL, and Cosmos DB let teams focus more on application code and workflows.",
            "App Service plans control compute for Web Apps. Deployment slots help safe releases. Configuration should come from environment settings or Key Vault, not hard-coded source.",
            "For data services, know the trade-off between relational Azure SQL and globally distributed NoSQL Cosmos DB. The choice depends on schema, consistency, partitioning, and query needs.",
        ],
        "git": [
            "Git tracks snapshots of source code. The working tree holds current files, the staging area prepares the next commit, and commits record project history.",
            "Branches isolate work. Pull requests create a review and integration point. Merge conflicts happen when changes touch the same lines and must be resolved intentionally.",
            "Good Git hygiene means small commits, clear messages, no secrets, no generated build folders, and regular pulls from the shared branch.",
        ],
        "power_skills": [
            "Power skills are employability skills: communication, email writing, listening, presentation, ownership, teamwork, meeting behavior, and interview readiness.",
            "Technical work is judged through communication. Clear status updates, concise explanations, and structured emails often decide whether your work is trusted.",
            "Practice STAR stories for behavioral interviews: Situation, Task, Action, Result. Use real training examples such as debugging, teamwork, deadlines, and learning a new tool.",
        ],
        "general": [
            "Treat this topic as part of the Sprint outcome. Identify definitions, commands or syntax, practical implementation steps, and common mistakes.",
            "When revising, create one small working example. Practical recall is stronger than reading notes passively.",
            "Connect the topic to a project scenario: what problem does it solve, where is it configured, and how would you debug it?",
        ],
    }
    return notes[cat]


def concept_notes(concepts: list[str], cat: str) -> list[str]:
    text = " ".join(concepts).lower()
    out: list[str] = []
    checks = [
        ("middleware", "Middleware is ordered. A request passes through middleware in registration order, and the response returns in reverse order. Misordering UseAuthentication and UseAuthorization can break security."),
        ("deployment", "Deployment is not only publishing files. You must decide runtime, hosting model, environment configuration, database connectivity, logging, and rollback approach."),
        ("microservices", "Microservices split a system into independently deployable services. Benefits include team autonomy and scaling, but costs include distributed tracing, data consistency, network failures, and deployment complexity."),
        ("boxing", "Boxing copies a value type into an object reference; unboxing extracts it back. It adds allocation and casting risk, so generics are usually preferred."),
        ("nullable", "Nullable value types such as int? represent a value or no value. Nullable reference types help the compiler warn about possible null references."),
        ("array", "Arrays are fixed-size indexed collections. Use them when size is known; use List<T> when the collection grows dynamically."),
        ("stringbuilder", "String is immutable, so repeated concatenation creates new strings. StringBuilder is better for many incremental modifications."),
        ("parse", "Parse throws when conversion fails, TryParse returns a bool and is safer for user input, and Convert handles nulls differently for some types."),
        ("breakpoint", "A breakpoint pauses execution so you can inspect variables, call stack, and control flow. Conditional breakpoints are excellent for loops and rare cases."),
        ("serilog", "Structured logging records named fields instead of only plain text. This makes logs searchable by request id, user id, endpoint, and error type."),
        ("inheritance", "Inheritance models an is-a relationship. Use it for true specialization, not just code reuse. Prefer composition when behavior varies independently."),
        ("interface", "Interfaces define contracts. They support dependency injection, testing with mocks, and interchangeable implementations."),
        ("generic", "Generics let one type or method work with many data types while preserving compile-time type safety."),
        ("regex", "Regular expressions are powerful but can become unreadable. Anchor patterns, keep them simple, and test valid plus invalid examples."),
        ("exception", "Catch exceptions only when you can add value, recover, translate, or log. Do not swallow errors silently."),
        ("garbage", "Garbage collection reclaims managed memory. Dispose is still needed for unmanaged resources such as files, streams, database connections, and sockets."),
        ("delegate", "Delegates represent method references. Events build on delegates to implement publish-subscribe behavior."),
        ("lambda", "Lambda expressions are concise anonymous functions, heavily used in LINQ, events, and callbacks."),
        ("async", "Async/await is best for I/O-bound work. It does not automatically make CPU-bound work faster, but it prevents blocking request threads."),
        ("lock", "Locks protect shared mutable data. Keep critical sections short and avoid locking on public objects."),
        ("linear search", "Linear search checks each item and is O(n). It works on unsorted data but becomes slow as input grows."),
        ("binary search", "Binary search is O(log n), but it requires sorted data and careful boundary handling."),
        ("merge sort", "Merge sort is stable and O(n log n), but needs extra memory for merging."),
        ("quick sort", "Quick sort is fast on average but can degrade to O(n^2) without good pivot strategy."),
        ("recursion", "Every recursive solution needs a base case and progress toward it. Also consider stack depth."),
        ("assert", "Assertions should express expected behavior clearly. Prefer specific assertions over vague true/false checks when possible."),
        ("mock", "Mock external dependencies such as repositories, email senders, queues, and HTTP clients. Do not mock simple value objects."),
        ("normalization", "Normalization reduces duplicate data and update anomalies. Know 1NF, 2NF, and 3NF at least."),
        ("join", "Joins combine rows across related tables. Inner joins require matches; left joins keep unmatched rows from the left table."),
        ("index", "Indexes speed reads but add write cost and storage. Index columns used frequently in search, joins, filters, and ordering."),
        ("stored procedure", "Stored procedures centralize database logic and can simplify permissions, but too much business logic in SQL can become hard to test."),
        ("deferred", "Deferred LINQ queries execute when enumerated. If source data changes before enumeration, results may change too."),
        ("repository", "Repository pattern hides data access details behind an interface. It is useful when it simplifies testing and isolates persistence logic."),
        ("migration", "Migrations are versioned schema changes. Always review generated migrations before applying them."),
        ("eager", "Eager loading with Include prevents lazy N+1 issues when you already know related data is needed."),
        ("raw sql", "Raw SQL is useful for complex queries, but parameterize inputs to avoid injection."),
        ("form", "HTML forms submit key-value pairs. In ASP.NET model binding, input name attributes must match parameter or property names."),
        ("semantic", "Semantic HTML improves accessibility, SEO, and maintainability. Use header, nav, main, section, article, aside, and footer where appropriate."),
        ("flex", "Flexbox is one-dimensional layout for rows or columns. Grid is two-dimensional layout for rows and columns together."),
        ("hoisting", "JavaScript hoists declarations. let and const avoid many var-related surprises because they are block scoped."),
        ("dom", "DOM manipulation changes the live document. Use event listeners and avoid mixing too much HTML string construction with logic."),
        ("dependency injection", "DI provides dependencies through constructors or services. It reduces tight coupling and supports testing."),
        ("model binding", "Model binding maps HTTP data to .NET parameters or objects. Understand [FromBody], [FromForm], [FromQuery], and [FromRoute]."),
        ("data annotation", "Data annotations drive validation and schema hints. For complex rules, consider FluentValidation or custom validation attributes."),
        ("viewmodel", "View models are tailored to a screen or request. They keep UI requirements separate from database entities."),
        ("razor pages", "Razor Pages group page markup with a PageModel. They suit page-focused apps; MVC suits controller/action organization and larger separation."),
        ("filter", "Filters run before or after actions and can handle authorization, resource checks, exceptions, and result processing."),
        ("jwt", "JWT bearer tokens are sent in Authorization headers. Validate issuer, audience, lifetime, and signing key."),
        ("rest", "REST APIs use resources and standard HTTP verbs. Avoid designing every endpoint as an arbitrary action when a resource shape fits."),
        ("status", "Status codes are part of the API contract. Clients depend on them for success, validation, auth, and missing-resource behavior."),
        ("cors", "CORS is enforced by browsers. Configure only trusted origins, methods, and headers."),
        ("dto", "DTOs are especially important for Web APIs because they prevent accidental exposure of database structure."),
        ("automapper", "AutoMapper reduces repetitive mapping but can hide complexity. Manual mapping is clearer for small DTOs."),
        ("fluent validation", "FluentValidation keeps validation rules expressive and testable, especially when rules become conditional."),
        ("global error", "Global error handling creates consistent responses and avoids repeating try-catch in every action."),
        ("caching", "Cache stable data carefully. Invalidate or expire cached values when underlying data changes."),
        ("versioning", "API versioning lets clients migrate gradually. Common approaches include URL segment, header, or query versioning."),
        ("swagger", "Swagger is excellent for discovery and manual testing, but automated integration tests are still needed."),
        ("azure devops", "Azure DevOps connects planning, source control, CI/CD, artifacts, and test management into one delivery flow."),
        ("capex", "CapEx buys long-lived assets upfront. OpEx pays for usage over time, which is a core cloud financial model."),
        ("resource group", "Resource groups are lifecycle containers for Azure resources. Use naming, tags, and ownership consistently."),
        ("rbac", "RBAC grants permissions to identities at scopes such as management group, subscription, resource group, or resource."),
        ("key vault", "Key Vault keeps secrets out of source code. Prefer managed identity so apps do not need stored credentials."),
        ("virtual machine", "VMs provide infrastructure control but require patching, monitoring, security, and cost management."),
        ("nsg", "NSGs filter traffic using rules. Avoid broad inbound rules such as any source to RDP/SSH."),
        ("private endpoint", "Private endpoints expose Azure services through private IPs inside a VNet."),
        ("load balancer", "Load balancers distribute traffic across backend instances and improve availability."),
        ("blob", "Blob Storage is designed for unstructured objects such as images, documents, backups, and logs."),
        ("queue", "Queues decouple producers and consumers. They improve resilience when downstream processing is slow or temporarily unavailable."),
        ("api management", "API Management adds gateway features such as policies, products, subscriptions, transformations, mock responses, and monitoring."),
        ("app service", "App Service hosts web apps and APIs without managing servers. App Service Plan controls compute and cost."),
        ("deployment slots", "Deployment slots allow staging and swap-based releases with safer rollback."),
        ("function", "Functions are event-driven and scale based on triggers. Keep functions focused and idempotent where possible."),
        ("durable", "Durable Functions orchestrate stateful workflows using orchestrator, activity, entity, and client functions."),
        ("logic app", "Logic Apps are workflow automation services with connectors and visual design, useful for integration scenarios."),
        ("cosmos", "Cosmos DB performance depends strongly on partition key choice and RU consumption. Poor partition design is expensive."),
        ("consistency", "Cosmos DB consistency levels trade latency, availability, and freshness of reads."),
        ("merge conflict", "Resolve merge conflicts by understanding both changes, editing the conflicted file, staging it, and committing the resolution."),
    ]
    for needle, note in checks:
        if needle in text and note not in out:
            out.append(note)
    if not out:
        out.extend(base_notes(cat)[:2])
    return out[:7]


def practice_for(cat: str) -> str:
    return {
        "dotnet_csharp": "Create a small console app that demonstrates the concept, then explain the output line by line.",
        "problem_solving": "Solve one problem by writing input, output, edge cases, pseudocode, code, and Big-O analysis.",
        "testing": "Write three tests: success case, validation failure, and exception/edge case.",
        "sql": "Create a tiny schema, insert sample rows, and write one select, join, aggregate, and stored procedure.",
        "linq": "Write the same query using query syntax and method syntax, then explain when it executes.",
        "efcore": "Create two related entities, add a migration, seed data, query with Include, and update one record.",
        "web": "Build one responsive form and validate it with HTML attributes, JavaScript, and server-side rules.",
        "mvc": "Create an MVC CRUD screen with view model, validation, layout, and a partial view.",
        "webapi": "Design REST endpoints for one resource and test them through Swagger and Postman.",
        "devops": "Sketch a CI pipeline that restores, builds, tests, and publishes an ASP.NET Core app.",
        "cloud": "Draw the Azure resources needed for a simple web app and explain ownership, cost, and security.",
        "azure_network_storage": "Design a secure storage/network setup using private access where possible.",
        "azure_paas_data": "Deploy or diagram a Web App/Function plus database flow and list configuration settings.",
        "git": "Create a branch, make a commit, merge it, and resolve a planned conflict.",
        "power_skills": "Prepare one written email, one 2-minute presentation, and one STAR story from your training.",
        "general": "Build a minimal example and teach the topic aloud in five minutes.",
    }[cat]


def interview_for(cat: str) -> str:
    return {
        "dotnet_csharp": "Be ready to explain what problem the feature solves, not only the syntax.",
        "problem_solving": "Always mention edge cases and complexity before declaring a solution final.",
        "testing": "Explain the difference between unit, integration, and end-to-end testing.",
        "sql": "Expect questions on keys, normalization, joins, indexes, and stored procedures.",
        "linq": "Expect deferred execution, IQueryable vs IEnumerable, and First vs Single.",
        "efcore": "Expect DbContext, migrations, tracking, Include, Code First vs DB First, and repository pattern.",
        "web": "Expect GET vs POST, semantic HTML, CSS box model, DOM events, and validation.",
        "mvc": "Expect MVC flow, routing, model binding, filters, ViewBag vs ViewData vs TempData, and Razor Pages vs MVC.",
        "webapi": "Expect REST, status codes, DTOs, CORS, Swagger, authentication, logging, and versioning.",
        "devops": "Expect components of Azure DevOps and what a pipeline does.",
        "cloud": "Expect CapEx vs OpEx, public/private/hybrid cloud, resource groups, RBAC, and Azure portal basics.",
        "azure_network_storage": "Expect VNets, subnets, NSGs, private endpoints, Blob/File/Queue/Table storage, and cost/security choices.",
        "azure_paas_data": "Expect App Service, Functions, Logic Apps, Azure SQL, Cosmos DB, deployment slots, and configuration.",
        "git": "Expect branch, merge, rebase, conflict resolution, stash, reset, revert, and pull request workflow.",
        "power_skills": "Expect email etiquette, presentation structure, teamwork, ownership, and behavioral examples.",
        "general": "Expect definition, use case, implementation step, and one pitfall.",
    }[cat]


CHAPTERS = [
    {
        "title": "Part 1 - .NET 8, .NET Core Architecture, and C# 12",
        "sections": [
            ("The .NET Platform", [
                ".NET is a developer platform for building console apps, web apps, APIs, cloud services, desktop apps, mobile apps, background workers, and libraries.",
                ".NET 8 is a long-term support release. In training, focus on SDK commands, project structure, runtime behavior, dependency management, and cross-platform execution.",
                "The SDK includes compilers, templates, build tools, and CLI commands. The runtime executes compiled applications. The base class library provides common APIs for strings, collections, file I/O, networking, JSON, threading, and security.",
                ".NET Framework is Windows-only and older. Modern .NET is cross-platform, modular, open-source, and optimized for cloud-native development.",
            ], "dotnet --info\ndotnet new console -n DemoApp\ndotnet run\ndotnet build"),
            ("Project Files and Execution", [
                "A .csproj file declares target framework, SDK type, package references, nullable settings, implicit usings, and build metadata.",
                "Program.cs is the application entry point. In ASP.NET Core, it also configures services and middleware.",
                "Build produces assemblies. Run executes the app. Publish prepares deployment output for a target runtime and hosting model.",
                "NuGet packages extend the app. Always keep package references intentional and avoid adding packages for tiny problems already solved by the framework.",
            ], "<Project Sdk=\"Microsoft.NET.Sdk.Web\">\n  <PropertyGroup>\n    <TargetFramework>net8.0</TargetFramework>\n    <Nullable>enable</Nullable>\n    <ImplicitUsings>enable</ImplicitUsings>\n  </PropertyGroup>\n</Project>"),
            ("C# Type System", [
                "Value types store data directly and include int, double, bool, DateTime, enum, and struct types. Reference types store references to objects and include class, string, arrays, delegates, and interfaces.",
                "Boxing converts a value type to object; unboxing converts back. It can allocate memory and fail at runtime if the wrong type is used.",
                "Nullable types handle missing values. int? means int or null. Nullable reference types are compile-time warnings that help avoid NullReferenceException.",
                "var is statically typed after inference. dynamic delays binding until runtime and should be used rarely.",
            ], "int number = 10;\nobject boxed = number;\nint unboxed = (int)boxed;\n\nvar name = \"Shiv\";      // string at compile time\ndynamic value = 10;      // runtime binding"),
            ("C# 12 and Modern Language Features", [
                "C# 12 adds features such as primary constructors, collection expressions, default lambda parameters, aliasing any type, and other improvements that reduce boilerplate.",
                "Modern C# favors expressive, safe, concise code. Use pattern matching, null-coalescing, object initializers, records where useful, and collection expressions when they improve readability.",
                "Do not chase syntax for its own sake. In interviews, explain how a feature improves clarity, type safety, or maintainability.",
            ], "public class Employee(string name, decimal salary)\n{\n    public string Name { get; } = name;\n    public decimal Salary { get; } = salary;\n}\n\nint[] marks = [80, 85, 90];"),
        ],
    },
    {
        "title": "Part 2 - C# OOP, Exceptions, Collections, Delegates, and Async",
        "sections": [
            ("Object-Oriented Programming", [
                "A class defines state and behavior. An object is an instance of a class. Encapsulation keeps data and operations together while hiding internal details.",
                "Inheritance models specialization. Polymorphism lets code depend on a base type or interface while runtime objects provide specific behavior.",
                "Abstraction exposes essential behavior and hides implementation. Interfaces and abstract classes are common abstraction tools.",
                "Access modifiers control visibility: private, public, protected, internal, protected internal, and private protected.",
            ], "public interface IEmployeeService\n{\n    Task<Employee?> GetByIdAsync(int id);\n}\n\npublic class EmployeeService : IEmployeeService\n{\n    public Task<Employee?> GetByIdAsync(int id) => Task.FromResult<Employee?>(null);\n}"),
            ("Constructors, Static Members, and Object Lifetime", [
                "Constructors initialize objects. Overloaded constructors support different creation paths, but too many constructors can make the API confusing.",
                "Static members belong to the type rather than an instance. Use static for pure helpers or shared constants, not for per-user web state.",
                "Destructors/finalizers are rarely used directly. IDisposable and using statements are the normal way to release unmanaged resources deterministically.",
            ], "using var stream = File.OpenRead(\"data.txt\");\n// stream.Dispose() is called automatically at the end of scope."),
            ("Exception Handling", [
                "Exceptions represent abnormal flow. Use try/catch/finally when you can recover, add context, translate to a user/API response, or guarantee cleanup.",
                "Throw preserves error signaling. Use throw; instead of throw ex; when rethrowing to preserve stack trace.",
                "Custom exceptions should add meaning. Do not create custom exception types for every small validation error.",
                "In Web API projects, prefer global exception handling middleware for consistent ProblemDetails responses.",
            ], "try\n{\n    var age = int.Parse(input);\n}\ncatch (FormatException ex)\n{\n    logger.LogWarning(ex, \"Invalid age input\");\n}\nfinally\n{\n    // cleanup if needed\n}"),
            ("Collections, Delegates, Events, and Lambdas", [
                "List<T> is a dynamic array. Dictionary<TKey,TValue> provides fast key lookup. HashSet<T> stores unique values. Queue<T> and Stack<T> model FIFO and LIFO.",
                "Delegates store references to methods. Events use delegates to notify subscribers without tightly coupling publisher and receiver.",
                "Lambda expressions are concise functions. LINQ, event handlers, and asynchronous callbacks use them heavily.",
                "Expression trees represent code as data and are used by LINQ providers such as EF Core to translate queries to SQL.",
            ], "var adults = people\n    .Where(p => p.Age >= 18)\n    .OrderBy(p => p.LastName)\n    .ToList();"),
            ("Threading, Tasks, Async, and Await", [
                "Threads are low-level execution units. Tasks represent asynchronous operations and are easier to compose.",
                "Use async/await for I/O: database calls, file reads, HTTP calls, queue operations, and cloud SDK calls.",
                "Do not block async code with .Result or .Wait in ASP.NET apps. It can hurt scalability and sometimes deadlock.",
                "Locks protect shared mutable state, but web apps should usually avoid shared mutable state when possible.",
            ], "public async Task<Employee?> GetEmployeeAsync(int id)\n{\n    return await _context.Employees.FindAsync(id);\n}"),
        ],
    },
    {
        "title": "Part 3 - Programming Foundation and Problem Solving",
        "sections": [
            ("Data Structures", [
                "Choose a data structure based on operations. Arrays are fast for indexing, linked lists are flexible for inserts if you already have a node, stacks reverse order, queues preserve arrival order, trees represent hierarchy, and graphs represent networks.",
                "Strings are sequences of characters but are immutable in C#. Use StringBuilder for repeated modification.",
                "Graphs require careful representation. Adjacency lists are memory efficient for sparse graphs; adjacency matrices are simple but memory-heavy.",
            ], "Stack<int> stack = new();\nstack.Push(10);\nstack.Push(20);\nint latest = stack.Pop();"),
            ("Searching and Sorting", [
                "Linear search works on any collection and is O(n). Binary search is O(log n) but requires sorted input.",
                "Bubble, selection, and insertion sort are simple O(n^2) algorithms. They are useful for learning but not ideal for large data.",
                "Merge sort and quick sort are common advanced sorting algorithms. Merge sort is stable; quick sort is often fast but pivot choice matters.",
                "Always ask about constraints. The best algorithm for 20 items may not be best for 2 million items.",
            ], "int BinarySearch(int[] arr, int target)\n{\n    int left = 0, right = arr.Length - 1;\n    while (left <= right)\n    {\n        int mid = left + (right - left) / 2;\n        if (arr[mid] == target) return mid;\n        if (arr[mid] < target) left = mid + 1;\n        else right = mid - 1;\n    }\n    return -1;\n}"),
            ("Recursion", [
                "Recursion solves a problem by reducing it to smaller versions of itself.",
                "Every recursive method needs a base case and a recursive case. Without a base case, recursion continues until stack overflow.",
                "Tail recursion performs the recursive call as the final step. Tree recursion branches into multiple recursive calls. Indirect recursion occurs when functions call each other.",
                "For production C#, iterative solutions may be safer for very deep input because .NET does not guarantee tail-call optimization in ordinary code.",
            ], "int Factorial(int n)\n{\n    if (n <= 1) return 1;\n    return n * Factorial(n - 1);\n}"),
        ],
    },
    {
        "title": "Part 4 - Unit Testing with xUnit and NUnit",
        "sections": [
            ("Testing Mindset", [
                "A good test documents expected behavior. It should be deterministic, isolated, readable, and fast.",
                "Use Arrange, Act, Assert. Arrange creates input and dependencies, Act calls the method, Assert verifies output or side effects.",
                "Unit tests are not a replacement for integration tests. Unit tests check small behavior; integration tests verify real wiring such as database or API configuration.",
            ], "[Fact]\npublic void Add_ReturnsSum()\n{\n    var calc = new Calculator();\n    var result = calc.Add(2, 3);\n    Assert.Equal(5, result);\n}"),
            ("Parameterized Tests, Fixtures, and Mocking", [
                "Parameterized tests run the same test logic with multiple inputs. In xUnit, use [Theory] and [InlineData]. In NUnit, use [TestCase].",
                "Fixtures share expensive setup between tests. Use them carefully so tests do not accidentally depend on shared mutable state.",
                "Mocking and stubbing replace dependencies such as repositories, HTTP clients, email senders, or cloud services.",
                "Parallel test execution improves speed but can expose shared-state problems.",
            ], "[Theory]\n[InlineData(2, 3, 5)]\n[InlineData(-1, 1, 0)]\npublic void Add_ReturnsExpected(int a, int b, int expected)\n{\n    Assert.Equal(expected, new Calculator().Add(a, b));\n}"),
        ],
    },
    {
        "title": "Part 5 - RDBMS, SQL Server, and T-SQL",
        "sections": [
            ("Database Fundamentals", [
                "A relational database stores data in tables and links tables through keys. Primary keys uniquely identify rows; foreign keys enforce relationships.",
                "Data integrity includes entity integrity, referential integrity, domain integrity, and user-defined rules.",
                "Normalization reduces duplication and update anomalies. Understand 1NF, 2NF, and 3NF well enough to apply them in examples.",
            ], "CREATE TABLE Authors (\n    AuthorId INT IDENTITY PRIMARY KEY,\n    Name NVARCHAR(100) NOT NULL\n);"),
            ("DDL, DML, DCL, and Joins", [
                "DDL defines schema: CREATE, ALTER, DROP. DML manipulates data: SELECT, INSERT, UPDATE, DELETE. DCL controls permissions: GRANT, DENY, REVOKE.",
                "INNER JOIN returns matching rows. LEFT JOIN keeps all rows from the left table. RIGHT JOIN keeps rows from the right table. FULL JOIN keeps all rows from both.",
                "Subqueries can be scalar, row-based, or table-based. Use them when a query depends on another query result.",
            ], "SELECT b.Title, a.Name AS AuthorName\nFROM Books b\nINNER JOIN Authors a ON a.AuthorId = b.AuthorId\nWHERE b.AvailableCopies > 0;"),
            ("Views, Indexes, Stored Procedures, and Execution Plans", [
                "Views save query definitions and can simplify reporting or security. They are not automatically performance magic.",
                "Indexes speed read queries but slow writes and consume storage. Index columns used in joins, filters, and ordering.",
                "Stored procedures package database logic with input/output parameters. Use TRY-CATCH in T-SQL for controlled error handling.",
                "Execution plans show how SQL Server executes a query. Learn to spot scans, seeks, joins, and missing index hints.",
            ], "CREATE PROCEDURE GetBooksByAuthor\n    @AuthorId INT\nAS\nBEGIN\n    SELECT * FROM Books WHERE AuthorId = @AuthorId;\nEND;"),
        ],
    },
    {
        "title": "Part 6 - LINQ and Entity Framework Core",
        "sections": [
            ("LINQ Fundamentals", [
                "LINQ integrates query operations into C#. It supports filtering, projection, ordering, grouping, joining, paging, and aggregation.",
                "Method syntax is usually more common in production C#. Query syntax can be clearer for joins and grouping.",
                "IEnumerable queries execute in memory. IQueryable queries can be translated by providers such as EF Core into SQL.",
            ], "var highEarners = employees\n    .Where(e => e.Salary > 50000)\n    .OrderByDescending(e => e.Salary)\n    .Select(e => new { e.Name, e.Salary })\n    .ToList();"),
            ("EF Core Overview and Patterns", [
                "DbContext is the unit of work. DbSet<T> represents a table. Entities are tracked, modified, and saved using SaveChangesAsync.",
                "Code First uses C# classes and migrations. Database First scaffolds from an existing database.",
                "Repository pattern can isolate data access. Use it when it reduces duplication or improves testability, not automatically for every simple app.",
            ], "builder.Services.AddDbContext<AppDbContext>(options =>\n    options.UseSqlServer(builder.Configuration.GetConnectionString(\"DefaultConnection\")));"),
            ("Migrations and Data Loading", [
                "Migrations create a history of schema changes. Review migrations before applying them because generated code reflects current model assumptions.",
                "Eager loading uses Include. Explicit loading loads navigation properties on demand. Lazy loading can be convenient but may hide performance problems.",
                "Raw SQL and stored procedures are available when LINQ is not expressive enough, but all user input must be parameterized.",
            ], "dotnet ef migrations add AddBooks\ndotnet ef database update"),
        ],
    },
    {
        "title": "Part 7 - Web Basics: HTML5, CSS3, JavaScript, DOM, and Validation",
        "sections": [
            ("HTML Forms and Semantic Structure", [
                "HTML defines document structure. Use semantic elements when they describe meaning: header, nav, main, section, article, aside, footer.",
                "Forms send user input. GET places data in the URL and suits search/filter. POST sends data in the body and suits creation or state changes.",
                "Labels improve accessibility and click behavior. Fieldsets and legends group related inputs.",
            ], "<form method=\"post\">\n  <label for=\"email\">Email</label>\n  <input id=\"email\" name=\"Email\" type=\"email\" required />\n  <button type=\"submit\">Save</button>\n</form>"),
            ("CSS Layout", [
                "The box model has content, padding, border, and margin. Many layout bugs come from misunderstanding which space belongs to which part.",
                "Positioning can be static, relative, absolute, fixed, or sticky. Avoid absolute positioning for normal page layout unless necessary.",
                "Flexbox is best for one-dimensional alignment. Grid is best for two-dimensional layout.",
            ], ".toolbar {\n  display: flex;\n  align-items: center;\n  justify-content: space-between;\n  gap: 1rem;\n}"),
            ("JavaScript, DOM, Events, and Fetch", [
                "JavaScript adds behavior in the browser. Understand variables, data types, functions, hoisting, scope, arrow functions, iterators, and generators.",
                "DOM APIs let scripts read and modify HTML. Event listeners respond to user actions.",
                "fetch calls APIs asynchronously and returns promises. Always handle non-success HTTP responses and parse JSON carefully.",
            ], "const response = await fetch('/api/products');\nif (!response.ok) throw new Error('Unable to load products');\nconst products = await response.json();"),
        ],
    },
    {
        "title": "Part 8 - ASP.NET Core MVC and Razor Pages",
        "sections": [
            ("MVC Request Flow", [
                "A browser request enters middleware, routing selects an endpoint, model binding creates action parameters, filters run, the action executes, and a result returns.",
                "Controllers should coordinate rather than contain all logic. Move data access and business rules into services or repositories when complexity grows.",
                "Views should render data, not perform heavy business logic. Use view models that match the screen.",
            ], "public async Task<IActionResult> Details(int id)\n{\n    var employee = await _service.GetByIdAsync(id);\n    return employee is null ? NotFound() : View(employee);\n}"),
            ("Routing, Actions, and Data Transfer", [
                "Conventional routing uses patterns like {controller=Home}/{action=Index}/{id?}. Attribute routing declares route templates on controllers/actions.",
                "Action return types include ViewResult, RedirectToAction, NotFound, File, JsonResult, IActionResult, and ActionResult<T>.",
                "ViewBag and ViewData are flexible but weakly typed. Strongly typed models and view models are safer for real applications.",
            ], "app.MapControllerRoute(\n    name: \"default\",\n    pattern: \"{controller=Home}/{action=Index}/{id?}\");"),
            ("Filters, Identity, JWT, and Deployment", [
                "Filters handle cross-cutting work around actions: authorization, resource checks, action logic, exceptions, and result processing.",
                "Authentication proves identity. Authorization checks permissions. ASP.NET Core Identity adds users, roles, password hashing, lockout, and token support.",
                "Deployment requires publish output, hosting target, configuration, database connection, logging, and environment-specific settings.",
            ], "app.UseAuthentication();\napp.UseAuthorization();"),
            ("Razor Pages vs MVC", [
                "Razor Pages organize around pages and PageModel handlers such as OnGet and OnPost. MVC organizes around controllers and actions.",
                "Choose Razor Pages for page-focused CRUD/admin workflows. Choose MVC when controller-based grouping or larger separation suits the app.",
                "Both use Razor syntax, model binding, validation, dependency injection, and middleware.",
            ], "public class EditModel : PageModel\n{\n    public IActionResult OnGet(int id) => Page();\n    public IActionResult OnPost() => RedirectToPage(\"Index\");\n}"),
        ],
    },
    {
        "title": "Part 9 - ASP.NET Core Web API",
        "sections": [
            ("REST and HTTP", [
                "REST-style APIs expose resources through URLs and standard verbs. GET reads, POST creates, PUT replaces/updates, PATCH partially updates, DELETE removes.",
                "Status codes are part of the contract: 200 OK, 201 Created, 204 NoContent, 400 BadRequest, 401 Unauthorized, 403 Forbidden, 404 NotFound, 409 Conflict, 500 Server Error.",
                "Web API differs from MVC views because the response is usually JSON/XML rather than HTML.",
            ], "[HttpGet(\"{id}\")]\npublic async Task<ActionResult<EmployeeDto>> Get(int id)\n{\n    var employee = await _service.GetByIdAsync(id);\n    return employee is null ? NotFound() : Ok(employee);\n}"),
            ("DTOs, Validation, Formatting, and CORS", [
                "DTOs define request and response shape. They reduce overposting and avoid exposing EF navigation graphs.",
                "Content negotiation lets clients and servers agree on response format. JSON is the usual default in ASP.NET Core APIs.",
                "CORS is a browser rule. Configure it only for trusted frontend origins and required methods/headers.",
                "FluentValidation can keep validation rules clear, testable, and separate from entity classes.",
            ], "builder.Services.AddCors(options =>\n{\n    options.AddPolicy(\"Frontend\", policy =>\n        policy.WithOrigins(\"https://example.com\")\n              .AllowAnyHeader()\n              .AllowAnyMethod());\n});"),
            ("Error Handling, Logging, Caching, Versioning, and Testing", [
                "Global error handling keeps error responses consistent. Prefer ProblemDetails for APIs.",
                "Logging should capture endpoint, request id, user id where appropriate, validation failure, exception, and timing.",
                "Caching can improve performance for stable data, but stale data is a real risk.",
                "Versioning protects clients when APIs change. Swagger and Postman help manual testing; unit and integration tests protect regression.",
            ], "return Problem(\n    title: \"Validation failed\",\n    statusCode: StatusCodes.Status400BadRequest);"),
        ],
    },
    {
        "title": "Part 10 - Azure DevOps and Cloud Fundamentals",
        "sections": [
            ("Azure DevOps Components", [
                "Boards manage work items, backlogs, sprints, and task tracking. Repos store source code. Pipelines automate build and release.",
                "Artifacts store packages. Test Plans manage manual and exploratory testing.",
                "A healthy team workflow links requirements to commits, builds, deployments, and test evidence.",
            ], "trigger:\n- main\n\nsteps:\n- script: dotnet restore\n- script: dotnet build --configuration Release\n- script: dotnet test"),
            ("Cloud Computing Basics", [
                "Cloud benefits include elasticity, global reach, managed services, high availability options, and faster provisioning.",
                "CapEx is upfront capital purchase. OpEx is usage-based operating cost. Cloud shifts many workloads toward OpEx.",
                "Public cloud is shared provider infrastructure. Private cloud is dedicated to one organization. Hybrid combines both.",
            ], "Common Azure hierarchy:\nManagement group -> Subscription -> Resource group -> Resource"),
            ("Azure Governance, Identity, and Portal Tools", [
                "Resource groups organize lifecycle and permissions. Azure Resource Manager handles deployment and management.",
                "Azure Active Directory is now Microsoft Entra ID. It manages identities, users, groups, app registrations, and authentication.",
                "RBAC controls what identities can do at a scope. Azure Policy enforces rules. Tags support cost and ownership reporting.",
                "Azure CLI, PowerShell, Cloud Shell, ARM templates, and Bicep help automate management.",
            ], "az group create --name rg-training --location eastus\naz webapp list --resource-group rg-training"),
        ],
    },
    {
        "title": "Part 11 - Azure Networking, Storage, API Management, and CDN",
        "sections": [
            ("Virtual Machines and VNets", [
                "Azure VMs provide infrastructure control. You manage operating system patching, security, monitoring, backups, and cost.",
                "VNets provide private address space. Subnets divide it. NSGs filter traffic. ASGs group NICs for security rules.",
                "Private IP communication should be used inside a VNet. Public exposure should be limited to required ports and trusted sources.",
            ], "Design example:\nVNet 10.0.0.0/16\nSubnet web 10.0.1.0/24\nSubnet data 10.0.2.0/24\nNSG allows HTTPS to web only"),
            ("Advanced Networking", [
                "Service endpoints keep traffic to supported Azure services on the Azure backbone while preserving public service endpoints.",
                "Private endpoints expose services through private IPs and are preferred for stronger private access.",
                "VNet peering connects virtual networks. VPN Gateway connects Azure networks to on-premises networks.",
                "Application Gateway is layer 7 load balancing with routing and optional WAF. CDN caches content closer to users.",
            ], "Common decision:\nUse Load Balancer for layer 4 traffic.\nUse Application Gateway for HTTP routing and WAF.\nUse CDN for static global content acceleration."),
            ("Storage Services", [
                "Blob Storage stores unstructured objects. Azure Files provides SMB/NFS file shares. Queues support asynchronous messaging. Tables store key-value data. Disks back VMs.",
                "Storage accounts have redundancy options such as LRS, ZRS, GRS, and GZRS. Choose based on durability and cost.",
                "Use SAS, RBAC, managed identity, private endpoints, encryption, lifecycle rules, and logging for secure storage design.",
            ], "Blob use cases:\nimages, documents, backups, logs, exports, static assets"),
            ("API Management", [
                "Azure API Management is a gateway layer for APIs. It can publish APIs, apply policies, transform requests/responses, mock responses, enforce subscriptions, and monitor usage.",
                "Products group APIs for consumers. Policies can add headers, rate limits, JWT validation, caching, routing, and transformations.",
                "Self-hosted gateway supports hybrid or on-premises API gateway scenarios.",
            ], "APIM flow:\nClient -> API Management policy pipeline -> Backend API -> Response policies -> Client"),
        ],
    },
    {
        "title": "Part 12 - Azure PaaS, Serverless, Logic Apps, Azure SQL, and Cosmos DB",
        "sections": [
            ("App Service and Deployment", [
                "App Service hosts web apps and APIs without managing servers. The App Service Plan controls compute, scaling, and cost.",
                "Deployment slots support staging, warm-up, swap, and rollback. Use them for safer production releases.",
                "Application configuration should come from App Service settings, Key Vault references, or environment variables.",
            ], "Typical deployment checks:\nBuild succeeds\nConnection string configured\nApp settings configured\nDatabase migration handled\nLogs enabled"),
            ("Azure Functions", [
                "Functions are serverless event-driven units. Common triggers include HTTP, timer, queue, blob, Event Grid, and Service Bus.",
                "Bindings simplify input/output integration. Keep functions focused, idempotent, and observable.",
                "Function keys protect HTTP functions but are not a complete identity system. Use stronger auth for production APIs.",
                "Durable Functions support orchestrated long-running workflows.",
            ], "[Function(\"Hello\")]\npublic IActionResult Run([HttpTrigger(AuthorizationLevel.Function, \"get\")] HttpRequest req)\n{\n    return new OkObjectResult(\"Hello\");\n}"),
            ("Logic Apps and Workflow Automation", [
                "Logic Apps automate workflows using connectors, triggers, and actions. They are useful for integration, approvals, scheduled jobs, and low-code process automation.",
                "Single-tenant Logic Apps run in a dedicated environment and offer more isolation. Multi-tenant Logic Apps use shared infrastructure.",
                "Automated deployment should use templates or infrastructure as code so workflows can move across environments.",
            ], "Examples:\nNew file in Blob -> send approval email -> write result to database\nSchedule -> call API -> post Teams message"),
            ("Azure SQL and Cosmos DB", [
                "Azure SQL is relational and suits structured data, transactions, joins, stored procedures, and familiar SQL Server tooling.",
                "Managed Instance offers broader SQL Server compatibility. Elastic pools share resources across databases.",
                "Cosmos DB is NoSQL, globally distributed, partitioned, and RU-based. Choose partition keys carefully because they decide scale and cost.",
                "Cosmos DB consistency levels include strong, bounded staleness, session, consistent prefix, and eventual. Each trades freshness, latency, and availability.",
            ], "Cosmos design rule:\nPick a partition key with high cardinality, even distribution, and common query alignment."),
        ],
    },
    {
        "title": "Part 13 - Git, Sprint Implementation, OCEAN L1, Gen AI, and Power Skills",
        "sections": [
            ("Git Workflow", [
                "Git tracks project history through commits. The daily loop is status, add, commit, pull, push.",
                "Branches isolate features. Pull requests enable review. Merge conflicts must be resolved by understanding both sides.",
                "Rewrite history carefully. Rebase and reset are powerful but can confuse shared branches if used carelessly.",
            ], "git status\ngit switch -c feature/library-api\ngit add .\ngit commit -m \"Add library API endpoints\"\ngit push -u origin feature/library-api"),
            ("Sprint Implementation Readiness", [
                "A Sprint project should have clear requirements, domain model, API contract, database design, UI flow, authentication needs, deployment plan, and test strategy.",
                "Start with a small vertical slice: one entity, one database table, one API endpoint, one UI screen, one test. Then expand.",
                "Keep a daily implementation log: completed work, blockers, next tasks, and risks. This helps stand-ups and evaluations.",
            ], "Vertical slice example:\nBook model -> DbContext -> migration -> GET /api/books -> MVC index view -> unit test"),
            ("OCEAN L1 and Gen AI Prep", [
                "For OCEAN-style tests, revise definitions, scenario questions, code output, SQL output, cloud service selection, and Git command behavior.",
                "For Gen AI, learn prompt clarity, context, constraints, examples, verification, privacy, and responsible use. Never paste secrets into AI tools.",
                "Use AI as a study partner: ask for quizzes, explain errors, generate practice tasks, and compare your answer with a model answer.",
            ], "Good prompt pattern:\nContext -> Task -> Constraints -> Expected output -> Example -> Verification request"),
            ("Power Skills", [
                "Communication skills include clarity, listening, questioning, email etiquette, grammar, and audience awareness.",
                "Presentation skills need objective, structure, timing, confident delivery, and clean visual support.",
                "Ownership means acknowledging work, risks, mistakes, and follow-up. Teamwork means communicating early and supporting shared goals.",
                "Use STAR for interviews: Situation, Task, Action, Result. Keep examples honest and measurable.",
            ], "Email structure:\nSubject\nGreeting\nPurpose\nContext\nAction needed\nDeadline\nClosing"),
        ],
    },
]


RELATED_TOPICS = [
    ("Clean Architecture", "Separate domain logic, application services, infrastructure, and presentation so the project remains testable and maintainable."),
    ("SOLID Principles", "Single responsibility, open/closed, Liskov substitution, interface segregation, and dependency inversion support better class design."),
    ("Docker", "Know images, containers, ports, volumes, Dockerfile, docker compose, and why containers help local SQL/API setups."),
    ("CI/CD", "Automate restore, build, test, publish, deployment, environment variables, and rollback."),
    ("Observability", "Use logs, metrics, traces, correlation ids, health checks, and Application Insights."),
    ("Security Hardening", "Use HTTPS, secure secrets, least privilege, input validation, upload validation, rate limiting, and dependency updates."),
    ("Transactions and Concurrency", "Use database transactions for multi-step updates and concurrency tokens for simultaneous edits."),
    ("API Documentation", "Use Swagger descriptions, examples, response codes, and versioning policies."),
    ("Accessibility", "Use labels, keyboard navigation, alt text, focus indicators, and sufficient color contrast."),
    ("Performance", "Watch N+1 queries, missing indexes, unnecessary tracking, large payloads, blocking calls, and unbounded pagination."),
]


def make_styles():
    styles = getSampleStyleSheet()
    for name in ["BookTitle", "BookSub", "H1Book", "H2Book", "BodyBook", "BulletBook", "SmallBook", "CodeBook"]:
        if name in styles:
            del styles[name]
    styles.add(ParagraphStyle("BookTitle", parent=styles["Title"], alignment=TA_CENTER, fontSize=27, leading=33, textColor=colors.HexColor("#17324D"), spaceAfter=18))
    styles.add(ParagraphStyle("BookSub", parent=styles["BodyText"], alignment=TA_CENTER, fontSize=11, leading=16, textColor=colors.HexColor("#334E68"), spaceAfter=8))
    styles.add(ParagraphStyle("H1Book", parent=styles["Heading1"], fontSize=17, leading=22, textColor=colors.HexColor("#12395B"), spaceBefore=10, spaceAfter=8))
    styles.add(ParagraphStyle("H2Book", parent=styles["Heading2"], fontSize=12.8, leading=16, textColor=colors.HexColor("#21506F"), spaceBefore=8, spaceAfter=5))
    styles.add(ParagraphStyle("BodyBook", parent=styles["BodyText"], fontSize=9.6, leading=13.2, spaceAfter=5))
    styles.add(ParagraphStyle("BulletBook", parent=styles["BodyText"], leftIndent=14, firstLineIndent=-9, fontSize=9.4, leading=12.7, spaceAfter=3.2))
    styles.add(ParagraphStyle("SmallBook", parent=styles["BodyText"], fontSize=8.0, leading=10.5))
    styles.add(ParagraphStyle("CodeBook", parent=styles["Code"], fontName="Courier", fontSize=7.5, leading=9.2, leftIndent=6, rightIndent=6, spaceBefore=4, spaceAfter=6))
    return styles


def para(text: object, styles, style="BodyBook") -> Paragraph:
    return Paragraph(escape(str(text)).replace("\n", "<br/>"), styles[style])


def add_heading(story: list, styles, text: str, level=1):
    story.append(Paragraph(escape(text), styles["H1Book" if level == 1 else "H2Book"]))


def add_bullets(story: list, styles, items: Iterable[str]):
    for item in items:
        story.append(Paragraph("- " + escape(item), styles["BulletBook"]))


def add_table(story: list, styles, rows: list[tuple[str, str]], widths: list[float]):
    table_data = [[para(a, styles, "SmallBook"), para(b, styles, "SmallBook")] for a, b in rows]
    t = Table(table_data, colWidths=widths, hAlign="LEFT")
    t.setStyle(TableStyle([
        ("GRID", (0, 0), (-1, -1), 0.25, colors.HexColor("#B7C4CF")),
        ("VALIGN", (0, 0), (-1, -1), "TOP"),
        ("ROWBACKGROUNDS", (0, 0), (-1, -1), [colors.white, colors.HexColor("#F7FAFC")]),
        ("LEFTPADDING", (0, 0), (-1, -1), 4),
        ("RIGHTPADDING", (0, 0), (-1, -1), 4),
        ("TOPPADDING", (0, 0), (-1, -1), 3),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 3),
    ]))
    story.append(t)
    story.append(Spacer(1, 7))


def on_page(canvas, doc):
    canvas.saveState()
    canvas.setFont("Helvetica", 8)
    canvas.setFillColor(colors.HexColor("#52606D"))
    canvas.drawString(1.4 * cm, 0.95 * cm, "Capgemini Sprint Programme Study Book")
    canvas.drawRightString(A4[0] - 1.4 * cm, 0.95 * cm, f"Page {doc.page}")
    canvas.restoreState()


def add_chapter(story: list, styles, chapter: dict):
    story.append(PageBreak())
    add_heading(story, styles, chapter["title"])
    for title, notes, code in chapter["sections"]:
        add_heading(story, styles, title, level=2)
        add_bullets(story, styles, notes)
        if code:
            story.append(Preformatted(code, styles["CodeBook"]))
        story.append(para("Revision checkpoint: define the topic, give one real project use case, name one common mistake, and write a minimal example from memory.", styles))


def add_syllabus_card(story: list, styles, row: SyllabusRow, index: int):
    cat = category(row.topic, row.detail)
    concepts = split_concepts(row.detail)
    card_title = concepts[0] if concepts else row.topic
    story.append(PageBreak())
    add_heading(story, styles, f"Syllabus Focus Sheet {index}: {card_title}")
    story.append(para(f"Topic family: {row.topic}", styles))
    if concepts:
        add_heading(story, styles, "Concepts to Cover", level=2)
        add_bullets(story, styles, concepts)
    add_heading(story, styles, "Detailed Notes", level=2)
    add_bullets(story, styles, base_notes(cat))
    add_heading(story, styles, "Important Details", level=2)
    add_bullets(story, styles, concept_notes(concepts, cat))
    add_heading(story, styles, "How to Practice", level=2)
    add_bullets(story, styles, [practice_for(cat)])
    add_heading(story, styles, "Interview Angle", level=2)
    add_bullets(story, styles, [interview_for(cat)])
    add_heading(story, styles, "Self-Test", level=2)
    add_bullets(story, styles, [
        "Can I explain this without reading the notes?",
        "Can I build or write a tiny example in less than 15 minutes?",
        "Can I identify one production risk or best practice related to it?",
    ])


def build_pdf(rows: list[SyllabusRow]):
    styles = make_styles()
    doc = SimpleDocTemplate(
        str(OUTPUT_PDF),
        pagesize=A4,
        rightMargin=1.45 * cm,
        leftMargin=1.45 * cm,
        topMargin=1.25 * cm,
        bottomMargin=1.45 * cm,
        title="Capgemini Sprint Programme Study Book",
        author="Codex",
    )
    story: list = []

    story.append(Spacer(1, 3.1 * cm))
    story.append(Paragraph("Capgemini Sprint Programme", styles["BookTitle"]))
    story.append(Paragraph("Detailed .NET Core with Azure Study Book", styles["BookTitle"]))
    story.append(Spacer(1, 0.5 * cm))
    story.append(Paragraph(f"Based on syllabus workbook: {escape(str(SYLLABUS))}", styles["BookSub"]))
    story.append(Paragraph(f"Generated: {date.today().isoformat()}", styles["BookSub"]))
    story.append(Spacer(1, 0.8 * cm))
    story.append(Paragraph(
        "This is a study-book style reference. It is organized by concepts, not by repository folders. "
        "It covers the syllabus deeply and adds related professional topics useful for Sprint implementation, evaluations, interviews, and OCEAN L1 preparation.",
        styles["BookSub"],
    ))

    story.append(PageBreak())
    add_heading(story, styles, "How to Study This Book")
    add_bullets(story, styles, [
        "Read the main chapters first to build conceptual understanding.",
        "Use the syllabus focus sheets for day-by-day coverage without treating them as a folder or submission tracker.",
        "For every technical topic, practice one tiny working example and one spoken explanation.",
        "Before assessment, revise the self-test questions, code snippets, commands, and interview angles.",
    ])
    add_heading(story, styles, "Curriculum Coverage Summary", level=2)
    topic_counts = pd.Series([r.topic for r in rows]).value_counts().to_dict()
    add_table(story, styles, [(topic, str(count)) for topic, count in topic_counts.items()], [12.0 * cm, 3.0 * cm])

    add_heading(story, styles, "Core Learning Path", level=2)
    add_bullets(story, styles, [
        ".NET and C# fundamentals create the language base.",
        "Problem solving, testing, SQL, LINQ, and EF Core create backend confidence.",
        "HTML/CSS/JavaScript, MVC, and Web API create application-building ability.",
        "Azure DevOps, cloud fundamentals, networking, storage, PaaS, and data services create deployment readiness.",
        "Git, power skills, Sprint implementation, OCEAN L1, and Gen AI prep complete workplace readiness.",
    ])

    for chapter in CHAPTERS:
        add_chapter(story, styles, chapter)

    story.append(PageBreak())
    add_heading(story, styles, "Related Topics Beyond the Syllabus")
    story.append(para("These topics are strongly related to the Sprint programme and should be revised even where they are not explicitly listed as a separate syllabus day.", styles))
    add_table(story, styles, RELATED_TOPICS, [5.0 * cm, 12.4 * cm])

    for index, row in enumerate(rows, start=1):
        add_syllabus_card(story, styles, row, index)

    story.append(PageBreak())
    add_heading(story, styles, "Final Sprint Readiness Checklist")
    add_bullets(story, styles, [
        "I can create, build, run, and explain a .NET 8 project.",
        "I can explain C# types, OOP, exceptions, collections, delegates, LINQ, async, and C# 12 features.",
        "I can solve basic algorithm questions and discuss complexity.",
        "I can write xUnit/NUnit tests with assertions and parameterized data.",
        "I can design SQL tables with keys and relationships, write joins, create views/indexes/procedures, and read execution plans at a basic level.",
        "I can use EF Core with DbContext, migrations, Code First, DB First, Include, tracking, and repository pattern.",
        "I can build HTML/CSS/JavaScript forms and explain DOM events, validation, fetch, and storage.",
        "I can build MVC CRUD using controllers, views, view models, validation, layouts, partial views, and filters.",
        "I can design Web APIs with REST routes, DTOs, status codes, Swagger, CORS, authentication, logging, caching, and versioning.",
        "I can explain Azure DevOps Boards, Repos, Pipelines, Artifacts, and Test Plans.",
        "I can choose Azure services for compute, storage, networking, identity, App Service, Functions, Logic Apps, Azure SQL, and Cosmos DB.",
        "I can use Git branch, merge, conflict resolution, history inspection, and pull request workflow.",
        "I can present my project clearly, write professional emails, and answer behavioral questions with STAR.",
    ])

    doc.build(story, onFirstPage=on_page, onLaterPages=on_page)


def build_md(rows: list[SyllabusRow]):
    lines: list[str] = []
    lines.append("# Capgemini Sprint Programme Study Book")
    lines.append("")
    lines.append(f"Based on `{SYLLABUS}`")
    lines.append(f"Generated: {date.today().isoformat()}")
    lines.append("")
    for chapter in CHAPTERS:
        lines.append(f"## {chapter['title']}")
        lines.append("")
        for title, notes, code in chapter["sections"]:
            lines.append(f"### {title}")
            for note in notes:
                lines.append(f"- {note}")
            if code:
                lines.append("")
                lines.append("```text")
                lines.append(code)
                lines.append("```")
            lines.append("")
    lines.append("## Related Topics Beyond the Syllabus")
    for topic, why in RELATED_TOPICS:
        lines.append(f"- **{topic}:** {why}")
    lines.append("")
    lines.append("## Syllabus Focus Sheets")
    for index, row in enumerate(rows, start=1):
        cat = category(row.topic, row.detail)
        concepts = split_concepts(row.detail)
        title = concepts[0] if concepts else row.topic
        lines.append(f"### Focus Sheet {index}: {title}")
        lines.append(f"Topic family: {row.topic}")
        lines.append("")
        lines.append("Concepts:")
        for concept in concepts:
            lines.append(f"- {concept}")
        lines.append("")
        lines.append("Detailed notes:")
        for note in base_notes(cat) + concept_notes(concepts, cat):
            lines.append(f"- {note}")
        lines.append("")
        lines.append(f"Practice: {practice_for(cat)}")
        lines.append(f"Interview angle: {interview_for(cat)}")
        lines.append("")
    OUTPUT_MD.write_text("\n".join(lines), encoding="utf-8")


def main():
    rows = load_syllabus()
    build_md(rows)
    build_pdf(rows)
    print(f"Rows used: {len(rows)}")
    print(f"Wrote {OUTPUT_MD}")
    print(f"Wrote {OUTPUT_PDF}")


if __name__ == "__main__":
    main()
