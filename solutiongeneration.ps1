
#﻿# Inward=$args[0]
## D:\websites\codegenerator.gipl.in\18122023\generate\Inward=$args[1]

## Inward="MyFirstASPCore"
## D:\websites\codegenerator.gipl.in\18122023\generate\Inward="D:\tmp\Codegen\MyFirstASPCore"

set-location D:\websites\codegenerator.gipl.in\18122023\generate\Inward
dotnet new sln -n Inward 
dotnet new mvc -n Inward --framework net8.0
dotnet new classlib -n Inward.Services --framework net8.0
dotnet new classlib -n Inward.Services.Abstraction --framework net8.0
dotnet new classlib -n Inward.Repository --framework net8.0
dotnet new classlib -n Inward.Repository.Abstraction --framework net8.0
dotnet new webapi -n Inward.API --framework net8.0
dotnet new classlib -n Inward.Common --framework net8.0
dotnet new classlib -n Inward.Entity --framework net8.0
dotnet sln Inward.sln add (ls -r **/*.csproj)

#adding reference here

dotnet add Inward/Inward.csproj reference Inward.Entity/Inward.Entity.csproj
#dotnet add Inward/Inward.csproj reference Inward.API/Inward.API.csproj
dotnet add Inward/Inward.csproj reference Inward.Common/Inward.Common.csproj
dotnet add Inward/Inward.csproj reference Inward.Services.Abstraction/Inward.Services.Abstraction.csproj
dotnet add Inward/Inward.csproj reference Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj
dotnet add Inward/Inward.csproj reference Inward.Repository/Inward.Repository.csproj
dotnet add Inward/Inward.csproj reference Inward.Services/Inward.Services.csproj


dotnet add Inward.Services.Abstraction/Inward.Services.Abstraction.csproj reference Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj
#dotnet add Inward.Services.Abstraction/Inward.Services.Abstraction.csproj reference Inward.Services/Inward.Services.csproj
dotnet add Inward.Services.Abstraction/Inward.Services.Abstraction.csproj reference Inward.Common/Inward.Common.csproj
dotnet add Inward.Services.Abstraction/Inward.Services.Abstraction.csproj reference Inward.Entity/Inward.Entity.csproj


dotnet add Inward.Services/Inward.Services.csproj reference Inward.Services.Abstraction/Inward.Services.Abstraction.csproj
dotnet add Inward.Services/Inward.Services.csproj reference Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj
dotnet add Inward.Services/Inward.Services.csproj reference Inward.Common/Inward.Common.csproj
dotnet add Inward.Services/Inward.Services.csproj reference Inward.Entity/Inward.Entity.csproj

#dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj reference Inward.Repository/Inward.Repository.csproj
dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj reference Inward.Common/Inward.Common.csproj
dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj reference Inward.Entity/Inward.Entity.csproj


dotnet add Inward.Repository/Inward.Repository.csproj reference Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj
dotnet add Inward.Repository/Inward.Repository.csproj reference Inward.Common/Inward.Common.csproj
dotnet add Inward.Repository/Inward.Repository.csproj reference Inward.Entity/Inward.Entity.csproj



## Package install starts here

dotnet add Inward/Inward.csproj package Microsoft.EntityFrameworkCore
dotnet add Inward/Inward.csproj package Dapper
dotnet add Inward/Inward.csproj package System.Data.SqlClient
dotnet add Inward/Inward.csproj package Newtonsoft.Json

dotnet add Inward.Repository/Inward.Repository.csproj package Microsoft.EntityFrameworkCore
dotnet add Inward.Repository/Inward.Repository.csproj package Dapper
dotnet add Inward.Repository/Inward.Repository.csproj package System.Data.SqlClient
dotnet add Inward.Repository/Inward.Repository.csproj package Newtonsoft.Json
dotnet add Inward.Repository/Inward.Repository.csproj package Microsoft.Extensions.Configuration

dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj package Microsoft.EntityFrameworkCore
dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj package Dapper
dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj package System.Data.SqlClient
dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj package Newtonsoft.Json


dotnet add Inward.Services.Abstraction/Inward.Services.Abstraction.csproj package Microsoft.EntityFrameworkCore
dotnet add Inward.Services.Abstraction/Inward.Services.Abstraction.csproj package Newtonsoft.Json

dotnet add Inward.Services/Inward.Services.csproj package Microsoft.EntityFrameworkCore
dotnet add Inward.Services/Inward.Services.csproj package Newtonsoft.Json

dotnet add Inward.Common/Inward.Common.csproj package Newtonsoft.Json
dotnet add Inward.Common/Inward.Common.csproj package Microsoft.AspNetCore.Mvc.ViewFeatures
dotnet add Inward.Common/Inward.Common.csproj package Microsoft.Reporting.NETCore



dotnet restore
dotnet build Inward.sln
$currdate = Get-Date
$currdatestr =  $currdate.ToString('ddMMyyyyHHmmss', [System.Globalization.CultureInfo]::InvariantCulture)
Move-Item -Path D:\websites\codegenerator.gipl.in\18122023\generate\Inward\solutiongeneration.ps1 -Destination ..\..\ps1\Inwardsolutiongeneration$currdatestr.ps1

<##﻿# Inward=$args[0]
## D:\websites\codegenerator.gipl.in\18122023\generate\Inward=$args[1]

## Inward="MyFirstASPCore"
## D:\websites\codegenerator.gipl.in\18122023\generate\Inward="D:\tmp\Codegen\MyFirstASPCore"

set-location D:\websites\codegenerator.gipl.in\18122023\generate\Inward
dotnet new sln -n Inward 
dotnet new mvc -n Inward --framework net6.0
dotnet new classlib -n Inward.Services --framework net6.0
dotnet new classlib -n Inward.Services.Abstraction --framework net6.0
dotnet new classlib -n Inward.Repository --framework net6.0
dotnet new classlib -n Inward.Repository.Abstraction --framework net6.0
dotnet new webapi -n Inward.API --framework net6.0
dotnet new classlib -n Inward.Common --framework net6.0
dotnet new classlib -n Inward.Entity --framework net6.0
dotnet sln Inward.sln add (ls -r **/*.csproj)

