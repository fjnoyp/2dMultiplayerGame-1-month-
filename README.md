# 2dMultiplayerGame-150-hours
 
I worked solo for four weeks on this project. I taught myself about Unity and the fundamentals of multiplayer synchronization and later used my new knowledge to tackle synchronization problems that were unique to my game. I was inspired by the art style of "Limbo" and made extensive use of Photoshop and particle systems to get a similar style in my game.

The bulk of my time was spent on getting multiplayer synchronization to work. Network messages are expensive and all messages are ~100ms old due to internet speeds, so basic game objects such as projectiles need special consideration. In my game, all projectile positions are a function of time and each client has different views of projectiles to compensate for the ~100ms delay to send messages over the internet.

This repo only contains source code, for project documentation, development journal, and demo trailers, see: http://dept.cs.williams.edu/~16ksc2/final-limbonuauts/build/doc/index.html
