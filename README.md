# A-Maze

A-maze, an interactive indoor game designed to test a user’s memory and dexterity, was designed to entertain children and young adults indoors, while involving some physical movement and stimulating the brain.

## Running the Game

We have provided scripts to automatically open the game and Movenet position tracking concurrently. Scripts are located in the **Setup_Scripts** directory.

Prior to running the scripts, make sure you have properly installed python3/anaconda and have set up your virtual anaconda dev environment.

Also, ensure you [build your game executable](https://docs.unity3d.com/Manual/PublishingBuilds.html).

### Windows

For Windows, a **LaunchGame.bat** file is provided. The first two variables, ANACONDA_DIR and ENV_NAME may have to be updated for custom Anaconda installation directories and custom Conda environment names (the default is 'yourenvname'). To use this script, launch a command prompt from the Setup_Scripts directory and run:

`LaunchGame.bat`

### MacOS

1. Initialize your conda env.

`conda activate $your_env_name`

2. Edit Unity executable name in **LaunchGame** script.

`PROJECT_NAME="MazeProject"`

3. Run the script.

```
cd Setup_Scripts
./LaunchGame
```

4. Deactivate your environment.

## Game Controls

### Keyboard Controls

- Forward/back/left/right: arrow keys/ WASD
- Rotate camera: 'z' (left) 'x' (right)
- Jump: space bar
- Duck: Left Shift
- Pause Menu: Esc

### Voice Commands

On Start Screen:

- "Enter": Go to Main Menu
- "Settings": Open Settings menu (not yet implemented)

On Main Menu:

- "Back": back to Start Screen
- "Levels": Open Level Selection Screen
- "Help": Open Help menu (not yet implemented)
- "Settings": Open Settings menu (not yet implemented)

On Level Selection Screen:

- "Back": back to Main Menu
- "Zero", "Tutorial": Start Tutorial Level
- "One": Start Level 1

On 'Are You Ready' Screen:

- "Yes": start game
- "No": go back to level selection menu

While Playing Game

- "Stop", "Pause": open pause menu

On Pause Menu

- "Start", "Resume": resume game
- "Menu": goes to Main Menu
- "Quit", "Exit": exit game

On End Screen

- "Menu": goes to Main Menu

### Web Cam

Ensure your arms are in frame, even when jumping. Best performance is when you are 1-3 ft from your webcam.

1. Jump

Light jumps are acceptable when 1-3 ft from the camera. Ensure to not duck too much before jumping, as this will be recognized as a duck instead of jump.

2. Duck

Small ducks are also acceptable, given you are 1-3 ft from the camera. Quick ducks are more recognizable than slow ducks.

## Development

Set up your local dev env.

1. [Unity](https://unity3d.com/get-unity/download)
2. [Python3](https://www.python.org/downloads/)
3. [Raspberry Pi](https://www.raspberrypi.com/products/)
