from __future__ import annotations

from collections import Counter
from datetime import date
from pathlib import Path
from xml.etree import ElementTree as ET
from xml.sax.saxutils import escape

from reportlab.lib import colors
from reportlab.lib.enums import TA_CENTER
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import ParagraphStyle, getSampleStyleSheet
from reportlab.lib.units import cm
from reportlab.platypus import (
    LongTable,
    PageBreak,
    Paragraph,
    Preformatted,
    SimpleDocTemplate,
    Spacer,
    Table,
    TableStyle,
)


ROOT = Path(__file__).resolve().parent
OUTPUT_PDF = ROOT / "Capgemini_Sprint_Study_Notes.pdf"
OUTPUT_MD = ROOT / "Capgemini_Sprint_Study_Notes.md"


FOLDER_ANALYSIS = [
    ("WEEK-1", "MARCH-7", "Static site assets, CSS, images, JavaScript vendor files.", "HTML document structure, CSS linking, static asset organization, browser-side page setup."),
    ("WEEK-1", "MARCH-9", "CSS exercises.", "Selectors, styling rules, page layout basics, separation of style from markup."),
    ("WEEK-1", "MARCH-10", "HTML pages plus JavaScript for forms, arrays, and regex.", "DOM access, form validation, arrays, email validation with regular expressions."),
    ("WEEK-1", "MARCH-11", "MVC_INTRODUCTION project.", "ASP.NET Core MVC structure, controllers, views, models, routing, ViewBag/ViewData, simple action results."),
    ("WEEK-1", "MARCH-12", "Expanded MVC_INTRODUCTION project.", "Strongly typed views, passing single and multiple objects, employee/department view models, LINQ filtering."),
    ("WEEK-1", "MARCH-13", "CRUD_OPERATIONS and DOG-MANAGEMENT-APP.", "MVC CRUD screens, EF Core setup, SQLite/SQL Server packages, Razor forms, image upload, search."),
    ("WEEK-1", "MARCH-14", "DOG-MANAGEMENT-APP with migrations.", "EF Core migrations, persisted CRUD flow, model validation, file handling, Delete/Edit patterns."),
    ("WEEK-2", "MARCH-16", "CodeFirst, DBFirst, MARCH-16, Student_Course_Management_System.", "EF Core Code First, Database First, DbContext, products, Northwind, student-course-enrollment relationships."),
    ("WEEK-2", "MARCH-18", "DBFirst, LayoutAndSection, WEB-APPLICATION.", "Scaffolded MVC CRUD, layouts, sections, department/employee management, Northwind queries."),
    ("WEEK-2", "MARCH-19", "CodeFirst, INVOICE-APPLICATION, SETUP-APPLICATION.", "Data annotations, entity relationships, invoice/order domain modeling, customer-product-line-item flows."),
    ("WEEK-2", "MARCH-20", "CodeFirst, PARTIAL-VIEWS, ROUTING.", "Partial views, conventional routing, student routes, repository interface pattern, post model exercises."),
    ("WEEK-3", "MARCH-23", "JQUERY and STATE-MANAGEMENT.", "jQuery selectors/events, TempData, Session, Cookies, HttpContext, stateless web concepts."),
    ("WEEK-3", "MARCH-24", "WEBAPI-DEMO.", "ASP.NET Core Web API, API controllers, EF Core, Swagger/OpenAPI, employee endpoints."),
    ("WEEK-3", "MARCH-25", "EMPLOYEE-MANAGEMENT-SYS.", "Service layer, REST endpoints, pagination, file upload with IFormFile, validation, response typing."),
    ("WEEK-3", "MARCH-26", "Employee management continuation and MARCH-26 app.", "API refinement, service abstraction, DTO/request classes, MVC/Razor practice."),
    ("WEEK-3", "MARCH-27", "WebApilnAsp.", "Employee API plus MVC UI client, DTOs, EF Core, Swagger, ClosedXML package for Excel-oriented work."),
    ("WEEK-3", "MARCH-28", "PRODUCT-MANAGEMENT-SYSTEM and PRODUCT-UI.", "Full-stack product CRUD, JavaScript fetch, API-backed UI, validation and search."),
    ("WEEK-4", "MARCH-30", "WebApilnAsp.", "Web API with employee services, DTOs, EF Core, Identity/JWT groundwork."),
    ("WEEK-4", "MARCH-31", "WebApilnAsp continuation.", "Authentication and employee API refinement, DI, Swagger, SQL Server persistence."),
    ("WEEK-4", "APRIL-1", "AZURE-MVC and WebApilnAsp.", "Azure Blob Storage, containers/blobs, metadata, Identity, JWT, role policies, Swagger security."),
    ("WEEK-4", "APRIL-2", "WebApiInAsp.netcoreMvcDemo.", "ASP.NET Core Web API + MVC, Identity, JWT bearer authentication, role-based endpoints."),
    ("WEEK-4", "APRIL-3", "AZURE and WEB-APPLICATION.", "Azure portal practice, deployment/cloud service exposure, basic MVC web application review."),
    ("WEEK-5", "APRIL-6", "WEB-APPLICATION with AzureContext and screenshots.", "Azure SQL style persistence, EF migrations, Person CRUD in MVC."),
    ("WEEK-5", "APRIL-8", "Day64 ProductAPIClient.", "Consuming APIs from MVC, configurable API base URL, client-side product display."),
    ("WEEK-5", "APRIL-9", "AZURE-FUNCTIONS and CONSOLE-APP.", "Azure Functions isolated worker, HTTP trigger, logging, simple console project."),
    ("WEEK-5", "APRIL-10", "AzureFunctionExample and TangyAzureFunc.", "MVC client calling an Azure Function, JSON serialization, queue output binding."),
    ("WEEK-5", "APRIL-11", "Azure Function continuation.", "Function app repetition/refinement, queue-bound sales request workflow, Application Insights packages."),
    ("WEEK-6", "APRIL-13", "AzureSpookyLoginApp.", "MVC app calling external/function endpoint with HttpClient and Newtonsoft.Json."),
    ("WEEK-6", "APRIL-14", "INTRODUCTION text placeholder.", "Review point for Azure/cloud theory and recap notes."),
    ("WEEK-6", "APRIL-15", "KEY-VAULT project.", "Azure Key Vault concept, secret storage, configuration security, managed identity preparation."),
    ("WEEK-6", "APRIL-16", "COSMOS-DB-MVC-APPLICATION.", "Azure Cosmos DB SDK, containers, partition keys, SQL-like queries, MVC CRUD over NoSQL data."),
    ("WEEK-6", "APRIL-17", "FILE text placeholder.", "Review/submission placeholder for cloud module work."),
    ("WEEK-7", "APRIL-20", "Soft skills and personality development assessment.", "Communication, assessment readiness, workplace behavior, self-presentation."),
    ("WEEK-7", "APRIL-21", "Image submissions/materials.", "Evidence/screenshots or visual records; treat as task-submission artifacts."),
    ("WEEK-7", "APRIL-22", "Image submissions/materials.", "Evidence/screenshots or visual records; review alongside trainer instructions if available."),
    ("WEEK-7", "APRIL-23", "Image submissions/materials.", "Evidence/screenshots or visual records; likely non-code practical work."),
    ("WEEK-7", "APRIL-24", "INFO text about Git commands.", "Git status, add, commit, branch, push, pull, merge, repository publishing workflow."),
    ("WEEK-7", "APRIL-25", "INFO text about Git commands.", "Git command revision and GitHub upload workflow."),
    ("WEEK-8", "APRIL-1", "README with May 1 note.", "Administrative/date note; no major code content found."),
    ("WEEK-8", "APRIL-27", "Azure VNet and VM setup notes.", "Virtual networks, subnets, NSGs, VM deployment, private connectivity, nested virtualization."),
    ("WEEK-8", "APRIL-28", "APRIL28-PROJECT WEB-API.", "Employee API, DTOs, IdentityDbContext, role seeding, EF Core SQL Server, image uploads."),
    ("WEEK-8", "APRIL-29", "APRIL29-PROJECT employee API frontend.", "MVC frontend consuming API via IHttpClientFactory, multipart forms, CRUD screens, export call."),
    ("WEEK-8", "APRIL-30", "LibraryManagementAPI.", "Database-first EF model, authors/books/categories/members, borrowing and returning workflow APIs."),
]


