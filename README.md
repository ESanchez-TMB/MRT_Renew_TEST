# MRT_Renew_TEST

## Overview

This repository contains the MRT Renewal Test System - a comprehensive .NET web application solution for managing Medical Record Technology renewal processes for the Texas Medical Board. The system includes multiple web applications, console applications, and shared class libraries.

**Last Updated**: November 10, 2025  
**Solution Format**: Visual Studio 2013 (Format Version 12.00)  
**Target Framework**: .NET Framework 4.0  
**Build Status**: ✅ All projects build successfully

## Solution Structure

The solution contains **6 active projects** with the following architecture:

### 🌐 **Web Applications**

- **mrtRenew** - MRT (Medical Record Technology) renewal web application
- **rspRenew** - RSP (Respiratory Care Practitioner) renewal web application

### 🖥️ **Console Applications**

- **Console_Proj_Process_Single_RSP_rnwl** - Console app for processing single RSP renewals
- **processRenewApps** - Main renewal processing application
- **special01** - Specialized processing utility

### 📚 **Class Libraries**

- **RClassLib** - Shared class library for renewal functionality

## Root Directory Structure

```text
📁 MRT_Renew_TEST/
├── 📄 .gitignore                                    # Git ignore rules
├── 📄 202Renew.sln                                  # Main Visual Studio solution file
├── 📄 202Renew.sln.docstates.suo                    # Visual Studio solution user options
├── 📄 202Renew.suo                                  # Visual Studio solution user options (legacy)
├── 📄 README.md                                     # This documentation file
├── 📄 UpgradeLog.htm                                # Visual Studio upgrade log
├── 📄 UpgradeLog2.htm                               # Visual Studio upgrade log (backup)
├── 📄 UpgradeLog3.htm                               # Visual Studio upgrade log (backup)
│
├── 📁 .git/                                         # Git version control data
│
├── 🌐 Console_Proj_Process_Single_RSP_rnwl/         # Console Application Project
│   ├── 📄 App.config                                # Application configuration
│   ├── 📄 Console_Proj_Process_Single_RSP_rnwl.csproj  # Project file
│   ├── 📄 Program.cs                                # Main application entry point
│   ├── 📄 cash.cs                                   # Payment processing logic
│   ├── 📄 DataAccess.cs                             # Database access layer
│   ├── 📄 Items.cs                                  # Item processing classes
│   ├── 📄 logger.cs                                 # Logging functionality
│   ├── 📁 bin/                                      # Compiled binaries
│   ├── 📁 obj/                                      # Build intermediate files
│   └── 📁 Properties/                               # Assembly properties
│
├── 🌐 mrtRenew/                                     # MRT Renewal Web Application
│   ├── 📄 Global.asax                               # Application global events
│   ├── 📄 Global.asax.cs                            # Global application code-behind
│   ├── 📄 mrtRenew.csproj                           # Project file
│   ├── 📄 mrtRenew.Master                           # Master page template
│   ├── 📄 mrtRenew.Master.cs                        # Master page code-behind
│   ├── 📄 Web.config                                # Web application configuration
│   ├── 📄 Login.aspx                                # Login page
│   ├── 📄 addresses.aspx                            # Address management page
│   ├── 📄 payment.aspx                              # Payment processing page
│   ├── 📄 questions.aspx                            # Questionnaire page
│   ├── 📄 review.aspx                               # Review and submit page
│   ├── 📄 success.aspx                              # Success confirmation page
│   ├── 📄 disclaimer.aspx                           # Legal disclaimer page
│   ├── 📄 [Additional .aspx pages...]               # Various workflow pages
│   ├── 📁 Account/                                  # User account management
│   ├── 📁 App_Code/                                 # Shared application code
│   ├── 📁 App_Data/                                 # Application data files
│   ├── 📁 Scripts/                                  # JavaScript files
│   ├── 📁 Styles/                                   # CSS stylesheets
│   ├── 📁 images/                                   # Web images and graphics
│   ├── 📁 Service References/                       # WCF service references
│   ├── 📁 bin/                                      # Compiled web application binaries
│   ├── 📁 obj/                                      # Build intermediate files
│   └── 📁 Properties/                               # Assembly and publish properties
│
├── 🖥️ processRenewApps/                             # Renewal Processing Console App
│   ├── 📄 App.config                                # Application configuration
│   ├── 📄 processRenewApps.csproj                   # Project file
│   ├── 📄 Program.cs                                # Main application entry point
│   ├── 📄 cash.cs                                   # Payment processing classes
│   ├── 📄 DataAccess.cs                             # Database access layer
│   ├── 📄 Items.cs                                  # Item processing classes
│   ├── 📄 logger.cs                                 # Logging functionality
│   ├── 📄 ClassDiagram1.cd                          # Visual Studio class diagram
│   ├── 📁 bin/                                      # Compiled binaries
│   ├── 📁 obj/                                      # Build intermediate files
│   └── 📁 Properties/                               # Assembly properties
│
├── 📚 RClassLib/                                    # Shared Class Library
│   ├── 📄 app.config                                # Library configuration
│   ├── 📄 RClassLibrary.csproj                      # Project file
│   ├── 📄 DataAccess.cs                             # Database access classes
│   ├── 📄 renewal.cs                                # Core renewal business logic
│   ├── 📄 renewDB.cs                                # Database renewal operations
│   ├── 📄 TracerDB.cs                               # Database tracing utilities
│   ├── 📄 utilities.cs                              # Common utility functions
│   ├── 📄 ObjectDumper.cs                           # Object serialization utility
│   ├── 📁 Service References/                       # External service references
│   ├── 📁 bin/                                      # Compiled library binaries
│   ├── 📁 obj/                                      # Build intermediate files
│   └── 📁 Properties/                               # Assembly properties
│
├── 🌐 rspRenew/                                     # RSP Renewal Web Application
│   ├── 📄 Global.asax                               # Application global events
│   ├── 📄 rspRenew.csproj                           # Project file
│   ├── 📄 rspRenew.Master                           # Master page template
│   ├── 📄 Web.config                                # Web application configuration
│   ├── 📄 Login.aspx                                # Login page
│   ├── 📄 addresses.aspx                            # Address management
│   ├── 📄 payment.aspx                              # Payment processing
│   ├── 📄 [Similar pages to mrtRenew...]            # Parallel workflow pages
│   ├── 📁 Account/                                  # User account management
│   ├── 📁 Old_App_Code/                             # Legacy application code
│   ├── 📁 Scripts/                                  # JavaScript files
│   ├── 📁 Styles/                                   # CSS stylesheets
│   ├── 📁 images/                                   # Web images and graphics
│   ├── 📁 Service References/                       # WCF service references
│   └── 📁 [Standard web app folders...]             # Standard ASP.NET structure
│
├── 🖥️ special01/                                   # Special Processing Utility
│   ├── 📄 App.config                                # Application configuration
│   ├── 📄 special01.csproj                          # Project file
│   ├── 📄 Program.cs                                # Main application entry point
│   ├── 📄 customer_order.cs                         # Customer order processing
│   ├── 📄 DataAccess.cs                             # Database access layer
│   ├── 📄 logger.cs                                 # Logging functionality
│   ├── 📄 ObjectDumper.cs                           # Object debugging utility
│   ├── 📄 receiptDetail.cs                          # Receipt processing
│   └── 📁 [Standard console app folders...]         # Standard .NET structure
│
├── 📁 Backup/                                       # Backup of Original Projects
│   ├── 📄 202Renew.sln                              # Original solution file
│   ├── 📁 lmpRenew/                                 # Removed LMP renewal project
│   ├── 📁 nctregRenew/                              # Removed NCT renewal project  
│   ├── 📁 prfRenew/                                 # Removed PRF renewal project
│   └── 📁 WCF1/                                     # Removed WCF service project
│
├── 📁 Backup1/                                      # Additional backup directory
├── 📁 Backup2/                                      # Additional backup directory
│   └── [Mirror of original projects...]             # Secondary backup copies
│
├── 📁 Visual Studio 2010/                          # Visual Studio 2010 artifacts
├── 📁 Visual Studio 2010Projects/                  # VS 2010 project templates
└── 📁 Visual Studio 2010Templates/                 # VS 2010 item templates
```

