# README Diagram Options

These diagrams are based on the project report and the implemented ASP.NET MVC controllers. Choose one diagram to place in the main README, or combine two if you want a richer presentation.

## Option 1: Role-Based Use Case Map

Best for showing what each user type can do.

```mermaid
flowchart LR
    Guest["Guest / Visitor"]
    Customer["Customer"]
    Provider["Service Provider"]
    Admin["Admin"]

    subgraph Public["Public Marketplace"]
        Browse["Browse services"]
        Categories["View wedding, graduation, tourism categories"]
        Blogs["Read blogs and banners"]
        Contact["Send contact inquiry"]
    end

    subgraph CustomerArea["Customer Features"]
        RegisterLogin["Register / login"]
        Profile["Manage profile"]
        Wallet["View wallet and transactions"]
        Deposit["Request wallet deposit"]
        Book["Book service"]
        Track["Track orders"]
        Comment["Rate and comment on services"]
        JoinProvider["Request to join as provider"]
    end

    subgraph ProviderArea["Provider Dashboard"]
        ProviderProfile["Manage provider profile"]
        ManageServices["Add, update, delete services"]
        ProviderOrders["View and confirm customer orders"]
        ProviderWallet["View earnings and wallet history"]
        Withdraw["Request fund withdrawal"]
        ReplyComments["Reply to customer comments"]
        ProviderStats["View service and order statistics"]
    end

    subgraph AdminArea["Admin Control Panel"]
        Accounts["Manage accounts and roles"]
        Approvals["Approve providers and services"]
        AdminCatalog["Manage categories, cities, banners, blogs"]
        AdminOrders["Monitor orders"]
        Moderate["Moderate comments and reviews"]
        Finance["Approve deposits and withdrawals"]
        Reports["Review platform data and reports"]
        Settings["Manage site settings"]
    end

    Guest --> Browse
    Guest --> Categories
    Guest --> Blogs
    Guest --> Contact

    Customer --> RegisterLogin
    Customer --> Profile
    Customer --> Wallet
    Customer --> Deposit
    Customer --> Book
    Customer --> Track
    Customer --> Comment
    Customer --> JoinProvider

    Provider --> ProviderProfile
    Provider --> ManageServices
    Provider --> ProviderOrders
    Provider --> ProviderWallet
    Provider --> Withdraw
    Provider --> ReplyComments
    Provider --> ProviderStats

    Admin --> Accounts
    Admin --> Approvals
    Admin --> AdminCatalog
    Admin --> AdminOrders
    Admin --> Moderate
    Admin --> Finance
    Admin --> Reports
    Admin --> Settings

    Book --> ProviderOrders
    Deposit --> Finance
    Withdraw --> Finance
    JoinProvider --> Approvals

    classDef actor fill:#0f172a,stroke:#0f172a,color:#ffffff;
    classDef public fill:#e0f2fe,stroke:#0284c7,color:#0f172a;
    classDef customer fill:#ecfdf5,stroke:#059669,color:#0f172a;
    classDef provider fill:#fff7ed,stroke:#ea580c,color:#0f172a;
    classDef admin fill:#fef2f2,stroke:#dc2626,color:#0f172a;

    class Guest,Customer,Provider,Admin actor;
    class Browse,Categories,Blogs,Contact public;
    class RegisterLogin,Profile,Wallet,Deposit,Book,Track,Comment,JoinProvider customer;
    class ProviderProfile,ManageServices,ProviderOrders,ProviderWallet,Withdraw,ReplyComments,ProviderStats provider;
    class Accounts,Approvals,AdminCatalog,AdminOrders,Moderate,Finance,Reports,Settings admin;
```

## Option 2: Booking and Wallet Workflow

Best for showing the real business flow from browsing to payment, provider approval, and withdrawal.

