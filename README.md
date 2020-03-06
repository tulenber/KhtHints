# Welcome
This repository contains a small library for Unity3D that helps handle hints on loading and other screens.

Welcome to Hints Library provided by the kihontekina.dev project!
You can read a blog post about hints and this library at https://kihontekina.dev/posts/hints_library/

# Capability
* Multiple hint groups for different screens.
* Two strategies for displaying hints: random and sequential
* A timer to show the next hint on long screens.

# Installation
Use this repository as a git submodule or copy it to your project Asset directory.

# Configuration
Hints configurable throw the KhtHintsData.json file.

khtHintsConfigs - block responsible for the parameters of the UI element
hintName - UI element name
usedGroupName - group with hints used to get text
strategy - hint strategy(0: random, 1: sequential)
updateTimeout - hint update timeout(seconds; 0: no update)

khtHintsGroupsData - block responsible for hints lists
groupName - the name of the group to use in usedGroupName from khtHintsConfigs
hintsList - list of hints

# Usage
Add Manager script to some object on loading; it uses the Singleton pattern.
![Manager script](https://kihontekina.dev/img/manager.png)
Add Hint script to UI Text object; if you want to define hint group, adds a name to it.
![Hint script](https://kihontekina.dev/img/hint.png)

# License
[MIT](https://github.com/tulenber/KhtHints/blob/master/LICENSE)