CORE_SECTIONS = [
    {
        "title": "1. Web Fundamentals: HTML, CSS, JavaScript, Regex, jQuery",
        "why": "Week 1 starts with static web basics before ASP.NET Core. These are the foundations behind Razor views, MVC forms, and API-backed pages.",
        "study": [
            "HTML gives structure: forms, inputs, buttons, labels, tables, links, images, and script/style references.",
            "CSS controls presentation: selectors, specificity, box model, spacing, colors, positioning, responsive layout, and Bootstrap utility classes.",
            "JavaScript adds behavior: functions, variables, arrays, DOM lookup, event handlers, conditions, loops, and async fetch calls.",
            "Regex is useful for input checks such as email validation. Know anchors (^ and $), character classes, quantifiers, escaping, and test methods.",
            "jQuery is older but still appears in MVC templates and validation packages. Know selectors, event binding, hide/show, attr/css, and AJAX patterns.",
        ],
        "repo": [
            "WEEK-1/MARCH-10 has form validation and email regex examples.",
            "WEEK-3/MARCH-23/JQUERY has jQuery practice pages.",
            "WEEK-3/MARCH-28/PRODUCT-UI/app.js uses fetch, JSON, DOM card creation, edit mode, delete calls, and client-side search.",
        ],
        "watch": [
            "Client-side validation improves experience but does not replace server-side validation.",
            "When checking invalid zip code, the usual condition is empty OR wrong length. Empty AND wrong length can miss some invalid cases.",
            "Never trust file names, file extensions, form values, or client JSON without server validation.",
        ],
        "practice": [
            "Build a form with name, email, phone, and country fields; validate it with JavaScript and then repeat validation on the server.",
            "Use fetch to call a simple GET endpoint, render cards, then add POST/PUT/DELETE buttons.",
            "Write regex patterns for email, phone number, pincode, and simple password rules.",
        ],
    },
    {
        "title": "2. ASP.NET Core MVC Fundamentals",
        "why": "Most early projects are MVC applications. MVC separates request handling, data shape, and UI rendering.",
        "study": [
            "Model: classes that represent data or view data. Examples include Employee, Department, Dog, Product, Customer, Invoice, and view models.",
            "View: Razor .cshtml templates. They use @model, tag helpers, layout pages, partials, forms, and server-rendered HTML.",
            "Controller: classes ending in Controller. Actions return IActionResult, ViewResult, RedirectToAction, NotFound, File, JSON, or strings.",
            "Program.cs configures services and middleware: AddControllersWithViews, AddDbContext, UseStaticFiles, UseRouting, UseAuthentication, UseAuthorization, MapControllerRoute.",
            "Conventional route pattern usually looks like {controller=Home}/{action=Index}/{id?}. Attribute routing is common in Web API.",
            "Model binding maps form fields, query string values, route values, and files into action parameters or model objects.",
            "Validation uses data annotations, ModelState.IsValid, validation tag helpers, and _ValidationScriptsPartial for client-side unobtrusive validation.",
            "Anti-forgery tokens protect POST forms from CSRF attacks. MVC scaffolded forms include [ValidateAntiForgeryToken].",
        ],
        "repo": [
            "WEEK-1/MARCH-11 and MARCH-12 show controller actions, ViewBag, ViewData, strongly typed views, and employee/department models.",
            "WEEK-1/MARCH-13 and MARCH-14 show MVC CRUD screens for dog management.",
            "WEEK-2/MARCH-18/LayoutAndSection and WEEK-2/MARCH-20/PARTIAL-VIEWS focus on layout, sections, and reusable UI fragments.",
        ],
        "watch": [
            "Prefer strongly typed view models over ViewBag/ViewData for real apps; they are easier to validate and refactor.",
            "Use RedirectToAction after successful POST to avoid duplicate form submission on browser refresh.",
            "Keep controllers thin. Move business rules to services when logic grows.",
        ],
        "practice": [
            "Explain the full flow of a POST form: browser form -> route -> controller action -> model binding -> ModelState -> database -> redirect.",
            "Create a view model that combines Employee and Department and render it in a strongly typed Razor view.",
            "Add a partial view for a repeated employee card and render it from an index page.",
        ],
    },
    {
        "title": "3. Entity Framework Core and Database Work",
        "why": "Weeks 1 and 2 move heavily into EF Core, SQL Server, SQLite, Code First, Database First, migrations, and relationships.",
        "study": [
            "DbContext represents a database session. DbSet<T> represents a table or collection of entities.",
            "Code First starts from C# classes and creates database schema through migrations.",
            "Database First starts from an existing database and scaffolds models/DbContext.",
            "Migrations capture schema changes. Core commands: dotnet ef migrations add Name, dotnet ef database update, dotnet ef migrations remove.",
            "Use async database calls: ToListAsync, FindAsync, FirstOrDefaultAsync, SaveChangesAsync.",
            "Use AsNoTracking for read-only queries to reduce change-tracking overhead.",
            "Use Include and ThenInclude to load related data such as Book -> Author and Book -> Category.",
            "Data annotations include [Key], [Required], [Range], [Column(TypeName=...)], [DatabaseGenerated], [NotMapped].",
            "Relationships: one-to-many for Author -> Books, Category -> Books, Member -> BorrowRecords; many-to-many often needs a join entity.",
        ],
        "repo": [
            "WEEK-2/MARCH-16 compares CodeFirst and DBFirst, and includes Student_Course_Management_System.",
            "WEEK-2/MARCH-19/INVOICE-APPLICATION models Customer, Product, Invoice, and InvoiceLineItem.",
            "WEEK-8/APRIL-30/LibraryManagementAPI uses a database-first ApplicationDbContext with relationships for books, authors, categories, members, and borrow records.",
        ],
        "watch": [
            "Do not hard-code production connection strings in source files. Use appsettings, user secrets, environment variables, or Key Vault.",
            "Avoid overposting by binding only allowed properties or using DTO/view model classes.",
            "For important multi-step updates such as borrow/return, consider transactions and concurrency handling.",
        ],
        "practice": [
            "Design a Student-Course-Enrollment schema and explain keys and navigation properties.",
            "Write LINQ queries for filtering, sorting, pagination, grouping, and joins.",
            "Create a migration, inspect the generated code, apply it, and explain Up and Down methods.",
        ],
    },
    {
        "title": "4. CRUD Application Patterns",
        "why": "Dog, Product, Employee, Person, Invoice, and Library assignments all repeat CRUD. Mastering one clean CRUD flow helps with nearly every Sprint task.",
        "study": [
            "Create: GET action returns an empty form; POST validates input, adds entity, saves changes, redirects.",
            "Read: Index lists records; Details loads one record and returns NotFound if missing.",
            "Update: GET loads existing record; POST checks route id matches model id, validates, updates, catches concurrency issues, redirects.",
            "Delete: GET confirms; POST deletes and redirects. In APIs, DELETE returns NoContent or deleted DTO.",
            "Search and pagination should be applied before ToListAsync so filtering happens in the database.",
            "File upload CRUD needs upload directory creation, unique names, size/type validation, and old-file cleanup on update/delete.",
        ],
        "repo": [
            "WEEK-1/MARCH-14/DOG-MANAGEMENT-APP shows image upload and in-memory list CRUD.",
            "WEEK-3/MARCH-25 employee API adds pagination and safer image upload validation.",
            "WEEK-8/APRIL-28 employee API and WEEK-8/APRIL-29 frontend show API plus MVC-client CRUD with multipart forms.",
        ],
        "watch": [
            "In-memory static lists reset when the app restarts. Use a database for persistent data.",
            "Do not expose wwwroot upload paths unless you intentionally want public files.",
            "When deleting uploaded files, ensure the path stays inside the intended upload folder.",
        ],
        "practice": [
            "Implement product CRUD once in MVC and once as a Web API.",
            "Add search and pagination to an index endpoint.",
            "Add server-side file validation for extension, content type, and max size.",
        ],
    },
    {
        "title": "5. State Management in ASP.NET Core",
        "why": "HTTP is stateless, so Week 3 introduces practical ways to carry small pieces of state between requests.",
        "study": [
            "ViewBag/ViewData last for the current request only.",
            "TempData survives one redirect/request and is useful for success/error messages. TempData.Keep preserves it longer.",
            "Session stores data per user session on the server, referenced by a session cookie.",
            "Cookies store small values in the browser and are sent with requests. Use security flags for sensitive scenarios.",
            "Static fields and instance fields in controllers are not reliable per-user state. Controllers are created per request, and static data is shared.",
        ],
        "repo": [
            "WEEK-3/MARCH-23/STATE-MANAGEMENT uses TempData, Session, Cookie comments, and HttpContext.",
            "The LoginController demonstrates session write/remove patterns, even though the Welcome condition should be reviewed carefully.",
        ],
        "watch": [
            "Session is convenient but can hide scaling problems in distributed deployments.",
            "Never store passwords, tokens, or sensitive data in plain cookies.",
            "For authenticated identity, use ASP.NET Core Identity or JWT rather than hand-made session checks.",
        ],
        "practice": [
            "Create a flash message with TempData after a POST redirect.",
            "Store a username in Session, display it, then clear it on logout.",
            "Explain the difference between authentication state and arbitrary session state.",
        ],
    },
    {
        "title": "6. ASP.NET Core Web API and REST",
        "why": "Weeks 3, 4, and 8 focus on API projects that expose data to JavaScript clients, MVC clients, and Swagger.",
        "study": [
            "[ApiController] enables automatic model validation behavior, binding source inference, and better error responses.",
            "[Route(\"api/[controller]\")] maps controller names to API paths. [HttpGet], [HttpPost], [HttpPut], [HttpDelete] declare operations.",
            "Use ControllerBase for APIs; use Controller when you also return views.",
            "Return appropriate status codes: 200 OK, 201 Created, 204 NoContent, 400 BadRequest, 401 Unauthorized, 403 Forbidden, 404 NotFound, 409 Conflict.",
            "DTOs keep API contracts separate from EF entities. This reduces overposting and lets you control response shape.",
            "Swagger/OpenAPI documents endpoints and allows test calls from the browser.",
            "IHttpClientFactory creates configured HttpClient instances for calling external APIs from MVC apps.",
            "CORS is needed when a browser-based frontend calls an API from a different origin.",
        ],
        "repo": [
            "WEEK-3/MARCH-24/WEBAPI-DEMO introduces API controllers and Swagger.",
            "WEEK-3/MARCH-28 combines PRODUCT-MANAGEMENT-SYSTEM with PRODUCT-UI fetch calls.",
            "WEEK-8/APRIL-29 uses IHttpClientFactory, multipart form data, JSON deserialization, and MVC views over an API.",
        ],
        "watch": [
            "Use CreatedAtAction for successful POST when the new resource has a URL.",
            "Do not return EF entities with large circular navigation graphs; shape responses with DTOs.",
            "Validate page and pageSize to avoid negative Skip values or huge queries.",
        ],
        "practice": [
            "Design a REST API for books with GET all, GET by id, POST, PUT, DELETE, and search.",
            "Add Swagger annotations or response types to document success and error cases.",
            "Consume your API from an MVC frontend and from a plain HTML/JS frontend.",
        ],
    },
    {
        "title": "7. Authentication, Authorization, Identity, and JWT",
        "why": "Week 4 and Week 8 introduce IdentityDbContext, roles, JWT bearer tokens, lockout, and policy-based authorization.",
        "study": [
            "Authentication answers: who is this user? Authorization answers: what can this user do?",
            "ASP.NET Core Identity provides users, roles, password hashing, lockout, claims, and token providers.",
            "UserManager creates/finds users; RoleManager manages roles; SignInManager validates passwords/sign-in attempts.",
            "JWT tokens usually include subject/user id, name, email, roles, issuer, audience, expiry, and a signature.",
            "JWT bearer middleware validates issuer, audience, expiry, and signing key before setting HttpContext.User.",
            "Role-based authorization checks roles directly; policy-based authorization names rules such as AdminOnly or EmployeeWrite.",
            "Swagger can be configured with Bearer security so protected endpoints can be tested with a token.",
        ],
        "repo": [
            "WEEK-4/APRIL-1/WebApilnAsp has AuthService, JwtOptions, BootstrapAdminOptions, Identity seeding, roles, and policies.",
            "WEEK-8/APRIL-28/WEB-API uses IdentityDbContext<IdentityUser> and seeds Admin, User, and HR roles.",
        ],
        "watch": [
            "JWT secrets must not be committed to source control. Store them in user secrets, environment variables, or Key Vault.",
            "Tokens should be short-lived; production systems often add refresh tokens.",
            "Use HTTPS for token-bearing requests.",
        ],
        "practice": [
            "Trace login: credentials -> user lookup -> password check -> claims -> JWT -> client Authorization header -> protected endpoint.",
            "Create Admin and User roles and protect one endpoint with [Authorize(Roles = \"Admin\")].",
            "Explain 401 versus 403 in an interview.",
        ],
    },
    {
        "title": "8. Azure Storage, Functions, Key Vault, Cosmos DB, and Networking",
        "why": "Weeks 4, 5, 6, and 8 move from local apps into cloud services and deployment-style thinking.",
        "study": [
            "Azure Blob Storage stores files/objects inside containers. Use BlobServiceClient, BlobContainerClient, and BlobClient.",
            "Blob metadata stores custom key/value information such as title and comment. Public access should be controlled carefully.",
            "Azure Functions are event-driven. The repo uses isolated worker functions with HttpTrigger and QueueOutput.",
            "Queue output binding lets a function receive HTTP input and place a message into Azure Storage Queue for later processing.",
            "Azure Key Vault stores secrets, keys, and certificates. Apps should access it through managed identity where possible.",
            "Cosmos DB is globally distributed NoSQL. Important ideas: database, container, item, id, partition key, RU/s, consistency, query iterator.",
            "Azure Virtual Network gives private network space. Subnets divide it. NSGs control allowed inbound/outbound traffic.",
            "Azure VMs can have public and private IPs. VM-to-VM communication inside a VNet should use private IPs when possible.",
            "Nested virtualization means running a VM inside another Azure VM and needs supported VM sizes.",
        ],
        "repo": [
            "WEEK-4/APRIL-1/AZURE-MVC manages containers and blobs and reads metadata.",
            "WEEK-5/APRIL-9 to APRIL-11 contains HTTP-trigger functions and queue output.",
            "WEEK-6/APRIL-16/COSMOS-DB-MVC-APPLICATION performs MVC CRUD against Cosmos DB.",
            "WEEK-8/APRIL-27 has VNet, subnet, NSG, VM, VM-to-VM, and nested virtualization notes.",
        ],
        "watch": [
            "Parameterized queries are safer than string interpolation in Cosmos DB search queries.",
            "Stop unused VMs and storage resources to avoid cloud cost surprises.",
            "Use least-privilege NSG rules. Do not expose ports that are not required.",
        ],
        "practice": [
            "Upload a file to Blob Storage, attach metadata, list blobs, and delete one blob.",
            "Create an HTTP-trigger Azure Function that writes a message to a queue.",
            "Draw a VNet with two subnets, two VMs, an NSG, and allowed RDP/SSH rules.",
        ],
    },
    {
        "title": "9. Git, GitHub, and Professional Workflow",
        "why": "Week 7 explicitly mentions Git commands for uploading code to GitHub. This matters in every project team.",
        "study": [
            "git status shows changed files. git add stages changes. git commit records a snapshot. git push uploads commits.",
            "git clone copies a remote repository. git pull fetches and merges remote changes. git fetch only downloads remote data.",
            "Branches isolate work. Use git switch -c feature-name, commit locally, push, and open a pull request.",
            "git diff shows unstaged changes. git diff --staged shows staged changes.",
            "git log --oneline --graph helps understand history.",
            ".gitignore prevents generated files such as bin, obj, .vs, node_modules, and secrets from being committed.",
            "Avoid destructive commands unless you understand them: git reset --hard and force push can discard work.",
        ],
        "repo": [
            "WEEK-7/APRIL-24 and APRIL-25 INFO files mention Git commands and GitHub upload workflow.",
            "The current repository itself is a day-wise training record and can be used to practice commit hygiene.",
        ],
        "watch": [
            "Do not commit secrets, connection strings, tokens, or large generated build folders.",
            "Commit messages should say what changed and why.",
            "Pull before pushing when collaborating to avoid conflicts.",
        ],
        "practice": [
            "Create a branch, edit a small README, commit it, inspect diff/log, then merge it back.",
            "Resolve a simple merge conflict in a text file.",
            "Explain staging area versus working tree versus repository history.",
        ],
    },
    {
        "title": "10. Capstone Domain Modeling: Employee and Library APIs",
        "why": "Week 8 consolidates the earlier material into realistic API-backed systems.",
        "study": [
            "Employee API: DTOs, EF Core, SQL Server, image upload, search, pagination, and role seeding.",
            "Employee MVC frontend: consumes API using IHttpClientFactory, serializes/deserializes JSON, sends multipart forms, and renders CRUD views.",
            "Library API: Authors, Books, Categories, Members, BorrowRecords, navigation properties, Include queries, and borrow/return actions.",
            "Borrow flow: validate book exists, validate available copies, validate member exists, decrement copies, create borrow record, save.",
            "Return flow: validate borrow record exists, reject double return, load book, set return date/status, increment copies, save.",
        ],
        "repo": [
            "WEEK-8/APRIL-28 has WEB-API with employee services and DTOs.",
            "WEEK-8/APRIL-29 has EMPLOYEE-API-FRONTEND that calls the API.",
            "WEEK-8/APRIL-30 has LibraryManagementAPI with BooksController and BorrowController.",
        ],
        "watch": [
            "Borrow/return should ideally use transactions to avoid copy-count inconsistencies.",
            "Directly setting EntityState.Modified can update all columns; DTO-based updates are safer.",
            "Database-first scaffolding may include warnings about connection strings. Move secrets out of source.",
        ],
        "practice": [
            "Add a due date and fine calculation to BorrowRecord.",
            "Add search by book title/author/category and pagination.",
            "Create DTOs so API responses do not expose full EF navigation graphs.",
        ],
    },
]


