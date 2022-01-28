:: Build movenet executable to execute from Unity
:: Ensure to activate conda env before running
:: This may take a few mins...
set MOVENET_DIR=..\Movenet\src
cd %MOVENET_DIR%

:: build single executable with all dependencies installed
pyinstaller -F --onefile position_tracking.py
:: executable must be in directory Movenet/src
:: due to relative model paths
cd dist
move position_tracking.exe ..