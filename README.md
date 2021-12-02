# A-Maze

A-maze, an interactive indoor game designed to test a user’s memory and dexterity, was designed to entertain children and young adults indoors, while involving some physical movement and stimulating the brain.

## Running the Game

For Windows, a LaunchGame.bat file is provided. The first two variables, ANACONDA_DIR and ENV_NAME may have to be updated for custom Anaconda installation directories and custom Conda environment names (the default is 'yourenvname'). To use this script, launch a command prompt from the base Team2 directory and run:
  > $ LaunchGame.bat

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

- Jump
- Duck

## Development

Set up your local dev env.

1. [Unity](https://unity3d.com/get-unity/download)
2. [Python3](https://www.python.org/downloads/)
3. [Raspberry Pi](https://www.raspberrypi.com/products/)

<img src="..\1.jpg" />
