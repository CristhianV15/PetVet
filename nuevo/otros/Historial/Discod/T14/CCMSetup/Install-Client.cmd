@echo off
pushd "%~dp0"
title Configuration Manager
Mode 100,20 & color 1F
:MENU
cls
echo.
echo ==========================================
echo ADMINISTRADOR DE IMPLEMENTACION DE CLIENTE
echo ==========================================
Echo.
echo 1. Instala cliente SCCM
echo 2. Remueve Cliente SCCM
echo.
set /p var= ^> Elija un numero: 
echo.
if "%var%"=="1" GOTO INSTALL
if "%var%"=="2" GOTO UNINSTALL
GOTO MENU

:INSTALL
powershell -executionpolicy bypass -file .\Install-Client.ps1 -Action Install
pause
GOTO MENU

:UNINSTALL
powershell -executionpolicy bypass -file .\Install-Client.ps1 -Action Remove
pause
GOTO MENU