MISSING_TOPICS = [
    ("Clean Architecture and SOLID", "The repo uses controllers, DbContext, and some services. Study single responsibility, dependency inversion, service boundaries, and why controllers should stay thin."),
    ("Testing", "Add xUnit tests for services, WebApplicationFactory integration tests for APIs, and mock-based tests for HttpClient/API clients."),
    ("Global Exception Handling", "Learn middleware or filters that convert exceptions into consistent ProblemDetails responses."),
    ("Logging and Monitoring", "Use ILogger, structured logs, Application Insights, log levels, correlation ids, and request tracing."),
    ("Configuration and Secrets", "Use appsettings by environment, user secrets locally, environment variables in deployment, and Azure Key Vault for cloud secrets."),
    ("CORS and Browser Security", "Know why browser clients need CORS, and how to restrict allowed origins/methods/headers."),
    ("Transactions and Concurrency", "Use database transactions for borrow/return and RowVersion/concurrency tokens for simultaneous updates."),
    ("API Versioning", "Learn URL/header versioning, backward compatibility, and deprecating old endpoints."),
    ("Validation Libraries", "Data annotations are present; also know FluentValidation for larger validation rules."),
    ("Object Mapping", "Manual DTO mapping is used; learn AutoMapper or explicit mapper classes and when each is appropriate."),
    ("SQL Performance", "Study indexes, query plans, N+1 queries, Include cost, pagination strategy, and AsNoTracking."),
    ("Deployment and CI/CD", "Add GitHub Actions, dotnet build/test, publish artifacts, Azure App Service deployment, and environment-specific configuration."),
    ("Docker Basics", "Understand Dockerfile, image, container, ports, volumes, and running SQL Server locally in a container."),
    ("Security Hardening", "HTTPS, secure cookies, authorization checks, upload scanning, rate limiting, password policy, token expiry, and least privilege."),
    ("Accessibility and Responsive UI", "Use labels, keyboard navigation, focus states, color contrast, and responsive Bootstrap layouts."),
]


