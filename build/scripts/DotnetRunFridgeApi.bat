REM Move to the root of the repository (from build\scripts -> build -> root)
cd /d "%~dp0\..\.."

echo Current folder:
cd

dotnet run --project src/Fridge.Api
