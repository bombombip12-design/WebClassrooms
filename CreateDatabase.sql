-- Script tạo database và các bảng cho ứng dụng Google Classroom Clone
-- Chạy script này trong SQL Server Management Studio

USE master;
GO

-- Xóa database cũ nếu tồn tại (tùy chọn, bỏ comment nếu muốn)
-- DROP DATABASE IF EXISTS DoAnChuyenNganh;
-- GO

-- Tạo database mới
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DoAnChuyenNganh')
BEGIN
    CREATE DATABASE DoAnChuyenNganh;
END
GO

USE DoAnChuyenNganh;
GO

-- Tạo bảng SystemRoles
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SystemRoles')
BEGIN
    CREATE TABLE SystemRoles (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        RoleName NVARCHAR(50) NOT NULL UNIQUE
    );

    INSERT INTO SystemRoles (RoleName) VALUES ('Admin'), ('User');
END
GO

-- Tạo bảng Users
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FullName NVARCHAR(255) NOT NULL,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(500) NULL,
        GoogleId NVARCHAR(255) NULL,
        AvatarUrl NVARCHAR(500) NULL,
        SystemRoleId INT NOT NULL DEFAULT 2,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Users_SystemRoles FOREIGN KEY (SystemRoleId) REFERENCES SystemRoles(Id)
    );

    CREATE UNIQUE INDEX IX_Users_Email ON Users(Email);
END
GO

-- Tạo bảng Classes
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Classes')
BEGIN
    CREATE TABLE Classes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ClassName NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        ClassUrl NVARCHAR(MAX) NULL,
        JoinCode NVARCHAR(20) NOT NULL UNIQUE,
        OwnerId INT NOT NULL,
        CoverImageUrl NVARCHAR(500) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Classes_Users FOREIGN KEY (OwnerId) REFERENCES Users(Id)
    );

    CREATE UNIQUE INDEX IX_Classes_JoinCode ON Classes(JoinCode);
END
GO

-- Tạo bảng Enrollments
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Enrollments')
BEGIN
    CREATE TABLE Enrollments (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        ClassId INT NOT NULL,
        Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Teacher', 'Student')),
        JoinedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Enrollments_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT FK_Enrollments_Classes FOREIGN KEY (ClassId) REFERENCES Classes(Id),
        CONSTRAINT UQ_Enrollment UNIQUE (UserId, ClassId)
    );
END
GO

-- Tạo bảng Assignments
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Assignments')
BEGIN
    CREATE TABLE Assignments (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ClassId INT NOT NULL,
        Title NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        DueDate DATETIME NULL,
        CreatedBy INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Assignments_Classes FOREIGN KEY (ClassId) REFERENCES Classes(Id),
        CONSTRAINT FK_Assignments_Users FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
    );
END
GO

-- Tạo bảng AssignmentFiles
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AssignmentFiles')
BEGIN
    CREATE TABLE AssignmentFiles (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        AssignmentId INT NOT NULL,
        DriveFileId NVARCHAR(255) NOT NULL,
        DriveFileUrl NVARCHAR(500) NOT NULL,
        CONSTRAINT FK_AssignmentFiles_Assignments FOREIGN KEY (AssignmentId) REFERENCES Assignments(Id) ON DELETE CASCADE
    );
END
GO

-- Tạo bảng Submissions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Submissions')
BEGIN
    CREATE TABLE Submissions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        AssignmentId INT NOT NULL,
        StudentId INT NOT NULL,
        SubmittedAt DATETIME NOT NULL DEFAULT GETDATE(),
        Score INT NULL,
        TeacherComment NVARCHAR(MAX) NULL,
        CONSTRAINT FK_Submissions_Assignments FOREIGN KEY (AssignmentId) REFERENCES Assignments(Id),
        CONSTRAINT FK_Submissions_Users FOREIGN KEY (StudentId) REFERENCES Users(Id),
        CONSTRAINT UQ_Submission UNIQUE (AssignmentId, StudentId)
    );
END
GO

-- Tạo bảng SubmissionFiles
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SubmissionFiles')
BEGIN
    CREATE TABLE SubmissionFiles (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SubmissionId INT NOT NULL,
        DriveFileId NVARCHAR(255) NOT NULL,
        DriveFileUrl NVARCHAR(500) NOT NULL,
        CONSTRAINT FK_SubmissionFiles_Submissions FOREIGN KEY (SubmissionId) REFERENCES Submissions(Id) ON DELETE CASCADE
    );
END
GO

-- Tạo bảng Comments
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Comments')
BEGIN
    CREATE TABLE Comments (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        ClassId INT NULL,
        AssignmentId INT NULL,
        SubmissionId INT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        IsPrivate BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Comments_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT FK_Comments_Classes FOREIGN KEY (ClassId) REFERENCES Classes(Id),
        CONSTRAINT FK_Comments_Assignments FOREIGN KEY (AssignmentId) REFERENCES Assignments(Id),
        CONSTRAINT FK_Comments_Submissions FOREIGN KEY (SubmissionId) REFERENCES Submissions(Id)
    );
END
GO

-- Tạo bảng Announcements
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Announcements')
BEGIN
    CREATE TABLE Announcements (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ClassId INT NOT NULL,
        UserId INT NOT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Announcements_Classes FOREIGN KEY (ClassId) REFERENCES Classes(Id),
        CONSTRAINT FK_Announcements_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

-- Tạo bảng AnnouncementFiles
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AnnouncementFiles')
BEGIN
    CREATE TABLE AnnouncementFiles (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        AnnouncementId INT NOT NULL,
        DriveFileId NVARCHAR(255) NOT NULL,
        DriveFileUrl NVARCHAR(500) NOT NULL,
        CONSTRAINT FK_AnnouncementFiles_Announcements FOREIGN KEY (AnnouncementId) REFERENCES Announcements(Id) ON DELETE CASCADE
    );
END
GO

PRINT 'Database và các bảng đã được tạo thành công!';
GO

