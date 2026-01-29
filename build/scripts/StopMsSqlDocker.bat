@echo off

REM Move to the root of the repository (from build\scripts -> build -> root)
cd  /d "%~dp0\..\.."

echo Current folder:
cd

REM Stop docker-compose
docker compose down

if errorlevel 1 (
    echo Error when stopping docker-compose.
    pause
    exit /b 1
)

echo Docker Compose successfully stopped.