INTERVIEW_QA = [
    ("What is MVC?", "MVC separates an app into Models for data, Views for UI, and Controllers for request handling and coordination."),
    ("What is the difference between ViewBag, ViewData, TempData, and Session?", "ViewBag/ViewData live for one request. TempData survives redirects briefly. Session stores per-user server-side data across requests."),
    ("What is model binding?", "ASP.NET Core maps route values, query strings, forms, headers, JSON bodies, and uploaded files into action parameters or model objects."),
    ("Why use DTOs?", "DTOs protect domain/EF entities, avoid overposting, shape API responses, and keep contracts stable."),
    ("What does [ApiController] do?", "It improves API behavior with automatic model validation responses, binding inference, and better client error responses."),
    ("Code First versus Database First?", "Code First starts from C# classes and migrations. Database First scaffolds classes from an existing database."),
    ("What is DbContext?", "It represents a session with the database and tracks entities, queries, and changes."),
    ("Why use async EF calls?", "They free the request thread while waiting on I/O and improve scalability under load."),
    ("What is dependency injection?", "A pattern where dependencies are supplied by the framework rather than manually constructed, improving testability and separation."),
    ("What is middleware?", "Components in the request pipeline that can inspect, modify, handle, or pass requests and responses."),
    ("401 versus 403?", "401 means not authenticated or invalid credentials. 403 means authenticated but not authorized."),
    ("What is JWT?", "A signed token containing claims used by APIs to authenticate requests without server-side session lookup."),
    ("What are roles and policies?", "Roles group users by responsibility; policies define named authorization rules, often based on roles or claims."),
    ("What is Swagger/OpenAPI?", "A machine-readable API description with UI tooling for documentation and testing."),
    ("What is CORS?", "A browser security policy that controls cross-origin API calls from frontend code."),
    ("What is Azure Blob Storage?", "Object storage for files such as images, documents, and logs, grouped into containers."),
    ("What is an Azure Function?", "A serverless function triggered by events such as HTTP requests, queues, timers, or blob changes."),
    ("What is Cosmos DB?", "A globally distributed NoSQL database service with containers, partition keys, RUs, and multiple APIs."),
    ("What is a VNet?", "A private network boundary in Azure that lets resources communicate securely using private IPs."),
    ("What is an NSG?", "A Network Security Group that filters inbound and outbound traffic by source, destination, port, and protocol."),
    ("What does git add do?", "It stages selected changes for the next commit."),
    ("What is a pull request?", "A review workflow for proposing branch changes before merging them into a target branch."),
]