```mermaid
sequenceDiagram
    autonumber
    actor Guest
    actor Customer
    participant App as ServeMe Web App
    participant DB as SQL Server Database
    actor Provider
    actor Admin

    Guest->>App: Browse public services, blogs, banners
    App->>DB: Load approved services, categories, images
    DB-->>App: Public catalog data

    Customer->>App: Register or login
    App->>DB: Create or validate Identity user
    DB-->>App: Customer session and role

    Customer->>App: Request wallet deposit with proof
    App->>DB: Save deposit request as pending
    Admin->>App: Review pending deposit
    App->>DB: Approve deposit and update wallet balance

    Customer->>App: Book selected service
    App->>DB: Create pending order
    Provider->>App: Review order request

    alt Provider accepts order
        App->>DB: Mark order accepted
        App->>DB: Deduct customer wallet and credit provider wallet
        App-->>Customer: Show accepted order and updated wallet
        App-->>Provider: Show earnings and order status
    else Provider rejects order
        App->>DB: Mark order rejected
        App-->>Customer: Show rejected order status
    end

    Customer->>App: Add rating or comment
    App->>DB: Save service comment
    Provider->>App: View or reply to feedback

    Provider->>App: Request withdrawal
    App->>DB: Save withdrawal request as pending
    Admin->>App: Approve or reject withdrawal
    App->>DB: Update withdrawal, wallet, and transaction history
```

## Option 3: System Architecture by Role

Best for showing how the application is organized technically while still making the roles clear.

```mermaid
flowchart TB
    subgraph Users["Platform Users"]
        Guest["Guest"]
        Customer["Customer"]
        Provider["Service Provider"]
        Admin["Admin"]
    end

    subgraph Web["ASP.NET Core MVC Web App"]
        PublicPages["Public storefront\nHome, Wedding, Graduation, Tourism, Blogs"]
        Identity["ASP.NET Core Identity\nLogin, register, roles"]
        CustomerDash["Customer area\nProfile, orders, wallet, deposits, comments"]
        ProviderDash["Provider area\nServices, order requests, earnings, withdrawals"]
        AdminPanel["Admin control panel\nUsers, roles, content, approvals, finance"]
        Controllers["MVC controllers\nRoute requests and enforce access"]
    end

    subgraph Data["SQL Server + EF Core"]
        IdentityTables["AspNetUsers\nAspNetRoles\nAspNetUserRoles"]
        CatalogTables["TbService\nTbCategory\nTbCity\nTbServiceImage"]
        ContentTables["TbBanner\nTbBlog\nTbGeneralSettings\nTbContactUs"]
        OrderTables["TbOrder\nTbOrderStatModel\nTbComment"]
        FinanceTables["Wallets\nWalletTransactions\nDepositRequests\nWithdrawalRequests"]
    end

    Guest --> PublicPages
    Customer --> PublicPages
    Provider --> PublicPages
    Customer --> Identity
    Provider --> Identity
    Admin --> Identity

    Identity --> CustomerDash
    Identity --> ProviderDash
    Identity --> AdminPanel

    PublicPages --> Controllers
    CustomerDash --> Controllers
    ProviderDash --> Controllers
    AdminPanel --> Controllers

    Controllers --> IdentityTables
    Controllers --> CatalogTables
    Controllers --> ContentTables
    Controllers --> OrderTables
    Controllers --> FinanceTables

    AdminPanel --> CatalogTables
    AdminPanel --> ContentTables
    AdminPanel --> FinanceTables
    ProviderDash --> CatalogTables
    ProviderDash --> OrderTables
    ProviderDash --> FinanceTables
    CustomerDash --> OrderTables
    CustomerDash --> FinanceTables

    classDef actor fill:#0f172a,stroke:#0f172a,color:#ffffff;
    classDef web fill:#eef2ff,stroke:#4f46e5,color:#111827;
    classDef data fill:#f8fafc,stroke:#64748b,color:#111827;

    class Guest,Customer,Provider,Admin actor;
    class PublicPages,Identity,CustomerDash,ProviderDash,AdminPanel,Controllers web;
    class IdentityTables,CatalogTables,ContentTables,OrderTables,FinanceTables data;
```
