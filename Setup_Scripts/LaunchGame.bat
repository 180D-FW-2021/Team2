:: user must be sure to update where their anaconda installation directory is
:: as well as the env name of their dependent files
set ANACONDA_DIR=D:\rache\anaconda3
set ENV_NAME=yourenvname 

:: set GAME_EXECUTABLE=../Unity/Build/MazeProject.exe
echo %ANACONDA_DIR%

set ANACONDA_PROMPT=%ANACONDA_DIR%\Scripts\activate.bat
set POSITION_TRACKING_DIR=../Movenet/src/
set POSITION_TRACKING_SCRIPT=position_tracking.py

:: Start up Maze game
:: start %GAME_EXECUTABLE%

:: Calling anaconda actions in cmd prompt
:: https://stackoverflow.com/questions/46305569/how-to-make-batch-files-run-in-anaconda-prompt
call %ANACONDA_PROMPT% %ANACONDA_DIR%
call conda activate %ENV_NAME%
call cd %POSITION_TRACKING_DIR%
call python %POSITION_TRACKING_SCRIPT% --username %1

:: When Python script exits, clean up and close conda/cmd prompt
call conda deactivate
exit
