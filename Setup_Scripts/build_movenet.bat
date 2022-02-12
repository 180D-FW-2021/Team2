:: Build movenet executable to execute from Unity
:: Ensure to activate conda env before running
:: This may take a few mins...
set MOVENET_DIR=..\Movenet\src
cd %MOVENET_DIR%

:: build single executable with all dependencies installed
pyinstaller -D position_tracking.p