## Project Dependencies

```text
📚 RClassLibrary (Core Library)
├── → 🌐 mrtRenew (depends on RClassLibrary)
├── → 🌐 rspRenew (depends on RClassLibrary)
├── → 🖥️ processRenewApps (depends on RClassLibrary)
├── → 🖥️ Console_Proj_Process_Single_RSP_rnwl (depends on RClassLibrary)
└── → 🖥️ special01 (depends on RClassLibrary + processRenewApps)
```

## Technology Stack

- **.NET Framework**: 4.0 (Client Profile for console apps)
- **Web Framework**: ASP.NET Web Forms
- **Database**: SQL Server (connection strings in web.config/app.config)
- **Build Tool**: MSBuild (Visual Studio 2013)
- **Version Control**: Git
- **Architecture**: Multi-tier (Web → Business Logic → Data Access)

## Build Configurations

The solution supports multiple build configurations:

- **Debug|Any CPU** - Development builds
- **Debug|Mixed Platforms** - Mixed architecture debugging  
- **Debug|x86** - 32-bit debugging
- **Release|Any CPU** - Production builds
- **Release|Mixed Platforms** - Mixed architecture release
- **Release|x86** - 32-bit release

## Getting Started

### Prerequisites

- **Visual Studio 2013** or later (recommended)
- **.NET Framework 4.0** or later
- **SQL Server** (connection details in config files)
- **IIS** (for web applications)
- **Git** for version control

