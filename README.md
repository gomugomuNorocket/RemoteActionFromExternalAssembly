# RemoteActionFromExternalAssembly

This project represent automated actions between a server and a client applications.
Normally this might be more meaningful if the server and the client reside on different computers. For the sake of simplicity, both of them running on the same machine.

The whole solution governed by a command-and-control philosophy. The server knows all. The client just does the server’s bidding.

The client receives the assembly of commands from the server and the server may require some action to be performed.

For more details see 'ReadMe.pdf' and 'demo.png'


Test scenario:
build solution, run multiple project and type 'SelectWindow Chrome' on the server to invoke the action on the client.