COMMANDS = [
    ("Run MVC/API app", "dotnet run"),
    ("Restore packages", "dotnet restore"),
    ("Build solution/project", "dotnet build"),
    ("Add EF migration", "dotnet ef migrations add InitialCreate"),
    ("Apply EF migration", "dotnet ef database update"),
    ("Remove last migration", "dotnet ef migrations remove"),
    ("Scaffold DB First example", "dotnet ef dbcontext scaffold \"<connection-string>\" Microsoft.EntityFrameworkCore.SqlServer -o Models"),
    ("Git status", "git status"),
    ("Stage files", "git add ."),
    ("Commit", "git commit -m \"message\""),
    ("Create branch", "git switch -c feature/topic"),
    ("Push branch", "git push -u origin feature/topic"),
    ("View history", "git log --oneline --graph --decorate"),
]


def is_ignored(path: Path) -> bool:
    parts = [part.lower() for part in path.relative_to(ROOT).parts]
    ignored = {".git", "bin", "obj", "build", ".vs", ".dotnet-home", ".packages", ".codex-run"}
    if any(part in ignored for part in parts):
        return True
    return "wwwroot" in parts and "lib" in parts


def rel(path: Path) -> str:
    return str(path.relative_to(ROOT)).replace("/", "\\")


def read_csproj(path: Path) -> dict[str, str]:
    text = path.read_text(encoding="utf-8", errors="ignore")
    try:
        root = ET.fromstring(text)
    except ET.ParseError:
        return {"target": "", "packages": "", "sdk": ""}

    target = ""
    packages: list[str] = []
    for element in root.iter():
        tag = element.tag.split("}", 1)[-1]
        if tag == "TargetFramework" and element.text:
            target = element.text.strip()
        if tag == "PackageReference":
            include = element.attrib.get("Include", "").strip()
            version = element.attrib.get("Version", "").strip()
            if not version:
                for child in element:
                    if child.tag.split("}", 1)[-1] == "Version" and child.text:
                        version = child.text.strip()
            if include:
                packages.append(f"{include} {version}".strip())
    return {
        "target": target,
        "packages": ", ".join(packages),
        "sdk": root.attrib.get("Sdk", ""),
    }