### Setup Instructions

1. **Clone Repository**:

   ```bash
   git clone <repository-url>
   cd MRT_Renew_TEST
   ```

2. **Open Solution**:

   ```bash
   # Open in Visual Studio
   start 202Renew.sln
   ```

3. **Build Solution**:

   ```bash
   # Using Visual Studio Developer Command Prompt
   msbuild 202Renew.sln /p:Configuration=Debug /p:Platform="Mixed Platforms"
   ```

4. **Configure Database**:
   - Update connection strings in `Web.config` and `App.config` files
   - Ensure SQL Server is running and accessible

5. **Run Projects**:
   - **Web Apps**: Set `mrtRenew` or `rspRenew` as startup project
   - **Console Apps**: Set desired console project as startup project

## Recent Changes

### November 7-10, 2025 Updates

- ✅ **Removed obsolete projects**: `lmpRenew`, `nctregRenew`, `prfRenew`, `WCF1`, `test1`
- ✅ **Fixed build errors**: Resolved namespace conflicts in `mrtRenew` project
- ✅ **Added missing references**: Fixed `special01` project dependencies
- ✅ **Updated solution file**: Clean 6-project solution structure
- ✅ **Verified builds**: All projects compile successfully
- 📁 **Preserved backups**: Original projects saved in `Backup/` folders

### Build Status: ✅ ALL PROJECTS VERIFIED WORKING

- 🟢 **RClassLibrary**: Builds successfully
- 🟢 **mrtRenew**: Builds successfully (namespace conflict resolved)
- 🟢 **rspRenew**: Builds successfully  
- 🟢 **special01**: Builds successfully (missing reference added)
- 🟢 **processRenewApps**: Builds successfully
- 🟢 **Console_Proj_Process_Single_RSP_rnwl**: Builds successfully

## Development Guidelines

- **Coding Standards**: Follow standard C# conventions
- **Branching**: Create feature branches for new development
- **Testing**: Test all functionality before committing
- **Documentation**: Update README when making structural changes
- **Configuration**: Keep sensitive data out of config files (use config transforms)

## License

This project is developed for the **Texas Medical Board** - State of Texas government application.
Not licensed for external use or distribution.

## Contact

**Project Maintainer**: Texas Medical Board IT Department  
**Last Updated**: November 10, 2025  
**Solution Version**: 12.0 (Visual Studio 2013 format)

---

## Troubleshooting

### Common Build Issues

- **Missing References**: Ensure all project dependencies are restored
- **Platform Mismatches**: Use "Mixed Platforms" configuration for compatibility
- **Database Connections**: Verify SQL Server connectivity and connection strings

### Web Application Deployment

- **IIS Configuration**: Ensure .NET Framework 4.0 is installed on target server
- **Web.config**: Update connection strings and application settings for target environment
- **File Permissions**: Ensure appropriate read/write permissions for application folders
