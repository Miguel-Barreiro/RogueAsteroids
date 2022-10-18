Brief explanation of the architecture.

We start in the MainController and the DependencyManager. They are the entry point to the game. 
In the DependencyManager we create all the Instances we are going to need (mostly systems) and in the MainController we enable some of those systems. 
After that systems will use events to drive the logic of the game. 
The game uses scriptable objects for configurations and object/entity pools for gameObject.