def infer_tech(project_path: str, packages: str, sdk: str) -> str:
    value = " ".join([project_path, packages, sdk]).lower()
    tech: list[str] = []
    checks = [
        ("Microsoft.NET.Sdk.Web".lower(), "ASP.NET Core"),
        ("entityframeworkcore", "EF Core"),
        ("sqlserver", "SQL Server"),
        ("sqlite", "SQLite"),
        ("identity", "Identity"),
        ("jwtbearer", "JWT"),
        ("swashbuckle", "Swagger"),
        ("closedxml", "Excel/ClosedXML"),
        ("azure.storage.blobs", "Azure Blob Storage"),
        ("azure.functions", "Azure Functions"),
        ("storage.queues", "Azure Queue"),
        ("azure.cosmos", "Cosmos DB"),
        ("newtonsoft", "Newtonsoft.Json"),
    ]
    for needle, label in checks:
        if needle in value and label not in tech:
            tech.append(label)
    if "web-api" in value or "webapi" in value or "api" in Path(project_path).name.lower():
        if "Web API" not in tech:
            tech.append("Web API")
    if "mvc" in value or "views" in value or "application" in value:
        if "MVC/Razor" not in tech:
            tech.append("MVC/Razor")
    return ", ".join(tech[:7]) or "ASP.NET/Core practice"


def get_inventory():
    files = [path for path in ROOT.rglob("*") if path.is_file() and not is_ignored(path)]
    ext_counts = Counter((path.suffix.lower() or "[no extension]") for path in files)
    projects = []
    for csproj in sorted(ROOT.rglob("*.csproj")):
        if is_ignored(csproj):
            continue
        info = read_csproj(csproj)
        projects.append(
            {
                "project": csproj.stem,
                "path": rel(csproj),
                "target": info["target"],
                "tech": infer_tech(rel(csproj), info["packages"], info["sdk"]),
                "packages": info["packages"],
            }
        )
    return files, ext_counts, projects


def make_styles():
    styles = getSampleStyleSheet()
    styles.add(
        ParagraphStyle(
            name="CoverTitle",
            parent=styles["Title"],
            alignment=TA_CENTER,
            fontSize=25,
            leading=31,
            spaceAfter=20,
            textColor=colors.HexColor("#17324D"),
        )
    )
    styles.add(
        ParagraphStyle(
            name="CoverSub",
            parent=styles["BodyText"],
            alignment=TA_CENTER,
            fontSize=11,
            leading=15,
            textColor=colors.HexColor("#3F4D5A"),
        )
    )
    styles.add(
        ParagraphStyle(
            name="H1x",
            parent=styles["Heading1"],
            fontSize=17,
            leading=22,
            spaceBefore=14,
            spaceAfter=8,
            textColor=colors.HexColor("#17324D"),
        )
    )
    styles.add(
        ParagraphStyle(
            name="H2x",
            parent=styles["Heading2"],
            fontSize=13,
            leading=17,
            spaceBefore=10,
            spaceAfter=6,
            textColor=colors.HexColor("#284B63"),
        )
    )
    styles.add(
        ParagraphStyle(
            name="Bodyx",
            parent=styles["BodyText"],
            fontSize=9.4,
            leading=13,
            spaceAfter=5,
        )
    )
    styles.add(
        ParagraphStyle(
            name="Smallx",
            parent=styles["BodyText"],
            fontSize=7.6,
            leading=10,
        )
    )
    styles.add(
        ParagraphStyle(
            name="Bulletx",
            parent=styles["BodyText"],
            leftIndent=12,
            firstLineIndent=-8,
            fontSize=9.2,
            leading=12,
            spaceAfter=3,
        )
    )
    styles.add(
        ParagraphStyle(
            name="CodeBlock",
            parent=styles["Code"],
            fontName="Courier",
            fontSize=7.6,
            leading=9,
            leftIndent=6,
            rightIndent=6,
            spaceBefore=4,
            spaceAfter=6,
        )
    )
    return styles


def P(text: object, styles, style: str = "Bodyx") -> Paragraph:
    safe = escape(str(text)).replace("\n", "<br/>")
    return Paragraph(safe, styles[style])


def heading(story, styles, text: str, level: int = 1):
    story.append(Paragraph(escape(text), styles["H1x" if level == 1 else "H2x"]))


def bullets(story, styles, items: list[str]):
    for item in items:
        story.append(Paragraph("- " + escape(item), styles["Bulletx"]))


