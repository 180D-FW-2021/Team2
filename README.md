# A-Maze

A-maze, an interactive indoor game designed to test a user’s memory and dexterity, was designed to entertain children and young adults indoors, while involving some physical movement and stimulating the brain.

## Web Application

The web application is located at https://amaze-webapp.herokuapp.com/! The web app shows a player leaderboard, sortable by level, and player history for each individual user.

## Game Controls

### Keyboard Controls

- Forward/back/left/right: arrow keys/ WASD
- Rotate camera: 'z' (left) 'x' (right)
- Jump: space bar
- Duck: Left Shift
- Pause Menu: Esc

### IMU Controls

|                          Neutral                          |                          Forward                          |                          Left                          |                          Right                          |
| :-------------------------------------------------------: | :-------------------------------------------------------: | :----------------------------------------------------: | :-----------------------------------------------------: |
| <img src="./images/1.jpg" alt="imu_neutral" width="200"/> | <img src="./images/2.jpg" alt="imu_forward" width="200"/> | <img src="./images/3.jpg" alt="imu_left" width="200"/> | <img src="./images/4.jpg" alt="imu_right" width="200"/> |

### Voice Commands

On Main Menu:

- "Back", "Log Out": log out and return to Start Screen
- "Levels": Open Level Selection Screen
- "Help": Open Help menu (not yet implemented)
- "Quit": Quit game

On Level Selection Screen:

- "Back": Return back to Main Menu
- "Zero", "Tutorial": Start Tutorial Level
- "One": Load Level 1
- "Two": Load Level 2
- "Three": Load Level 3
- "Four": Load Level 4

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

<img src="./images/movenet_position.png" alt="movenet_position" width="200"/>

|                                                                           Jump                                                                            |                                                           Duck                                                            |
| :-------------------------------------------------------------------------------------------------------------------------------------------------------: | :-----------------------------------------------------------------------------------------------------------------------: |
| Light jumps are acceptable when 1-3 ft from the camera. Ensure to not duck too much before jumping, as this will be recognized as a duck instead of jump. | Small ducks are also acceptable, given you are 1-3 ft from the camera. Quick ducks are more recognizable than slow ducks. |

## Development

Set up your development environment.

### Installation

First, download our tech stack and obtain our hardware:

1. [Unity](https://unity3d.com/get-unity/download)
2. [Python3](https://www.python.org/downloads/)
3. [Raspberry Pi Zero](https://www.raspberrypi.com/news/zero-wh/)

In addition, we recommend you install [anaconda](https://www.anaconda.com/products/individual) and [conda](https://docs.conda.io/projects/conda/en/latest/user-guide/install/index.html). We also recommend you [create a virtual environment](https://uoa-eresearch.github.io/eresearch-cookbook/recipe/2014/11/20/conda/) for this project. Later instructions (including our Windows LaunchGame.bat script) assume you have initialized a virtual environment $yourenvname.

#### Movenet

Install python dependencies for Movenet.

```
$ conda activate $yourenvname
# ensure you have pip
$ conda install pip
# install dependencies
$ cd Setup_Scripts
$ pip install -r movenet_requirements.txt
$ conda deactivate
```

Run the movement detection script:

```
$ cd Movenet/src
$ python3 position_tracking.py
```

You can also follow installation instructions in **Movenet/README.md** if this does not work for you.

#### Raspberry Pi

##### SSH into raspberry pi

```
ssh pi@raspberrypi.local
```

##### Install all dependencies on raspberry pi

```
pip install paho-mqtt
pip install numpy
```

##### Startup Script for Raspberry Pi 

```
vim .bashrc
```
At the bottom of the file, add the following:
```
sudo python IMU/berryIMU.py
```
Note: If you have the repository in a different directory, please update the path above. 
To save in vim, press the following:
1. escape button 
2. : 
3. w 
4. q

##### Reboot Raspberry Pi 
```
sudo shutdown -r now
```
After you SSH into the raspberry pi again, the startup script will run. 

#### Unity

1. As stated above, download [Unity Hub](https://unity3d.com/get-unity/download).

2. Within Unity Hub, install Unity Editor version 2018.4.36f1. We recommend you use this version to ensure cross-compatability.

3. Within Unity Hub, open the Unity subdirectory. From here, you should be able to open the project from Unity Hub for development.

### Run the Game

We have provided scripts to automatically open the game and Movenet position tracking concurrently. Scripts are located in the **Setup_Scripts** directory.

Prior to running the scripts, make sure you have properly installed python3 and have set up your virtual conda environment.

Also, ensure you [build your game executable](https://docs.unity3d.com/2018.4/Documentation/Manual/PublishingBuilds.html).

#### Windows

For Windows, a **LaunchGame.bat** file is provided. The first two variables, ANACONDA_DIR and ENV_NAME may have to be updated for custom Anaconda installation directories and custom Conda environment names (the default is 'yourenvname'). To use this script, launch a command prompt from the Setup_Scripts directory and run:

```
$ LaunchGame.bat
```

#### MacOS

1. Initialize your conda environment.

```
$ conda activate $your_env_name
```

2. Edit Unity executable name in **LaunchGame** script.

```
PROJECT_NAME="MazeProject"
```

3. Run the script.

```
$ cd Setup_Scripts
$ ./LaunchGame
```

4. Deactivate your environment.

```
$ conda deactivate
```


## Resources
We would like to give attribution to the following Resources.

By level, the following audio sources were used:
- Maze Level 0: "Riviera" by Maarten Schellekens
- Maze Level 1: "04 Night-Shadows" by Ketsa
- Maze Level 2 and 4: "Flowers Don't Say Anything, They Just Think" by One Man Book
- Maze Level 3: "Nocturn" by M33 Project

The background used in the Unity in-game menus are credited to user3802032 on freepik.com

