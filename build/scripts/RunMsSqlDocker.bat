@echo off
setlocal EnableExtensions

REM Move to the root of the repository (from build\scripts -> build -> root)
cd /d "%~dp0\..\.."

echo Current folder:
cd

REM --- Check Docker daemon ---
docker info >nul 2>&1
if not errorlevel 1 goto docker_ready

echo Docker is not running. Starting Docker Desktop...

if exist "C:\Program Files\Docker\Docker\Docker Desktop.exe" (
    start "" "C:\Program Files\Docker\Docker\Docker Desktop.exe"
) else (
    echo Docker Desktop not found at:
    echo   C:\Program Files\Docker\Docker\Docker Desktop.exe
    echo Start Docker Desktop manually or update the path in this script.
    pause
    exit /b 1
)

REM Wait until Docker daemon becomes available (up to ~120 seconds)
set /a retries=60

:wait_docker
docker info >nul 2>&1
if not errorlevel 1 goto docker_ready

set /a retries-=1
if %retries%==0 (
    echo Docker did not become ready in time.
    pause
    exit /b 1
)

timeout /t 2 /nobreak >nul
goto wait_docker

:docker_ready
echo Docker is running.

REM --- Run compose (prefer docker-compose, fallback to docker compose) ---
where docker-compose >nul 2>&1
if not errorlevel 1 (
    docker-compose up -d
) else (
    docker compose up -d
)

if errorlevel 1 (
    echo Error when running compose.
    pause
    exit /b 1
)

echo Docker Compose successfully started.
endlocal