# StickyAR

**An AR sticky note app**

StickyAR is a cross-platform mobile app that recreates sticky notes in augmented reality. It detects vertical surfaces like walls, and you place **workspaces** on them. You then put **sticky notes** on those workspaces and change each note's color, size, rotation, position and text. A minimap shows where your workspaces and notes are, and you can save sessions and load them again later.

You get what's useful about sticky notes without the clutter: your notes go wherever your phone goes, and nothing piles up on your desk.

**Demo video:** https://youtu.be/W_m--NwvnZI

**Team:** Rafail Khalilov, Kathleen Lee, Joey Hou and Patricia Luc  
Built for COMS 4172 (3D User Interfaces and Augmented Reality) at Columbia University, May 2022.

---

## Features

### Two manipulation modes

StickyAR has two modes, so you don't pick the wrong object when notes and workspaces overlap:

- **Workspace mode:** detect walls, then place, select, rename, hide or delete workspaces.
- **Sticky note mode:** create, edit, recolor, select and delete notes. Workspace controls are off while you're in this mode.

### Workspaces

- Tap **Detect Walls** to start detecting vertical surfaces. A rectangle sized to the detected plane follows the center of the screen, so you can see how much usable space a wall has.
- Tap the rectangle to place a workspace there. You can place as many as you want. Tap the green **Accept** button when you're done.
- Tap a workspace to select it (it gets a bold outline). With exactly one workspace selected, a text field appears so you can **rename** it.
- **Delete** selected workspaces along with their notes, or use the eye button to **show or hide** all workspaces.

### Sticky notes

- **Create:** In note mode, tap **Create Note**. A new note appears about as far from the camera as a real note would be while you write on it, and it moves with the phone.
- **Edit text:** Tap the note to open the phone's keyboard, just like tapping a text box in any other app.
- **Scale:** Pinch to make the note bigger or smaller before you place it.
- **Rotate:** Turn your phone. The note follows the camera's orientation.
- **Place:** Tap a workspace to stick the note to it.
- **Recolor:** Select one or more placed notes and pick a color from the texture menu.
- **Delete and restore:** **Delete Note** removes the note you're holding. Deleted notes are kept for the rest of the session. **Undo** brings them back in front of you, the left and right arrows scroll through them, and **Restore** returns the current one so you can edit and place it again.

### Selection feedback

Selected notes and workspaces get a bold outline, drawn with the [Quick Outline](https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488) asset. You can select several at once. Tap a selected object again to deselect it. Switching modes deselects everything, so you don't have to clear selections one by one.

### Minimap

A top-down orthographic minimap follows you and rotates with you:

| Marker | Meaning |
|---|---|
| Player icon | Your location |
| Red / yellow dot | A sticky note. The color changes when the note is selected. |
| Blue | A workspace. The color changes when the workspace is selected. |

When a workspace has more than one note, the note markers shrink so the minimap doesn't get cluttered.

### Saving and loading

- **Save Session** writes every workspace that has notes on it to a log file, `StickyAR Session Log.txt`, in the app's persistent data folder (`Application.persistentDataPath`).
- When the app starts, it reads that log back into a list of saved workspaces (`LoadSession.cs`).
- In the full design, **Load Workspaces** opens a scrollable list showing each saved workspace's name, creation date and note count. Tapping an entry while exactly one workspace is selected renames that workspace and rebuilds the saved notes on it. See [Known issues](#known-issues-and-missing-features) for how much of this is in the current code.

Log format, repeated for each workspace:

```
<workspace ID>
<workspace name>
<creation date MM dd yyyy>
<number of notes N>
  ── repeated N times ──
  <position (x, y, z)>
  <rotation (x, y, z, w)>
  <scale (x, y, z)>
  <material name>
  <note text>
```

### Movement and wayfinding

You move around by physically moving and turning the phone. The minimap helps you find notes that are hard to spot, such as ones placed very high or scaled down.

## Tech stack

| | |
|---|---|
| Engine | Unity **2020.3.26f1** |
| AR | AR Foundation 4.1.9, ARKit XR Plugin 4.1.9, ARCore XR Plugin 4.1.9 |
| Other packages | Input System 1.3.0, TextMeshPro 3.0.6, uGUI |
| Third-party asset | [Quick Outline](https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488) (free, Unity Asset Store), included in `Assets/QuickOutline` |
| Platforms | iOS and Android |
| Tested on | iPhone 13 Pro, iOS 15.4.1 |

## Getting started

### Prerequisites

