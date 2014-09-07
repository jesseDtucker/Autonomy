Autonomy
========

Experimental project for testing an algorithm that would allow for multithreaded operations on a collection
of items. The individual items would have no concept of multithreaded access and therefore would have no internal
locks or other data protection schemes. Furthermore most operations are lockless and instead the order of operations
is manipulated to ensure that concurrent access to a single item never occurs.

The idea, in a nutshell, is to organize and divide the collection using a tree where items that have some relation
are located close together in the tree (ie. share a common ancestor that is removed by as few levels as possible).
Then when an operation needs to occur the operation also specifies what data it will operate on. Given this information
the task is queued at the lowest common parent of all the items specified and will be executed later. Periodically (or
continuously) the tree structure will update and execute all queued operations. The update starts at the root node and
executes all operations queued at the root. Then each child performs the same work independently of the other children.

The result is that the children can update in parallel. So if there is a large amount of data and the data is effectively
divided into sets each set can be updated independantely of the others while operations that overlap several sets
will not allow concurrent access.

This project tests this algorithm in a simple manner by introducing a large 2D space and then placing a number of independent
entities (called Trogo's, no idea why I named them that) that then proceed to move around the space and bump into each other.
The entities spatial coordinates provide a simple means to sort them into a tree structure using a method similar to a
BSP tree. The method used here is not a proper BSP system, it was just a simple mock that allowed me to divide the entities.

Once divided the entities move around and collide with one another, changing direction upon collision. Each entity has no concept
of multithreaded access. However, the movement and collisions are handled with multiple threads as all entities are contained
in the previously described tree structure which ensure concurrent access will not occur while allowing most entities to update
independantly of most other entities. Only entities that are near the vertical center line of the grid will frequently need
to perform updates that cannot be broken onto 2 or more threads.

Overall the algorithm appears to work, there are certainly bugs as it appears some Entities will occasionally pass through others
but attempts to force a case of concurrent access never occured (Note: the code that tested this was hacked up locally and never
included in the submitted project). The advantage of this is it allows for some collections of data to hopefully spread operations
across more than 1 thread without having to add locking code to the underlying data objects. Also most operations are lock free
(though I believe there may be a lock or two related to some secondary operations). 

The downside is that the interface is, at least in this initial test, quite cumbersome and difficult to code against. 
I believe this could be improved and simplified but I haven't taken the time to do so. This code was a quick hack
done in a couple of weekends to verify if the idea was sound. The other issue discovered is that the overhead for determining
where in the tree to perform an update proved to be quite high. Admittedly in this case the collisions checks and position
updates were very cheap operations (collisions were handled with circles and rectangles) so it is possible this algorithm would
perform better in cases where the operations being performed were both numerous and expensive.

I may revisit the concepts in this code sometime in the future, most likely to rewrite the algorithm, improve the interface
and find a more valid test case for evaluating the performance of the algorithm and looking for ways to reduce the overhead.
