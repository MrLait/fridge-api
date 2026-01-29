@echo off

REM Move to the root of the repository (from build\scripts -> build -> root)
cd  /d "%~dp0\..\.."

echo Current folder:
cd

REM Run docker-compose in the background
docker-compose up -d

if errorlevel 1 (
    echo Error when running docker-compose.
    pause
    exit /b 1
)

echo Docker Compose successfully started.