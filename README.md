## Synopsis

This is an evolutionary algorithm done in unity which was made to demo the use genetic programming for a complex task.
The demo involves randomly creating a rocket which is then optimized around a random payload. As the generations progress, 
the rocket optimizes itself to fulfill a fitness function.
The fitness function was designed to maximize the height the rocket achieves, while minimizing the cost of parts, and the weight.
For additional details on the theory, see the provided pdf.

IT IS STRONGLY RECOMMENDED THAT YOU READ THE ATTACHED REPORT BEFORE ATTEMPTING TO USE THIS FOR APPLICATION PURPOSES

## Installation

Download or pull the repo, and open within unity. Currently the only interface is through unity itself

## Quick-Setup

A Video guide to installation can be found [here](https://youtu.be/LjwAEPznd-w)

There's three included setups;
	Infinite Flight:
		- The rockets are relatively small, but fuel consumption is incredibly small. This leads to
		rockets which are able to fly regardless of weight.
	Smaller Unrealistic:
		-The rockets are incredibly small, and the fuel force is relatively high. These rockets tend to behave
		The most interestingly
	Realistic Simulation:
		-Using realistic fuel approximations, these rockets fly how they should if they were real. Hint: they dont
	
	CONTROLS:
		Pressing left control will alert the simulation that you would like it to end. It will finish simulating the current generation, then
		once it is done, present to you the best individual thus far. If you change your mind before the current generation finishes, you can
		press left control again to tell the simulation you no longer wish to cancel the simulation.
	
	
## Block Manager
	The block manager is where a user is able to define the physical properties of the blocks themselves.
	To alter the size of possible blocks, simply increase the size of the block types array.
	Each block type must have a
	
	UID: Used for looking up blocks in the internal logic.
	Name: Used for defining the different block functions.
	***********ALL SIMULATIONS MUST HAVE A FUEL, A THRUSTER, AND A PAYLOAD NAMED BLOCK*************
	Cost: The relative cost (could be money, time, etc.) to use this piece
	Weight: The weight (kg) of the piece being added
	Model: You can prepare your own block model and instantiate it as a prefab here.
		In order for it to work naturally, the model must be 1m^3 in size when a scale of 1 is applied.
		The model must also have a collider. The more complex the collider, the slower the simulation, however
		the more simple the collider, the less accurate the simulation.
		To test the size and orientation of a desired block. Drag in the payload prefab to see how the custom block
		compares.
		
## Individual Manager
	The individual manager is where parameters involving individual rockets go.
	these consist of
	
	Fuel Force: This is the force in Newtons which is applied by thrusters, in the direction they are facing, per delta time.
		This delta time is processor dependant so the number needs to be tweaked to individual computers.
	Fuel consumption. Each fuel block holds 1m^3 volume of fuel. This number signifies how much of the total sum of all fuel is consumed
		per delta time.
	Max individual size: When adding pieces to the rocket, it will never get bigger than this
	Payload size: Because payloads are random, this is the volume of the initial random payload
	Start Individual Size: The individual size of the rockets in generation 1
	
## Mutation Manager
	The mutation manager holds the probabilities are variables having to do with the genetic aspect of the algortihm.
	
	Addition Probability: On each generational update, the spots in which a new piece could be added are taken into account
		and each spot has this probability of having a new random piece inserted with random orientation
	Do Nothing Probability: On each update, each individual block already part of the individual is checked whether to be
		altered or deleted. Each block has this probability of nothing happening to it.
	Alter or Delete Probability: After deciding if the block does not mutate, in the event that it does, it either mutates or
		deletes. This is the probability of it mutating where it deleting is 1-p where p is this probability.
	Generation Size: The number of individuals in a generation
	Degree to mutate: The number of iterations that each block is inspected during mutation
	Num of Generations without change: The number of consecutive generations without a change in the current best
	
	
## Known Bugs
	If the time step is too fast, occasionally the threading will cause the rocket object to delete mid-flight which will cause a null exception.
	In the event of this happening, decrease the time (~60 is about as high as I would feel comfortable to leave it over night), and try again.
	
	If the max individual size gets above 8000 m^3, then issues with the indexing for organizing start happening. I think this is an issue with how unity
	holds information about the 3d space. Essentially the blocks exists beyond the bounds of unity's world and physics/graphics stop working at that point.


## Contributors

	Author : Cooper Davies, 2017