#adding reference here

dotnet add Inward/Inward.csproj reference Inward.Entity/Inward.Entity.csproj
dotnet add Inward/Inward.csproj reference Inward.API/Inward.API.csproj
dotnet add Inward/Inward.csproj reference Inward.Common/Inward.Common.csproj
dotnet add Inward/Inward.csproj reference Inward.Services.Abstraction/Inward.Services.Abstraction.csproj
dotnet add Inward/Inward.csproj reference Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj
dotnet add Inward/Inward.csproj reference Inward.Repository/Inward.Repository.csproj
dotnet add Inward/Inward.csproj reference Inward.Services/Inward.Services.csproj


dotnet add Inward.Services.Abstraction/Inward.Services.Abstraction.csproj reference Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj
#dotnet add Inward.Services.Abstraction/Inward.Services.Abstraction.csproj reference Inward.Services/Inward.Services.csproj
dotnet add Inward.Services.Abstraction/Inward.Services.Abstraction.csproj reference Inward.Common/Inward.Common.csproj
dotnet add Inward.Services.Abstraction/Inward.Services.Abstraction.csproj reference Inward.Entity/Inward.Entity.csproj


dotnet add Inward.Services/Inward.Services.csproj reference Inward.Services.Abstraction/Inward.Services.Abstraction.csproj
dotnet add Inward.Services/Inward.Services.csproj reference Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj
dotnet add Inward.Services/Inward.Services.csproj reference Inward.Common/Inward.Common.csproj
dotnet add Inward.Services/Inward.Services.csproj reference Inward.Entity/Inward.Entity.csproj

#dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj reference Inward.Repository/Inward.Repository.csproj
dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj reference Inward.Common/Inward.Common.csproj
dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj reference Inward.Entity/Inward.Entity.csproj


dotnet add Inward.Repository/Inward.Repository.csproj reference Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj
dotnet add Inward.Repository/Inward.Repository.csproj reference Inward.Common/Inward.Common.csproj
dotnet add Inward.Repository/Inward.Repository.csproj reference Inward.Entity/Inward.Entity.csproj



## Package install starts here

dotnet add Inward/Inward.csproj package Microsoft.EntityFrameworkCore --version 6.0.24
dotnet add Inward/Inward.csproj package Dapper
dotnet add Inward/Inward.csproj package System.Data.SqlClient
dotnet add Inward/Inward.csproj package Newtonsoft.Json

dotnet add Inward.Repository/Inward.Repository.csproj package Microsoft.EntityFrameworkCore --version 6.0.24
dotnet add Inward.Repository/Inward.Repository.csproj package Dapper
dotnet add Inward.Repository/Inward.Repository.csproj package System.Data.SqlClient
dotnet add Inward.Repository/Inward.Repository.csproj package Newtonsoft.Json
dotnet add Inward.Repository/Inward.Repository.csproj package Microsoft.Extensions.Configuration

dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj package Microsoft.EntityFrameworkCore --version 6.0.24
dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj package Dapper
dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj package System.Data.SqlClient
dotnet add Inward.Repository.Abstraction/Inward.Repository.Abstraction.csproj package Newtonsoft.Json


dotnet add Inward.Services.Abstraction/Inward.Services.Abstraction.csproj package Microsoft.EntityFrameworkCore --version 6.0.24
dotnet add Inward.Services.Abstraction/Inward.Services.Abstraction.csproj package Newtonsoft.Json

dotnet add Inward.Services/Inward.Services.csproj package Microsoft.EntityFrameworkCore --version 6.0.24
dotnet add Inward.Services/Inward.Services.csproj package Newtonsoft.Json

dotnet add Inward.Common/Inward.Common.csproj package Newtonsoft.Json
dotnet add Inward.Common/Inward.Common.csproj package Microsoft.AspNetCore.Mvc.ViewFeatures
dotnet add Inward.Common/Inward.Common.csproj package Microsoft.Reporting.NETCore



dotnet restore
dotnet build Inward.sln
$currdate = Get-Date
$currdatestr =  $currdate.ToString('ddMMyyyyHHmmss', [System.Globalization.CultureInfo]::InvariantCulture)
Move-Item -Path D:\websites\codegenerator.gipl.in\18122023\generate\Inward\solutiongeneration.ps1 -Destination ..\..\ps1\Inwardsolutiongeneration$currdatestr.ps1
#>