- Unity Hub with Unity **2020.3.26f1**, including the iOS and/or Android Build Support modules
- **iOS:** a Mac with Xcode and an ARKit-capable iPhone or iPad
- **Android:** an [ARCore-supported device](https://developers.google.com/ar/devices)

### Build and run

1. Clone the repository:
   ```bash
   git clone https://github.com/pbluc/StickyAR.git
   ```
2. Open the project folder in Unity Hub with Unity 2020.3.26f1. The packages in `Packages/manifest.json` are resolved automatically.
3. Open `Assets/Scenes/SampleScene.unity`.
4. In **File → Build Settings**, switch the platform to **iOS** or **Android**.
5. In **Project Settings → XR Plug-in Management**, enable **ARKit** for iOS or **ARCore** for Android.
6. Build to your device. On iOS, open the generated Xcode project, set your signing team, and run it on the device.
7. Allow camera access when the app asks.

> **Note:** The build settings reference an **AR Foundation Remote** loader config. That's an optional paid editor plugin for testing on a device from inside the Unity editor, and it isn't in the repo because `Assets/Plugins` is gitignored. You don't need it to build to a device. If Unity warns about the missing config, you can remove it from the build settings.

## How to use

1. **Place workspaces.** Tap **Detect Walls** and slowly point the phone at a wall until the preview rectangle appears. Tap it to place a workspace, repeat for more walls, then tap **Accept**.
2. **Manage workspaces.** Tap workspaces to select them. Then you can rename one (when exactly one is selected), delete them, or toggle visibility.
3. **Add notes.** Tap **Sticky Note Mode**, then **Create Note**. Tap the note to type, pinch to resize, turn the phone to rotate, and tap a workspace to place it.
4. **Change colors.** In note mode, select placed notes and choose a color.
5. **Delete or undo.** Delete the note you're holding with **Delete Note**. Tap **Undo** to browse deleted notes, and **Restore** to get one back.
6. **Save.** Tap **Save Session** to keep your workspaces and notes for later.

## Project structure

```
Assets/
├── Scenes/
│   └── SampleScene.unity                  # Main app scene
├── Scripts/
│   ├── DetectVerticalWallsWithRectangles.cs  # Vertical plane detection, preview rectangle, workspace placement
│   ├── VerticalWallLoader.cs              # Earlier tap-to-place workspace implementation
│   ├── StartDetectingVerticalSurfaces.cs  # "Detect Walls" button
│   ├── StopDetectingVerticalSurfaces.cs   # "Accept" button
│   ├── WorkspaceSelection.cs              # Tap to select/deselect workspaces
│   ├── SetWorkspaceName.cs                # Rename the selected workspace
│   ├── RemoveWorkspace.cs                 # Delete workspaces; scale minimap note icons
│   ├── ToggleWorkspaceVisibility.cs       # Show/hide workspaces
│   ├── EnterStickyNoteMode.cs             # Switch between workspace and note modes
│   ├── OpenNote.cs                        # Create, type, pinch-scale, rotate and place notes
│   ├── StickyNoteSelection.cs             # Tap to select/deselect notes
│   ├── ChangeTexture.cs                   # Recolor selected notes
│   ├── DeleteNote.cs                      # Delete the current note (kept for undo)
│   ├── ToggleDeletedNotes.cs              # Browse and restore deleted notes
│   ├── MinimapController.cs               # Top-down minimap camera that follows the user
│   ├── SaveSession.cs                     # Write the session log
│   └── LoadSession.cs                     # Parse the session log into workspaces and notes
├── Prefabs/          # StickyNote, Wall Workspace, AR Default Plane
├── Materials/        # Note colors, workspace and minimap icon materials
├── NewIcons/         # Current UI icons
├── Icons/            # Older icons from before the UI restyle
├── Textures/         # Minimap render texture
├── QuickOutline/     # Third-party outline asset
└── XR/               # XR plug-in management settings
```

## Known issues and missing features

- **Implementation status:** The written project description also mentions a **zoom-in view** that shows selected notes up close, and placing saved workspaces from the load list. Neither is wired up in this snapshot of the repo. `LoadSession.cs` parses the log, but the **Load Workspaces** button has no handler yet.
- **No save confirmation:** Nothing tells you the session was saved.
- **Loading saved workspaces:**
  - Saving and loading can overwrite workspaces.
  - Note colors aren't carried over when notes are loaded onto a new workspace.
  - Note positions are saved in world space, so they aren't adjusted to the new workspace.
  - Loaded notes are scaled slightly wrong.
- **Deleted notes:** A highlighted note that you delete can stay in the trash view until you tap it.
- **UI layout:** Some UI elements are spaced unevenly or overlap.

## Acknowledgments

- [Quick Outline](https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488) by Chris Nolet (Unity Asset Store) is used for the selection outlines.
