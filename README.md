# The Revenge of the Chick 🐥✨

Welcome to **The Revenge of the Chick** - a Star Wars inspired Xonix-style adventure where the fearless chicken battles the moo forces!

https://github.com/user-attachments/assets/463ea2a8-a255-4cb5-abe5-2f9951ea9aa4


- **Victory:** The Chick strikes again! 🐥⚡  
- **Defeat:** The Moo side grows stronger… 🐄💀

---

This Unity project includes **three scenes** that work together to run the game:
- **`WelcomeScene`** — the main menu  
- **`GameScene`** — where gameplay happens  
- **`WonOrLostScene`** — shows whether you win or lose  

Before pressing **Play**, make sure the scenes are set up correctly so everything works as expected.

---

## Getting Started

### 1. Open the Project
Open the project in **Unity**.

### 2. Add Scenes to Build Settings
Go to **File → Build Settings**, and ensure these three scenes are added:

### 3. Open Scenes Additively
You’ll need all three scenes visible in the **Hierarchy** for setup:
1. In the menu bar, go to **Scene → Open Scene Additive** and select **GameScene**.  
2. Repeat the same for **WonOrLostScene**.

### 4. Unload Extra Scenes
Once they’re open, **unload** the last two scenes (**GameScene** and **WonOrLostScene**):

- In the **Hierarchy**, right-click each → **Unload Scene**

Leave **only** `WelcomeScene` loaded.

---

Your scene setup should look like this:

```
├── WelcomeScene (Loaded)
├── GameScene (Unloaded)
└── WonOrLostScene (Unloaded)
```

---

### You’re Ready! 
Press **Play** and help the Chick strike back against the Moo forces! May the feathers be with you 🐥✨

---

### Win & Loss States

Here is what awaits you at the end of your journey:
| **Win** | **Lose** |
| :---: | :---: |
| <img src="https://github.com/KerenStoller/Xonix/blob/master/assets/Won%20screen.png" width="100%"> | <img src="https://github.com/KerenStoller/Xonix/blob/master/assets/lost%20screen.png" width="100%"> |
