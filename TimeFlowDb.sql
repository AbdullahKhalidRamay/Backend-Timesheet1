-- TimeFlow Database Creation Script
-- Generated based on DOCUMENTATION.md and BACKEND.md

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TimeFlowDb')
BEGIN
    CREATE DATABASE TimeFlowDb;
END
GO

USE TimeFlowDb;
GO

-- Users Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Email NVARCHAR(100) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(MAX) NOT NULL,
        Name NVARCHAR(100) NOT NULL,
        FirstName NVARCHAR(50) NULL,
        LastName NVARCHAR(50) NULL,
        Role NVARCHAR(20) NOT NULL,
        JobTitle NVARCHAR(100) NOT NULL,
        BillableRate DECIMAL(18, 2) NULL,
        AvailableHours DECIMAL(5, 2) NOT NULL DEFAULT 8.0,
        TotalBillableHours DECIMAL(10, 2) NOT NULL DEFAULT 0.0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- Departments Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Departments')
BEGIN
    CREATE TABLE Departments (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        ManagerId UNIQUEIDENTIFIER NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_Departments_Users_ManagerId FOREIGN KEY (ManagerId) REFERENCES Users(Id) ON DELETE SET NULL
    );
END
GO

-- Teams Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Teams')
BEGIN
    CREATE TABLE Teams (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        DepartmentId UNIQUEIDENTIFIER NOT NULL,
        LeaderId UNIQUEIDENTIFIER NULL,
        CreatedBy UNIQUEIDENTIFIER NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_Teams_Departments_DepartmentId FOREIGN KEY (DepartmentId) REFERENCES Departments(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Teams_Users_LeaderId FOREIGN KEY (LeaderId) REFERENCES Users(Id) ON DELETE SET NULL,
        CONSTRAINT FK_Teams_Users_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
    );
END
GO

-- Projects Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Projects')
BEGIN
    CREATE TABLE Projects (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        ClientName NVARCHAR(100) NULL,
        StartDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        EndDate DATETIME2 NULL,
        BudgetHours DECIMAL(10, 2) NOT NULL DEFAULT 0,
        BillingRate DECIMAL(10, 2) NOT NULL DEFAULT 0,
        BillableHours DECIMAL(10, 2) NOT NULL DEFAULT 0,
        ActualHours DECIMAL(10, 2) NOT NULL DEFAULT 0,
        IsActive BIT NOT NULL DEFAULT 1,
        IsBillable BIT NOT NULL DEFAULT 0,
        Status NVARCHAR(20) NOT NULL DEFAULT 'active',
        CreatedBy UNIQUEIDENTIFIER NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_Projects_Users_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
    );
END
GO

-- ProjectLevels Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProjectLevels')
BEGIN
    CREATE TABLE ProjectLevels (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        ProjectId UNIQUEIDENTIFIER NOT NULL,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_ProjectLevels_Projects_ProjectId FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
    );
END
GO

-- ProjectTasks Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProjectTasks')
BEGIN
    CREATE TABLE ProjectTasks (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        LevelId UNIQUEIDENTIFIER NOT NULL,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_ProjectTasks_ProjectLevels_LevelId FOREIGN KEY (LevelId) REFERENCES ProjectLevels(Id) ON DELETE CASCADE
    );
END
GO

-- ProjectSubtasks Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProjectSubtasks')
BEGIN
    CREATE TABLE ProjectSubtasks (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        TaskId UNIQUEIDENTIFIER NOT NULL,
        ProjectTaskId UNIQUEIDENTIFIER NOT NULL,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_ProjectSubtasks_ProjectTasks_TaskId FOREIGN KEY (TaskId) REFERENCES ProjectTasks(Id) ON DELETE CASCADE,
        CONSTRAINT FK_ProjectSubtasks_ProjectTasks_ProjectTaskId FOREIGN KEY (ProjectTaskId) REFERENCES ProjectTasks(Id)
    );
END
GO

-- TimeEntries Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TimeEntries')
BEGIN
    CREATE TABLE TimeEntries (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        UserId UNIQUEIDENTIFIER NOT NULL,
        ProjectId UNIQUEIDENTIFIER NOT NULL,
        Date DATE NOT NULL,
        ClockIn TIME NULL,
        ClockOut TIME NULL,
        BreakTime INT NULL,
        ActualHours DECIMAL(5, 2) NOT NULL,
        BillableHours DECIMAL(5, 2) NOT NULL,
        TotalHours DECIMAL(5, 2) NOT NULL,
        AvailableHours DECIMAL(5, 2) NOT NULL,
        Task NVARCHAR(MAX) NOT NULL,
        Status NVARCHAR(20) NOT NULL DEFAULT 'pending',
        Description NVARCHAR(MAX) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_TimeEntries_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
        CONSTRAINT FK_TimeEntries_Projects_ProjectId FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
    );
END
GO

-- TeamMembers Junction Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TeamMembers')
BEGIN
    CREATE TABLE TeamMembers (
        TeamId UNIQUEIDENTIFIER NOT NULL,
        UserId UNIQUEIDENTIFIER NOT NULL,
        JoinedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT PK_TeamMembers PRIMARY KEY (TeamId, UserId),
        CONSTRAINT FK_TeamMembers_Teams_TeamId FOREIGN KEY (TeamId) REFERENCES Teams(Id) ON DELETE CASCADE,
        CONSTRAINT FK_TeamMembers_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    );
END
GO

-- TeamProjects Junction Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TeamProjects')
BEGIN
    CREATE TABLE TeamProjects (
        TeamId UNIQUEIDENTIFIER NOT NULL,
        ProjectId UNIQUEIDENTIFIER NOT NULL,
        AssignedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT PK_TeamProjects PRIMARY KEY (TeamId, ProjectId),
        CONSTRAINT FK_TeamProjects_Teams_TeamId FOREIGN KEY (TeamId) REFERENCES Teams(Id) ON DELETE CASCADE,
        CONSTRAINT FK_TeamProjects_Projects_ProjectId FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
    );
END
GO

-- Notifications Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notifications')
BEGIN
    CREATE TABLE Notifications (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        UserId UNIQUEIDENTIFIER NOT NULL,
        Title NVARCHAR(100) NOT NULL,
        Message NVARCHAR(200) NOT NULL,
        Type NVARCHAR(50) NULL,
        RelatedEntityId UNIQUEIDENTIFIER NULL,
        RelatedEntityType NVARCHAR(50) NULL,
        IsRead BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_Notifications_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    );
END
GO

-- ApprovalActions Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ApprovalActions')
BEGIN
    CREATE TABLE ApprovalActions (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        TimeEntryId UNIQUEIDENTIFIER NOT NULL,
        RequestedById UNIQUEIDENTIFIER NOT NULL,
        ApprovedById UNIQUEIDENTIFIER NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
        Comments NVARCHAR(MAX) NULL,
        RequestedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        ActionedAt DATETIME2 NULL,
        CONSTRAINT FK_ApprovalActions_TimeEntries_TimeEntryId FOREIGN KEY (TimeEntryId) REFERENCES TimeEntries(Id) ON DELETE CASCADE,
        CONSTRAINT FK_ApprovalActions_Users_RequestedById FOREIGN KEY (RequestedById) REFERENCES Users(Id),
        CONSTRAINT FK_ApprovalActions_Users_ApprovedById FOREIGN KEY (ApprovedById) REFERENCES Users(Id)
    );
END
GO

-- Create Indexes for better performance

-- Users table indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Email' AND object_id = OBJECT_ID('Users'))
BEGIN
    CREATE UNIQUE INDEX IX_Users_Email ON Users(Email);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Role' AND object_id = OBJECT_ID('Users'))
BEGIN
    CREATE INDEX IX_Users_Role ON Users(Role);
END

-- TimeEntries table indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_TimeEntries_UserId' AND object_id = OBJECT_ID('TimeEntries'))
BEGIN
    CREATE INDEX IX_TimeEntries_UserId ON TimeEntries(UserId);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_TimeEntries_ProjectId' AND object_id = OBJECT_ID('TimeEntries'))
BEGIN
    CREATE INDEX IX_TimeEntries_ProjectId ON TimeEntries(ProjectId);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_TimeEntries_Date' AND object_id = OBJECT_ID('TimeEntries'))
BEGIN
    CREATE INDEX IX_TimeEntries_Date ON TimeEntries(Date);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_TimeEntries_Status' AND object_id = OBJECT_ID('TimeEntries'))
BEGIN
    CREATE INDEX IX_TimeEntries_Status ON TimeEntries(Status);
END

-- Projects table indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Projects_CreatedBy' AND object_id = OBJECT_ID('Projects'))
BEGIN
    CREATE INDEX IX_Projects_CreatedBy ON Projects(CreatedBy);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Projects_Status' AND object_id = OBJECT_ID('Projects'))
BEGIN
    CREATE INDEX IX_Projects_Status ON Projects(Status);
END

-- Teams table indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Teams_DepartmentId' AND object_id = OBJECT_ID('Teams'))
BEGIN
    CREATE INDEX IX_Teams_DepartmentId ON Teams(DepartmentId);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Teams_LeaderId' AND object_id = OBJECT_ID('Teams'))
BEGIN
    CREATE INDEX IX_Teams_LeaderId ON Teams(LeaderId);
END

-- Notifications table indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Notifications_UserId' AND object_id = OBJECT_ID('Notifications'))
BEGIN
    CREATE INDEX IX_Notifications_UserId ON Notifications(UserId);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Notifications_IsRead' AND object_id = OBJECT_ID('Notifications'))
BEGIN
    CREATE INDEX IX_Notifications_IsRead ON Notifications(IsRead);
END

-- ApprovalActions table indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ApprovalActions_TimeEntryId' AND object_id = OBJECT_ID('ApprovalActions'))
BEGIN
    CREATE INDEX IX_ApprovalActions_TimeEntryId ON ApprovalActions(TimeEntryId);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ApprovalActions_RequestedById' AND object_id = OBJECT_ID('ApprovalActions'))
BEGIN
    CREATE INDEX IX_ApprovalActions_RequestedById ON ApprovalActions(RequestedById);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ApprovalActions_Status' AND object_id = OBJECT_ID('ApprovalActions'))
BEGIN
    CREATE INDEX IX_ApprovalActions_Status ON ApprovalActions(Status);
END

-- UserSettings Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserSettings')
BEGIN
    CREATE TABLE UserSettings (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        UserId UNIQUEIDENTIFIER NOT NULL,
        Theme NVARCHAR(50) NOT NULL DEFAULT 'light',
        DefaultView NVARCHAR(50) NOT NULL DEFAULT 'week',
        NotificationsEnabled BIT NOT NULL DEFAULT 1,
        EmailNotificationsEnabled BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_UserSettings_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    );
END
GO

-- SystemSettings Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SystemSettings')
BEGIN
    CREATE TABLE SystemSettings (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Key NVARCHAR(100) NOT NULL UNIQUE,
        Value NVARCHAR(MAX) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        Type NVARCHAR(50) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- ReportTemplates Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ReportTemplates')
BEGIN
    CREATE TABLE ReportTemplates (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        Type NVARCHAR(50) NOT NULL,
        Configuration NVARCHAR(MAX) NOT NULL,
        CreatedBy UNIQUEIDENTIFIER NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_ReportTemplates_Users_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
    );
END
GO

-- SavedReports Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SavedReports')
BEGIN
    CREATE TABLE SavedReports (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        TemplateId UNIQUEIDENTIFIER NULL,
        Parameters NVARCHAR(MAX) NOT NULL,
        UserId UNIQUEIDENTIFIER NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT FK_SavedReports_ReportTemplates_TemplateId FOREIGN KEY (TemplateId) REFERENCES ReportTemplates(Id) ON DELETE SET NULL,
        CONSTRAINT FK_SavedReports_Users_UserId FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    );
END
GO

-- Create Indexes for new tables

-- UserSettings table indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_UserSettings_UserId' AND object_id = OBJECT_ID('UserSettings'))
BEGIN
    CREATE UNIQUE INDEX IX_UserSettings_UserId ON UserSettings(UserId);
END

-- SystemSettings table indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SystemSettings_Key' AND object_id = OBJECT_ID('SystemSettings'))
BEGIN
    CREATE UNIQUE INDEX IX_SystemSettings_Key ON SystemSettings(Key);
END

-- ReportTemplates table indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ReportTemplates_Type' AND object_id = OBJECT_ID('ReportTemplates'))
BEGIN
    CREATE INDEX IX_ReportTemplates_Type ON ReportTemplates(Type);
END

-- SavedReports table indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SavedReports_UserId' AND object_id = OBJECT_ID('SavedReports'))
BEGIN
    CREATE INDEX IX_SavedReports_UserId ON SavedReports(UserId);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_SavedReports_TemplateId' AND object_id = OBJECT_ID('SavedReports'))
BEGIN
    CREATE INDEX IX_SavedReports_TemplateId ON SavedReports(TemplateId);
END

PRINT 'TimeFlow Database Schema Created Successfully';
GO