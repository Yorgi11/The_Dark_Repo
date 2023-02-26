# The_Dark
PITCH:
https://youtu.be/NzBa7QYCGUw

TRELLO:
https://trello.com/b/Mngml54B/graphics  (Must me logged into a trello account to view)

https://trello.com/invite/b/Mngml54B/ATTI8557c34be66d413bc95b99b1839e8d2903D7D71B/graphics  (Backup link if the first does not work)

CAPTIONS:
Hello, my name is Michael Giorgi. I am the lead programmer of Cavelight Studios. 

We are developing a Fps/Mystery/Horror game where the players objective is to explore and uncover the secrets of a hidden facility underneath an old hotel

We will be using CG to create a toon shader as well as an inked line shader to create a visual style somewhere between an Archer comic and the borderlands series. We will be implementing Specular, and Toon ramp, as well any other Illumination fields we find thats fits or enchances the style. We will also be implementing fog, particles, ambient occlusion, shadows and bloom. Another feature I would like is a lensflare effect that is also affected by the Toon shader and/or the ink lines shader.

I will be attempting to implement the majourity of this my self as I am the only memeber of my group in this class.

I belive that throughout this course I will be able to develop all of these CG features, and implement them in a non-performance intensive mannor, to keep the game smooth and playable.


# Group Assignment
Feb 26, 2023

Contributing Members: Michael Giorgi

Integrating from the individual assignmnet; I have integrated illumination and toon shader from the idividual assignment
Modifications: The toon shader has been re-writen as a vertex shader, and shadows as well as a second shadow caster pass have been added. The reverse hull method to render outlines has been removed and replaced with a post processing effect.

Description: Our game is a first person fps game which has puzzle elements and mystery solving as well as a somewhat comedic view of itself with a variety of weapons to use as well as areas to explore

Contributions (Michael Giorgi): All scripts except for those under the 'DetectiveInteraction' folder have been written 100% by me

Visual effects: I have implemented two particles sytems, one for the gun sparks, and one for the muzzle flash/smoke

Postprocessing Effects: I have implemented Bloom as well as a screen space outline effect.
Bloom: There are two main bloom effects one on the overhead lights to give the illusion they are being used to illuminate the scene, and one on the muzzle of the guns to add a glowing effect durring consecutive firing.

Known Issues: Since adding shadows unity has broken and every build even those with shadows removed opens with a black screen on the main game scene

Video Report: https://youtu.be/T6SlbqBx2sg

Presentation: https://docs.google.com/presentation/d/1fISQ-rJypNm8CEsS6Y9XYnj23unxz7zCNwczh_2-a7M/edit?usp=sharing
