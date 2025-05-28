# Stackling

A cozy, meditative sandbox game about stacking wooden blocks in a child's playroom.

---

## 🎮 Core Gameplay Systems

### 1. Block Interaction
- Grab and drag blocks using the **mouse**
- Rotate blocks using **keyboard keys** (e.g., Q/E or arrow keys)
- **Placement preview indicator** (ghosted or outlined block)
- Object is **placed deliberately** — not dropped
- After placement, **gravity is enabled**, allowing for collapse

### 2. Physics Simulation
- Blocks have physical properties (mass, friction, bounciness)
- Structures can fall if unstable
- Physics tuned for **soft, satisfying behavior**

### 3. Camera Control
- **Fixed orbit camera** that rotates around a central point
- Smooth mouse drag or keys to rotate view
- Optional: Zoom in/out with scroll wheel

---

## 🧸 Environment & Objects

### 4. Playroom Background (Skybox Style)
- **Blurred skybox** based on a photograph of a child’s playroom
- Soft ambient lighting to match the background tone
- Keeps focus on blocks and foreground

### 5. Wooden Blocks
- Various **shapes** (cubes, cylinders, triangles, arches, etc.)
- **Different sizes** and soft **color variations**
- **All blocks start scattered** in the play area

---

## 🔊 Audio & Feel

### 6. Sound Design
- Soft **pickup**, **place**, and **falling** wooden sounds
- **Ambient background music** for a calm atmosphere

### 7. Visual Feedback
- Soft shadows or AO (ambient occlusion)
- **Placement highlight** (ghost or outline)
- Optional: Gentle bounce or wiggle when placing blocks

---

## ⚙️ UI & Controls

### 8. Minimal UI
- No main menu — game starts immediately
- In-game **settings panel** for:
  - Master volume
  - Music volume
  - Sound effects volume
- Optional: **Reset button** to restart the scene

### 9. Controls
- **Mouse** for object movement and camera drag
- **Keyboard** for rotation (Q/E or arrow keys)
- ESC or similar key to open volume settings

---

## 📦 Packaging & Release

### 10. Build & Export
- PC build (Windows, optionally Mac)
- Playable standalone executable

### 11. Itch.io Page
- Warm and cozy **description**
- Screenshots or GIFs of gameplay
- Tags: `#cozy`, `#sandbox`, `#physics`, `#toys`, `#relaxing`

---

## 🗓️ Development Timeline

**Start Date:** May 18, 2025
**Planned Duration:** 8 weeks (ending around July 13, 2025)

This schedule assumes a relaxed pace suitable for a solo developer with limited daily time.

---

## 🗓️ 2-Month Milestone Timeline

### Week 1: Setup & Core Foundation
- Create Unity project
- Import or create basic block prefabs
- Set up physics materials (e.g., soft wood feel)
- Implement mouse drag + object pickup
- Rotate objects using keys (Q/E or arrow keys)
- Simple placement system (no preview yet)
- Basic scene with skybox and ground plane

### Week 2: Physics + Preview + Orbit Camera
- Add placement preview (ghost object or outline)
- Enable gravity **after** placement
- Implement orbit camera (rotate around fixed point)
- Smooth camera drag (mouse) and optional zoom
- Add restart/reset scene button

### Week 3: Blocks & Playroom Feel
- Finalize 5–8 block shapes and sizes
- Create soft color palette for blocks
- Randomly scatter blocks in room at game start
- Blur and set up real photo skybox background
- Add soft ambient lighting

### Week 4: Audio
- Add background ambient music
- Add block interaction sounds:
  - Pickup
  - Placement
  - Collision / collapse
- Tweak volume levels and mixing

### Week 5: UI & Settings
- In-game settings menu (accessible via ESC or icon)
  - Master volume
  - Music volume
  - SFX volume
- Basic pause/resume logic

### Week 6: Polish & Visual Feedback
- Improve placement indicator (scale in/out, outline)
- Add subtle feedback on placement (e.g., squish, sound)
- Adjust physics to feel calm but responsive
- Add light post-processing (bloom, vignette, color grading)

### Week 7: Playtesting & Fixes
- Internal playtesting
- Tweak controls and interactions based on feel
- Fix bugs (e.g., overlapping blocks, jitter, stuck physics)
- Add optional quality-of-life features (e.g., undo last block)

### Week 8: Release Prep
- Build and test standalone PC version
- Record GIFs/screenshots for itch.io
- Write cozy game description
- Set up and publish itch.io page
- Share link with others or communities for feedback

## ✅ Progress Log

### May 18, 2025
- Created new Unity project named `Stackling` using 3D URP template
- Initialized Git repository with appropriate `.gitignore`
- Connected to GitHub and made first commit
- Chose to manage version control using Git + GitHub (not Unity Cloud)
- Verified project is clean and ready for next steps

### May 28, 2025
- Implemented mouse drag and object pickup system
- Completed simple block placement system (gravity applies after placement)
- Implemented basic object rotation *(in progress — needs refinement)*
- Added placement preview *(ghost object — needs polish)*
- Gravity on placement and collapse behavior completed
- Orbit camera system added *(zoom works, smooth drag pending)*
- Not yet started: block prefabs, physics material tuning, skybox, ambient lighting, reset button