def table(story, styles, headers, rows, col_widths):
    data = [[P(h, styles, "Smallx") for h in headers]]
    for row in rows:
        data.append([P(cell, styles, "Smallx") for cell in row])
    t = LongTable(data, colWidths=col_widths, repeatRows=1, hAlign="LEFT")
    t.setStyle(
        TableStyle(
            [
                ("BACKGROUND", (0, 0), (-1, 0), colors.HexColor("#DDEAF3")),
                ("TEXTCOLOR", (0, 0), (-1, 0), colors.HexColor("#17324D")),
                ("GRID", (0, 0), (-1, -1), 0.25, colors.HexColor("#B9C6D1")),
                ("VALIGN", (0, 0), (-1, -1), "TOP"),
                ("ROWBACKGROUNDS", (0, 1), (-1, -1), [colors.white, colors.HexColor("#F7FAFC")]),
                ("LEFTPADDING", (0, 0), (-1, -1), 4),
                ("RIGHTPADDING", (0, 0), (-1, -1), 4),
                ("TOPPADDING", (0, 0), (-1, -1), 3),
                ("BOTTOMPADDING", (0, 0), (-1, -1), 3),
            ]
        )
    )
    story.append(t)
    story.append(Spacer(1, 8))


def on_page(canvas, doc):
    canvas.saveState()
    canvas.setFont("Helvetica", 8)
    canvas.setFillColor(colors.HexColor("#52606D"))
    canvas.drawString(1.6 * cm, 1.0 * cm, "Capgemini Sprint Study Notes")
    canvas.drawRightString(A4[0] - 1.6 * cm, 1.0 * cm, f"Page {doc.page}")
    canvas.restoreState()


def build_pdf(ext_counts: Counter, projects: list[dict[str, str]]):
    styles = make_styles()
    doc = SimpleDocTemplate(
        str(OUTPUT_PDF),
        pagesize=A4,
        rightMargin=1.45 * cm,
        leftMargin=1.45 * cm,
        topMargin=1.35 * cm,
        bottomMargin=1.55 * cm,
        title="Capgemini Sprint Study Notes",
        author="Codex",
    )

    story = []
    story.append(Spacer(1, 3.0 * cm))
    story.append(Paragraph("Capgemini Sprint Programme", styles["CoverTitle"]))
    story.append(Paragraph("Detailed Study Notes Based on Shiv-Assignments", styles["CoverTitle"]))
    story.append(Spacer(1, 0.5 * cm))
    story.append(Paragraph(f"Workspace analysed: {escape(str(ROOT))}", styles["CoverSub"]))
    story.append(Paragraph(f"Generated: {date.today().isoformat()}", styles["CoverSub"]))
    story.append(Spacer(1, 1.0 * cm))
    story.append(
        Paragraph(
            "This PDF turns the week-by-week assignment folders into a revision guide. "
            "It covers the topics present in the workspace and adds closely related topics "
            "that are worth preparing before assessments, interviews, and practical labs.",
            styles["CoverSub"],
        )
    )
    story.append(PageBreak())

    heading(story, styles, "How to Use These Notes")
    bullets(
        story,
        styles,
        [
            "Start with the folder analysis to understand the learning arc from web basics to Azure and APIs.",
            "Revise the core sections topic by topic, then implement the practice prompts in a small app.",
            "Use the missing-topic checklist to fill gaps before Sprint evaluation or technical discussion.",
            "Use the interview Q&A as quick oral revision; answer aloud without reading, then compare.",
        ],
    )

    heading(story, styles, "Repository Scan Summary")
    total_files = sum(ext_counts.values())
    story.append(P(f"Analysed non-generated files: {total_files}. Ignored .git, bin, obj, build, .vs, .dotnet-home, packages, and wwwroot/lib vendor folders.", styles))
    top_ext = ext_counts.most_common(12)
    table(
        story,
        styles,
        ["Extension", "Count"],
        [(ext, str(count)) for ext, count in top_ext],
        [5.0 * cm, 3.0 * cm],
    )

    heading(story, styles, "Folder-by-Folder Analysis")
    story.append(P("This table covers the visible week/date folders and maps them to study topics.", styles))
    table(
        story,
        styles,
        ["Week", "Date", "What is in the folder", "Study focus"],
        FOLDER_ANALYSIS,
        [2.0 * cm, 2.3 * cm, 5.3 * cm, 8.1 * cm],
    )

    heading(story, styles, "Detected .NET Project Matrix")
    project_rows = [
        (
            p["project"],
            p["target"] or "-",
            p["tech"],
            p["path"],
        )
        for p in projects
    ]
    table(
        story,
        styles,
        ["Project", "Target", "Technology clues", "Path"],
        project_rows,
        [3.7 * cm, 1.7 * cm, 5.5 * cm, 6.8 * cm],
    )

    heading(story, styles, "Learning Roadmap")
    roadmap_rows = [
        ("Stage 1", "Web basics", "HTML forms, CSS, JavaScript validation, arrays, regex, jQuery, fetch."),
        ("Stage 2", "MVC", "Controllers, actions, Razor views, layouts, partials, routing, model binding, validation."),
        ("Stage 3", "Data access", "EF Core, SQL Server, SQLite, Code First, DB First, migrations, relationships, LINQ."),
        ("Stage 4", "CRUD systems", "Dog, product, employee, invoice, person, and student/course applications."),
        ("Stage 5", "Web API", "REST endpoints, DTOs, Swagger, status codes, pagination, search, file upload."),
        ("Stage 6", "Security", "Identity, JWT bearer tokens, roles, policies, secrets, authenticated API calls."),
        ("Stage 7", "Azure", "Blob Storage, Functions, Queues, Key Vault, Cosmos DB, Azure SQL, VNet/VM/NSG."),
        ("Stage 8", "Professional workflow", "Git/GitHub, review habits, deployment readiness, testing, logging."),
    ]
    table(story, styles, ["Stage", "Theme", "What to master"], roadmap_rows, [2.3 * cm, 4.0 * cm, 11.4 * cm])

    for section in CORE_SECTIONS:
        heading(story, styles, section["title"])
        story.append(P(section["why"], styles))
        heading(story, styles, "Study Notes", level=2)
        bullets(story, styles, section["study"])
        heading(story, styles, "Where It Appears in Your Folders", level=2)
        bullets(story, styles, section["repo"])
        heading(story, styles, "Watch Points", level=2)
        bullets(story, styles, section["watch"])
        heading(story, styles, "Practice Tasks", level=2)
        bullets(story, styles, section["practice"])

    heading(story, styles, "Missing but Related Topics to Add to Prep")
    story.append(P("These topics are not equally visible in the folder contents, but they are strongly related to the programme path and common .NET/Azure assessment expectations.", styles))
    table(
        story,
        styles,
        ["Topic", "Why it matters"],
        MISSING_TOPICS,
        [5.0 * cm, 12.7 * cm],
    )

    heading(story, styles, "Command Cheat Sheet")
    table(
        story,
        styles,
        ["Task", "Command"],
        COMMANDS,
        [5.0 * cm, 12.7 * cm],
    )

    heading(story, styles, "Key Code Patterns to Memorize")
    patterns = [
        (
            "MVC POST pattern",
            """[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Model model)
{
    if (!ModelState.IsValid) return View(model);
    _context.Add(model);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}""",
        ),
        (
            "API GET by id pattern",
            """[HttpGet("{id}")]
public async Task<ActionResult<Dto>> GetById(int id)
{
    var item = await _service.GetByIdAsync(id);
    return item is null ? NotFound() : Ok(item);
}""",
        ),
        (
            "Pagination pattern",
            """page = Math.Max(page, 1);
pageSize = Math.Clamp(pageSize, 1, 100);
var items = await query
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();""",
        ),
        (
            "Safe upload outline",
            """Validate extension and size.
Create upload folder if missing.
Generate a GUID file name.
Copy stream to wwwroot/uploads.
Store only the relative URL/path.
Delete old file on update/delete when appropriate.""",
        ),
    ]
    for title, code in patterns:
        heading(story, styles, title, level=2)
        story.append(Preformatted(code, styles["CodeBlock"]))

    heading(story, styles, "Interview and Viva Q&A")
    qa_rows = [(q, a) for q, a in INTERVIEW_QA]
    table(story, styles, ["Question", "Short answer"], qa_rows, [6.2 * cm, 11.5 * cm])

    heading(story, styles, "Seven-Day Revision Plan")
    plan_rows = [
        ("Day 1", "Frontend + MVC basics", "Revise HTML/CSS/JS, Razor, controllers, actions, routing, ViewBag/ViewData/TempData."),
        ("Day 2", "EF Core", "Build one Code First model, create migration, run CRUD, explain relationships and LINQ."),
        ("Day 3", "MVC CRUD", "Rebuild a Dog/Product style CRUD app with validation, search, and file upload."),
        ("Day 4", "Web API", "Create REST endpoints with DTOs, Swagger, pagination, search, and proper status codes."),
        ("Day 5", "Security", "Implement Identity/JWT login, roles, policies, and protected endpoints."),
        ("Day 6", "Azure", "Review Blob, Functions, Queue, Key Vault, Cosmos DB, VNet/VM/NSG and deployment concepts."),
        ("Day 7", "Capstone + Git", "Explain the employee and library APIs, run Git workflow practice, answer interview Q&A aloud."),
    ]
    table(story, styles, ["Day", "Focus", "Output"], plan_rows, [2.0 * cm, 4.0 * cm, 11.7 * cm])

    heading(story, styles, "Final Assessment Checklist")
    bullets(
        story,
        styles,
        [
            "I can explain MVC request flow and build a CRUD screen without copying.",
            "I can create an EF Core model, migration, relationship, and LINQ query.",
            "I can design REST endpoints and return correct status codes.",
            "I can explain DTOs, model binding, validation, and overposting.",
            "I can secure an endpoint with JWT and roles.",
            "I can describe Blob Storage, Azure Functions, Queues, Key Vault, Cosmos DB, VNet, VM, and NSG.",
            "I can use Git branch/add/commit/push/pull and explain a pull request.",
            "I can discuss missing production concerns: tests, logs, secrets, transactions, CI/CD, and deployment.",
        ],
    )

    doc.build(story, onFirstPage=on_page, onLaterPages=on_page)


def build_markdown(ext_counts: Counter, projects: list[dict[str, str]]):
    lines: list[str] = []
    lines.append("# Capgemini Sprint Programme Study Notes")
    lines.append("")
    lines.append(f"Workspace analysed: `{ROOT}`")
    lines.append(f"Generated: {date.today().isoformat()}")
    lines.append("")
    lines.append("## Repository Scan Summary")
    lines.append("")
    lines.append(f"- Analysed non-generated files: {sum(ext_counts.values())}")
    lines.append("- Ignored generated/vendor folders: `.git`, `bin`, `obj`, `build`, `.vs`, `.dotnet-home`, package folders, and `wwwroot/lib`.")
    lines.append("")
    lines.append("Top extensions:")
    for ext, count in ext_counts.most_common(12):
        lines.append(f"- `{ext}`: {count}")
    lines.append("")
    lines.append("## Folder-by-Folder Analysis")
    lines.append("")
    for week, day, contents, focus in FOLDER_ANALYSIS:
        lines.append(f"### {week} / {day}")
        lines.append(f"- Contents: {contents}")
        lines.append(f"- Study focus: {focus}")
        lines.append("")
    lines.append("## Detected .NET Projects")
    lines.append("")
    for p in projects:
        lines.append(f"- `{p['path']}` - {p['target'] or '-'} - {p['tech']}")
    lines.append("")
    for section in CORE_SECTIONS:
        lines.append(f"## {section['title']}")
        lines.append("")
        lines.append(section["why"])
        lines.append("")
        lines.append("Study notes:")
        for item in section["study"]:
            lines.append(f"- {item}")
        lines.append("")
        lines.append("Where it appears:")
        for item in section["repo"]:
            lines.append(f"- {item}")
        lines.append("")
        lines.append("Watch points:")
        for item in section["watch"]:
            lines.append(f"- {item}")
        lines.append("")
        lines.append("Practice tasks:")
        for item in section["practice"]:
            lines.append(f"- {item}")
        lines.append("")
    lines.append("## Missing but Related Topics")
    lines.append("")
    for topic, why in MISSING_TOPICS:
        lines.append(f"- **{topic}:** {why}")
    lines.append("")
    lines.append("## Command Cheat Sheet")
    lines.append("")
    for task, command in COMMANDS:
        lines.append(f"- {task}: `{command}`")
    lines.append("")
    lines.append("## Interview and Viva Q&A")
    lines.append("")
    for question, answer in INTERVIEW_QA:
        lines.append(f"- **{question}** {answer}")
    lines.append("")
    OUTPUT_MD.write_text("\n".join(lines), encoding="utf-8")


def main():
    _, ext_counts, projects = get_inventory()
    build_markdown(ext_counts, projects)
    build_pdf(ext_counts, projects)
    print(f"Wrote {OUTPUT_MD}")
    print(f"Wrote {OUTPUT_PDF}")


if __name__ == "__main__":
    main()
